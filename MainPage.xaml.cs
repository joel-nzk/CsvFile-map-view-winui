using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Windows.Storage;
using CansatMapViewer.Data;
using Windows.Storage.Pickers;
using System.Diagnostics;
using Windows.UI.Core;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CansatMapViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string acces_key = "bing_map_api_key";

        private double lat_start = 46.82156331870691;
        private double lon_start = 6.935189547606109;
        private double StartAltitude = 483;
        private double EndAltitude = 509;

        private static MapIcon endMarker = new MapIcon();
        private static MapIcon startMarker = new MapIcon();

        private DataFrame previous_data_frame;


        public MainPage()
        {

            this.InitializeComponent();
            InitializeMap();



            Window.Current.Content = this;

        }

        private void CreaterStartEndMarkers()
        {
            DataFrame startFrame = DataHandler.Datas.First();
            DataFrame endFrame = DataHandler.Datas.Last();

            startMarker.Location = new Geopoint(new BasicGeoposition { Latitude = startFrame.Lat, Longitude = startFrame.Lon });
            startMarker.Title = "Start";


            endMarker.Location = new Geopoint(new BasicGeoposition { Latitude = endFrame.Lat, Longitude = endFrame.Lon});
            endMarker.Title = "End";

            mapControl.MapElements.Add(startMarker);
            mapControl.MapElements.Add(endMarker);

        }

        private async void ReadCSVFile()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".csv");
            StorageFile file = await picker.PickSingleFileAsync();

            if(file != null)
            {

                //https://docs.microsoft.com/en-us/uwp/api/windows.storage.storagefile.copyasync?view=winrt-22000


                var stream = await  file.OpenAsync(FileAccessMode.Read);

                using (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    string fileLines = await reader.ReadToEndAsync();

                    List<string> frames = fileLines.Split("\r\n").ToList();

                    frames.RemoveAt(0);

                    foreach (var frame in frames)
                    {
                        DataHandler.Datas.Add(new DataFrame(frame));
                    }

          

                }

                

            }

            FillMapWithData();
        }


        private async void FillMapWithData()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CreaterStartEndMarkers();



                foreach (var data_frame in DataHandler.Datas)
                {



                    if (previous_data_frame != null)
                    {
                        var line = new MapPolyline
                        {
                            Path = new Geopath(new List<BasicGeoposition>()
                            {
                                new BasicGeoposition{Latitude = previous_data_frame.Lat, Longitude = previous_data_frame.Lon, Altitude = previous_data_frame.Altitude },
                                new BasicGeoposition{Latitude = data_frame.Lat, Longitude = data_frame.Lon, Altitude = data_frame.Altitude},
                            },AltitudeReferenceSystem.Geoid),
                            StrokeColor = Windows.UI.Colors.Black,
                            StrokeThickness = 2,
                            

                        };

                        mapControl.MapElements.Add(line);



                    }


                    previous_data_frame = data_frame;


                }


            });

           
            


        }




        private void InitializeMap()
        {
            mapControl.MapServiceToken = acces_key;
            mapControl.Style = MapStyle.Aerial3DWithRoads;

            mapControl.Center = new Geopoint(new BasicGeoposition
            {
                Latitude = lat_start,
                Longitude = lon_start,
            });

            mapControl.ZoomLevel = 14;
        }

       
        private void btn_read_Click(object sender, RoutedEventArgs e)
        {
            ReadCSVFile();
        }
    }
}
