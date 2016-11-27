using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dziennik
{
    class Cwiczenie
    {
        public string Cwiczenie_v { get; set; }
        public string IloscSerii_v { get; set; }
        public string IloscPowtorzen_v { get; set; }
        public byte [] Image {get; set;}

    }
}