using FieldService.Bridge.Mapping;
using FieldService.Bridge.Queueing;
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
            Uri distributorUri = new Uri("http://fieldservicedistributor.azurewebsites.net/api/distributor/",
                UriKind.Absolute);
            Uri identityUri = new Uri("http://fieldservicedistributor.azurewebsites.net/api/technicianIdentifier/",
                UriKind.Absolute);

            var messageIdMap = new MessageIdMap();
            var scanner = new FieldServiceScanner(
                queueFolderPath, distributorUri, identityUri, messageIdMap);
            var subscriber = new FieldServiceSubscriber(messageIdMap);

            scanner.Start();
            subscriber.Start();
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            scanner.Stop();
            subscriber.Stop();
        }
    }
}
