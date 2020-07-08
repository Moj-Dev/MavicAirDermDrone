using System;
using ProtoBuf;



//using WebEye.Controls.Wpf.StreamPlayerControl;


namespace ClassLibrary1
{


    public class bridge_com
    {


        [ProtoContract]
        public class Person
        {
            [ProtoMember(1)]
            public int x_pose { get; set; }

            [ProtoMember(2)]
            public int y_pose { get; set; }
        }





        // public  string AppName { get; set; }
        private static string AppUrl { get; set; }
                public static float X_pos { get; set; }
                public static float Y_pos { get; set; }


                private static string z_position ;
                public static string Z_position_
                {
                    get { return z_position;   }
                    set { z_position = value;  }
                } 


                public bridge_com(string input)
                {
                    z_position = input;
                }
                public static string Z_updating()
                {  
                    return z_position;
                }

                public static void Z_feeding(string new_z)
                {
                    z_position = new_z;
                }


            











    }





}
