using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using OpenGTINScanner.Helpers;
using System.Net;
using System.Xml;

namespace OpenGTINScanner
{
    public partial class MainPage : ContentPage
    {
        string code_found = null;
        bool request_sent = false;
        
        public MainPage()
        {
            InitializeComponent();

            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false
            };
        }

        // if - barcode detected => update label
        protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (!request_sent)
            {
                code_found = e.Results[0].Value.ToString();
                Dispatcher.Dispatch(() => BarcodeLabel.Text = $"Folgender Code wurde erkannt: {code_found}");

                // Anfrage senden
                string opengtin_response = OpenGTINHelper.SendHttpRequest(code_found).Result;
                request_sent = true;

                // if - response ist nicht null?
                if (opengtin_response != null)
                {
                    // Teile die Antwort anhand der Trennzeichen auf
                    string[] sections = opengtin_response.Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries);

                    // Überprüfe den Error-Check
                    string errorSection = sections.Length > 0 ? sections[0] : string.Empty;
                    if (errorSection.Contains("error=0"))
                    {

                        // Extrahiere die Werte von "name" und "detailname"
                        string productSection = sections.Length > 1 ? sections[1] : string.Empty;
                        string[] attributes = productSection.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        string name = GetValue(attributes, "name=");
                        string detailName = GetValue(attributes, "detailname=");


                        Dispatcher.Dispatch(() => KategorieLabel.Text += name);
                        Dispatcher.Dispatch(() => BezeichnungLabel.Text += detailName);
                    }
                }
            }  
        }

        private string GetValue(string[] attributes, string key)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.StartsWith(key))
                {
                    return attribute.Substring(key.Length);
                }
            }
            return string.Empty;
        }
    }
}
