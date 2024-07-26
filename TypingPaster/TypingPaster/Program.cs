namespace TypingPaster;

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TextCopy;
using WindowsInput;
using WindowsInput.Native;

public static class Program
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    const int HotkeyIdPaste = 1;
    const int HotkeyIdQuit = 2;
    const int HotkeyIdSpeedUp = 3;
    const int HotkeyIdSpeedDown = 4;
    const uint ModAlt = 0x0001;
    const uint ModControl = 0x0002;
    const uint VkV = 0x56;
    const uint VkQ = 0x51;

    private static int _typingSpeed = 60; // Default speed in WPM
    private static readonly InputSimulator InputSimulator = new();

    private static void RegisterHotKeys()
    {
        RegisterHotKey(IntPtr.Zero, HotkeyIdPaste, ModAlt | ModControl, VkV);
        RegisterHotKey(IntPtr.Zero, HotkeyIdQuit, ModAlt | ModControl, VkQ);
        // Register a hot key to increase typing speed Ctr + Alt + Up Arrow
        RegisterHotKey(IntPtr.Zero, HotkeyIdSpeedUp, ModAlt | ModControl, 0x26);
        // Register a hot key to decrease typing speed Ctr + Alt + Down Arrow
        RegisterHotKey(IntPtr.Zero, HotkeyIdSpeedDown, ModAlt | ModControl, 0x28);

    }

    private static void UnRegisterHotKeys()
    {
        UnregisterHotKey(IntPtr.Zero, HotkeyIdPaste);
        UnregisterHotKey(IntPtr.Zero, HotkeyIdQuit);
        UnregisterHotKey(IntPtr.Zero, HotkeyIdSpeedUp);
        UnregisterHotKey(IntPtr.Zero, HotkeyIdSpeedDown);

    }
    public static void Main()
    {
        try
        {
            RegisterHotKeys();
    
            Console.WriteLine("TypeyClip is running.");
            Console.WriteLine("Press Ctrl+Alt+V to paste, Ctrl+Alt+Q to stop.");
            Console.WriteLine($"Current typing speed: {_typingSpeed} WPM");
            Console.WriteLine("Enter new typing speed (WPM) or press Enter to keep current speed:");

            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int newSpeed))
            {
                _typingSpeed = newSpeed;
                Console.WriteLine($"Typing speed set to {_typingSpeed} WPM");
            }

            while (true)
            {
                if (ProcessHotKey())
                {
                    break;
                }

                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            UnRegisterHotKeys();
        }
    }

    static bool ProcessHotKey()
    {
        if (!PeekMessage(out NativeMessage msg, IntPtr.Zero, 0, 0, 0x0001)) return false;
        if (msg.message != 0x0312) return false;
        switch (msg.wParam.ToInt32())
        {
            // WM_HOTKEY
            case HotkeyIdPaste:
            {
                var clipboardText = ClipboardService.GetText();
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    SimulateTyping(clipboardText);
                }

                break;
            }
            case HotkeyIdSpeedUp:
                _typingSpeed += 10;
                Console.WriteLine($"Typing speed set to {_typingSpeed} WPM");
                break;
            case HotkeyIdSpeedDown:
                _typingSpeed -= 10;
                Console.WriteLine($"Typing speed set to {_typingSpeed} WPM");
                break;

            case HotkeyIdQuit:
                Console.WriteLine("Stop hotkey pressed. Exiting...");
                return true;
        }

        return false;
    }

    private static void SimulateTyping(string text)
    {
        int sleepTime = CalculateSleepTime(_typingSpeed);
        // remove \r\n and replace with \n
        text = text.Replace("\r\n", "\n");
        // remove \t\t and replace with \t
        text = text.Replace("\t\t", "\t");
        // remove "  " and replace with " "
        text = text.Replace("  ", " ");
        foreach (char c in text)
        {
            switch (c)
            {
                case '\n':
                case '\r':
                    InputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                    break;
                case '\t':
                    InputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                    break;
                default:
                    InputSimulator.Keyboard.TextEntry(c);
                    break;
            }

            Task.Delay(sleepTime);
        }
    }

    private static int CalculateSleepTime(int wpm)
    {
        double charactersPerMinute = wpm * 5;
        double millisecondsPerCharacter = 60000 / charactersPerMinute;
        return (int)Math.Round(millisecondsPerCharacter);
    }

    [DllImport("user32.dll")]
    static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax,
        uint wRemoveMsg);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativeMessage
    {
        public IntPtr handle;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point point;
    }
}