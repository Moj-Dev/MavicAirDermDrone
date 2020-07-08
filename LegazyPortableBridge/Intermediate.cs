using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegazyPortableBridge
{
    public class Intermediate
    {



        private static string z_location;
        public static string Z_location_
        {
            get { return z_location; }
            set { z_location = value; }
        }

        public static string Z_updating()
        {
            return z_location;
        }

        public static void Z_feeding(string new_z)
        {
            z_location = new_z;
        }


    }
}
