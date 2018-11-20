using System;
using System.Collections.Generic;
using System.Windows.Forms;
using No8.Solution.Factory;
using No8.Solution.Model;
using No8.Solution.Repositories;

namespace No8.Solution
{
    public class PrinterManager
    {
        public event EventHandler<PrintingEventArgs> Printing = delegate { };


        private readonly IPrinterRepository _repository;

        public PrinterManager(IPrinterRepository repository) => this._repository = repository;

        public void AddNewPrinter(PrinterType type, string name, string model)
        {
            if (_repository.TryGetPrinter(name, model))
            {
                throw new InvalidOperationException("The printer already exists");
            }

            switch (type)
            {
                case PrinterType.Epson:
                    _repository.AddPrinter(new EpsonPrinterCreator().Create(model));
                    break;
                case PrinterType.Canon:
                    _repository.AddPrinter(new CanonPrinterCreator().Create(model));
                    break;
                case PrinterType.Unknown:
                    _repository.AddPrinter(new UnknownPrinterCreator().Create(name, model));
                    break;
                default:
                    throw new ArgumentException($"{nameof(type)} is invalid");
            }
        }

        public byte[] Print(Printer printer)
        {
            OnPrinting(new PrintingEventArgs(DateTime.Now, "Printing started"));
            var o = new OpenFileDialog();
            o.ShowDialog();

            byte[] result = printer.Print(o.FileName);
            OnPrinting(new PrintingEventArgs(DateTime.Now, "Printing ended"));

            return result;
        }

        public List<Printer> GetPrintersAccordingToTypeList(PrinterType type) =>
            _repository.GetPrintersAccordingToType(type);
        
        protected virtual void OnPrinting(PrintingEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException($"{nameof(eventArgs)} is null");
            }

            Printing(this, eventArgs);
        }
    }
}