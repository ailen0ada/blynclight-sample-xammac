using System;
using AppKit;
using Foundation;
using System.Diagnostics;

namespace BlynclightColors
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private readonly ColorTouchBarDelegate _touchBarDelegate = new ColorTouchBarDelegate();
        private IDisposable _colorChangedObserver;
        private int _numberOfDevices;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this._touchBarDelegate.ColorSelected += (_, color) => this.ColorSelector.Color = color;

            BlynclightControl.FindDevices(ref this._numberOfDevices);
        }

		public override void ViewWillAppear()
		{
            base.ViewWillAppear();
            if (this.SupportsTouchBar())
            {
                this.View.Window.SetTouchBar(null);
                var bar = new NSTouchBar
                {
                    Delegate = _touchBarDelegate,
                    DefaultItemIdentifiers = ColorTouchBarDelegate.DefaultIdentifiers
                };
                this.View.Window.SetTouchBar(bar);
            }

            this._colorChangedObserver = this.ColorSelector.AddObserver(
                "color",
                NSKeyValueObservingOptions.New, 
                _ => this.OnColorChanged(this.ColorSelector.Color.UsingColorSpace(NSColorSpace.CalibratedRGB)));
		}

		public override void ViewWillDisappear()
		{
            base.ViewWillDisappear();
            this._colorChangedObserver.Dispose();
		}

		private bool SupportsTouchBar() => ObjCRuntime.Class.GetHandle("NSTouchBar") != IntPtr.Zero;

        private const float ToNumbersConstant = 255.99999f;

        private void OnColorChanged(NSColor color)
        {
            var r = (byte)(color.RedComponent * ToNumbersConstant);
            var g = (byte)(color.GreenComponent * ToNumbersConstant);
            var b = (byte)(color.BlueComponent * ToNumbersConstant);
            Debug.WriteLine($"(r,g,b) = ({r},{g},{b})");

            if (this._numberOfDevices < 1)
                return;
            // note: first device
            BlynclightControl.TurnOnRGBLights(0, r, g, b);
        }
    }
}
