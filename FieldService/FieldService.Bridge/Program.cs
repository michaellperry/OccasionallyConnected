using FieldService.Bridge.Scanning;
using System;
using System.IO;

namespace FieldService.Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FieldService",
                "Scanner");
            Directory.CreateDirectory(queueFolderPath);
            string messageFile = Path.Combine(queueFolderPath,
                "messageQueue.json");
            if (!File.Exists(messageFile))
                File.WriteAllText(messageFile, string.Empty);
            Uri distributorUri = new Uri("http://localhost:20624/",
                UriKind.Absolute);

            var scanner = new FieldServiceScanner(
                queueFolderPath, distributorUri);

            scanner.Start();
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            scanner.Stop();
        }
    }
}
