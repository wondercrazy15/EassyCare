using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;

namespace EasyCare.Models
{
    public class AzConnection
    {
        // Event Hub-compatible endpoint
        private const string EventHubsCompatibleEndpoint = "sb://iothub-ns-contosotes-3598466-da57701095.servicebus.windows.net/"; //-- ContosoHub

        // Event Hub-compatible name
        private const string EventHubName = "contosotesthub4253"; // --ContosoHub

        // az iot hub policy show --name service --query primaryKey --hub-name {your IoT Hub name}
        private const string IotHubSasKeyName = "service";
        private const string IotHubSasKey = "QNLUqkannaWJiJLKRL+pu152ZZnvz1dJtQvjWAsZWhM="; // -- ContosoHub

        // Asynchronously create a PartitionRMeceiver for a partition and then start
        // reading any messages sent from the simulated client.
        private static async Task ReceiveMessagesFromDeviceAsync(CancellationToken cancellationToken)
        {
            // If you chose to copy the "Event Hub-compatible endpoint" from the "Built-in endpoints" section
            // of your IoT Hub instance in the Azure portal, you can set the connection string to that value
            // directly and remove the call to "BuildEventHubsConnectionString".
            string connectionString = BuildEventHubsConnectionString(EventHubsCompatibleEndpoint, IotHubSasKeyName, IotHubSasKey);

            // Create the consumer using the default consumer group using a direct connection to the service.
            // Information on using the client with a proxy can be found in the README for this quick start, here:
            //   https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/master/iot-hub/Quickstarts/read-d2c-messages/README.md#websocket-and-proxy-support
            //
            await using EventHubConsumerClient consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, connectionString, EventHubName);

            Console.WriteLine("Listening for messages on all partitions");
            try
            {
                // Begin reading events for all partitions, starting with the first event in each partition and waiting indefinitely for
                // events to become available.  Reading can be canceled by breaking out of the loop when an event is processed or by
                // signaling the cancellation token.
                //
                // The "ReadEventsAsync" method on the consumer is a good starting point for consuming events for prototypes
                // and samples.  For real-world production scenarios, it is strongly recommended that you consider using the
                // "EventProcessorClient" from the "Azure.Messaging.EventHubs.Processor" package.
                //
                // More information on the "EventProcessorClient" and its benefits can be found here:
                //  https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/eventhub/Azure.Messaging.EventHubs.Processor/README.md
                //
                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationToken))
                {
                    DateTime messageTime = new DateTime();
                    DateTime lastTimeStamp = new DateTime();
                    Console.WriteLine("Message received on partition {0}:", partitionEvent.Partition.PartitionId);
                    bool time = partitionEvent.Data.SystemProperties.TryGetValue("iothub-enqueuedtime", out object date);
                    messageTime = (DateTime)date;
                    // Filter for messages from last hour
                    if(messageTime.Hour >= (DateTime.Now.ToUniversalTime().Hour) && messageTime.Day == DateTime.Now.ToUniversalTime().Day && lastTimeStamp != messageTime)
                    {
                        string data = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                        Console.WriteLine("\t{0}:", data);

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        };
                        var pushMessage = new PushMessage();
                        pushMessage = JsonSerializer.Deserialize<PushMessage>(data, options);

                        if (time)
                        {
                            pushMessage.Date = (DateTime)messageTime;
                        }

                        //await App.Database.SavePushMessageAsync(pushMessage);

                        Console.WriteLine("Application properties (set by device):");
                        foreach (var prop in partitionEvent.Data.Properties)
                        {
                            Console.WriteLine("\t{0}: {1}", prop.Key, prop.Value);
                        }

                        Console.WriteLine("System properties (set by IoT Hub):");
                        foreach (var prop in partitionEvent.Data.SystemProperties)
                        {
                            Console.WriteLine("\t{0}: {1}", prop.Key, prop.Value);
                        }
                        lastTimeStamp = messageTime;
                    }

                }
            }
            catch (TaskCanceledException ex)
            {
                // This is expected when the token is signaled; it should not be considered an
                // error in this scenario.
                Console.WriteLine("!!!!" + ex);
            }
        }

        public static async Task AzConnect()
        {
            Console.WriteLine("IoT Hub - Read device to cloud messages.");
            using var cancellationSource = new CancellationTokenSource();

            await ReceiveMessagesFromDeviceAsync(cancellationSource.Token);
        }

        private static string BuildEventHubsConnectionString(string eventHubsEndpoint,
                                                             string iotHubSharedKeyName,
                                                             string iotHubSharedKey) =>
            $"Endpoint={ eventHubsEndpoint };SharedAccessKeyName={ iotHubSharedKeyName };SharedAccessKey={ iotHubSharedKey }";

    }
}

