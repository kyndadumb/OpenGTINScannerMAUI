using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using OpenGTINScanner.Helpers;
using System.Net;

namespace OpenGTINScanner
{
    public partial class MainPage : ContentPage
    {
        string code_found = null;
        
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
            code_found = e.Results[0].Value.ToString();
            Dispatcher.Dispatch(() => BarcodeLabel.Text = $"Folgender Code wurde erkannt: {code_found}");

            // Anfrage senden
            OpenGTINHelper.SendHttpRequest(code_found);
        }
    }
}
