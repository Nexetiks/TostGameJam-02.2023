using System;
using System.Runtime.InteropServices;

namespace MultiMouse.Internal
{
    public class MultiMouseDefs
    {
        public enum HIDUsagePage : ushort
        {
            Undefined = 0,
            Generic = 1,
            Simulation = 2,
            VR = 3,
            Sport = 4,
            Game = 5,
            Keyboard = 7,
            LED = 8,
            Button = 9,
            Ordinal = 10, // 0x000A
            Telephony = 11, // 0x000B
            Consumer = 12, // 0x000C
            Digitizer = 13, // 0x000D
            PID = 15, // 0x000F
            Unicode = 16, // 0x0010
            AlphaNumeric = 20, // 0x0014
            Medical = 64, // 0x0040
            MonitorPage0 = 128, // 0x0080
            MonitorPage1 = 129, // 0x0081
            MonitorPage2 = 130, // 0x0082
            MonitorPage3 = 131, // 0x0083
            PowerPage0 = 132, // 0x0084
            PowerPage1 = 133, // 0x0085
            PowerPage2 = 134, // 0x0086
            PowerPage3 = 135, // 0x0087
            BarCode = 140, // 0x008C
            Scale = 141, // 0x008D
            MSR = 142, // 0x008E
        }

        public enum HIDUsage : ushort
        {
            KeyboardNoEvent = 0,
            KeyboardRollover = 1,
            LEDNumLock = 1,
            Pointer = 1,
            TelephonyPhone = 1,
            KeyboardPostFail = 2,
            LEDCapsLock = 2,
            Mouse = 2,
            TelephonyAnsweringMachine = 2,
            KeyboardUndefined = 3,
            LEDScrollLock = 3,
            TelephonyMessageControls = 3,
            Joystick = 4,
            KeyboardaA = 4,
            LEDCompose = 4,
            TelephonyHandset = 4,
            Gamepad = 5,
            LEDKana = 5,
            TelephonyHeadset = 5,
            Keyboard = 6,
            LEDPower = 6,
            TelephonyKeypad = 6,
            Keypad = 7,
            LEDShift = 7,
            TelephonyProgrammableButton = 7,
            LEDDoNotDisturb = 8,
            LEDMute = 9,
            LEDToneEnable = 10, // 0x000A
            LEDHighCutFilter = 11, // 0x000B
            LEDLowCutFilter = 12, // 0x000C
            LEDEqualizerEnable = 13, // 0x000D
            LEDSoundFieldOn = 14, // 0x000E
            LEDSurroundFieldOn = 15, // 0x000F
            LEDRepeat = 16, // 0x0010
            LEDStereo = 17, // 0x0011
            LEDSamplingRateDirect = 18, // 0x0012
            LEDSpinning = 19, // 0x0013
            LEDCAV = 20, // 0x0014
            LEDCLV = 21, // 0x0015
            LEDRecordingFormatDet = 22, // 0x0016
            LEDOffHook = 23, // 0x0017
            LEDRing = 24, // 0x0018
            LEDMessageWaiting = 25, // 0x0019
            LEDDataMode = 26, // 0x001A
            LEDBatteryOperation = 27, // 0x001B
            LEDBatteryOK = 28, // 0x001C
            KeyboardzZ = 29, // 0x001D
            LEDBatteryLow = 29, // 0x001D
            Keyboard1 = 30, // 0x001E
            LEDSpeaker = 30, // 0x001E
            LEDHeadset = 31, // 0x001F
            LEDHold = 32, // 0x0020
            LEDMicrophone = 33, // 0x0021
            LEDCoverage = 34, // 0x0022
            LEDNightMode = 35, // 0x0023
            LEDSendCalls = 36, // 0x0024
            LEDCallPickup = 37, // 0x0025
            LEDConference = 38, // 0x0026
            Keyboard0 = 39, // 0x0027
            LEDStandBy = 39, // 0x0027
            KeyboardReturn = 40, // 0x0028
            LEDCameraOn = 40, // 0x0028
            KeyboardEscape = 41, // 0x0029
            LEDCameraOff = 41, // 0x0029
            KeyboardDelete = 42, // 0x002A
            LEDOnLine = 42, // 0x002A
            LEDOffLine = 43, // 0x002B
            LEDBusy = 44, // 0x002C
            LEDReady = 45, // 0x002D
            LEDPaperOut = 46, // 0x002E
            LEDPaperJam = 47, // 0x002F
            LEDRemote = 48, // 0x0030
            X = 48, // 0x0030
            LEDForward = 49, // 0x0031
            Y = 49, // 0x0031
            LEDReverse = 50, // 0x0032
            Z = 50, // 0x0032
            LEDStop = 51, // 0x0033
            RelativeX = 51, // 0x0033
            LEDRewind = 52, // 0x0034
            RelativeY = 52, // 0x0034
            LEDFastForward = 53, // 0x0035
            RelativeZ = 53, // 0x0035
            LEDPlay = 54, // 0x0036
            Slider = 54, // 0x0036
            Dial = 55, // 0x0037
            LEDPause = 55, // 0x0037
            LEDRecord = 56, // 0x0038
            Wheel = 56, // 0x0038
            HatSwitch = 57, // 0x0039
            KeyboardCapsLock = 57, // 0x0039
            LEDError = 57, // 0x0039
            CountedBuffer = 58, // 0x003A
            KeyboardF1 = 58, // 0x003A
            LEDSelectedIndicator = 58, // 0x003A
            ByteCount = 59, // 0x003B
            LEDGenericIndicator = 59, // 0x003B
            LEDInUseIndicator = 59, // 0x003B
            LEDMultiModeIndicator = 60, // 0x003C
            MotionWakeup = 60, // 0x003C
            LEDIndicatorOn = 61, // 0x003D
            LEDIndicatorFlash = 62, // 0x003E
            LEDIndicatorSlowBlink = 63, // 0x003F
            LEDIndicatorFastBlink = 64, // 0x0040
            VX = 64, // 0x0040
            LEDIndicatorOff = 65, // 0x0041
            VY = 65, // 0x0041
            LEDFlashOnTime = 66, // 0x0042
            VZ = 66, // 0x0042
            LEDSlowBlinkOnTime = 67, // 0x0043
            VBRX = 67, // 0x0043
            LEDSlowBlinkOffTime = 68, // 0x0044
            VBRY = 68, // 0x0044
            KeyboardF12 = 69, // 0x0045
            LEDFastBlinkOnTime = 69, // 0x0045
            VBRZ = 69, // 0x0045
            KeyboardPrintScreen = 70, // 0x0046
            LEDFastBlinkOffTime = 70, // 0x0046
            VNO = 70, // 0x0046
            KeyboardScrollLock = 71, // 0x0047
            LEDIndicatorColor = 71, // 0x0047
            LEDRed = 72, // 0x0048
            LEDGreen = 73, // 0x0049
            LEDAmber = 74, // 0x004A
            KeyboardNumLock = 83, // 0x0053
            SystemControl = 128, // 0x0080
            SystemControlPower = 129, // 0x0081
            SystemControlSleep = 130, // 0x0082
            SystemControlWake = 131, // 0x0083
            SystemControlContextMenu = 132, // 0x0084
            SystemControlMainMenu = 133, // 0x0085
            SystemControlApplicationMenu = 134, // 0x0086
            SystemControlHelpMenu = 135, // 0x0087
            SystemControlMenuExit = 136, // 0x0088
            SystemControlMenuSelect = 137, // 0x0089
            SystemControlMenuRight = 138, // 0x008A
            SystemControlMenuLeft = 139, // 0x008B
            SystemControlMenuUp = 140, // 0x008C
            SystemControlMenuDown = 141, // 0x008D
            SimulationRudder = 186, // 0x00BA
            SimulationThrottle = 187, // 0x00BB
            KeyboardLeftControl = 224, // 0x00E0
            KeyboardLeftShift = 225, // 0x00E1
            KeyboardLeftALT = 226, // 0x00E2
            KeyboardLeftGUI = 227, // 0x00E3
            KeyboardRightControl = 228, // 0x00E4
            KeyboardRightShift = 229, // 0x00E5
            KeyboardRightALT = 230, // 0x00E6
            KeyboardRightGUI = 231, // 0x00E7
        }

