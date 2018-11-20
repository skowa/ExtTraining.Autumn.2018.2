using System.IO;

namespace No8.Solution.Model
{
    /// <summary>
    /// The abstract class of printer
    /// </summary>
    public abstract class Printer
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="name">
        /// The name of the printer to be created.
        /// </param>
        /// <param name="model">
        /// The model of the printer to be created.
        /// </param>
        public Printer(string name, string model)
        {
            Name = name;
            Model = model;
        }

        /// <summary>
        /// Gets the name of the printer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the model of the printer.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Gets the type of the printer.
        /// </summary>
        public PrinterType Type { get; protected set; }

        /// <summary>
        /// Gets bytes from the file with <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">
        /// The name of file to be printed.
        /// </param>
        /// <returns>
        /// The array of bytes printed.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown when file with <paramref name="fileName"/> does not exist.
        /// </exception>
        public virtual byte[] Print(string fileName)
        {
            byte[] result;

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"{nameof(fileName)} is not found");
            }

            using (FileStream fs = File.OpenRead(fileName))
            {
                result = new byte[fs.Length];

                fs.Read(result, 0, result.Length);
            }

            return result;
        }

        /// <summary>
        /// Represents <see cref="Printer"/> instance as a string.
        /// </summary>
        /// <returns>
        /// <see cref="Printer"/> instance as a string.
        /// </returns>
        public override string ToString() => $"{Name} - {Model}";
    }
}