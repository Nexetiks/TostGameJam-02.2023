using System;
using System.Runtime.InteropServices;

namespace MultiMouse.Internal
{
    public static class MultiMouseApi
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("ComCtl32.dll", EntryPoint = "SetWindowSubclass")]
        public static extern IntPtr SetWndProc(
          IntPtr hWnd,
          IntPtr wndProc,
          IntPtr subclassId,
          IntPtr referenceData);

        [DllImport("ComCtl32.dll", EntryPoint = "RemoveWindowSubclass")]
        public static extern IntPtr RemoveWndProc(IntPtr hWnd, IntPtr wndProc, IntPtr subclassId);

        [DllImport("ComCtl32.dll", EntryPoint = "DefSubclassProc")]
        public static extern IntPtr DefWndProc(
          IntPtr hWnd,
          uint msg,
          IntPtr wParam,
          IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr RegisterRawInputDevices(
          [In] IntPtr pcRawInputDevices,
          [In] IntPtr uiNumDevices,
          [In] IntPtr cbSize);

        [DllImport("User32.Dll")]
        public static extern IntPtr GetRawInputDeviceList(
          IntPtr pRawInputDeviceList,
          out IntPtr puiNumDevices,
          IntPtr cbSize);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetRawInputDeviceInfo(
          [In] IntPtr hDevice,
          [In] MultiMouseDefs.UiCommand uiCommand,
          IntPtr pMouseInfo,
          [In, Out] ref IntPtr pcbSize);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetRawInputDeviceInfo(
          [In] IntPtr hDevice,
          [In] MultiMouseDefs.UiCommand uiCommand,
          [In, Out] ref MultiMouseDefs.DeviceInfo devInfo,
          [In, Out] ref IntPtr pcbSize);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetRawInputData(
          [In] IntPtr hRawInput,
          [In] MultiMouseDefs.RawInputCommand uiCommand,
          IntPtr pRawInput,
          [In, Out] ref IntPtr pcbSize,
          [In] IntPtr cbSizeHeader);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetRawInputData(
          [In] IntPtr hRawInput,
          [In] MultiMouseDefs.RawInputCommand uiCommand,
          [In, Out] ref MultiMouseDefs.RawInput rawInput,
          [In, Out] ref IntPtr pcbSize,
          [In] IntPtr cbSizeHeader);
    }
}
