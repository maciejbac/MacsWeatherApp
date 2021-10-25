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

    // Declaring classes for the postcode data json and the nested result json. The data required (longitude and latitude) are within the nested "Result" JSON. 
    public class LocationJson
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

        // Handle button click
        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            GenerateImage(longitude_input.Text, latitude_input.Text);
        }

        // Generate image with weather data for a given location
        private void GenerateImage(string longitude, string latitude)
        {
            // Pre-defined api call
            string fullFilePath = @"http://www.7timer.info/bin/astro.php?lon=" + longitude + "&lat=" + latitude + "&ac=0&lang=en&unit=metric&output=internal&tzshift=0";

            // Save returned image into a bitmap
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            // Display the image in the window
            ImageViewer1.Source = bitmap;
        }

        // Pre-defined London coordinates for testing
        private void BtnLondonAutofill_Click(object sender, RoutedEventArgs e)
        {
            longitude_input.Text = "-0.1";
            latitude_input.Text = "51.5";
        }

        // Clear all variables
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            longitude_input.Text = "";
            latitude_input.Text = "";
            postcode_input.Text = "";
            ImageViewer1.Source = null;
        }

        // Handle postcode information and send an API call to obtain location information
        async void ButtonPostcode_Click(object sender, RoutedEventArgs e)
        {
            // Pre-defined api call to the postcode API using the postcode data
            string urlRequest = "http://api.postcodes.io/postcodes/" + postcode_input.Text.ToString();
            // send the API call and save the returned JSON as a variable
            HttpResponseMessage postcodeJson = await client.GetAsync(urlRequest);
            // Deserialise the JSON into the LocationJson object so I can access the variables
            LocationJson location = JsonSerializer.Deserialize<LocationJson>(await postcodeJson.Content.ReadAsStringAsync());
            // Generate the image using location data from the location object
            GenerateImage(location.result.longitude.ToString(), location.result.latitude.ToString());
        }
    }
}


