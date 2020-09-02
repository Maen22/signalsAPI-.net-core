using Server.Audit;
using System;

namespace Server.Entities
{
    public class Signal : Auditable<string>
    {
        public int Id { get; set; }
        public string SignalId { get; set; }
        public double EntryPrice { get; set; }
        public double TL { get; set; }
        public double SL { get; set; }
        public string Symbol { get; set; }
        public DateTime CreationDT { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Active,
        InActive
    }
}