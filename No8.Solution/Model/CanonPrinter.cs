using System;
using System.IO;

namespace No8.Solution.Model
{
    internal class CanonPrinter : Printer
    {
        public CanonPrinter(string model) : base("Canon", model) { }

        protected override void ConcretePrint(Stream stream, TextWriter writer)
        {
            using (stream)
            using (writer)
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                
                foreach (var @byte in bytes)
                {
                    writer.WriteLine(@byte);
                }
            }
        }
    }
}