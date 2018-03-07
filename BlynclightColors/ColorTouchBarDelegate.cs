using System;
using AppKit;
namespace BlynclightColors
{
    public class ColorTouchBarDelegate : NSTouchBarDelegate
    {
        public event EventHandler<NSColor> ColorSelected;

        public event EventHandler TurnOffRequested;

        public static string[] DefaultIdentifiers =
        {
            "jp.ailen0ada.blynclightcolors.applecolor",
            "jp.ailen0ada.blynclightcolors.turnofflight"
        };

		public override NSTouchBarItem MakeItem(NSTouchBar touchBar, string identifier)
        {
            switch (Array.IndexOf(DefaultIdentifiers, identifier))
            {
                case 0:
                    {
                        var item = NSColorPickerTouchBarItem.CreateColorPicker(identifier, NSImage.ImageNamed(NSImageName.TouchBarColorPickerFill));
                        item.ColorList = NSColorList.ColorListNamed("Apple");
                        item.ShowsAlpha = false;
                        item.Activated += (_, __) =>
                        {
                            var color = item.Color.UsingColorSpace(NSColorSpace.CalibratedRGB);
                            this.ColorSelected.Invoke(item, color);
                        };
                        return item;
                    }
                case 1:
                    {
                        var item = new NSCustomTouchBarItem(identifier);
                        item.View = NSButton.CreateButton(NSImage.ImageNamed(NSImageName.TouchBarRecordStopTemplate), () => this.TurnOffRequested.Invoke(this, null));
                        return item;
                    }
                default:
                    return null;
            }
		}
	}
}
