using System;
using AppKit;
namespace BlynclightColors
{
    public class ColorTouchBarDelegate : NSTouchBarDelegate
    {
        public event EventHandler<NSColor> ColorSelected;

        public static string[] DefaultIdentifiers =
        {
            "jp.ailen0ada.blynclightcolors.applecolor"
        };

		public override NSTouchBarItem MakeItem(NSTouchBar touchBar, string identifier)
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
	}
}
