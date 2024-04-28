using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Wordle
{
    internal class RandomWordGenerator : WordGenerator
    {
        static string apiKey = Environment.GetEnvironmentVariable("API_KEY");
        static string url = $"https://api.wordnik.com/v4/words.json/randomWord?hasDictionaryDef=true&minCorpusCount=0&minLength=5&maxLength=5&api_key={apiKey}";
        string WordGenerator.GenerateWord()
        {
            return GetWord().Result;
        }

        public static async Task<string> GetWord()
        {
            using (HttpClient client = new HttpClient())
            {
                // replace YOUR_API_KEY with your own key

                var response = await client.GetStringAsync(url);
                var word = JObject.Parse(response).SelectToken("word").ToString();
                return word;
            }
        }
    }
}
