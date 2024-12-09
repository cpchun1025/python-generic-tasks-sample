public class ApiRequest
{
    public string Endpoint { get; set; }
    public string Payload { get; set; }
}

public class ApiResponse
{
    public string Endpoint { get; set; }
    public string ResponseBody { get; set; }
    public DateTime Timestamp { get; set; }
}

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        string apiUrl = "https://api.example.com/endpoint";
        var requestPayload = new { key1 = "value1", key2 = "value2" }; // JSON payload

        // Serialize request payload
        string requestJson = JsonSerializer.Serialize(requestPayload);

        // Call the API
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(apiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json"));

        // Get response string
        string responseJson = await response.Content.ReadAsStringAsync();

        // Save to database and file
        SaveToDatabase(apiUrl, requestJson, responseJson);
        SaveToFile(apiUrl, requestJson, responseJson);
    }

    private static void SaveToDatabase(string endpoint, string requestJson, string responseJson)
    {
        string connectionString = "YourConnectionStringHere"; // Replace with your database connection

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                INSERT INTO ApiLogs (Endpoint, RequestJson, ResponseJson, Timestamp)
                VALUES (@Endpoint, @RequestJson, @ResponseJson, @Timestamp)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Endpoint", endpoint);
                command.Parameters.AddWithValue("@RequestJson", requestJson);
                command.Parameters.AddWithValue("@ResponseJson", responseJson);
                command.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void SaveToFile(string endpoint, string requestJson, string responseJson)
    {
        string logDir = "C:\\path\\to\\logs";
        Directory.CreateDirectory(logDir);

        string logFile = Path.Combine(logDir, $"{DateTime.Now:yyyyMMdd_HHmmss}_log.txt");

        File.WriteAllText(logFile, $"Endpoint: {endpoint}\nRequest: {requestJson}\nResponse: {responseJson}");
    }
}