using System;
using HealthKit;
using Foundation;

namespace SimpleConnection.WatchOSExtension
{
	public class GenericEventArgs<T> : EventArgs
	{
		public T Value { get; protected set; }

		public DateTime Time { get; protected set; }

		public GenericEventArgs(T value)
		{
			this.Value = value;
			Time = DateTime.Now;
		}
	}

	public delegate void GenericEventHandler<T>(object sender, GenericEventArgs<T> args);

	public sealed class HeartRateModel : NSObject
	{
		//Thread-safe singleton: Overkill for the sample app, but proper design
		private static volatile HeartRateModel singleton;
		private static object syncRoot = new Object();
        public static double BPM { get; set; }
        private HeartRateModel()
		{
		}

		public static HeartRateModel Instance
		{
			get
			{
				//Double-check lazy initialization
				if (singleton == null)
				{
					lock (syncRoot)
					{
						if (singleton == null)
						{
							singleton = new HeartRateModel();
						}
					}
				}

				return singleton;
			}
		}

		// Default state for HK permissions, false => dnied, true=> granted
		private bool enabled = false;

		// Event declarations 
		public event GenericEventHandler<bool> EnabledChanged;
		public event GenericEventHandler<String> ErrorMessageChanged;
		public event GenericEventHandler<Double> HeartRateStored;

		public bool Enabled
		{
			get { return enabled; }
			set
			{
				if (enabled != value)
				{
					enabled = value;
					InvokeOnMainThread(() => EnabledChanged(this, new GenericEventArgs<bool>(value)));
				}
			}
		}

		public void PermissionsError(string msg)
		{
			Enabled = false;
			InvokeOnMainThread(() => ErrorMessageChanged(this, new GenericEventArgs<string>(msg)));
		}

		//Converts its argument into a strongly-typed quantity representing the value in beats-per-minute
		public HKQuantity HeartRateInBeatsPerMinute(ushort beatsPerMinute)
		{
			var heartRateUnitType = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
			var quantity = HKQuantity.FromQuantity(heartRateUnitType, beatsPerMinute);

			return quantity;
		}

		//Attempts to store in the Health Kit database a quantity, which must be of a type compatible with beats-per-minute
		public void StoreHeartRate(HKQuantity quantity)
		{
			var bpm = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
			//Confirm that the value passed in is of a valid type (can be converted to beats-per-minute)
			if (!quantity.IsCompatible(bpm))
			{
				InvokeOnMainThread(() => ErrorMessageChanged(this, new GenericEventArgs<string>("Units must be compatible with BPM")));
			}

			HKQuantityTypeIdentifier heartRateId = HKQuantityTypeIdentifier.HeartRate;
			HKQuantityType heartRateQuantityType = HKQuantityType.Create(heartRateId);
			// Create a HKQuantity sample that can be stored, in HK Database
			HKQuantitySample heartRateSample = HKQuantitySample.FromType(heartRateQuantityType, quantity, new NSDate(), new NSDate(), new HKMetadata());

			using (var healthKitStore = new HKHealthStore())
			{
				healthKitStore.SaveObject(heartRateSample, (success, error) => {
					InvokeOnMainThread(() => {
						if (success)
						{
							HeartRateStored(this, new GenericEventArgs<Double>(quantity.GetDoubleValue(bpm)));
						}
						else
						{
							ErrorMessageChanged(this, new GenericEventArgs<string>("Save failed"));
						}
						if (error != null)
						{
							//If there's some kind of error, disable 
							Enabled = false;
							ErrorMessageChanged(this, new GenericEventArgs<string>(error.ToString()));
						}
					});
				});
			}
		}

		//Attempts to store in the Health Kit database a quantity, which must be of a type compatible with beats-per-minute
		public double ReadHeartRate()
		{
			//double usersBPM = new double();
			var bpm = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
			//Confirm that the value passed in is of a valid type (can be converted to beats-per-minute)

			var heartRateId = HKQuantityTypeIdentifier.HeartRate;
			var heartRateQuantityType = HKQuantityType.Create(heartRateId);

			FetchMostRecentData(heartRateQuantityType, (mostRecentQuantity, error) => {
				if (error != null)
				{
					Console.WriteLine("An error occured fetching the user's height information. " +
					"In your app, try to handle this gracefully. The error was: {0}.", error.LocalizedDescription);
					return;
				}

				if (mostRecentQuantity != null)
				{
					
					var bpmUnit = HKUnit.Count.UnitDividedBy(HKUnit.Minute);
					BPM = mostRecentQuantity.GetDoubleValue(bpmUnit);
				}
			});

			return BPM;
		}

		void FetchMostRecentData(HKQuantityType quantityType, Action<HKQuantity, NSError> completion)
		{
			var timeSortDescriptor = new NSSortDescriptor(HKSample.SortIdentifierEndDate, false);

			var healthKitStore = new HKHealthStore();

			var query = new HKSampleQuery(quantityType, null, 1, new NSSortDescriptor[] { timeSortDescriptor },
							(HKSampleQuery resultQuery, HKSample[] results, NSError error) => {
								if (completion != null && error != null)
								{
									completion(null, error);
									return;
								}

								HKQuantity quantity = null;
								if (results.Length != 0)
								{
									var quantitySample = (HKQuantitySample)results[results.Length - 1];
									quantity = quantitySample.Quantity;
								}

								if (completion != null)
									completion(quantity, error);
							});

			healthKitStore.ExecuteQuery(query);
		}
	}
}

