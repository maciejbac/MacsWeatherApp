using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace maciejsWeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class weatherJson
    {
        public int status { get; set; }
        public result result { get; set; }
    }

    public class result
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            generateImage(longitude.Text, latitude.Text);

        }

        private void generateImage(string longitude, string latitude)
        {
            var fullFilePath = @"http://www.7timer.info/bin/astro.php?lon=" + longitude + "&lat=" + latitude + "&ac=0&lang=en&unit=metric&output=internal&tzshift=0";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            ImageViewer1.Source = bitmap;
        }

        private void btnLondonAutofill_Click(object sender, RoutedEventArgs e)
        {
            longitude.Text = "-0.1";
            latitude.Text = "51.5";
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            longitude.Text = "";
            latitude.Text = "";
            ImageViewer1.Source = null;
            postcodeOutput.Content = null;
        }

        async void ButtonPostcode_Click(object sender, RoutedEventArgs e)
        {
            var urlRequest = "http://api.postcodes.io/postcodes/" + postcode.Text.ToString();
            HttpResponseMessage postcodeJson = await client.GetAsync(urlRequest);
            postcodeJson.EnsureSuccessStatusCode();

            weatherJson weather = JsonSerializer.Deserialize<weatherJson>(await postcodeJson.Content.ReadAsStringAsync(););

            generateImage(weather.result.longitude.ToString(), weather.result.latitude.ToString());
        }
    }
}


