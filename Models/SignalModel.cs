using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class SignalModel
    {
        [Required] 
        public string Symbol { get; set; }
        [Range(1, 1000)]
        public double EntryPrice { get; set; }
        [Range(1, 1000)]
        public double TL { get; set; }
        [Range(1, 1000)]
        public double SL { get; set; }

        

    }
}
