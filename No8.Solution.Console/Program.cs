using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using No8.Solution.Model;
using No8.Solution.Repositories;

namespace No8.Solution.Console
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        public static void Main(string[] args)
        {
            PrinterManager manager = new PrinterManager(new ListPrinterRepository());
            manager.Printing += Print;

            while (true)
            {
                Menu();
                if (int.TryParse(System.Console.ReadLine(), out int key))
                {
                    if (key == 1)
                    {
                        CreatePrinter(manager);
                        System.Console.Clear();
                    }
                    else if (key > 1 && key <= Enum.GetValues(typeof(PrinterType)).Length + 1)
                    {
                        PrinterType type = (PrinterType)key - 2;
                        Print(type, manager);
                        System.Console.Clear();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            manager.Printing -= Print;
        }

        private static void Menu()
        {
            System.Console.WriteLine("Select your choice:");
            System.Console.WriteLine("1:Add new printer");
            int count = 2;
            foreach (var type in Enum.GetValues(typeof(PrinterType)))
            {
                System.Console.WriteLine($"{count++}:Print on {type}");
            }
            System.Console.WriteLine($"{count}:Exit");
        }

        private static void Print(PrinterType type, PrinterManager manager)
        {
            System.Console.Clear();
            List<Printer> printers = manager.GetPrintersAccordingToTypeList(type);
            int index = 0;
            if (printers.Count != 0)
            {
                foreach (var printer in printers)
                {
                    System.Console.WriteLine($"{index++}: {printer}");
                }

                if (int.TryParse(System.Console.ReadLine(), out index))
                {
                    DefineInWhatToPrintAndPrint(manager, index, printers);
                }
                else
                {
                    System.Console.WriteLine("Error! Press smth to continue");
                }
            }
            else
            {
                System.Console.WriteLine("No printers! Press smth to continue");
                System.Console.ReadLine();
            }
        }

        private static void CreatePrinter(PrinterManager manager)
        {
            System.Console.Clear();
            System.Console.WriteLine("Select your choice:");
            foreach (var type in Enum.GetValues(typeof(PrinterType)))
            {
                System.Console.WriteLine($"{(int)type} - {type}");
            }

            if (Enum.TryParse(System.Console.ReadLine(), out PrinterType choice))
            {
                string name = choice.ToString();
                if (choice == PrinterType.Unknown)
                {
                    System.Console.WriteLine("Enter printer name");
                    name = System.Console.ReadLine();
                }

                System.Console.WriteLine("Enter printer model");
                string model = System.Console.ReadLine();

                try
                {
                    manager.AddNewPrinter(choice, name, model);
                    Logger.Trace($"New printer is added {name}-{model}");
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error(e.StackTrace);
                    System.Console.WriteLine(e);
                    System.Console.WriteLine("Press smth to continue");
                    System.Console.ReadLine();
                }
                catch (ArgumentException e)
                {
                    System.Console.WriteLine(e);
                    Logger.Error(e.StackTrace);
                    System.Console.WriteLine("Press smth to continue");
                    System.Console.ReadLine();
                }
            }
        }

        private static void Print(object sender, PrintingEventArgs eventArgs)
        {
            Logger.Trace($"{eventArgs.Message} at {eventArgs.Time}");
        }

        private static void DefineInWhatToPrintAndPrint(PrinterManager manager, int index, List<Printer> printers)
        {
            if (index < printers.Count && index >= 0)
            {
                try
                {
                    foreach (var b in manager.Print(printers[index]))
                    {
                        System.Console.WriteLine(b);
                    }
                }
                catch (FileNotFoundException e)
                {
                    System.Console.WriteLine(e);
                    Logger.Error(e.StackTrace);
                }


                System.Console.ReadLine();
            }
        }
    }
}
