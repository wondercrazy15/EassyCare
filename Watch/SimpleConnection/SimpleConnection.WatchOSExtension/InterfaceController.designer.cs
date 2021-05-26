// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SimpleConnection.WatchOSExtension
{
	[Register ("InterfaceController")]
	partial class InterfaceController
	{
		[Outlet]
		WatchKit.WKInterfaceButton myButton { get; set; }

		[Outlet]
		WatchKit.WKInterfaceButton PairButton { get; set; }

		[Action ("EmergencyButtonClick")]
		partial void EmergencyButtonClick ();

		[Action ("PairButtonClick")]
		partial void PairButtonClick ();
		
		void ReleaseDesignerOutlets ()
		{
			if (PairButton != null) {
				PairButton.Dispose ();
				PairButton = null;
			}

			if (myButton != null) {
				myButton.Dispose ();
				myButton = null;
			}
		}
	}
}
