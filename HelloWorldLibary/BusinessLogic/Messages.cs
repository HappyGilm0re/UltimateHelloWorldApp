using HelloWorldLibary.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace HelloWorldLibary.BusinessLogic
{
    public class Messages
    {
        private readonly ILogger<Messages> _log;

        public Messages(ILogger<Messages> log)
        {
            _log = log;
        }

        public string Greeting(string language)
        {
            string output = LookUpCustomText("Greeting", language);
            return output;
        }

        private string LookUpCustomText(string key, string language)
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                List<CustomText>? messageSets = JsonSerializer
               .Deserialize<List<CustomText>>
               (
                   File.ReadAllText("CustomText.json"), options
               );

                CustomText? messages = messageSets?.Where(x => x.Language == language).First();

                if (messages is null)
                {
                    throw new NullReferenceException("The Specified language was not found in AppSettings.json.");
                }

                return messages.Tranlations[key];
            }
            catch(Exception ex)
            {
                _log.LogError("Error: Looking up the customer text", ex);
                throw;
            }
           


        }
    }
}
