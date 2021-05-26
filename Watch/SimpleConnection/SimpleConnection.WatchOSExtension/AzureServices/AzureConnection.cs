using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace SimpleConnection.WatchOSExtension.AzureServices
{
    // Class for connections to Azur IoT Hub
    [Obsolete]
    public sealed class AzureConnection
    {
        // Declaration of Properties
        private static string AzIotHub { get; set; }
        private static string AzDeviceId { get; set; }
        private static string AzApiVersion { get; set; }
        private static string AzDeviceSas { get; set; }
        private static DateTime AzSasExpiry { get; set; }

        private AzureConnection()
        {
            AzIotHub = "ContosoTestHub4253";
            AzDeviceId = "EasyCare-Sender";
            AzApiVersion = "2019-10-01";
        }

        // Generate Single instance of AzureConnection
        public static AzureConnection Instance
        {
            get { return lazy.Value; }
        }

        private static readonly Lazy<AzureConnection> lazy = new Lazy<AzureConnection>(() => new AzureConnection());


        // Method to send a message to Azure IoT-Hub
        public string SendToIotHub(object body)
        {
            try
            {
                string restUriPost = $"https://{AzIotHub}.azure-devices.net/devices/{AzDeviceId}/messages/events?api-version={AzApiVersion}";
                string resourceUri = $"{AzIotHub}.azure-devices.net/devices/{AzDeviceId}";

                // Check if stored SAS-Token is still valid
                if (DateTime.UtcNow.CompareTo(AzSasExpiry) >= 0)
                {
                    // Generate a new one if necessary
                    AzDeviceSas = generateSasToken(resourceUri, "RGv0jCPcSinwZxGVWFt98p8IkMlYxzImB8eVdTMWBy8=", null);
                }

                // Create HttpClient 
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", AzDeviceSas);

                var content = JsonConvert.SerializeObject(body);
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                // Post message to Azure Client
                var resultPost = client.PostAsync(restUriPost, stringContent).Result;
                Console.WriteLine(resultPost.Content.ToString());

                // Result output for debugging
                Console.WriteLine($"Sent {resultPost}");
                // Return null when everytrhing is fine
                if (resultPost.StatusCode == HttpStatusCode.NoContent)
                {
                    return null;
                }
                else
                {
                    return resultPost.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                // Retrun exception as errror message
                Console.WriteLine(ex);
                return ex.Message;
            }
        }


        // Method to generate a SAS-Token for auhentication against Azure
        private string generateSasToken(string resourceUri, string key, string policyName, int expiryInSeconds = 3600)
        {
            AzSasExpiry = DateTime.UtcNow.AddSeconds(expiryInSeconds);

            TimeSpan fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string expiry = Convert.ToString((int)fromEpochStart.TotalSeconds + expiryInSeconds);

            string stringToSign = WebUtility.UrlEncode(resourceUri) + "\n" + expiry;

            HMACSHA256 hmac = new HMACSHA256(Convert.FromBase64String(key));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            string token = String.Format(CultureInfo.InvariantCulture, "sr={0}&sig={1}&se={2}", WebUtility.UrlEncode(resourceUri), WebUtility.UrlEncode(signature), expiry);

            if (!String.IsNullOrEmpty(policyName))
            {
                token += "&skn=" + policyName;
            }

            return token;
        }
    }

}
