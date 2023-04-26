using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CansatMapViewer.Data
{


    //pression - hP
    //co2 - ppm
    //pm1/2.5/10 - microgramme/m3
    //tvoc - ppb


    public class DataFrame 
    {
        public string Id { get; set; }
        public double Counter { get; set; }

        public double Altitude { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string NS_gps { get; set; }
        public string EW_gps { get; set; }



        public DataFrame(string frame)
        {
            DecodeFrame(frame);
        }

        private void DecodeFrame(string frame)
        {
            frame = frame.Replace("\x02", "").Replace("\x03", "");
            string[] frameData = frame.Split(';');

            try
            {
                Counter = double.Parse(frameData[0]);
                Altitude = double.Parse(frameData[4]);
                Lat = double.Parse(frameData[10]);
                NS_gps = frameData[11];
                Lon = double.Parse(frameData[12]);
                EW_gps = frameData[13];
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"ERROR : An error occured when parsing data in Dataframe.cs");
                DataHandler.Datas.Remove(this);
                
            }

 

            
        }
        

    }
}
