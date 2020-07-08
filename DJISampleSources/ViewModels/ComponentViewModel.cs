using DJI.WindowsSDK;
using DJIUWPSample.Commands;
using DJIUWPSample.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.IO;
using ProtoBuf;
using System.Threading;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace DJIWindowsSDKSample.ViewModels
{
    class ComponentViewModel : ViewModelBase
    {



        //  ClassLibrary1.Class1.AppName

        public static int examplevalue { get; set; }
        public static int counter { get; set; }

        public static double X_Vel =0;

  







        Velocity3D _aircraftVelocity3D;
        public Velocity3D AircraftVelocity
        {
            get
            {
                return _aircraftVelocity3D;
            }
            set
            {
                _aircraftVelocity3D = value;
                OnPropertyChanged("AircraftVelocityXString");
                OnPropertyChanged("AircraftVelocityYString");
                OnPropertyChanged("AircraftVelocityZString");
               // _aircraftVelocity3D.z = ClassLibrary1.Class1.SomethingCount;

            }
        }
        public String AircraftVelocityXString
        {
            get { return _aircraftVelocity3D.x.ToString() + " m/s"; }
        }
        public String AircraftVelocityYString
        {
           get {return _aircraftVelocity3D.y.ToString() + " m/s"; }

        }
        public String AircraftVelocityZString
        {
            get{return _aircraftVelocity3D.z.ToString() + " m/s";  }
 
        }

        public ICommand _registerVelocityChangedObserver;
        public ICommand RegisterVelocityChangedObserver
        {


            get
            {
                if (_registerVelocityChangedObserver == null)
                {
                    _registerVelocityChangedObserver = new RelayCommand(delegate ()
                    {
                      //  Debug.WriteLine("*****************************************create thread************************************************");
 

                          DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).VelocityChanged += ComponentHandingPage_VelocityChanged;
                    }, delegate () { return true; });



                }
                return _registerVelocityChangedObserver;
            }
        }

 
            private async void ComponentHandingPage_VelocityChanged(object sender, Velocity3D? value)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AircraftVelocity = value.Value;
              //  X_Vel = (double) AircraftVelocity.x;
            });
        }
       
        public ICommand _startTakeoff;
        public ICommand StartTakeoff
        {
            get
            {
                if (_startTakeoff == null)
                {
                    _startTakeoff = new RelayCommand(async delegate ()
                    {
                        var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartTakeoffAsync();
                        var messageDialog = new MessageDialog(String.Format("Start send takeoff command: {0}", res.ToString()));
                        await messageDialog.ShowAsync();
                    }, delegate () { return true; });
                }
                return _startTakeoff;
            }
        }


        public ICommand _startLanding;
        public ICommand StartLanding
        {
            get
            {
                if (_startLanding == null)
                {
                    _startLanding = new RelayCommand(async delegate ()
                    {
                        var rbm = new BoolMsg();
                        rbm.value = false;


                        var LandingError = await DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).SetLandingProtectionEnabledAsync(rbm);
                        var res = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();

        
                        var messageDialog = new MessageDialog(String.Format("Start send landing command: {0}", res.ToString()));

                        await messageDialog.ShowAsync();
                    }, delegate () { return true; });
                }
                return _startLanding;
            }
        }


        public ICommand _ConfirmLanding;
        public ICommand ConfirmLanding
        {
            get
            {
                if (_ConfirmLanding == null)
                {
                    _ConfirmLanding = new RelayCommand(async delegate ()
                    {

                        var res1 = await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).ConfirmLandingAsync();
                        var messageDialog1 = new MessageDialog(String.Format("Start send confirm landing command: {0}", res1.ToString()));
                        await messageDialog1.ShowAsync();
                    }, delegate () { return true; });
                }
                return _ConfirmLanding;
            }
        }




        public ICommand _DownwardPitchGimbal;
        public ICommand DownwardPitchGimbal
        {
            get
            {
                if (_DownwardPitchGimbal == null)
                {
                    _DownwardPitchGimbal = new RelayCommand(async delegate ()
                    {
                         var gimbalRotation = new GimbalSpeedRotation();
                        var gimbalstate= new GimbalAngleRotation();
                        gimbalstate.pitchIgnored = true;

                        gimbalRotation.pitch = -45;
                        gimbalRotation.yaw = 0;


                        //  gimbalRotation.pitch = -5;
                           DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);
                        DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateByAngleAsync(gimbalstate);

                        //  DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateByAngleAsync(gimbalangle);

                    }, delegate () { return true; });
                }
                return _DownwardPitchGimbal;
            }
        }
        public ICommand _UpwardPitchGimbal;
        public ICommand UpwardPitchGimbal
        {
            get
            {
                if (_UpwardPitchGimbal == null)
                {
                    _UpwardPitchGimbal = new RelayCommand(async delegate ()
                    {
                        var gimbalRotation = new GimbalSpeedRotation();
                        var gimbalstate = new GimbalAngleRotation();
                        gimbalstate.pitchIgnored = true;
                        
                        gimbalRotation.pitch = 5;
                        gimbalRotation.yaw = 0;

                        DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);
                        DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateByAngleAsync(gimbalstate);

                    }, delegate () { return true; });
                }
                return _UpwardPitchGimbal;
            }
        }
        public ICommand _MoveForward;
        public ICommand MoveForward
        {
            get
            {
                if (_MoveForward == null)
                {
                    _MoveForward = new RelayCommand(async delegate ()
                    {
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0.4f, 0);

                    }, delegate () { return true; });
                }
                return _MoveForward;
            }
        }
        public ICommand _MoveBack;
        public ICommand MoveBack
        {
            get
            {
                if (_MoveBack == null)
                {
                    _MoveBack = new RelayCommand(async delegate ()
                    {
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, -0.4f, 0);
                        

                    }, delegate () { return true; });
                }
                return _MoveBack;
            }
        }
        public ICommand _MoveRight;
        public ICommand MoveRight
        {
            get
            {
                if (_MoveRight == null)
                {
                    _MoveRight = new RelayCommand(async delegate ()
                    {
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0, 0.4f);

                    }, delegate () { return true; });
                }
                return _MoveRight;
            }
        }
        public ICommand _MoveLeft;
        public ICommand MoveLeft
        {
            get
            {
                if (_MoveLeft == null)
                {
                    _MoveLeft = new RelayCommand(async delegate ()
                    {
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0, -0.4f);

                    }, delegate () { return true; });
                }
                return _MoveLeft;
            }
        }
        public ICommand _HoldPosition;
        public ICommand HoldPosition
        {
            get
            {
                if (_HoldPosition == null)
                {
                    _HoldPosition = new RelayCommand(async delegate ()
                    {

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0,0,0,0);
                        }, delegate () { return true; });
                }
                return _HoldPosition;
            }
        }
        public ICommand _MoveUpward;
        public ICommand MoveUpward
        {
            get
            {
                if (_MoveUpward == null)
                {
                    _MoveUpward = new RelayCommand(async delegate ()
                    {
                        
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0.2f, 0, 0, 0);
                    }, delegate () { return true; });
                }
                return _MoveUpward;
            }
        }
        public ICommand _MoveDownward;
        public ICommand MoveDownward
        {
            get
            {
                if (_MoveDownward == null)
                {
                    _MoveDownward = new RelayCommand(async delegate ()
                    {

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(-0.2f, 0, 0, 0);
                    }, delegate () { return true; });
                }
                return _MoveDownward;
            }
        }

        public ICommand _CCW;
        public ICommand CCW
        {
            get
            {
                if (_CCW == null)
                {
                    _CCW = new RelayCommand(async delegate ()
                    {

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -0.4f, 0, 0);
                    }, delegate () { return true; });
                }
                return _CCW;
            }
        }


        public ICommand _CW;
        public ICommand CW
        {
            get
            {
                if (_CW == null)
                {
                    _CW = new RelayCommand(async delegate ()
                    {

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0.4f, 0, 0);
                    }, delegate () { return true; });
                }
                return _CW;
            }
        }











    }
}
