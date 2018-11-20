using System;
using System.Collections.Generic;
using System.Windows.Forms;
using No8.Solution.Factory;
using No8.Solution.Model;
using No8.Solution.Repositories;

namespace No8.Solution
{
    /// <summary>
    /// The printer manager class.
    /// </summary>
    public class PrinterManager
    {
        /// <summary>
        /// The event on printing
        /// </summary>
        public event EventHandler<PrintingEventArgs> Printing = delegate { };

        private readonly IPrinterRepository _repository;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="repository">
        /// The <see cref="IPrinterRepository"/> instance where <see cref="Printer"/> instances are stored.
        /// </param>
        public PrinterManager(IPrinterRepository repository) => this._repository = repository;

        /// <summary>
        /// The method that adds the new printer.
        /// </summary>
        /// <param name="type">
        /// The type of printer to be added.
        /// </param>
        /// <param name="name">
        /// The name of printer to be added.
        /// </param>
        /// <param name="model">
        /// The model of printer to be added.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the printer with <paramref name="name"/> and <paramref name="model"/> already exists.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="type"/> is incorrect.
        /// </exception>
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

        /// <summary>
        /// The method that gets bytes to prints.
        /// </summary>
        /// <param name="printer">
        /// The printer that prints.
        /// </param>
        /// <returns>
        /// The byte array to be shown.
        /// </returns>
        public byte[] Print(Printer printer)
        {
            var o = new OpenFileDialog();
            o.ShowDialog();

            OnPrinting(new PrintingEventArgs(DateTime.Now, "Printing started"));
            byte[] result = printer.Print(o.FileName);
            OnPrinting(new PrintingEventArgs(DateTime.Now, "Printing ended"));

            return result;
        }

        /// <summary>
        /// The method that gets list of printers according to the type.
        /// </summary>
        /// <param name="type">
        /// The type of printers to be added to the list.
        /// </param>
        /// <returns>
        /// The list of <see cref="Printer"/> instances with specified <paramref name="type"/>
        /// </returns>
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