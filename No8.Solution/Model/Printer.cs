using System.IO;

namespace No8.Solution.Model
{
    public abstract class Printer
    {
        public Printer(string name, string model)
        {
            Name = name;
            Model = model;
        }

        public string Name { get; protected set; }

        public string Model { get; protected set; }

        public PrinterType  Type { get; protected set; }

        public virtual byte[] Print(string fileName)
        {
            byte[] result;

            using (FileStream fs = File.OpenRead(fileName))
            {
                result = new byte[fs.Length];

                fs.Read(result, 0, result.Length);
            }

            return result;
        }

        public override string ToString() => $"{Name} - {Model}";
    }
}