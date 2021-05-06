using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SignRequest.Api;
using SignRequest.Client;
using SignRequest.Model;

namespace SignrequestTest
{
    class Program
    {
        static async Task Main()
        {
            await SignrequestTest();
            await Base64Test();

            Console.ReadLine();
        }

        private static async Task SignrequestTest()
        {
            var file = await File.ReadAllBytesAsync("./demo_document.pdf");
            var pdfBase64 = Convert.ToBase64String(file);

            var api = new SignrequestQuickCreateApi
            {
                Configuration = { BasePath = "https://{your_team}.signrequest.com/api/v1" }
            };

            api.Configuration.AddApiKey("Authorization", "YOUR_TOKEN_HERE");
            api.Configuration.AddApiKeyPrefix("Authorization", "Token");

            var data = new SignRequestQuickCreate(
                signers: new List<Signer> { new("email@provider.com") },
                fromEmail: "email+1@provider.com",
                fileFromContent: pdfBase64,
                fileFromContentName: "demo_document.pdf",
                who: SignRequestQuickCreate.WhoEnum.O);

            try
            {
                var result = await api.SignrequestQuickCreateCreateAsync(data);
                Console.WriteLine("Success!");
                Console.WriteLine(result);
                Environment.Exit(0);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task Base64Test()
        {
            var file = await File.ReadAllBytesAsync("./demo_document.pdf");
            var pdfBase64 = Convert.ToBase64String(file);

            var pdfBytes = Convert.FromBase64String(pdfBase64);
            await File.WriteAllBytesAsync("./demo_document_dupe.pdf", pdfBytes);
        }
    }
}