        [Flags]
        public enum RawInputDeviceFlags : uint
        {
            None = 0,
            Remove = 1,
            Exclude = 16, // 0x00000010
            PageOnly = 32, // 0x00000020
            NoLegacy = PageOnly | Exclude, // 0x00000030
            InputSink = 256, // 0x00000100
            CaptureMouse = 512, // 0x00000200
            NoHotKeys = CaptureMouse, // 0x00000200
            AppKeys = 1024, // 0x00000400
        }

        public enum RawInputDeviceType
        {
            Mouse,
            Keyboard,
            HID,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RAWINPUTDEVICE
        {
            public MultiMouseDefs.HIDUsagePage usagePage;
            public MultiMouseDefs.HIDUsage usage;
            public MultiMouseDefs.RawInputDeviceFlags flags;
            public IntPtr windowHandle;
        }

        public struct RAWINPUTDEVICELIST
        {
            public IntPtr deviceHandle;
            public MultiMouseDefs.RawInputDeviceType deviceType;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct DeviceInfo
        {
            [FieldOffset(0)]
            public int Size;
            [FieldOffset(4)]
            public int Type;
            [FieldOffset(8)]
            public MultiMouseDefs.DeviceInfoMouse MouseInfo;
            [FieldOffset(8)]
            public MultiMouseDefs.DeviceInfoKeyboard KeyboardInfo;
            [FieldOffset(8)]
            public MultiMouseDefs.DeviceInfoHID HIDInfo;
        }

