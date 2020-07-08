using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;




using DJI.WindowsSDK;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace DJIWindowsSDKSample
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public static class StringData_
    {
        public static string stringData;

    }



    public sealed partial class MainPage : Page
    {
        public string input;


 


        private struct SDKModuleSampleItems
        {
            public String header;
            public List<KeyValuePair<String, Type>> items;
        }

        private List<SDKModuleSampleItems> navigationModules = new List<SDKModuleSampleItems>
        {
            new SDKModuleSampleItems() {
                header = "Activation", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Activating Account", typeof(DJISDKInitializing.ActivatingPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Video Stream", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Video Stream", typeof(FPV.FPVPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Component Handling", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Component Handling", typeof(ComponentHandling.ComponentHandingPage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Waypoint", items = new List<KeyValuePair<String, Type>>()
                {
                  //  new KeyValuePair<string, Type>("Navigation", typeof(WaypointHandling.SimulatorPage)),
                    new KeyValuePair<string, Type>("Waypoint Mission", typeof(WaypointHandling.WaypointMissionPage)),
                },
            },
           /* new SDKModuleSampleItems() {
                header = "Tello", items = new List<KeyValuePair<String, Type>>()
                {

                   
                  //  new KeyValuePair<string, Type>("Account Management", typeof()),
                },
            },
            new SDKModuleSampleItems() {
                header = "Flysafe", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Flyzone", typeof(Flysafe.FlyzonePage)),
                },
            },
            new SDKModuleSampleItems() {
                header = "Playback", items = new List<KeyValuePair<String, Type>>()
                {
                    new KeyValuePair<string, Type>("Playback", typeof(Playback.PlaybackPage)),
                },
            },*/

        };

        public MainPage()
        {



            Debug.WriteLine("main page in dji starttttttttttttttttttttttttttt");
            Thread thread = new Thread(new ThreadStart(WorkThreadFunctionForTCP));
            Debug.WriteLine("main page in dji thread");


            BackgroundWorker bwMavic = new BackgroundWorker();
            if (bwMavic.IsBusy != true)
            {
                bwMavic.RunWorkerAsync();

            }



            bwMavic.DoWork += bwMavic_DoWork;

            bwMavic.WorkerReportsProgress = true;
            bwMavic.WorkerSupportsCancellation = true; //Allow for the process to be cancelled


            // thread.Start();
            this.InitializeComponent();
            var module = navigationModules[0];




            NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = module.header });
            foreach (var item in module.items)
            {
                NavView.MenuItems.Add(item.Key);
            }
        }


        private void WorkThreadFunctionForTCP()
        {
        }


            private void bwMavic_DoWork(object sender, DoWorkEventArgs e)
            {


                bool flag = false;
            while (true)
            {
                byte[] data = new byte[1024];
                int COUNTER = 0;
                IPEndPoint ipep = new IPEndPoint(
                                IPAddress.Parse("127.10.10.1"), 9070);

                Socket server = new Socket(AddressFamily.InterNetwork,
                               SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    server.Connect(ipep);
                    flag = true;
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine("Unable to connect to server.");
                    Console.WriteLine("Unable to connect to server.");
                    Console.WriteLine(ex.ToString());
                    // return;
                }





                while (flag)
                {
                    // Debug.WriteLine("while.");

                    try
                    {
                        int recv = server.Receive(data);
                    DJIWindowsSDKSample.StringData_.stringData = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine(DJIWindowsSDKSample.StringData_.stringData);


                    // input = Console.ReadLine();
                    COUNTER++;
                    if (COUNTER > 1000) COUNTER = 1;
                    input = COUNTER.ToString();
                    if (input == "exit")
                        break;
                    server.Send(Encoding.ASCII.GetBytes(input));
                    data = new byte[1024];
                    recv = server.Receive(data);
                    DJIWindowsSDKSample.StringData_.stringData = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine(DJIWindowsSDKSample.StringData_.stringData);
                    Debug.WriteLine(DJIWindowsSDKSample.StringData_.stringData);
                    string[] s2 = DJIWindowsSDKSample.StringData_.stringData.Split(',');

                    DJIWindowsSDKSample.ComponentHandling.xPosition_comp.xPose_comp = s2[0];
                    DJIWindowsSDKSample.ComponentHandling.yPosition_comp.yPose_comp = s2[1];
                    DJIWindowsSDKSample.ComponentHandling.yaw_comp_.yaw_comp =  s2[2];
                    DJIWindowsSDKSample.ComponentHandling.zPosition_comp.zPose_comp = s2[3];
                    DJIWindowsSDKSample.ComponentHandling.DataLost.flag = s2[4];


                        //   Console.WriteLine("TCP WORKING IN MAVIC AIR");
                        System.Threading.Thread.Sleep(20);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Video image processing receive thread error:" + ex.Message);
                    }




                }
                Console.WriteLine("Disconnecting from server...");
                // server.Shutdown(SocketShutdown.Both);
                //  server.Close();

            }


        }

            private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            String invokedName = args.InvokedItem as String;
            foreach (var module in navigationModules)
            {
                foreach (var item in module.items)
                {
                    if (invokedName == item.Key)
                    {
                        if (ContentFrame.SourcePageType != item.Value)
                        {
                            ContentFrame.Navigate(item.Value);
                        }
                        return;
                    }
                }
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationEvent;
        }

        private async void Instance_SDKRegistrationEvent(SDKRegistrationState state, SDKError resultCode)
        {
            if (resultCode == SDKError.NO_ERROR)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    for (int i = 1; i < navigationModules.Count;++i)
                    {
                        var module = navigationModules[i];
                        NavView.MenuItems.Add(new NavigationViewItemHeader() { Content = module.header });
                        foreach (var item in module.items)
                        {
                            NavView.MenuItems.Add(item.Key);
                        }
                    }
                });
            }
        }
    }
}
