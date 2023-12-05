using System.Diagnostics;
using Memory;

namespace UndertaleModMenu;

public static class MemMgr
{
    public static Process? Undertale { get => Process.GetProcessesByName("UNDERTALE").FirstOrDefault(); }
    public static IntPtr? BaseAddress { get => Undertale?.MainModule?.BaseAddress; }
    public static Mem? M = null;
    
    public static void Init()
    {
        if (M == null)
        {
            Console.WriteLine("Initializing memory manager...");
            M = new Mem();
            // Make sure the undertale process is running. If it's not, wait for it to start.
            while (Undertale == null)
            {
                Console.WriteLine("Waiting for UNDERTALE to start...");
                Thread.Sleep(1000);
            }
            Console.WriteLine("UNDERTALE found! PID: " + Undertale.Id);
            M.OpenProcess(Undertale.Id);
            Console.WriteLine("Opened process UNDERTALE with handle " + M.mProc.Handle);
        }
    }
}