using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WiredBrainCoffee.EventHub.Model;

namespace WiredBrainCoffee.EventHub.Receiver.Direct
{
    public class WiredBrainCoffeeParitionReceiverHandler : IPartitionReceiveHandler
    {
        public WiredBrainCoffeeParitionReceiverHandler(string partitionId)
        {
            PartitionId = partitionId;
        }

        public string PartitionId { get; }
       

        int IPartitionReceiveHandler.MaxBatchSize { get => 10; set => throw new NotImplementedException(); }

        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");

            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(IEnumerable<EventData> eventDatas)
        {
            if (eventDatas != null)
            {
                foreach (var eventData in eventDatas)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    var coffeeMachineData = JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
                    Console.WriteLine($"{coffeeMachineData} | Partition: {PartitionId} | Offset: {eventData.SystemProperties.Offset}" );
                }
            }

            return Task.CompletedTask;
        }
    }
}
