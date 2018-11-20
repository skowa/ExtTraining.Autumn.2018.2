using System;
using System.Collections.Generic;
using System.Linq;
using No8.Solution.Model;

namespace No8.Solution.Repositories
{
    /// <summary>
    /// The class of printer repository list.
    /// </summary>
    public class ListPrinterRepository : IPrinterRepository
    {
        private readonly List<Printer> _printers = new List<Printer>();

        /// <summary>
        /// Adds <paramref name="printer"/> to repository.
        /// </summary>
        /// <param name="printer">
        /// The printer to be added.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="printer"/> is null.
        /// </exception>
        public void AddPrinter(Printer printer)
        {
            if (printer == null)
            {
                throw new ArgumentNullException($"{nameof(printer)} is null");
            }

            _printers.Add(printer);
        }

        /// <summary>
        /// The method that tries to get printer with <paramref name="name"/> and <paramref name="model"/>
        /// </summary>
        /// <param name="name">
        /// The name of the printer to be searched for.
        /// </param>
        /// <param name="model">
        /// The model of the printer to be searched for.
        /// </param>
        /// <returns>
        /// True, if the printer was found; otherwise false.
        /// </returns>
        public bool TryGetPrinter(string name, string model) =>
            _printers.Any(p => p.Name == name && p.Model == model);

        /// <summary>
        /// Gets list of printers with specified type.
        /// </summary>
        /// <param name="type">
        /// The type of printers to be searched.
        /// </param>
        /// <returns>
        /// List of printers with <paramref name="type"/>.
        /// </returns>
        public List<Printer> GetPrintersAccordingToType(PrinterType type) => _printers.Where(p => p.Type == type).ToList();
    }
}