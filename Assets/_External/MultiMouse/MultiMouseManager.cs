using MultiMouse.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MultiMouse
{
    public class MultiMouseManager : MonoBehaviour, IDisposable
    {
        private static readonly string logHeader = "<color=white>MultiMouse</color> -";
        private static MultiMouseManager instance = (MultiMouseManager)null;
        [SerializeField]
        private bool initializeOnStart;
        [SerializeField]
        private Canvas canvasToClamp;
        [SerializeField]
        private bool cursorVisibleOnInitialize;
        [SerializeField]
        private CursorLockMode cursorLockModeOnInitialize = CursorLockMode.Locked;
        [SerializeField]
        private CursorSpawnMode cursorSpawnMode;
        [SerializeField]
        [Tooltip("Position of a pixel on the screen.")]
        private Vector2 cursorSpawnPositionPixel = Vector2.zero;
        [SerializeField]
        [Tooltip("From 0 to 1, where 0 is bottom-left corner and 1 is top-right.")]
        private Vector2 cursorSpawnPositionNormalized = Vector2.zero;
        private IntPtr newWndProc = IntPtr.Zero;
        private IntPtr hwnd = IntPtr.Zero;
        private bool requireRefresh;
        private bool wasRunning = true;
        private bool outOfFocus;
        private CursorLockMode preInitCursorLockMode;
        private bool preInitCursorVisibility;
        private List<MultiMouseDefs.RAWINPUTDEVICELIST> deviceList = new List<MultiMouseDefs.RAWINPUTDEVICELIST>();
        private Queue<MultiMouseDevice> activatedMice = new Queue<MultiMouseDevice>();
        private List<MultiMouseDevice> connectedMice = new List<MultiMouseDevice>();
        public List<MultiMouseDevice> Mice = new List<MultiMouseDevice>();

        public event MultiMouseManager.MouseChangedDelegate OnMouseConnected;

        public event MultiMouseManager.MouseChangedDelegate OnMouseDisconnected;

        public static MultiMouseManager Instance
        {
            get
            {
                if ((UnityEngine.Object)MultiMouseManager.instance == (UnityEngine.Object)null)
                    MultiMouseManager.instance = UnityEngine.Object.FindObjectOfType<MultiMouseManager>();
                return MultiMouseManager.instance;
            }
        }
        public Architecture Architecture => IntPtr.Size == 8 ? Architecture.x64 : Architecture.x86;
        public bool IsRunning { get; private set; }

        private void Awake() => Application.wantsToQuit += new Func<bool>(this.Application_wantsToQuit);

        private void Start()
        {
            if (!this.initializeOnStart)
                return;
            if (this.outOfFocus)
                this.wasRunning = true;
            else
                this.Initialize();
        }

        private void Update()
        {
            if (this.requireRefresh)
            {
                this.GetInputDevices();
                this.GetMice();
                this.requireRefresh = false;
            }
            while (this.activatedMice.Count > 0)
            {
                MultiMouseDevice mouse = this.activatedMice.Dequeue();
                this.SetMouseInitialPosition(mouse);
                this.Mice.Add(mouse);
                MultiMouseManager.MouseChangedDelegate onMouseConnected = this.OnMouseConnected;
                if (onMouseConnected != null)
                    onMouseConnected(mouse);
            }
        }

        private void LateUpdate()
        {
            foreach (MultiMouseDevice mouse in this.Mice)
            {
                mouse.Delta = Vector2.zero;
                int buttonCount = mouse.ButtonCount;
                for (int buttonId = 0; buttonId < buttonCount; ++buttonId)
                    mouse.SetLastButtonState(buttonId, mouse.GetMouseButton(buttonId));
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            this.outOfFocus = !focus;
            if (!focus && this.IsRunning)
            {
                this.Dispose();
                this.wasRunning = true;
            }
            if (!focus || !this.wasRunning)
                return;
            this.Initialize();
            this.wasRunning = false;
        }

        public void Initialize()
        {
            if (MultiMouseManager.Instance.IsRunning)
                return;
            Application.wantsToQuit += new Func<bool>(this.Application_wantsToQuit);
            this.preInitCursorLockMode = Cursor.lockState;
            this.preInitCursorVisibility = Cursor.visible;
            Cursor.lockState = this.cursorLockModeOnInitialize;
            Cursor.visible = this.cursorVisibleOnInitialize;
            Debug.Log((object)(MultiMouseManager.logHeader + " Architecture: " + (this.Architecture == Architecture.x64 ? "x64" : "x86")));
            this.hwnd = MultiMouseApi.GetActiveWindow();
            if (this.hwnd == IntPtr.Zero)
            {
                Debug.Log((object)(MultiMouseManager.logHeader + " Windows handle is null"));
            }
            else
            {
                Debug.Log((object)string.Format("{0} Windows handle: {1}", (object)MultiMouseManager.logHeader, (object)this.hwnd.ToInt64()));
                if (!this.SetWndProd())
                    this.Dispose();
                else if (!this.RegisterMice())
                    this.Dispose();
                else
                    this.requireRefresh = true;
            }
        }

        private bool SetWndProd()
        {
            this.newWndProc = Marshal.GetFunctionPointerForDelegate<MultiMouseManager.WndProcDelegate>(new MultiMouseManager.WndProcDelegate(MultiMouseManager.WndProc));
            try
            {
                IntPtr result = MultiMouseApi.SetWndProc(this.hwnd, this.newWndProc, IntPtr.Zero, IntPtr.Zero);
                if (result == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    Debug.LogError((object)(MultiMouseManager.logHeader + $" Couldn't change the WndProc to a new one; disposing: {error}"));
                    return false;
                }
                else
                {
                    this.IsRunning = true;
                    Debug.Log((object)(MultiMouseManager.logHeader + " Setting WndProd successful"));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError((object)(MultiMouseManager.logHeader + $" Couldn't change the WndProc to a new one; disposing: {ex}"));
                return false;
            }
        }

        private bool RegisterMice()
        {
            MultiMouseDefs.RAWINPUTDEVICE structure = new MultiMouseDefs.RAWINPUTDEVICE()
            {
                usagePage = MultiMouseDefs.HIDUsagePage.Generic,
                usage = MultiMouseDefs.HIDUsage.Mouse,
                flags = MultiMouseDefs.RawInputDeviceFlags.None,
                windowHandle = this.hwnd
            };
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MultiMouseDefs.RAWINPUTDEVICE)));
            try
            {
                Marshal.StructureToPtr<MultiMouseDefs.RAWINPUTDEVICE>(structure, num1, true);
                IntPtr num2 = MultiMouseApi.RegisterRawInputDevices(num1, (IntPtr)1, (IntPtr)Marshal.SizeOf(typeof(MultiMouseDefs.RAWINPUTDEVICE)));
                bool error = num2.ToInt64() == 0L;
                Debug.Log((object)(MultiMouseManager.logHeader + " Registering Mice " + (error ? "failed" : "successful")));
                if (error)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Debug.Log((object)(MultiMouseManager.logHeader + $" Error code: {errorCode}"));
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError((object)(MultiMouseManager.logHeader + $" Couldn't register mice: {ex}"));
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(num1);
            }
        }

        private void GetInputDevices()
        {
            IntPtr cbSize = (IntPtr)Marshal.SizeOf(typeof(MultiMouseDefs.RAWINPUTDEVICELIST));
            this.deviceList.Clear();
            IntPtr puiNumDevices;
            MultiMouseApi.GetRawInputDeviceList(IntPtr.Zero, out puiNumDevices, cbSize);
            if (puiNumDevices == IntPtr.Zero)
            {
                Debug.Log((object)(MultiMouseManager.logHeader + " No devices found."));
            }
            else
            {
                IntPtr num = Marshal.AllocHGlobal(puiNumDevices.ToInt32() * cbSize.ToInt32());
                if (MultiMouseApi.GetRawInputDeviceList(num, out puiNumDevices, cbSize) == IntPtr.Zero)
                {
                    Debug.Log((object)(MultiMouseManager.logHeader + " Couldn't fill the device list"));
                    Marshal.FreeHGlobal(num);
                }
                else
                {
                    for (int index = 0; index < puiNumDevices.ToInt32(); ++index)
                        this.deviceList.Add(Marshal.PtrToStructure<MultiMouseDefs.RAWINPUTDEVICELIST>(num + index * cbSize.ToInt32()));
                    Marshal.FreeHGlobal(num);
                }
            }
        }

        private void GetMice()
        {
            List<MultiMouseDevice> multiMouseDeviceList = new List<MultiMouseDevice>();
            foreach (MultiMouseDefs.RAWINPUTDEVICELIST device1 in this.deviceList)
            {
                MultiMouseDefs.RAWINPUTDEVICELIST device = device1;
                if (device.deviceType == MultiMouseDefs.RawInputDeviceType.Mouse)
                {
                    int index = this.connectedMice.FindIndex((Predicate<MultiMouseDevice>)(x => x.Handle == device.deviceHandle));
                    if (index != -1)
                    {
                        multiMouseDeviceList.Add(this.connectedMice[index]);
                    }
                    else
                    {
                        string mouseName = this.GetMouseName(device);
                        if (mouseName != null)
                        {
                            MultiMouseDevice mouseModel = this.GetMouseModel(device, mouseName);
                            multiMouseDeviceList.Add(mouseModel);
                        }
                    }
                }
            }
            for (int index = 0; index < this.Mice.Count; ++index)
            {
                MultiMouseDevice mouse = this.Mice[index];
                if (!multiMouseDeviceList.Contains(mouse))
                {
                    this.Mice.RemoveAt(index);
                    --index;
                    MultiMouseManager.MouseChangedDelegate mouseDisconnected = this.OnMouseDisconnected;
                    if (mouseDisconnected != null)
                        mouseDisconnected(mouse);
                }
            }
            this.connectedMice = multiMouseDeviceList;
        }

        private string GetMouseName(MultiMouseDefs.RAWINPUTDEVICELIST device)
        {
            IntPtr pcbSize = IntPtr.Zero;
            MultiMouseApi.GetRawInputDeviceInfo(device.deviceHandle, MultiMouseDefs.UiCommand.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
            pcbSize = new IntPtr(pcbSize.ToInt64() * 2L);
            IntPtr num = Marshal.AllocHGlobal(pcbSize);
            try
            {
                MultiMouseApi.GetRawInputDeviceInfo(device.deviceHandle, MultiMouseDefs.UiCommand.RIDI_DEVICENAME, num, ref pcbSize);
                return Marshal.PtrToStringAuto(num);
            }
            catch (Exception ex)
            {
                Debug.Log((object)(MultiMouseManager.logHeader + $" Couldn't retrieve the name of a mouse device: {ex}"));
                return (string)null;
            }
            finally
            {
                Marshal.FreeHGlobal(num);
            }
        }

        private MultiMouseDevice GetMouseModel(
          MultiMouseDefs.RAWINPUTDEVICELIST device,
          string mouseName)
        {
            IntPtr zero = IntPtr.Zero;
            MultiMouseApi.GetRawInputDeviceInfo(device.deviceHandle, MultiMouseDefs.UiCommand.RIDI_DEVICEINFO, IntPtr.Zero, ref zero);
            MultiMouseDefs.DeviceInfo devInfo = new MultiMouseDefs.DeviceInfo();
            devInfo.Size = Marshal.SizeOf(typeof(MultiMouseDefs.DeviceInfo));
            MultiMouseApi.GetRawInputDeviceInfo(device.deviceHandle, MultiMouseDefs.UiCommand.RIDI_DEVICEINFO, ref devInfo, ref zero);
            return new MultiMouseDevice(device.deviceHandle, mouseName, (int)devInfo.MouseInfo.NumberOfButtons);
        }

        private static void UpdateMouseInput(IntPtr deviceHandle)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr cbSizeHeader = (IntPtr)Marshal.SizeOf(typeof(MultiMouseDefs.RawInputHeader));
            MultiMouseApi.GetRawInputData(deviceHandle, MultiMouseDefs.RawInputCommand.Input, IntPtr.Zero, ref zero, cbSizeHeader);
            MultiMouseDefs.RawInput rawInput = new MultiMouseDefs.RawInput();
            MultiMouseApi.GetRawInputData(deviceHandle, MultiMouseDefs.RawInputCommand.Input, ref rawInput, ref zero, cbSizeHeader);
            foreach (MultiMouseDevice connectedMouse in MultiMouseManager.Instance.connectedMice)
            {
                if (!(connectedMouse.Handle != rawInput.Header.Device))
                {
                    if (!connectedMouse.IsActive)
                    {
                        connectedMouse.IsActive = true;
                        MultiMouseManager.Instance.activatedMice.Enqueue(connectedMouse);
                    }
                    connectedMouse.Delta = new Vector2((float)rawInput.Data.Mouse.LastX, (float)-rawInput.Data.Mouse.LastY);
                    connectedMouse.Position += connectedMouse.Delta;
                    if ((UnityEngine.Object)MultiMouseManager.Instance.canvasToClamp != (UnityEngine.Object)null)
                    {
                        Vector2 size = MultiMouseManager.Instance.canvasToClamp.pixelRect.size;
                        connectedMouse.Position = Vector2.Min(size, Vector2.Max(Vector2.zero, connectedMouse.Position));
                    }
                    Vector2 scrollDelta = connectedMouse.ScrollDelta;
                    scrollDelta.y = (float)rawInput.Data.Mouse.Data.ButtonData;
                    connectedMouse.ScrollDelta = scrollDelta;
                    int buttonCount = connectedMouse.ButtonCount;
                    for (int buttonId = 0; buttonId < buttonCount; ++buttonId)
                    {
                        if ((rawInput.Data.Mouse.Data.ButtonFlags & (MultiMouseDefs.RawMouseButtons)(1 << buttonId * 2)) > MultiMouseDefs.RawMouseButtons.None)
                            connectedMouse.SetButtonState(buttonId, true);
                        if ((rawInput.Data.Mouse.Data.ButtonFlags & (MultiMouseDefs.RawMouseButtons)(1 << buttonId * 2 + 1)) > MultiMouseDefs.RawMouseButtons.None)
                            connectedMouse.SetButtonState(buttonId, false);
                    }
                }
            }
        }

        private static IntPtr WndProc(
          IntPtr hWnd,
          uint msg,
          IntPtr wParam,
          IntPtr lParam,
          IntPtr subclassId,
          IntPtr referenceData)
        {
            switch ((MultiMouseDefs.WindowsMessages)msg)
            {
                case MultiMouseDefs.WindowsMessages.INPUT:
                    MultiMouseManager.UpdateMouseInput(lParam);
                    break;
                case MultiMouseDefs.WindowsMessages.DEVICECHANGE:
                    MultiMouseManager.Instance.requireRefresh = true;
                    break;
            }
            IntPtr zero = IntPtr.Zero;
            try
            {
                return MultiMouseApi.DefWndProc(hWnd, msg, wParam, lParam);
            }
            catch (Exception ex)
            {
                Debug.LogError((object)(MultiMouseManager.logHeader + $" Couldn't call unity window proc; disposing: {ex}"));
                MultiMouseManager.Instance.Dispose();
                return zero;
            }
        }

        private void SetMouseInitialPosition(MultiMouseDevice mouse)
        {
            Vector2 vector2_1 = new Vector2((float)Screen.width, (float)Screen.height);
            Vector2 vector2_2;
            switch (this.cursorSpawnMode)
            {
                case CursorSpawnMode.CustomPosition:
                    vector2_2 = this.cursorSpawnPositionPixel;
                    break;
                case CursorSpawnMode.CustomPositionNormalized:
                    vector2_2 = vector2_1 * this.cursorSpawnPositionNormalized;
                    break;
                case CursorSpawnMode.LastMousePositionOrCenter:
                    vector2_2 = this.Mice.Count != 0 ? this.Mice[this.Mice.Count - 1].Position : vector2_1 * 0.5f;
                    break;
                case CursorSpawnMode.LastMousePositionOrCustomPosition:
                    vector2_2 = this.Mice.Count != 0 ? this.Mice[this.Mice.Count - 1].Position : this.cursorSpawnPositionPixel;
                    break;
                case CursorSpawnMode.LastMousePositionOrCustomNormalized:
                    vector2_2 = this.Mice.Count != 0 ? this.Mice[this.Mice.Count - 1].Position : vector2_1 * this.cursorSpawnPositionNormalized;
                    break;
                default:
                    vector2_2 = vector2_1 * 0.5f;
                    break;
            }
            mouse.Position = vector2_2;
        }

        private bool Application_wantsToQuit()
        {
            if (!this.IsRunning)
                return true;
            Debug.Log((object)(MultiMouseManager.logHeader + " Disposing Multi Mouse"));
            this.StartCoroutine(this.ForceQuit());
            return false;
        }

        private IEnumerator ForceQuit()
        {
            this.Dispose();
            yield return (object)null;
            Application.Quit();
        }

        public void Dispose()
        {
            if (!this.IsRunning)
                return;
            MultiMouseApi.RemoveWndProc(this.hwnd, this.newWndProc, IntPtr.Zero);
            Cursor.lockState = this.preInitCursorLockMode;
            Cursor.visible = this.preInitCursorVisibility;
            this.IsRunning = false;
            Debug.Log((object)(MultiMouseManager.logHeader + " Multi Mouse Disposed"));
        }

        public delegate void MouseChangedDelegate(MultiMouseDevice mouse);

        private delegate IntPtr WndProcDelegate(
          IntPtr hWnd,
          uint msg,
          IntPtr wParam,
          IntPtr lParam,
          IntPtr subclassId,
          IntPtr referenceData);
    }
}