        public struct DeviceInfoMouse
        {
            public uint ID;
            public uint NumberOfButtons;
            public uint SampleRate;
        }

        public struct DeviceInfoKeyboard
        {
            public uint Type;
            public uint SubType;
            public uint KeyboardMode;
            public uint NumberOfFunctionKeys;
            public uint NumberOfIndicators;
            public uint NumberOfKeysTotal;
        }

        public struct DeviceInfoHID
        {
            public uint VendorID;
            public uint ProductID;
            public uint VersionNumber;
            public ushort UsagePage;
            public ushort Usage;
        }

        public enum UiCommand
        {
            RIDI_PREPARSEDDATA = 536870917, // 0x20000005
            RIDI_DEVICENAME = 536870919, // 0x20000007
            RIDI_DEVICEINFO = 536870923, // 0x2000000B
        }

        public enum WindowsMessages
        {
            INPUT = 255, // 0x000000FF
            DEVICECHANGE = 537, // 0x00000219
        }

        public enum RawInputType
        {
            Mouse,
            Keyboard,
            HID,
            Other,
        }

        public struct RawInputHeader
        {
            public MultiMouseDefs.RawInputType Type;
            public int Size;
            public IntPtr Device;
            public IntPtr wParam;
        }

        [Flags]
        public enum RawKeyboardFlags : ushort
        {
            KeyMake = 0,
            KeyBreak = 1,
            KeyE0 = 2,
            KeyE1 = 4,
            TerminalServerSetLED = 8,
            TerminalServerShadow = 16, // 0x0010
            TerminalServerVKPACKET = 32, // 0x0020
        }

