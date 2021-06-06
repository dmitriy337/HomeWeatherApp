using System;
using System.Collections.Generic;

#nullable disable

namespace HomeWeatherApp
{
    public partial class Recording
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double? Temp { get; set; }
        public double? Hum { get; set; }
    }
}
