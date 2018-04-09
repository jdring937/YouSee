using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace YouSee
{
    class User
    {
        public String userName { get; set; }
        public double userLat { get; set; }
        public double userLng { get; set; }

        public List<int> userID { get; set; }
        public ObservableCollection<String> userNames {get;set;}
        public List<double> userLats { get; set; }
        public List<double> userLngs { get; set; }

    }
}
