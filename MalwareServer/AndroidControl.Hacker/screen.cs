using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AndroidControl.Hacker
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct DEVMODE
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmDeviceName;

		public short dmSpecVersion;
		public short dmDriverVersion;
		public short dmSize;
		public short dmDriverExtra;
		public int dmFields;
		public int dmPositionX;
		public int dmPositionY;
		public int dmDisplayOrientation;
		public int dmDisplayFixedOutput;
		public short dmColor;
		public short dmDuplex;
		public short dmYResolution;
		public short dmTTOption;
		public short dmCollate;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmFormName;

		public short dmLogPixels;
		public short dmBitsPerPel;
		public int dmPelsWidth;
		public int dmPelsHeight;
		public int dmDisplayFlags;
		public int dmDisplayFrequency;
		public int dmICMMethod;
		public int dmICMIntent;
		public int dmMediaType;
		public int dmDitherType;
		public int dmReserved1;
		public int dmReserved2;
		public int dmPanningWidth;
		public int dmPanningHeight;
	};

	public class NativeMethods
	{
		// PInvoke declaration for EnumDisplaySettings Win32 API
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

		// PInvoke declaration for ChangeDisplaySettings Win32 API
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwFlags);

		// add more functions as needed …

		// constants
		public const int ENUM_CURRENT_SETTINGS = -1;
		public const int DMDO_DEFAULT = 0;
		public const int DMDO_90 = 1;
		public const int DMDO_180 = 2;
		public const int DMDO_270 = 3;
		// add more constants as needed …
	}
	public static class Screen
	{
		public static void ChangeScreen()
		{
			// initialize the DEVMODE structure
			DEVMODE dm = new DEVMODE();
			dm.dmDeviceName = new string(new char[32]);
			dm.dmFormName = new string(new char[32]);
			dm.dmSize = (short)Marshal.SizeOf(dm);

			if (0 != NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
			{
				// swap width and height
				int temp = dm.dmPelsHeight;
				dm.dmPelsHeight = dm.dmPelsWidth;
				dm.dmPelsWidth = temp;

				// determine new orientation
				switch (dm.dmDisplayOrientation)
				{
					case NativeMethods.DMDO_DEFAULT:
						dm.dmDisplayOrientation = NativeMethods.DMDO_270;
						break;
					case NativeMethods.DMDO_270:
						dm.dmDisplayOrientation = NativeMethods.DMDO_180;
						break;
					case NativeMethods.DMDO_180:
						dm.dmDisplayOrientation = NativeMethods.DMDO_90;
						break;
					case NativeMethods.DMDO_90:
						dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
						break;
					default:
						// unknown orientation value
						// add exception handling here
						break;
				}

				int iRet = NativeMethods.ChangeDisplaySettings(ref dm, 0);
			}
		}
	}
}

