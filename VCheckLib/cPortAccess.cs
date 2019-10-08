using System;
using System.Runtime.InteropServices;

namespace VCheckLib
{
    public class cPortAccess
    {
        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Output(int adress, int value);
    }
}
