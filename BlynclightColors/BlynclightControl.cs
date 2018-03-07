using System;
using System.Runtime.InteropServices;
namespace BlynclightColors
{
    public static class BlynclightControl
    {
        [DllImport("__Internal")]
        public static extern void ReleaseDevices();

        [DllImport("__Internal")]
        public static extern int FindDevices(ref int numberOfBlyncDevices);

        [DllImport("__Internal")]
        public static extern int TurnOnRGBLights(byte byDeviceIndex, byte byRedLevel, byte byGreenLevel, byte byBlueLevel);

        [DllImport("__Internal")]
        public static extern int TurnOffV30Light(byte byDeviceIndex);
    }
}
