using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using No8.Solution.Factory;
using No8.Solution.Model;

namespace No8.Solution
{
    /// <summary>
    /// The printer manager class.
    /// </summary>
    public class PrinterManager
    {
        private readonly List<Printer> _printers;

        /// <summary>
        /// The constructor.
        /// </summary>
        public PrinterManager(Logger logger)
        {
            Logger = logger;
            _printers = new List<Printer>();
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public Logger Logger { get; }

        /// <summary>
        /// The method that adds the new printer.
        /// </summary>
        /// <param name="name">
        /// The name of printer to be added.
        /// </param>
        /// <param name="model">
        /// The model of printer to be added.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the printer with <paramref name="name"/> and <paramref name="model"/> already exists.
        /// </exception>
        public void AddNewPrinter(string name, string model)
        {
            if (TryGetPrinter(name, model))
            {
                throw new InvalidOperationException("The printer already exists");
            }

            Printer printer = PrinterFactory.CreatePrinter(name, model);
            _printers.Add(printer);
            Logger.Trace($"Printer {name} - {model} is added.");

            printer.StartPrint += LogEvent;
            printer.EndPrint += LogEvent;
        }

        /// <summary>
        /// The method sends printing task to <paramref name="printer"/>.
        /// </summary>
        /// <param name="printer">
        /// The printer that prints.
        /// </param>
        /// <param name="fileName">
        /// The file to be printed.
        /// </param>
        /// <param name="writer">
        /// The output stream.
        /// </param>
        public void Print(Printer printer, string fileName, TextWriter writer)
        {
            if (printer == null)
            {
                throw new ArgumentNullException($"{nameof(printer)} is null");
            }

            if (writer == null)
            {
                throw new ArgumentNullException($"{nameof(writer)} is null");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"{nameof(fileName)} is invalid");
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"{nameof(fileName)} is not found");
            }

            printer.Print(File.OpenRead(fileName), writer);
        }

        /// <summary>
        /// The method that gets list of printers according to the type.
        /// </summary>
        /// <param name="name">
        /// The type of printers to be added to the list.
        /// </param>
        /// <returns>
        /// The list of <see cref="Printer"/> instances with specified <paramref name="name"/>
        /// </returns>
        public List<Printer> GetPrintersAccordingToType(string name) =>
            _printers.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

        /// <summary>
        /// Unregister the logger from the printers events.
        /// </summary>
        public void UnregisterLogger()
        {
            foreach (var printer in _printers)
            {
                printer.StartPrint -= LogEvent;
                printer.EndPrint -= LogEvent;
            }
        }

        private void LogEvent(object sender, PrintingEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            Logger.Trace($"{eventArgs.Message} - {eventArgs.Time}");
        }

        private bool TryGetPrinter(string name, string model) =>
            _printers.Any(p => p.Name == name && p.Model == model);
    }
}