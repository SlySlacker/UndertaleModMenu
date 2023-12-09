using Memory;
using System;
using System.Diagnostics;
using System.Threading;
class Program
{

    static void Main(string[] args)
    {
        Logging log = new Logging();
        log.configConsole();
        Mem mem = new Mem();
        try
        {
            int pid = mem.GetProcIdFromName("UNDERTALE");
            if (pid < 1)
            {
                throw new Exception("Failed to find Undertale process"); // if undertale isn't found, throw an exception
            }
                mem.OpenProcess(pid);
            UndertaleMod um = new UndertaleMod();
            Thread th = new Thread(() => um.Cons(mem));
            th.Start();
            while (true)
            {
                int bruh = mem.GetProcIdFromName("UNDERTALE");
                if (bruh < 1) throw new Exception("Undertale was closed"); // if undertale isn't found, throw an exception
                Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            log.logWrite("Failed: " + ex);
            Process.GetCurrentProcess().Kill();
        }
    }
}


