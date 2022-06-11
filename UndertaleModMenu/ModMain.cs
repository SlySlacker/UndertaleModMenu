using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

class UndertaleMod
{
    Logging logging = new Logging();
    public string CurrentHpPtr = "Undertale.exe+00408950,44,10,D0,460"; // Current health pointer
    public string MaxHpPtr = "Undertale.exe+00408950,44,10,D0,450"; // Max health pointer
    public string EquWeapon = "Undertale.exe+19F1A5F0,44,10,700,120"; // Current Weapon 
    public string Gold = "Undertale.exe+03F12420,0,44,10,392,440"; // Gold

    public void Cons(Mem mem)
    {

        while (true)
        {
            string? input = Console.ReadLine();
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
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unknown command!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                { 
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
            Console.WriteLine("By SlySlacker & VastraKai");;
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("WIP Commands:");
            Console.WriteLine("");
            Console.WriteLine("changeweapon, setgold");
            Console.ForegroundColor = ConsoleColor.White;


            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unknown error has occured");
        }
    }

    public void SetHp(Mem mem, string Hp)
    {
        try
        {
            mem.WriteMemory(CurrentHpPtr, "double", Hp);
            Console.WriteLine("Hp has been set to '" + Hp + "'");
        }
        catch (Exception ex)
        {
            logging.logWrite($"Couldn't write memory: {ex}");
        }
    }
    public void FreezeHealth(Mem mem)
    {
        try
        {
            mem.FreezeValue(CurrentHpPtr, "double", mem.ReadDouble(CurrentHpPtr).ToString());
            Console.WriteLine("Hp has been frozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());

        }
        catch (Exception ex)
        {
            logging.logWrite($"Unable to freeze health: " + ex);
        }
    }
    public void UnfreezeHealth(Mem mem)
    {
        try
        {
            mem.UnfreezeValue(CurrentHpPtr);
            Console.WriteLine("Hp has been unfrozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());
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
            Console.WriteLine("MaxHp has been set to '" + Hp + "'");
        }
        catch (Exception ex)
        {
            logging.logWrite($"Couldn't write memory: {ex}");
        }
    }
    public void ReadValues(Mem mem)
    {
        Double hp = mem.ReadDouble(CurrentHpPtr);
        Double maxhp = mem.ReadDouble(MaxHpPtr);
        Console.WriteLine("Current hp: " + hp);
        Console.WriteLine("Max hp: " + maxhp);

    }
}