        public enum VirtualKeys : ushort
        {
            LeftButton = 1,
            RightButton = 2,
            Cancel = 3,
            MiddleButton = 4,
            ExtraButton1 = 5,
            ExtraButton2 = 6,
            Back = 8,
            Tab = 9,
            Clear = 12, // 0x000C
            Return = 13, // 0x000D
            Shift = 16, // 0x0010
            Control = 17, // 0x0011
            Menu = 18, // 0x0012
            Pause = 19, // 0x0013
            CapsLock = 20, // 0x0014
            Hangeul = 21, // 0x0015
            Hangul = 21, // 0x0015
            Kana = 21, // 0x0015
            Junja = 23, // 0x0017
            Final = 24, // 0x0018
            Hanja = 25, // 0x0019
            Kanji = 25, // 0x0019
            Escape = 27, // 0x001B
            Convert = 28, // 0x001C
            NonConvert = 29, // 0x001D
            Accept = 30, // 0x001E
            ModeChange = 31, // 0x001F
            Space = 32, // 0x0020
            Prior = 33, // 0x0021
            Next = 34, // 0x0022
            End = 35, // 0x0023
            Home = 36, // 0x0024
            Left = 37, // 0x0025
            Up = 38, // 0x0026
            Right = 39, // 0x0027
            Down = 40, // 0x0028
            Select = 41, // 0x0029
            Print = 42, // 0x002A
            Execute = 43, // 0x002B
            Snapshot = 44, // 0x002C
            Insert = 45, // 0x002D
            Delete = 46, // 0x002E
            Help = 47, // 0x002F
            N0 = 48, // 0x0030
            N1 = 49, // 0x0031
            N2 = 50, // 0x0032
            N3 = 51, // 0x0033
            N4 = 52, // 0x0034
            N5 = 53, // 0x0035
            N6 = 54, // 0x0036
            N7 = 55, // 0x0037
            N8 = 56, // 0x0038
            N9 = 57, // 0x0039
            A = 65, // 0x0041
            B = 66, // 0x0042
            C = 67, // 0x0043
            D = 68, // 0x0044
            E = 69, // 0x0045
            F = 70, // 0x0046
            G = 71, // 0x0047
            H = 72, // 0x0048
            I = 73, // 0x0049
            J = 74, // 0x004A
            K = 75, // 0x004B
            L = 76, // 0x004C
            M = 77, // 0x004D
            N = 78, // 0x004E
            O = 79, // 0x004F
            P = 80, // 0x0050
            Q = 81, // 0x0051
            R = 82, // 0x0052
            S = 83, // 0x0053
            T = 84, // 0x0054
            U = 85, // 0x0055
            V = 86, // 0x0056
            W = 87, // 0x0057
            X = 88, // 0x0058
            Y = 89, // 0x0059
            Z = 90, // 0x005A
            LeftWindows = 91, // 0x005B
            RightWindows = 92, // 0x005C
            Application = 93, // 0x005D
            Sleep = 95, // 0x005F
            Numpad0 = 96, // 0x0060
            Numpad1 = 97, // 0x0061
            Numpad2 = 98, // 0x0062
            Numpad3 = 99, // 0x0063
            Numpad4 = 100, // 0x0064
            Numpad5 = 101, // 0x0065
            Numpad6 = 102, // 0x0066
            Numpad7 = 103, // 0x0067
            Numpad8 = 104, // 0x0068
            Numpad9 = 105, // 0x0069
            Multiply = 106, // 0x006A
            Add = 107, // 0x006B
            Separator = 108, // 0x006C
            Subtract = 109, // 0x006D
            Decimal = 110, // 0x006E
            Divide = 111, // 0x006F
            F1 = 112, // 0x0070
            F2 = 113, // 0x0071
            F3 = 114, // 0x0072
            F4 = 115, // 0x0073
            F5 = 116, // 0x0074
            F6 = 117, // 0x0075
            F7 = 118, // 0x0076
            F8 = 119, // 0x0077
            F9 = 120, // 0x0078
            F10 = 121, // 0x0079
            F11 = 122, // 0x007A
            F12 = 123, // 0x007B
            F13 = 124, // 0x007C
            F14 = 125, // 0x007D
            F15 = 126, // 0x007E
            F16 = 127, // 0x007F
            F17 = 128, // 0x0080
            F18 = 129, // 0x0081
            F19 = 130, // 0x0082
            F20 = 131, // 0x0083
            F21 = 132, // 0x0084
            F22 = 133, // 0x0085
            F23 = 134, // 0x0086
            F24 = 135, // 0x0087
            NumLock = 144, // 0x0090
            ScrollLock = 145, // 0x0091
            Fujitsu_Jisho = 146, // 0x0092
            NEC_Equal = 146, // 0x0092
            Fujitsu_Masshou = 147, // 0x0093
            Fujitsu_Touroku = 148, // 0x0094
            Fujitsu_Loya = 149, // 0x0095
            Fujitsu_Roya = 150, // 0x0096
            LeftShift = 160, // 0x00A0
            RightShift = 161, // 0x00A1
            LeftControl = 162, // 0x00A2
            RightControl = 163, // 0x00A3
            LeftMenu = 164, // 0x00A4
            RightMenu = 165, // 0x00A5
            BrowserBack = 166, // 0x00A6
            BrowserForward = 167, // 0x00A7
            BrowserRefresh = 168, // 0x00A8
            BrowserStop = 169, // 0x00A9
            BrowserSearch = 170, // 0x00AA
            BrowserFavorites = 171, // 0x00AB
            BrowserHome = 172, // 0x00AC
            VolumeMute = 173, // 0x00AD
            VolumeDown = 174, // 0x00AE
            VolumeUp = 175, // 0x00AF
            MediaNextTrack = 176, // 0x00B0
            MediaPrevTrack = 177, // 0x00B1
            MediaStop = 178, // 0x00B2
            MediaPlayPause = 179, // 0x00B3
            LaunchMail = 180, // 0x00B4
            LaunchMediaSelect = 181, // 0x00B5
            LaunchApplication1 = 182, // 0x00B6
            LaunchApplication2 = 183, // 0x00B7
            OEM1 = 186, // 0x00BA
            OEMPlus = 187, // 0x00BB
            OEMComma = 188, // 0x00BC
            OEMMinus = 189, // 0x00BD
            OEMPeriod = 190, // 0x00BE
            OEM2 = 191, // 0x00BF
            OEM3 = 192, // 0x00C0
            OEM4 = 219, // 0x00DB
            OEM5 = 220, // 0x00DC
            OEM6 = 221, // 0x00DD
            OEM7 = 222, // 0x00DE
            OEM8 = 223, // 0x00DF
            OEMAX = 225, // 0x00E1
            OEM102 = 226, // 0x00E2
            ICOHelp = 227, // 0x00E3
            ICO00 = 228, // 0x00E4
            ProcessKey = 229, // 0x00E5
            ICOClear = 230, // 0x00E6
            Packet = 231, // 0x00E7
            OEMReset = 233, // 0x00E9
            OEMJump = 234, // 0x00EA
            OEMPA1 = 235, // 0x00EB
            OEMPA2 = 236, // 0x00EC
            OEMPA3 = 237, // 0x00ED
            OEMWSCtrl = 238, // 0x00EE
            OEMCUSel = 239, // 0x00EF
            OEMATTN = 240, // 0x00F0
            OEMFinish = 241, // 0x00F1
            OEMCopy = 242, // 0x00F2
            OEMAuto = 243, // 0x00F3
            OEMENLW = 244, // 0x00F4
            OEMBackTab = 245, // 0x00F5
            ATTN = 246, // 0x00F6
            CRSel = 247, // 0x00F7
            EXSel = 248, // 0x00F8
            EREOF = 249, // 0x00F9
            Play = 250, // 0x00FA
            Zoom = 251, // 0x00FB
            Noname = 252, // 0x00FC
            PA1 = 253, // 0x00FD
            OEMClear = 254, // 0x00FE
        }

