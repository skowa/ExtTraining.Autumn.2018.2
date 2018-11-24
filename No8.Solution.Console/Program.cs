using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLog;
using No8.Solution.Model;

namespace No8.Solution.Console
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            PrinterManager manager = new PrinterManager(LogManager.GetCurrentClassLogger());

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
                    else if (key == 2 || key == 3)
                    {
                        string name = key == 2 ? "Epson" : "Canon";
                        Print(manager, name);
                        System.Console.Clear();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            manager.UnregisterLogger();
        }

        private static void Menu()
        {
            System.Console.WriteLine("Select your choice:");
            System.Console.WriteLine("1:Add new printer");
            System.Console.WriteLine("2:Print on Epson");
            System.Console.WriteLine("3:Print on Canon");
            System.Console.WriteLine("4:Exit");
        }

        private static void Print(PrinterManager manager, string name)
        {
            System.Console.Clear();
            List<Printer> printers = manager.GetPrintersAccordingToType(name);
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
            System.Console.WriteLine("1 - Epson");
            System.Console.WriteLine("2 - Canon");

            if (int.TryParse(System.Console.ReadLine(), out int choice))
            {
                if (choice == 1 || choice == 2)
                {
                    System.Console.WriteLine("Enter printer model");
                    string model = System.Console.ReadLine();

                    string name = choice == 1 ? "Epson" : "Canon";
                    try
                    {
                        manager.AddNewPrinter(name, model);
                    }
                    catch (ArgumentException e)
                    {
                        manager.Logger.Error($"{e.Message} - {e.StackTrace}");
                        System.Console.WriteLine(e);
                        System.Console.WriteLine("Press smth to continue");
                        System.Console.ReadLine();
                    }
                    catch (InvalidOperationException e)
                    {
                        manager.Logger.Error($"{e.Message} - {e.StackTrace}");
                        System.Console.WriteLine(e);
                        System.Console.WriteLine("Press smth to continue");
                        System.Console.ReadLine();
                    }
                }
            }
        }
        
        private static void DefineInWhatToPrintAndPrint(PrinterManager manager, int index, List<Printer> printers)
        {
            if (index < printers.Count && index >= 0)
            {
                try
                {
                    var o = new OpenFileDialog();
                    o.ShowDialog();

                    TextWriter printTextWriter = System.Console.Out;
                    manager.Print(printers[index], o.FileName, printTextWriter);
                }
                catch (FileNotFoundException e)
                {
                    manager.Logger.Error($"{e.Message} - {e.StackTrace}");
                    System.Console.WriteLine(e.StackTrace);
                }
                catch (ArgumentException e)
                {
                    manager.Logger.Error($"{e.Message} - {e.StackTrace}");
                    System.Console.WriteLine(e.StackTrace);
                }

                System.Console.ReadLine();
            }
        }
    }
}
