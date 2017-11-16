using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class MorrisBar
    {
        public int Tanggal { get; set; }
        public int Open { get; set; }
        public int Closed { get; set; }
        public int Count { get; set; }
    }
    public class MorrisDonut
    {
        public string label { get; set; }
        public int value { get; set; }
    }
}