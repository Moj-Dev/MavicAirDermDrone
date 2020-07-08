using DJI.WindowsSDK;
using DJIUWPSample.Commands;
using DJIUWPSample.ViewModels;

using DJIWindowsSDKSample.DJISDKInitializing;
using DJIWindowsSDKSample.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI;
using Windows.Media;
using System.Threading.Tasks;

namespace DJIWindowsSDKSample.ComponentHandling
{


    public static class xPosition_comp
    {
        public static string xPose_comp="none";

    }

    public static class yPosition_comp
    {
        public static string yPose_comp = "none";

    }
    public static class zPosition_comp
    {
        public static string zPose_comp = "none";

    }

    public static class yaw_comp_
    {
        public static string yaw_comp = "none";

    }
    public static class DataLost
    {
        public static string flag = "none";

    }


    public sealed partial class ComponentHandingPage : Page

    {

        bool controller_flag = false;
        int XSetpointController = 0;
        int YSetpointController = 0;
        int ZSetpointController = 0;
        int YawSetpointController = 0;
        int XerrorController;
        int YerrorController;
        int ZerrorController;
        int YawerrorController;

        int XpointController;
        int YpointController;
        int ZpointController;
        int YawpointController;

        float XoutputController;
        float YoutputController;
        float ZoutputController;
        float YawoutputController;

        float XKpController;
        float YKpController;
        float ZKpController;
        float YawKpController;

        int XCircleCenter;
        int YCircleCenter;
        int CircleRadius;
        double CircleThetha;
        double CircleThethaDelta;

        int TAKE_OFF_ALT= 167;
        int CHEST_ALT = 137;
        int NAV_START_FLAG = 0;
        int NAV_START_CNT = 0;
        int ZDelta = 1;
        int data_lost_flag = 0;

        int error_limit = 200;
        int gimbal_change_angle_state = 0;


        private static System.DateTime last_time;//for connection timeouts.

        public object StartLanding { get; private set; }

        public ComponentHandingPage() 
        {
            InitializeComponent();
            DataContext = new ComponentViewModel();
            Thread thread     = new Thread(new ThreadStart(LocalizationFeedback));


            thread.Start();











        }
        private void LocalizationFeedback()
        {
            while (true)
            {

              //  Debug.WriteLine(DJIWindowsSDKSample.ViewModels.ComponentViewModel.counter);

                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>  {
                    Pose.Text = xPosition_comp.xPose_comp + "  ,  " + yPosition_comp.yPose_comp ;// + DJIWindowsSDKSample.ViewModels.ComponentViewModel.counter.ToString();
                    xpoint.Text =    xPosition_comp.xPose_comp;
                    ypoint.Text =  yPosition_comp.yPose_comp;
                    zpoint.Text = zPosition_comp.zPose_comp;
                    yawpoint.Text = yaw_comp_.yaw_comp;



                    XchangeSetpointText.Text = XSetpointController.ToString();
                    YchangeSetpointText.Text = YSetpointController.ToString();
                    YawchangeSetpointText.Text = YawSetpointController.ToString();
                    ZchangeSetpointText.Text = ZSetpointController.ToString();






                    Int32.TryParse(xPosition_comp.xPose_comp, out XpointController);
                    Int32.TryParse(yPosition_comp.yPose_comp, out YpointController);
                    Int32.TryParse(zPosition_comp.zPose_comp, out ZpointController);
                    Int32.TryParse(DataLost.flag, out data_lost_flag);

                    Int32.TryParse(yaw_comp_.yaw_comp, out YawpointController);



                  // battery_level_text.Text =  DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).GetChargeRemainingInPercentAsync().ToString();




                    





                });




                DJIWindowsSDKSample.ViewModels.ComponentViewModel.counter++;
                if (DJIWindowsSDKSample.ViewModels.ComponentViewModel.counter > 2000) DJIWindowsSDKSample.ViewModels.ComponentViewModel.counter = 0;

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        public void HandleCheck(object sender, RoutedEventArgs e)
        {
            text2.Text = "Button is Checked";
            controller_flag = true;
            Thread PoseThread = new Thread(new ThreadStart(PositionControllerThread));

            PoseThread.Start();



        }

        private void HandleUnCheck(object sender, RoutedEventArgs e)
        {
            text2.Text = "Button is unchecked.";
            controller_flag = false;
            DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0, 0);


        }


