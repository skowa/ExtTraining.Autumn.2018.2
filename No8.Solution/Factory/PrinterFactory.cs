using System;
using No8.Solution.Model;

namespace No8.Solution.Factory
{
    internal static class PrinterFactory
    {
        public static Printer CreatePrinter(string name, string model)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} is null");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException($"{nameof(model)} is null");
            }

            switch (name)
            {
                case "Epson":
                    return new EpsonPrinter(model);
                case "Canon":
                    return new CanonPrinter(model);
                default:
                    throw new ArgumentException($"{nameof(name)} is invalid");
            }
        }
    }
}