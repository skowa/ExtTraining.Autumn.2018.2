using System.IO;

namespace No8.Solution.Model
{
    internal class EpsonPrinter : Printer
    {
        public EpsonPrinter(string model) : base("Epson", model) { }

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