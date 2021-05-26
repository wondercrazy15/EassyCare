using System;
using CoreMotion;
using Foundation;
using HealthKit;
using Xamarin.Essentials;

namespace SimpleConnection.WatchOSExtension.Services
{
    public interface ISensorService
    {
        double ReadHeartRate();
        int ReadBatteryLevel();
        bool BatteryIsCharge();
        int ReadStepCount();
        string ReadLocation();
        void ValidateAuthorization();
    }

    public class SensorService : ISensorService
    {
        private double _bmp;
        private nint _steps = 0;
        private HKHealthStore _healthKitStore;
        private LocationService _locationService;

        public SensorService()
        {
            _locationService = new LocationService();
            _healthKitStore = new HKHealthStore();
        }
        
        public double ReadHeartRate()
        {
            var bpm = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
            var heartRateId = HKQuantityTypeIdentifier.HeartRate;
            var heartRateQuantityType = HKQuantityType.Create(heartRateId);

            FetchMostRecentData(heartRateQuantityType, (mostRecentQuantity, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("An error occured fetching the user's height information. " +
                                      "In your app, try to handle this gracefully. The error was: {0}.", 
                        error.LocalizedDescription);
                    return;
                }

                if (mostRecentQuantity != null)
                {
                    var bpmUnit = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
                    _bmp = mostRecentQuantity.GetDoubleValue(bpmUnit);
                }
            });

            return _bmp;
        }

        public int ReadBatteryLevel()
        {
            return (int) (Battery.ChargeLevel * 100);
        }

        public bool BatteryIsCharge()
        {
            return Battery.PowerSource != BatteryPowerSource.Battery;
        }

        public int ReadStepCount()
        {
            if (!CMPedometer.IsStepCountingAvailable) return -1;
            return 0;
        }

        public void ValidateAuthorization()
        {
            //var heartRateId = HKQuantityTypeIdentifier.HeartRate;
            //var heartRateType = HKQuantityType.Create(heartRateId);
            
            //var typesToWrite = new NSSet(new[] { heartRateType });
            //var typesToRead = new NSSet(new[] { heartRateType });

            //_healthKitStore.RequestAuthorizationToShare(
            //    typesToWrite,
            //    typesToRead,
            //    ReactToHealthCarePermissions);
        }

        private void ReactToHealthCarePermissions(bool success, NSError error)
        {
            var access = _healthKitStore.GetAuthorizationStatus(HKQuantityType.Create(HKQuantityTypeIdentifier.HeartRate));
            HeartRateModel.Instance.Enabled = access.HasFlag(HKAuthorizationStatus.SharingAuthorized);
        }

        private void FetchMostRecentData(HKQuantityType quantityType, Action<HKQuantity, NSError> completion)
        {
            var timeSortDescriptor = new NSSortDescriptor(HKSample.SortIdentifierEndDate, false);
            var healthKitStore = new HKHealthStore();
            var query = new HKSampleQuery(quantityType, null, 1, new NSSortDescriptor[] {timeSortDescriptor},
                (HKSampleQuery resultQuery, HKSample[] results, NSError error) =>
                {
                    if (completion != null && error != null)
                    {
                        completion(null, error);
                        return;
                    }

                    HKQuantity quantity = null;
                    if (results.Length != 0)
                    {
                        var quantitySample = (HKQuantitySample) results[results.Length - 1];
                        quantity = quantitySample.Quantity;
                    }

                    if (completion != null)
                        completion(quantity, error);
                });

            healthKitStore.ExecuteQuery(query);
        }

        public string ReadLocation()
        {
            return _locationService.ToString();
        }
    }
}