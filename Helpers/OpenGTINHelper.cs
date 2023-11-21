using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGTINScanner.Helpers
{
    internal class OpenGTINHelper
    {
        public static async Task<string> SendHttpRequest(string ean)
        {
            try
            {
                // Die URL mit den ersetzen Parametern
                string url = $"http://opengtindb.org/?ean={ean}&cmd=query&queryid={400000000}";

                // HttpClient erstellen
                using (HttpClient client = new HttpClient())
                {
                    // HTTP-Request senden
                    HttpResponseMessage response = await client.GetAsync(url);

                    string result = await response.Content.ReadAsStringAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                // Hier kannst du mit Ausnahmen umgehen, die während des HTTP-Requests auftreten können
                Console.WriteLine($"Fehler: {ex.Message}");

                return null;
            }
        }
    }
}
