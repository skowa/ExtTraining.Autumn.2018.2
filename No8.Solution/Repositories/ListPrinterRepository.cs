using System;
using System.Collections.Generic;
using System.Linq;
using No8.Solution.Model;

namespace No8.Solution.Repositories
{
    public class ListPrinterRepository : IPrinterRepository
    {
        private readonly List<Printer> _printers = new List<Printer>();

        public void AddPrinter(Printer printer)
        {
            if (printer == null)
            {
                throw new ArgumentNullException($"{nameof(printer)} is null");
            }

            _printers.Add(printer);
        }

        public bool TryGetPrinter(string name, string model) =>
            _printers.Any(p => p.Name == name && p.Model == model);

        public List<Printer> GetPrintersAccordingToType(PrinterType type) => _printers.Where(p => p.Type == type).ToList();
    }
}