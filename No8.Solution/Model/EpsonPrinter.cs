namespace No8.Solution.Model
{
    internal class EpsonPrinter : Printer
    {
        public EpsonPrinter(string name, string model) : base(name, model)
        {
            Type = PrinterType.Epson;
        }
    }
}