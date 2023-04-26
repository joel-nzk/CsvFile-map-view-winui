using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CansatMapViewer.Data
{

    public static class DataHandler
    {
        public static ObservableCollection<DataFrame> Datas = new ObservableCollection<DataFrame>();
        public static int coordIndex = 0;

 




    }
}
