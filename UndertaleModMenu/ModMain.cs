using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

class UndertaleMod
{
    public void killProc() => Process.GetCurrentProcess().Kill();
    Logging l = new Logging();
    public string CurrentHpPtr = "Undertale.exe+00408950,44,10,D0,460"; // Current health pointer
    public string MaxHpPtr = "Undertale.exe+00408950,44,10,D0,450"; // Max health pointer
    public string EquWeapon = "Undertale.exe+19F1A5F0,44,10,700,120"; // Current Weapon 
    public string CurrentGold = "Undertale.exe+003F9F44,0,44,10,364,400"; // Gold
    public string CurrentLove = "Undertale.exe+003F9F44,0,44,10,364,3E0"; // Love 

    public void Cons(Mem mem)
    {
        l.logWrite("Successfully hooked to Undertale.exe");

        while (true) 
        {
            string? input = Console.ReadLine(); // anything that passes the input string to a method will need to have the input string filtered 
            if (input != null)
            {
                input = input.ToLower().Trim();
                if (input.Contains("sethp "))
                {
                    input = input.Replace("sethp ", "");
                    SetHp(mem, input);
                }
                else if (input.Contains("setmaxhp "))
                {
                    input = input.Replace("setmaxhp ", "");
                    SetMaxHp(mem, input);
                }
                else if (input.Contains("setlove "))
                {
                    input = input.Replace("setlove ", "");
                    SetLove(mem, input);
                }
                else if (input.Contains("setgold"))
                {
                    input = input.Replace("setgold ", "");
                    SetGold(mem, input);
                }
                else if (input.Contains("kill"))
                {
                    Kill(mem);
                }
                else if (input.Contains("help"))
                {
                    Help(mem);
                }
                else if (input.Contains("readall"))
                {
                    ReadValues(mem);
                }
                else if (input.Contains("freezehp"))
                {
                    FreezeHealth(mem);
                }
                else if (input.Contains("unfreezehp"))
                {
                    UnfreezeHealth(mem);
                }
                 else if (input.Contains("unfreezehp"))
                {
                    UnfreezeHealth(mem);
                }

                else if (input == "help")
                {
                    Help(mem);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown command!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }

    public void Help(Mem mem)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("UndertaleModMenu");
            Console.WriteLine("By SlySlacker & VastraKai"); ;
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("sethp: Sets the current players HP.");
            Console.WriteLine("This will reset if you SAVE or leave a battle.");
            Console.WriteLine("Leaving a battle will slightly raise the HP if the user has some left over.");
            Console.WriteLine("");
            Console.WriteLine("setmaxhp: Sets the currents players max HP.");
            Console.WriteLine("Does not reset if you save or gain LOVE.");
            Console.WriteLine("Restarting the game will not keep this score.");
            Console.WriteLine("");
            Console.WriteLine("freezehealth/unfreezehealth: Freezes the health at its current state.");
            Console.WriteLine("Nothing can change the value (except for restarting) unless you unfreeze it.");
            Console.WriteLine("If an attack does more damage than the amount set, you will die.");
            Console.WriteLine("");
            Console.WriteLine("kill: Kills the player if you are in battle.");
            Console.WriteLine("");
            Console.WriteLine("setlove: Sets the current LOVE value."); 
            Console.WriteLine("Your health will be updated if you go into battle, unless set otherwise.");
            Console.WriteLine("Saving in game will make the value stick.");
            Console.WriteLine("");
            Console.WriteLine("setgold: Sets the current Gold value.");
            Console.WriteLine("Saving in game will make the value stick.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("WIP Commands:");
            Console.WriteLine("");
            Console.WriteLine("changeweapon");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached) {
                Console.WriteLine("Error at help: " + ex);
            } else {
                Console.WriteLine("An unknown error has occured");
            }
            
        }
    }

    public void SetHp(Mem mem, string Hp)
    {
        try
        {
            mem.WriteMemory(CurrentHpPtr, "double", Hp);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Hp has been set to '" + Hp + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }

    public void SetLove(Mem mem, string Love)
    {
        try
        {
            mem.WriteMemory(CurrentLove, "double", Love);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Love has been set to '" + Love + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }


    public void SetGold(Mem mem, string Gold)
    {
        try
        {
            mem.WriteMemory(CurrentGold, "double", Gold);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Gold has been set to '" + Gold + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }

    public void FreezeHealth(Mem mem)
    {
        try
        {
            mem.FreezeValue(CurrentHpPtr, "double", mem.ReadDouble(CurrentHpPtr).ToString());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Hp has been frozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());
            Console.ForegroundColor = ConsoleColor.White;

        }
        catch (Exception ex)
        {
            l.logWrite($"Unable to freeze health: " + ex);
        }
    }
    public void Kill(Mem mem)
    {
        try
        {
            mem.WriteMemory(CurrentHpPtr, "double", "-1");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Player killed.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        catch { }


}

    public void UnfreezeHealth(Mem mem)
    {
        try
        {
            mem.UnfreezeValue(CurrentHpPtr);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Hp has been unfrozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to freeze health: " + ex);
        }
    }

    public void SetMaxHp(Mem mem, string Hp)
    {
        try
        {
            mem.WriteMemory(MaxHpPtr, "double", Hp);
            mem.FreezeValue(MaxHpPtr, "double", mem.ReadDouble(MaxHpPtr).ToString());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("MaxHp has been set to '" + Hp + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Couldn't write memory: {ex}");
        }
    }

    public void ReadValues(Mem mem)
    {
        Double hp = mem.ReadDouble(CurrentHpPtr);
        Double maxhp = mem.ReadDouble(MaxHpPtr);
        Double gold = mem.ReadDouble(CurrentGold);
        Double love = mem.ReadDouble(CurrentLove);
        Double weapon = mem.ReadDouble(EquWeapon);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Current hp: {hp}/{maxhp}");
        Console.WriteLine($"Current gold: {gold}");
        //Console.WriteLine($"Current weapon: {weapon}");
        Console.WriteLine($"Current love: {love}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}

