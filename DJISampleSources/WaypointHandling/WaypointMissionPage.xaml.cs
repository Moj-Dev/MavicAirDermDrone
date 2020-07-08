#define EMULATOR_OFF

#if EMULATOR_ON
using Messenger.Simulator;
using Tello.Simulator;
#else
using Messenger.Udp;
using System.Net;
#endif

using Repository.Sqlite;
using System.Threading;
using Tello.App.MvvM;
//using Tello.App.UWP.Services;
using Tello.App.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Media.MediaProperties;
using Windows.Media.Core;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DJI.WindowsSDK;
using DJIWindowsSDKSample.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;





namespace DJIWindowsSDKSample.WaypointHandling
{
    public sealed class OperationException : Exception
    {
        public OperationException(String message, SDKError error) : base(String.Format(message))
        {
        }
    }
    public sealed partial class WaypointMissionPage

    {

        public WaypointMissionPage()
        {
            

        }





    }
}
