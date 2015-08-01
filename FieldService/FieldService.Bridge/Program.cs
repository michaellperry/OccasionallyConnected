using FieldService.Models;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string messageFile = Path.Combine(queueFolderPath, "messageQueue.json");
            if (!File.Exists(messageFile))
                File.WriteAllText(messageFile, string.Empty);
            Uri distributorUri = new Uri("http://fieldservicedistributor.azurewebsites.net/api/distributor/", UriKind.Absolute);
            var scanner = new Scanner(queueFolderPath, distributorUri);

            scanner.Start();
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            scanner.Stop();
        }
    }
}
