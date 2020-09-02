using Server.Audit;
using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class Signal : Auditable<string>
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string SignalId { get; set; }
        public double EntryPrice { get; set; }
        public double TL { get; set; }
        public double SL { get; set; }

        [MaxLength(5)]
        public string Symbol { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDT { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Active,
        InActive
    }
}