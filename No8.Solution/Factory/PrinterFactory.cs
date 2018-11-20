using No8.Solution.Model;

namespace No8.Solution.Factory
{
    internal abstract class PrinterFactory
    {
        public abstract Printer Create(string name, string model);
    }
}