using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace WiredBrainCoffee.EventHub.Receiver.Processor
{
    class Program
    {
        const string eventHubPath = "rb-wiredbraincoffeeeh";
        const string consumerGroupName = "wired_brain_coffee_console_processor";
        const string eventHubConnectionString = "Endpoint=sb://rb-wiredbraincoffeeehns.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=Y+KiJxZKklT/W7TlxTXxFt3Uu3CQ8/p31KlCf2t509g=;EntityPath=rb-wiredbraincoffeeeh";
        const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=rbwiredbraincoffeesa;AccountKey=oluzy+UXO1C98J7VTmLijWHSNTv4mIb4nrO8cmgmZMKiYBAHhf7A6nbhRXHZDQo/pmhMjvFZlFH3jbXSi9ij6A==;EndpointSuffix=core.windows.net";
        const string leaseContainerName = "processorcheckpoint";

        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var eventProcessorHost = new EventProcessorHost(eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString, leaseContainerName);
            await eventProcessorHost.RegisterEventProcessorAsync<WiredBrainCoffeeEventProcessorHost>();

            Console.WriteLine("Waiting for incoming events....");
            Console.WriteLine("Press any key to shutdown.");

            Console.ReadLine();

            await eventProcessorHost.UnregisterEventProcessorAsync();

        }
    }
}
