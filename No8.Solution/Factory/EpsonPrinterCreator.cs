using System;
using No8.Solution.Model;

namespace No8.Solution.Factory
{
    internal class EpsonPrinterCreator : PrinterFactory
    {
        public Printer Create(string model) => Create("Epson", model);

        public override Printer Create(string name, string model)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} is null");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException($"{nameof(model)} is null");
            }

            return new EpsonPrinter(name, model);
        }
    }
}