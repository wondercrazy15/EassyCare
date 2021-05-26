using System;
using System.Diagnostics;
using System.Linq;
using CoreLocation;
using Foundation;

namespace SimpleConnection.WatchOSExtension.Services
{
    public class LocationService : CLLocationManagerDelegate
    {
        private CLLocationManager _locationManager;
        private CLLocation _location;

        protected LocationService(NSObjectFlag t) : base(t) { }

        protected internal LocationService(IntPtr handle) : base(handle) { }

        public LocationService()
        {
            _locationManager = new CLLocationManager();
            _locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;

            _locationManager.Delegate = this;

            StartLocationUpdates();
        }

        public void RequestAuthorizationToShare()
        {
            _locationManager.RequestWhenInUseAuthorization();
        }

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                _locationManager.StartUpdatingLocation();
                _locationManager.RequestLocation();

                Debug.WriteLine(string.Format("RequestLocation() Data:D {0}", DateTime.UtcNow));
            }
        }

        public override void Failed(CLLocationManager manager, NSError error)
        {
            Debug.WriteLine(string.Format("Failed() Data:D {0} Error {1}", DateTime.UtcNow, error.Description));
        }

        public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
        {
            Debug.WriteLine(string.Format("LocationsUpdated() Data:D {0}", DateTime.UtcNow));
            _location = locations.LastOrDefault();
        }

        public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
        {
            Debug.WriteLine(string.Format("Authorization changed() Data: {0}", status));
        }

        public override string ToString()
        {
            return _location is null ? String.Empty : $"{_location.Coordinate.Latitude},{_location.Coordinate.Longitude}";
        }
    }
}
