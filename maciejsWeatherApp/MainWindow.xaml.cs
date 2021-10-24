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

namespace maciejsWeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {

            var image = new Image();

            string lon = longitude.Text;
            string lat = latitude.Text;

            var fullFilePath = @"http://www.7timer.info/bin/astro.php?lon="+lon+"&lat="+lat+"&ac=0&lang=en&unit=metric&output=internal&tzshift=0";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            ImageViewer1.Source = bitmap;
        }

        private void btnCardiffAutofill_Click(object sender, RoutedEventArgs e)
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
            string responseBody = await postcodeJson.Content.ReadAsStringAsync();

            //todo need to deserialise the json object to pull the location values

            postcodeOutput.Content = "hello world";
        }
    }
}


