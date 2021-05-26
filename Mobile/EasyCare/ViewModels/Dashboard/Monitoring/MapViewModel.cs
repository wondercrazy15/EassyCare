using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Dto;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    [Preserve(AllMembers = true)]
    public class MapViewModel : BaseViewModel
    {
        private Geocoder _geocoder = new Geocoder();

        public Map Map { get; set; }

        public MapViewModel(SeniorDto senior, double latitude, double longitude, DateTime date)
        {
            try
            {
                var position = new Position(longitude, latitude);

                var mapSpan = new MapSpan(position, 0.01, 0.01);
                mapSpan.WithZoom(1000);

                Map = new Map(mapSpan)
                {
                    MapType = MapType.Street,
                };

                var pin = new Pin
                {
                    Label = $"{senior.FirstName} {senior.SecondName} letzte position {(DateTime.UtcNow - date).TotalMinutes} Minuten",
                    Address =  GetAdressesForPositionAsync(longitude, latitude),
                    Type = PinType.Generic,
                    Position = position,
                };

                var circle = new Circle
                {
                    Center = position,
                    Radius = new Distance(250),
                    StrokeColor = Color.FromHex("#88FF0000"),
                    StrokeWidth = 8,
                    FillColor = Color.FromHex("#88FFC0CB")
                };

                Map.Pins.Add(pin);
                Map.MapElements.Add(circle);
                Map.MoveToRegion(mapSpan);
            }
            catch(Exception ex)
            {
                // If this is catched, most probably the Pin.Label was null -> this means we don't hav a Senior for now
                Debug.WriteLine(ex.Message);
            }
        }

        private string GetAdressesForPositionAsync(double longitude, double latitude)
        {
            return string.Empty;
        }
    }

}