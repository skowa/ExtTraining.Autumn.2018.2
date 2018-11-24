using System;
using System.IO;

namespace No8.Solution.Model
{
    /// <summary>
    /// The abstract class of printer
    /// </summary>
    public abstract class Printer : IEquatable<Printer>
    {
        /// <summary>
        /// The start of printing event.
        /// </summary>
        public event EventHandler<PrintingEventArgs> StartPrint = delegate { };

        /// <summary>
        /// The end of printing event.
        /// </summary>
        public event EventHandler<PrintingEventArgs> EndPrint = delegate { };

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
        /// Simulates printing.
        /// </summary>
        /// <param name="stream">
        /// The stream to be printed.
        /// </param>
        /// <param name="writer">
        /// The output stream.
        /// </param>
        public void Print(Stream stream, TextWriter writer)
        {
            if (stream == null)
            {
                throw new ArgumentNullException($"{nameof(stream)} is null");
            }

            if (writer == null)
            {
                throw new ArgumentNullException($"{nameof(writer)} is null");
            }

            OnStartPrint(new PrintingEventArgs(DateTime.Now, "Printing started"));
            ConcretePrint(stream, writer);
            OnEndPrint(new PrintingEventArgs(DateTime.Now, "Printing ended"));
        }
        
        /// <summary>
        /// Represents <see cref="Printer"/> instance as a string.
        /// </summary>
        /// <returns>
        /// <see cref="Printer"/> instance as a string.
        /// </returns>
        public override string ToString() => $"{Name} - {Model}";

        /// <summary>
        /// Defines whether two printers are equal.
        /// </summary>
        /// <param name="other">
        /// The printer to be compared to.
        /// </param>
        /// <returns>
        /// True, if printers are equal; otherwise false.
        /// </returns>
        public bool Equals(Printer other)
        {
            if (other == null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || string.Equals(Name, other.Name) && string.Equals(Model, other.Model);
        }

        /// <summary>
        /// Gets hashcode of the <see cref="Printer"/> instance.
        /// </summary>
        /// <returns>
        /// The <see cref="Printer"/> instance hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Model != null ? Model.GetHashCode() : 0);
            }
        }

        protected abstract void ConcretePrint(Stream stream, TextWriter writer);

        protected virtual void OnStartPrint(PrintingEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException($"{nameof(eventArgs)} is null");
            }

            StartPrint?.Invoke(this, eventArgs);
        }

        protected virtual void OnEndPrint(PrintingEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException($"{nameof(eventArgs)} is null");
            }

            EndPrint?.Invoke(this, eventArgs);
        }
    }

    public sealed class PrintingEventArgs : EventArgs
    {
        public PrintingEventArgs(DateTime time, string message)
        {
            Time = time;
            Message = message;
        }

        public DateTime Time { get; }

        public string Message { get; }
    }
}