        public struct RawKeyboard
        {
            public short MakeCode;
            public MultiMouseDefs.RawKeyboardFlags Flags;
            public short Reserved;
            public MultiMouseDefs.VirtualKeys VirtualKey;
            public MultiMouseDefs.WindowsMessages Message;
            public int ExtraInformation;
        }

        [Flags]
        public enum RawMouseFlags : ushort
        {
            MoveRelative = 0,
            MoveAbsolute = 1,
            VirtualDesktop = 2,
            AttributesChanged = 4,
        }

        [Flags]
        public enum RawMouseButtons : ushort
        {
            None = 0,
            LeftDown = 1,
            LeftUp = 2,
            RightDown = 4,
            RightUp = 8,
            MiddleDown = 16, // 0x0010
            MiddleUp = 32, // 0x0020
            Button4Down = 64, // 0x0040
            Button4Up = 128, // 0x0080
            Button5Down = 256, // 0x0100
            Button5Up = 512, // 0x0200
            MouseWheel = 1024, // 0x0400
        }

        public struct RawMouse
        {
            public MultiMouseDefs.RawMouseFlags Flags;
            public MultiMouseDefs.RawMouse.MouseData Data;
            public uint RawButtons;
            public int LastX;
            public int LastY;
            public uint ExtraInformation;

            [StructLayout(LayoutKind.Explicit)]
            public struct MouseData
            {
                [FieldOffset(0)]
                public uint Buttons;
                [FieldOffset(2)]
                public ushort ButtonData;
                [FieldOffset(0)]
                public MultiMouseDefs.RawMouseButtons ButtonFlags;
            }
        }

        public struct RawHID
        {
            public int Size;
            public int Count;
            public IntPtr Data;
        }

        public struct RawInput
        {
            public MultiMouseDefs.RawInputHeader Header;
            public MultiMouseDefs.RawInput.Union Data;

            [StructLayout(LayoutKind.Explicit)]
            public struct Union
            {
                [FieldOffset(0)]
                public MultiMouseDefs.RawMouse Mouse;
                [FieldOffset(0)]
                public MultiMouseDefs.RawKeyboard Keyboard;
                [FieldOffset(0)]
                public MultiMouseDefs.RawHID HID;
            }
        }

        public enum RawInputCommand
        {
            Input = 268435459, // 0x10000003
            Header = 268435461, // 0x10000005
        }
    }
}
