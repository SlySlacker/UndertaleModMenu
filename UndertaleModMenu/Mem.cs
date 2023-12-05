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
            Console.Log.WriteLine("MemMgr", "&bInitializing memory manager...");
            M = new Mem();
            // Make sure the undertale process is running. If it's not, wait for it to start.
            bool flag = false;
            while (Undertale == null)
            {
                if (!flag)
                {
                    Console.Log.WriteLine("MemMgr", "&cUNDERTALE not found! &bPlease open the game...", LogLevel.Warning);
                    flag = true;
                }
                Thread.Sleep(1000);
            }
            //Console.WriteLine("UNDERTALE found! PID: " + Undertale.Id);
            Console.Log.WriteLine("MemMgr", "&vUNDERTALE &afound! PID: &v" + Undertale.Id);
            M.OpenProcess(Undertale.Id);
            //Console.WriteLine("Opened process UNDERTALE with handle " + M.mProc.Handle);
            Console.Log.WriteLine("MemMgr", "&aOpened process &vUNDERTALE&a with handle &v" + M.mProc.Handle);
            
            Console.Log.WriteLine("MemMgr", "&bMemory manager initialized!");
        }
    }
}