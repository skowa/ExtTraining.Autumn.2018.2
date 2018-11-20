namespace No8.Solution.Model
{
    internal class UnknownTypePrinter : Printer
    {
        public UnknownTypePrinter(string name, string model) : base(name, model) => Type = PrinterType.Unknown;
    }
}