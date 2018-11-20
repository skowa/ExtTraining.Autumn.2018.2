using System.Collections.Generic;
using No8.Solution.Model;

namespace No8.Solution.Repositories
{
    public interface IPrinterRepository
    {
        void AddPrinter(Printer printer);

        bool TryGetPrinter(string name, string model);

        List<Printer> GetPrintersAccordingToType(PrinterType type);
    }
}