using System;
using System.Collections.Generic;
using Server.Entities;
using Server.Helpers;

namespace Server.Services
{
    public interface ISignalService
    {
        Signal create(Signal signal);

        void activateSignal(int id);

        void deactivateSignal(int id);

        public IEnumerable<Signal> GetAll();
    }

    public class SignalService : ISignalService
    {
        private SignalDbContext _context;

        public SignalService(SignalDbContext context)
        {
            _context = context;
        }

        public Signal create(Signal signal)
        {
            if (AppSettings.AutoConfirm)
            {
                signal.Status = Status.Active;
            }
            else
            {
                signal.Status = Status.InActive;
            }

            signal.CreationDT = DateTime.UtcNow;
            _context.Signals.Add(signal);
            _context.SaveChanges();
            return signal;
        }

        public void activateSignal(int id)
        {
            var signal = _context.Signals.Find(id);

            if (signal == null)
                throw new Exception("Signal not found");

            signal.Status = Status.Active;
            _context.Signals.Update(signal);
            _context.SaveChanges();
        }

        public void deactivateSignal(int id)
        {
            var signal = _context.Signals.Find(id);

            if (signal == null)
                throw new Exception("Signal not found");

            signal.Status = Status.InActive;
            _context.Signals.Update(signal);
            _context.SaveChanges();
        }

        public IEnumerable<Signal> GetAll()
        {
            return _context.Signals;
        }
    }
}