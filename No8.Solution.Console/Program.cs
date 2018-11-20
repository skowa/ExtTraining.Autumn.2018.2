using System;
using System.Collections.Generic;
using NLog;
using No8.Solution.Model;
using No8.Solution.Repositories;

namespace No8.Solution.Console
{
    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        public static void Main(string[] args)
        {
            int key;

            bool flag = false;

            PrinterManager manager = new PrinterManager(new ListPrinterRepository());
            while(true)
            {
                Menu();
                if (int.TryParse(System.Console.ReadLine(), out key))
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
                    if (index < printers.Count && index >= 0)
                    {
                        foreach (var b in manager.Print(printers[index]))
                        {
                            System.Console.WriteLine(b);
                        }

                        System.Console.ReadLine();
                    }
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
                string name = string.Empty;
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
                }
                catch (InvalidOperationException e)
                {
                    Logger.Error(e.StackTrace);
                    System.Console.WriteLine(e);
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
    }
}
