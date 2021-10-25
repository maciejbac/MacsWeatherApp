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
    public class WeatherJson
    {
        public int Status { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            GenerateImage(longitude_input.Text, latitude_input.Text);

        }

        private void GenerateImage(string longitude, string latitude)
        {
            string fullFilePath = @"http://www.7timer.info/bin/astro.php?lon=" + longitude + "&lat=" + latitude + "&ac=0&lang=en&unit=metric&output=internal&tzshift=0";

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            ImageViewer1.Source = bitmap;
        }

        private void BtnLondonAutofill_Click(object sender, RoutedEventArgs e)
        {
            longitude_input.Text = "-0.1";
            latitude_input.Text = "51.5";
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            longitude_input.Text = "";
            latitude_input.Text = "";
            postcode_input.Text = "";
            ImageViewer1.Source = null;
        }

        async void ButtonPostcode_Click(object sender, RoutedEventArgs e)
        {
            string urlRequest = "http://api.postcodes.io/postcodes/" + postcode_input.Text.ToString();
            HttpResponseMessage postcodeJson = await client.GetAsync(urlRequest);
            postcodeJson.EnsureSuccessStatusCode();

            WeatherJson weather = JsonSerializer.Deserialize<WeatherJson>(await postcodeJson.Content.ReadAsStringAsync());

            //if(weather.Status == 200)
            //{
            GenerateImage(weather.result.longitude.ToString(), weather.result.latitude.ToString());
            //}
        }
    }
}


