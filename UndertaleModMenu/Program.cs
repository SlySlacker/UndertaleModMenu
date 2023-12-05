global using ExtendedConsole;
global using Console = ExtendedConsole.Console;
using Memory;
using System;
using System.Diagnostics;
using System.Threading;
using UndertaleModMenu.Command;
using UndertaleModMenu.Extensions;

namespace UndertaleModMenu;

class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.Title = "Undertale Mod Menu";
            Console.Config.SetupConsole();
            MemMgr.Init();
            CommandManager.Init();
            
            while (true)
            {
                Console.Write("> ");
                string? command = Console.ReadLine();
                if (command.IsNullOrEmpty())
                {
                    continue;
                }
                CommandManager.ExecuteCommand(command);
            }
            
        } catch (Exception e)
        {
            Console.Log.WriteLine("Main", "&cError initializing memory manager!", LogLevel.Critical);
            Console.Log.WriteLine("Main", "&c" + e, LogLevel.Critical);
            Console.WaitForEnter("Press enter to exit...");
        }
    }
}


