using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Threading.Tasks;

public class EmailSender
{
    private static async Task SendEmailWithAttachment(string filePath, string recipientEmail)
    {
        var clientId = "your-client-id";
        var tenantId = "your-tenant-id";
        var clientSecret = "your-client-secret";

        var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithTenantId(tenantId)
            .WithClientSecret(clientSecret)
            .Build();

        var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
        {
            var authResult = await confidentialClient.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }));

        var attachmentContent = File.ReadAllBytes(filePath);

        var email = new Message
        {
            Subject = "Processed Excel File",
            Body = new ItemBody
            {
                ContentType = BodyType.Text,
                Content = "Please find the attached processed Excel file."
            },
            ToRecipients = new[]
            {
                new Recipient { EmailAddress = new EmailAddress { Address = recipientEmail } }
            },
            Attachments = new MessageAttachmentsCollectionPage
            {
                new FileAttachment
                {
                    ODataType = "#microsoft.graph.fileAttachment",
                    Name = Path.GetFileName(filePath),
                    ContentBytes = attachmentContent,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                }
            }
        };

        await graphClient.Me.SendMail(email, null).Request().PostAsync();
    }
}