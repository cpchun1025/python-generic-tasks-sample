using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundDaemon
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly HttpClient _httpClient;

        public MyBackgroundService()
        {
            // Load the custom certificate (e.g., root CA) from a .pem file
            var certFilePath = "/path/to/your/cert.pem";
            var cert = new X509Certificate2(File.ReadAllBytes(certFilePath));

            // Create an HttpClientHandler and set the custom certificate
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);

            // Optionally configure the handler to use custom server validation (if needed)
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                // Validate the server certificate against the provided CA
                return chain.Build(cert); // Validate using the provided cert chain
            };

            // Create the HttpClient with the configured handler
            _httpClient = new HttpClient(handler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Define the URL and data payload
                    var url = "https://example.com/api/data";
                    var data = new
                    {
                        key1 = "value1",
                        key2 = "value2"
                    };

                    // Serialize the data as JSON
                    var jsonData = JsonSerializer.Serialize(data);

                    // Create an HTTP content object
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Perform the POST request
                    var response = await _httpClient.PostAsync(url, content, stoppingToken);

                    // Check the response status
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Request succeeded: {await response.Content.ReadAsStringAsync()}");
                    }
                    else
                    {
                        Console.WriteLine($"Request failed: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                // Wait for a minute before sending the next request
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}