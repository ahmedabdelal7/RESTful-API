using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;

class Program
{
    [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern int MessageBox(IntPtr hWnd,  String Msg, String Caption, int Type);
    static void Main()
    {
        MessageBox(IntPtr.Zero, "Hello World!", "No Caption",0);

    }


}