        public async void HandleCheckRecordVideo(object sender, RoutedEventArgs e)
        {
            if (DJISDKManager.Instance.ComponentManager != null)
            {
                var retCode = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).StartRecordAsync();

                if (retCode != SDKError.NO_ERROR)
                {
                    Debug.WriteLine("Failed to record video,");

                }
                else
                {
                    Debug.WriteLine("Record record video successfully");

                }
            }
            else
            {
                Debug.WriteLine("SDK hasn't been activated yet.");
            }



        }

        private async void HandleUnCheckRecordVideo(object sender, RoutedEventArgs e)
        {
            if (DJISDKManager.Instance.ComponentManager != null)
            {
                var retCode = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).StopRecordAsync();
                if (retCode != SDKError.NO_ERROR)
                {

                    Debug.WriteLine("Failed to stop record video");

                }
                else
                {
                    Debug.WriteLine("Stop record video successfully");

                }
            }
            else
            {
                Debug.WriteLine("SDK hasn't been activated yet.");


            }


        }


        private void PositionControllerThread()
        {
           // System.IO.TextWriter tw = new StreamWriter("D:/MetaOptima/MAVIC AIR/Windows-SDK-master - v2/log.txt");

            /* CircleRadius = 900;
             XCircleCenter = 1850;
             YCircleCenter = 1400;
             CircleThetha = 22.5 ;
             CircleThethaDelta = 1;// 22.5;
             YawSetpointController = 8;*/
            CircleRadius  = 1050;
            XCircleCenter = 1800;
            YCircleCenter = 1500;
            CircleThetha  = 10;// 22.5 ;
            CircleThethaDelta = 5;// 22.5/2;
            YawSetpointController = 0;
            //YawSetpointController = 0;
            //ZSetpointController = 
            ZSetpointController = TAKE_OFF_ALT;
            ZDelta = 1;




            while (controller_flag)
            {


              //  if (data_lost_flag == 1) {

                    if (NAV_START_FLAG == 11)
                    {
                        if (ZpointController > 115)
                        {

                            ZerrorController = TAKE_OFF_ALT - ZpointController;
                            ZKpController = 0.03f;
                            ZoutputController = (float)(ZerrorController) * ZKpController;
                            if (ZoutputController > 0.25f) ZoutputController = 0.25f;
                            if (ZoutputController < -0.25f) ZoutputController = -0.25f;

                            if (ZerrorController > -2 && ZerrorController < 2)
                            {
                                NAV_START_CNT += 1;
                                if (NAV_START_CNT > 25)
                                {
                                    NAV_START_FLAG = 1;
                                NAV_START_CNT = 0;
                                }

                            }
                            else
                            {
                                DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(ZoutputController, 0, 0, 0);
                            }

                        }
                        else
                        {
                            //nothing
                        }
                    }

                if (NAV_START_FLAG == 0)
                {
                    if (ZpointController > 115)
                    {


                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0, 0);

                        var gimbalRotation = new GimbalSpeedRotation();


                        //  gimbalRotation.pitch = -5;
                        //  DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);

                        NAV_START_CNT += 1;
                        if (NAV_START_CNT > 50)
                        {
                            gimbal_change_angle_state += 1;
                            NAV_START_CNT = 0;
                        }


                        if (gimbal_change_angle_state == 1)
                        {
                            gimbalRotation.pitch = -40;
                            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);

                        }

                        else if (gimbal_change_angle_state == 2)
                        {
                            gimbalRotation.pitch = 40;
                            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);


                        }

                        else if (gimbal_change_angle_state == 3)
                        {
                            gimbalRotation.pitch = 20;
                            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);


                        }
                        else if (gimbal_change_angle_state == 4)
                        {
                            gimbalRotation.pitch = 0;
                            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateBySpeedAsync(gimbalRotation);


                        }

                        else if (gimbal_change_angle_state == 5)
                        {
                            NAV_START_FLAG = 11;
                            NAV_START_CNT = 0;


                        }








                    }

                }


                if (NAV_START_FLAG == 1)
                    {
                        if (ZpointController > 115)
                        {

                            ZerrorController = CHEST_ALT - ZpointController;
                            ZKpController = 0.03f;
                            ZoutputController = (float)(ZerrorController) * ZKpController;
                            if (ZoutputController > 0.25f) ZoutputController = 0.25f;
                            if (ZoutputController < -0.25f) ZoutputController = -0.25f;

                            if (ZerrorController > -2 && ZerrorController < 2)
                            {
                                NAV_START_CNT += 1;
                                if (NAV_START_CNT > 5)
                                {
                                    NAV_START_FLAG = 2;
                                    NAV_START_CNT = 0;
                                }

                            }
                            else
                            {
                                DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(ZoutputController, 0, 0, 0);
                            }

                        }
                        else
                        {
                            //nothing
                        }
                    }

                    if (NAV_START_FLAG == 2)
                    {
                        XSetpointController = (int)(XCircleCenter + (CircleRadius * Math.Sin(Math.PI * CircleThetha / 180)));
                        YSetpointController = (int)(YCircleCenter + (CircleRadius * Math.Cos(Math.PI * CircleThetha / 180)));




                        XerrorController = XSetpointController - XpointController;
                        YerrorController = YSetpointController - YpointController;
                        ZerrorController = ZSetpointController - ZpointController;
                        YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0006f;// 0.0006f;
                        YKpController = 0.0006f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.04f;// 0.125f;// 0.04f;


                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));



                    if (YawSetpointController > 0 &&  YawSetpointController < 18)
                    {
                        error_limit = 150;
                    }
                    else
                    {
                        error_limit = 240;
                    }





                    if (XerrorController > -error_limit && XerrorController < error_limit && YerrorController > -error_limit && YerrorController < error_limit && YawerrorController > -8 && YawerrorController < 8)
                        // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                        // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                        {
                            if (YawSetpointController > 350)
                            {

                                NAV_START_CNT += 1;
                                if (NAV_START_CNT > 5)
                                {
                                    XoutputController = 0.0f;
                                    YoutputController = 0.0f;
                                    YawoutputController = 0.0f;

                                    if (NAV_START_CNT > 15)
                                    {

                                        NAV_START_FLAG = 3;
                                        NAV_START_CNT = 0;
                                    }
                                }

                            }

                            else
                            {
                                YawSetpointController = YawSetpointController + (int)CircleThethaDelta;
                                if (YawSetpointController > 360) YawSetpointController = 360;

                                CircleThetha = CircleThetha + CircleThethaDelta;
                                ZSetpointController = TAKE_OFF_ALT - (int)((CircleThetha - CircleThethaDelta) / 6);
                                if (CircleThetha > 725) CircleThetha = 726;
                            }






                        }
                        if (CircleThetha == 726)
                        {
                            XerrorController = 0;
                            YerrorController = 0;
                            ZerrorController = 0;
                            YawerrorController = 0;


                            var rbm = new BoolMsg();
                            rbm.value = false;


                            var LandingError = DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).SetLandingProtectionEnabledAsync(rbm);
                            var res = DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();



                        }




                        if (XoutputController > 0.25f) XoutputController = 0.25f;
                        if (XoutputController < -0.25f) XoutputController = -0.25f;

                        if (YoutputController > 0.25f) YoutputController = 0.25f;
                        if (YoutputController < -0.25f) YoutputController = -0.25f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }

                    if (NAV_START_FLAG == 3) //stop at zero degree
                    {
                        XSetpointController = (int)(XCircleCenter);
                        YSetpointController = (int)(YCircleCenter + CircleRadius);




                        XerrorController = XSetpointController - XpointController;
                        YerrorController = YSetpointController - YpointController;
                        ZerrorController = ZSetpointController - ZpointController;
                        YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0006f;// 0.0006f;
                        YKpController = 0.0006f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.04f;// 0.125f;// 0.04f;


                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));






                        if (XerrorController > -100 && XerrorController < 100 && YerrorController > -100 && YerrorController < 100 && YawerrorController > -8 && YawerrorController < 8)
                        // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                        // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                        {


                            NAV_START_CNT += 1;
                            if (NAV_START_CNT > 5)
                            {
                                XoutputController = 0.0f;
                                YoutputController = 0.0f;
                                YawoutputController = 0.0f;

                            if (NAV_START_CNT > 15)
                            {

                                NAV_START_FLAG = 4;
                                NAV_START_CNT = 0;
                                YawSetpointController = 360;

                                CircleThetha = 350;
                            }
                            }









                        }
                        if (CircleThetha == 726)
                        {
                            XerrorController = 0;
                            YerrorController = 0;
                            ZerrorController = 0;
                            YawerrorController = 0;


                            var rbm = new BoolMsg();
                            rbm.value = false;


                            var LandingError = DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).SetLandingProtectionEnabledAsync(rbm);
                            var res = DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();



                        }




                        if (XoutputController > 0.25f) XoutputController = 0.25f;
                        if (XoutputController < -0.25f) XoutputController = -0.25f;

                        if (YoutputController > 0.25f) YoutputController = 0.25f;
                        if (YoutputController < -0.25f) YoutputController = -0.25f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }


                    if (NAV_START_FLAG == 6)
                    {
                      //  XSetpointController = (int)(XCircleCenter);
                     //   YSetpointController = (int)(YCircleCenter) + (int)(CircleRadius - 480);

                    XSetpointController = (int)(XCircleCenter + ((650) * Math.Sin(Math.PI * CircleThetha / 180)));
                    YSetpointController = (int)(YCircleCenter + ((650) * Math.Cos(Math.PI * CircleThetha / 180)));





                    XerrorController = XSetpointController - XpointController;
                    YerrorController = YSetpointController - YpointController;
                    ZerrorController = ZSetpointController - ZpointController;
                    YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0005f;// 0.0006f;
                        YKpController = 0.0005f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.04f;// 0.125f;// 0.04f;


                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));








                        if (XerrorController > -150 && XerrorController < 150 && YerrorController > -150 && YerrorController < 150 && YawerrorController > -3 && YawerrorController < 3)
                        // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                        // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                        {

                            NAV_START_CNT += 1;
                            if (NAV_START_CNT > 5)
                            {
                                XoutputController = 0.0f;
                                YoutputController = 0.0f;
                                YawoutputController = 0.0f;

                                if (NAV_START_CNT > 30)
                                {

                                    NAV_START_FLAG = 7;
                                    NAV_START_CNT = 0;
                                }
                            }







                        }
                        if (CircleThetha == 726)
                        {
                            XerrorController = 0;
                            YerrorController = 0;
                            ZerrorController = 0;
                            YawerrorController = 0;


                            var rbm = new BoolMsg();
                            rbm.value = false;


                            var LandingError = DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).SetLandingProtectionEnabledAsync(rbm);
                            var res = DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();



                        }


                        if (XoutputController > 0.22f) XoutputController = 0.22f;
                        if (XoutputController < -0.22f) XoutputController = -0.22f;

                        if (YoutputController > 0.22f) YoutputController = 0.22f;
                        if (YoutputController < -0.22f) YoutputController = -0.22f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }
                    if (NAV_START_FLAG == 7)
                    {
                    XSetpointController = (int)(XCircleCenter + (CircleRadius * Math.Sin(Math.PI * CircleThetha / 180)));
                    YSetpointController = (int)(YCircleCenter + (CircleRadius * Math.Cos(Math.PI * CircleThetha / 180)));





                    XerrorController = XSetpointController - XpointController;
                        YerrorController = YSetpointController - YpointController;
                        ZerrorController = ZSetpointController - ZpointController;
                        YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0005f;// 0.0006f;
                        YKpController = 0.0005f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.04f;// 0.125f;// 0.04f;


                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));






                        if (XerrorController > -150 && XerrorController < 150 && YerrorController > -150 && YerrorController < 150 && YawerrorController > -3 && YawerrorController < 3)
                        // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                        // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                        {

                            //  NAV_START_FLAG = 4;
                            NAV_START_CNT += 1;
                            if (NAV_START_CNT > 5)
                            {


                                XoutputController = 0.0f;
                                YoutputController = 0.0f;
                                YawoutputController = 0.0f;

                                if (NAV_START_CNT > 30)
                                {

                                CircleThetha = 290;
                                YawSetpointController = 285;

                                NAV_START_FLAG = 8;
                                    NAV_START_CNT = 0;


                                }
                            }
                        }




                        if (XoutputController > 0.22f) XoutputController = 0.22f;
                        if (XoutputController < -0.22f) XoutputController = -0.22f;

                        if (YoutputController > 0.22f) YoutputController = 0.22f;
                        if (YoutputController < -0.22f) YoutputController = -0.22f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;


                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }



                    if (NAV_START_FLAG == 4)
                    {
                        XSetpointController = (int)(XCircleCenter + (CircleRadius * Math.Sin(Math.PI * CircleThetha / 180)));
                        YSetpointController = (int)(YCircleCenter + (CircleRadius * Math.Cos(Math.PI * CircleThetha / 180)));




                        XerrorController = XSetpointController - XpointController;
                        YerrorController = YSetpointController - YpointController;
                        ZerrorController = ZSetpointController - ZpointController;
                        YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0005f;// 0.0006f;
                        YKpController = 0.0005f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.027f;// 0.125f;// 0.04f;

                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));


                       if (YawSetpointController > 345  || YawSetpointController < 290)
                    {
                        error_limit = 140;
                    }
                    else
                    {
                        error_limit = 250;
                    }




                        if (XerrorController > -error_limit && XerrorController < error_limit && YerrorController > -error_limit && YerrorController < error_limit && YawerrorController > -4 && YawerrorController < 4)

                        {


                            YawSetpointController = YawSetpointController - (int)CircleThethaDelta;
                            if (YawSetpointController > 360) YawSetpointController = 0;

                            CircleThetha = CircleThetha - CircleThethaDelta;
                            ZSetpointController = TAKE_OFF_ALT - (int)((CircleThetha - CircleThethaDelta) / 6);
                            if (CircleThetha < 280)
                            {
                                CircleThetha = 280;
                                YawSetpointController = 280;

                                NAV_START_CNT += 1;
                                if (NAV_START_CNT > 5)
                                {


                                    XoutputController = 0.0f;
                                    YoutputController = 0.0f;
                                    YawoutputController = 0.0f;

                                    if (NAV_START_CNT > 20)
                                    {
                                        YawSetpointController = 285;
                                        CircleThetha = 280;

                                        NAV_START_FLAG = 5;
                                        NAV_START_CNT = 0;


                                    }
                                }

                            }




                        }




                        if (XoutputController > 0.22f) XoutputController = 0.22f;
                        if (XoutputController < -0.22f) XoutputController = -0.22f;

                        if (YoutputController > 0.22f) YoutputController = 0.22f;
                        if (YoutputController < -0.22f) YoutputController = -0.22f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }


                if (NAV_START_FLAG == 5)
                {
                    XSetpointController = (int)(XCircleCenter + (CircleRadius * Math.Sin(Math.PI * CircleThetha / 180)));
                    YSetpointController = (int)(YCircleCenter + (CircleRadius * Math.Cos(Math.PI * CircleThetha / 180)));





                    XerrorController = XSetpointController - XpointController;
                    YerrorController = YSetpointController - YpointController;
                    ZerrorController = ZSetpointController - ZpointController;
                    YawerrorController = YawSetpointController - YawpointController;

                    //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                    if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                    XKpController = 0.0005f;// 0.0006f;
                    YKpController = 0.0005f; // 0.0006f;
                    ZKpController = 0.02f;
                    YawKpController = 0.04f;// 0.125f;// 0.04f;


                    var XoutputController_temp = (float)(XerrorController) * XKpController;
                    var YoutputController_temp = (float)(YerrorController) * YKpController;
                    ZoutputController = (float)(ZerrorController) * ZKpController;
                    YawoutputController = (float)(YawerrorController) * YawKpController;


                    XoutputController_temp = XoutputController_temp;
                    YoutputController_temp = -YoutputController_temp;



                    XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                    YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));






                    if (XerrorController > -150 && XerrorController < 150 && YerrorController > -150 && YerrorController < 150 && YawerrorController > -3 && YawerrorController < 3)
                    // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                    // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                    {

                        //  NAV_START_FLAG = 4;
                        NAV_START_CNT += 1;
                        if (NAV_START_CNT > 5)
                        {


                            XoutputController = 0.0f;
                            YoutputController = 0.0f;
                            YawoutputController = 0.0f;

                            if (NAV_START_CNT > 20)
                            {

                                NAV_START_FLAG = 6;
                                NAV_START_CNT = 0;


                            }
                        }
                    }




                    if (XoutputController > 0.22f)  XoutputController = 0.22f;
                    if (XoutputController < -0.22f) XoutputController = -0.22f;

                    if (YoutputController > 0.22f)  YoutputController = 0.22f;
                    if (YoutputController < -0.22f) YoutputController = -0.22f;

                    if (YawoutputController > 0.25f)  YawoutputController = 0.25f;
                    if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                    if (ZoutputController > 0.20f)  ZoutputController = 0.20f;
                    if (ZoutputController < -0.20f) ZoutputController = -0.20f;


                    DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                }


                if (NAV_START_FLAG == 8)
                    {
                        XSetpointController = (int)(XCircleCenter + (CircleRadius * Math.Sin(Math.PI * CircleThetha / 180)));
                        YSetpointController = (int)(YCircleCenter + (CircleRadius * Math.Cos(Math.PI * CircleThetha / 180)));




                        XerrorController = XSetpointController - XpointController;
                        YerrorController = YSetpointController - YpointController;
                        ZerrorController = ZSetpointController - ZpointController;
                        YawerrorController = YawSetpointController - YawpointController;

                        //if (ZerrorController > -3 && ZerrorController < 3) ZerrorController = 0;


                        if (YawerrorController > 180 || YawerrorController < -180) YawerrorController = -YawerrorController;


                        XKpController = 0.0005f;// 0.0006f;
                        YKpController = 0.0005f; // 0.0006f;
                        ZKpController = 0.02f;
                        YawKpController = 0.027f;// 0.125f;// 0.04f;




                        var XoutputController_temp = (float)(XerrorController) * XKpController;
                        var YoutputController_temp = (float)(YerrorController) * YKpController;
                        ZoutputController = (float)(ZerrorController) * ZKpController;
                        YawoutputController = (float)(YawerrorController) * YawKpController;


                        XoutputController_temp = XoutputController_temp;
                        YoutputController_temp = -YoutputController_temp;



                        XoutputController = (float)((XoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))));
                        YoutputController = (float)((-XoutputController_temp * Math.Sin((double)(Math.PI * YawpointController / 180.0))) + (YoutputController_temp * Math.Cos((double)(Math.PI * YawpointController / 180.0))));





                    if (YawSetpointController > 345 || YawSetpointController < 295)
                    {
                        error_limit = 140;
                    }
                    else
                    {
                        error_limit = 250;
                    }




                    if (XerrorController > -error_limit && XerrorController < error_limit && YerrorController > -error_limit && YerrorController < error_limit && YawerrorController > -4 && YawerrorController < 4)
                        // if ( Math.Sqrt(Math.Pow( (double) (XerrorController),2) + Math.Pow( (double) (YerrorController),2) ) < 351  && YawerrorController > -10 && YawerrorController < 10)
                        // 351 = 2*CircleRadius*Math.Sin(Math.PI * (22.5/2) / 180)
                        {


                            YawSetpointController = YawSetpointController + (int)CircleThethaDelta;
                            if (YawSetpointController > 360) YawSetpointController = 0;

                            CircleThetha = CircleThetha + CircleThethaDelta;
                            ZSetpointController = TAKE_OFF_ALT - (int)((CircleThetha - CircleThethaDelta) / 6);

                            if (CircleThetha > 360)
                            {
                                CircleThetha = 360;
                                YawSetpointController = 0;

                                NAV_START_CNT += 1;
                                if (NAV_START_CNT > 5)
                                {



                                    if (NAV_START_CNT > 25)
                                    {

                                        NAV_START_CNT = 0;


                                        var rbm = new BoolMsg();
                                        rbm.value = false;


                                        var LandingError = DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).SetLandingProtectionEnabledAsync(rbm);
                                        var res = DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();



                                    }
                                }


                            }






                        }


                        if (XoutputController > 0.25f) XoutputController = 0.25f;
                        if (XoutputController < -0.25f) XoutputController = -0.25f;

                        if (YoutputController > 0.25f) YoutputController = 0.25f;
                        if (YoutputController < -0.25f) YoutputController = -0.25f;

                        if (YawoutputController > 0.25f) YawoutputController = 0.25f;
                        if (YawoutputController < -0.25f) YawoutputController = -0.25f;

                        if (ZoutputController > 0.20f) ZoutputController = 0.20f;
                        if (ZoutputController < -0.20f) ZoutputController = -0.20f;

                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, YoutputController, XoutputController);


                    }


               /* }
                else
                {

                    DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, 0, 0, 0);
                    Debug.WriteLine(" data lost");

                }*/



                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                 xout.Text = XoutputController.ToString();
                 yout.Text = YoutputController.ToString();
                 zout.Text = ZoutputController.ToString();
                 yawout.Text = YawoutputController.ToString(); 


                });




                var Dt = System.DateTime.Now - last_time;
                last_time = System.DateTime.Now;

                // DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(0, -YawoutputController, -XoutputController, YoutputController);
                //  DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(ZoutputController, 0, 0, 0);




                // Debug.WriteLine("time duration : " + Dt.Milliseconds.ToString());

                // Debug.WriteLine("update rc");
                 Debug.WriteLine("NAV_START_FLAG: " + NAV_START_FLAG);

                
                Thread.Sleep(40);




            }


        }

        private void XchangeSetPoint(object sender, TextChangedEventArgs e)
        {


        }
        private void YchangeSetPoint(object sender, TextChangedEventArgs e)
        {
        

        }
        private void YawchangeSetPoint(object sender, TextChangedEventArgs e)
        {


        }
        private void ZchangeSetPoint(object sender, TextChangedEventArgs e)
        {


        }


        private void SetpointButton_Click(object sender, RoutedEventArgs e)
        {
            xpoint.Text = XchangeSetpointText.Text;
            ypoint.Text = YchangeSetpointText.Text;

           // Int32.TryParse(XchangeSetpointText.Text, out XSetpointController);
           // Int32.TryParse(YchangeSetpointText.Text, out YSetpointController);
           // Int32.TryParse(ZchangeSetpointText.Text, out ZSetpointController);
           // Int32.TryParse(YawchangeSetpointText.Text, out YawSetpointController);








        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }



}
