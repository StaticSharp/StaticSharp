using EnvDTE;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace StaticSharp.Gears;

public class VisualStudio {

    public static class Marshal2 {
        internal const string OLEAUT32 = "oleaut32.dll";
        internal const string OLE32 = "ole32.dll";

        [SecurityCritical]  // auto-generated_required
        public static object GetActiveObject(string progID) {
            //Object obj = null;
            Guid clsid;

            // Call CLSIDFromProgIDEx first then fall back on CLSIDFromProgID if
            // CLSIDFromProgIDEx doesn't exist.
            try {
                CLSIDFromProgIDEx(progID, out clsid);
            }
            //            catch
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                CLSIDFromProgID(progID, out clsid);
            }

            GetActiveObject(ref clsid, IntPtr.Zero, out var obj);
            return obj;
        }

        //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
        [DllImport(OLE32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]  // auto-generated
        private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
        [DllImport(OLE32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]  // auto-generated
        private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        //[DllImport(Microsoft.Win32.Win32Native.OLEAUT32, PreserveSig = false)]
        [DllImport(OLEAUT32, PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]  // auto-generated
        private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }


    static class Windows {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(nint hWnd);

    }


    public static bool Open(string filePath, int line) {
        try {
            var dte = Marshal2.GetActiveObject("VisualStudio.DTE") as DTE;
            if (dte == null)
                return false;
            var hwnd = dte.MainWindow.HWnd;
            Windows.SetForegroundWindow(hwnd);
            dte.ExecuteCommand("File.OpenFile", filePath);
            dte.ExecuteCommand("Edit.GoTo", line.ToString());

            var selection = dte.ActiveDocument.Selection as EnvDTE.TextSelection;
            selection?.GotoLine(line, true);

            /*if (!selection.ActivePoint.AtStartOfLine)
                selection.StartOfLine();
            selection.LineDown(true, 1);*/

            return true;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
