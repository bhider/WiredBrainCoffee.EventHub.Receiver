using Microsoft.Azure.EventHubs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WiredBrainCoffee.EventHub.Receiver.Direct
{
    class Program
    {
        const string EventHubConnectionString = "Endpoint=sb://rb-wiredbraincoffeeehns.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=Y+KiJxZKklT/W7TlxTXxFt3Uu3CQ8/p31KlCf2t509g=;EntityPath=rb-wiredbraincoffeeeh";
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Connecting to Event Hub.....");
            var eventHubClient = EventHubClient.CreateFromConnectionString(EventHubConnectionString);
            Console.WriteLine("Waiting to process incoming messages....");
            var eventRuntimeInformation = await eventHubClient.GetRuntimeInformationAsync();
            //var partitionReceivers = eventRuntimeInformation.PartitionIds.Select(partitionId => eventHubClient.CreateReceiver("$Default",partitionId, EventPosition.FromEnqueuedTime(DateTime.Now))).ToList();
            var partitionReceivers = eventRuntimeInformation.PartitionIds.Select(partitionId => eventHubClient.CreateReceiver("wired_brain_coffee_console_direct", partitionId, EventPosition.FromOffset(partitionId == "0" ? "24384" : "23800", true))).ToList();

            foreach (var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(new WiredBrainCoffeeParitionReceiverHandler(partitionReceiver.PartitionId));
            }

            
            

            Console.WriteLine("Press any key to shutdown....");

            Console.ReadLine();

            await eventHubClient.CloseAsync();
        }


    }
}
