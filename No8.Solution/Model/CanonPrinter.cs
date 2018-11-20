namespace No8.Solution.Model
{
    internal class CanonPrinter : Printer
    {
        public CanonPrinter(string name, string model) : base(name, model)
        {
            Type = PrinterType.Canon;
        }
    }
}