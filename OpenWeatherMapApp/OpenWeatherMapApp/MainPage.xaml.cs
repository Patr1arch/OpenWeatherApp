using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenWeatherMapApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Dictionary<string, double> tempTypes = new Dictionary<string, double> { { "Kelvin", 273.15 } };
        const string appid = "&appid=99ec7978059875fb951bd6126f07cb66";
        public MainPage()
        {
            InitializeComponent();
            main.BackgroundImageSource = ImageSource.FromUri(
                new Uri("https://images.unsplash.com/photo-1515694346937-94d85e41e6f0?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&w=1000&q=80"));
            //var menu = new ToolbarItem
            //{
            //    IconImageSource = "https://f0.pngfuel.com/png/958/251/document-sheet-icon-illustration-png-clip-art-thumbnail.png",
            //    Order = ToolbarItemOrder.Primary,
            //    Priority = 1
            //};
            //this.ToolbarItems.Add(menu);
            HttpWebRequest webRequest = 
                (HttpWebRequest)WebRequest.Create("https://openweathermap.org/data/2.5/weather?id=2172797" + appid);
            //var response = webRequest.GetResponse();
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var probablyJson = JObject.Parse(reader.ReadToEnd());
                ShowWeather(probablyJson);
                reader.Close();
                stream.Close();
                response.Close();
            }
        }

        private void ShowWeather(JObject json)
        {
            info.Children.Clear();
            Label tempLabel = new Label { FontAttributes = FontAttributes.Bold, TextColor = Color.White,
                FontSize = 80, HorizontalOptions = LayoutOptions.Center, Opacity = 228 };
            var hm = Double.Parse((string)json["main"]["temp"], CultureInfo.InvariantCulture);
            hm = hm - tempTypes["Kelvin"];
            tempLabel.Text = (hm).ToString() + "°C";
            info.Children.Add(tempLabel);

            StackLayout weatherStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center, HeightRequest = 30 };

            Label weatherLabel = new Label { Text = json["weather"][0]["main"].ToString(), FontSize = 25, TextColor = Color.White };
             weatherStackLayout.Children.Add(weatherLabel);

            Image weathericon = new Image { Source = ImageSource.FromUri(
                new Uri("http://openweathermap.org/img/wn/" + json["weather"][0]["icon"].ToString() + "@2x.png"))};
            weatherStackLayout.Children.Add(weathericon);
            //Image windmill = new Image { Source = ImageSource.FromUri(
            //    new Uri("https://i.pinimg.com/originals/f2/50/95/f250958979ba717571bf069ea43f053c.gif")) };
            //info.Children.Add(windmill);
            info.Children.Add(weatherStackLayout);

            StackLayout windAndHumidity = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 250,
                BackgroundColor = Color.Blue
            };


            info.Children.Add(windAndHumidity);
        } 
    }
}
