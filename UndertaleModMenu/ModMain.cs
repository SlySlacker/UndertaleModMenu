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
    public void blank() => Console.WriteLine("");
    Logging l = new Logging();
    // all of these are doubles for some shitty reason
    public string CurrentHpPtr = "Undertale.exe+00408950,44,10,D0,460";                // Current health pointer
    public string MaxHpPtr = "Undertale.exe+00408950,44,10,D0,450";                    // Max health pointer
    public string EquWeapon = "Undertale.exe+19F1A5F0,44,10,700,120";                  // Current Weapon 
    public string CurrentGoldPtr = "Undertale.exe+003F9F44,0,44,10,364,400";           // Gold
    public string CurrentLovePtr = "Undertale.exe+003F9F44,0,44,10,364,3E0";           // Love 
    public string isInBattle = "Undertale.exe+003F9F44,0,44,10,1A8,30";                // Battle Check
    public string ExpPtr = "Undertale.exe+003F9F44,0,44,10,364,3F0";                   // EXP pointer
    public string EnemyOnePtr = "Undertale.exe+004099B4,48,8,50,14,5C0";               // Enemy One pointer
    public string EnemyTwoPtr = "Undertale.exe+004099B4,48,8,50,14,5D0";               // Enemy Two pointer
    public string RoomPtr = "Undertale.exe+618EA0";                                    // Room Pointer
    public string NamePtr = "Undertale.exe+003FC5EC,0,14,0,8,18,10,2B0";               // Name Pointer

    public void Cons(Mem mem)
    {
        CheckVersion(mem);
        WaitForPlayer(mem);
        Double hp = mem.ReadDouble(CurrentHpPtr);
        Double maxhp = mem.ReadDouble(MaxHpPtr);
        Double gold = mem.ReadDouble(CurrentGoldPtr);
        Double love = mem.ReadDouble(CurrentLovePtr);
        Double weapon = mem.ReadDouble(EquWeapon);
        Double battle = mem.ReadDouble(isInBattle);
        Double exp = mem.ReadDouble(ExpPtr);
        Double room = mem.Read2Byte(RoomPtr);
        string name = mem.ReadString(NamePtr);

        l.logWrite("Successfully hooked to Undertale.exe");
        l.logWrite($"Player Name: {name}");
        Console.WriteLine("");
        l.logWrite($"LOVE: {love}");
        l.logWrite($"EXP: {exp}");
        l.logWrite($"Room ID: {room}");
        l.logWrite($"Gold: {gold}");
        Console.WriteLine("");

        while (true) 
        {
            string? input = Console.ReadLine(); // anything that passes the input string to a method will need to have the input string filtered 
            if (input != null)
            {
                string inputUpper = input.Trim();
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
                else if (input == "kill")
                {
                    Kill(mem);
                }
                else if (input.Contains("help"))
                {
                    Help(mem);
                }
                else if(input == "info")
                {
                    ReadValues(mem);
                }
                else if (input.Contains("setexp "))
                {
                    input = input.Replace("setexp ", "");
                    SetExp(mem, input);
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
                else if (input == "onehit")
                {
                    OneHit(mem);
                }
                else if (input.Contains("changename "))
                {
                    inputUpper = inputUpper.Replace("changename ", "");
                    SetName(mem, inputUpper);
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
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
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
            Console.WriteLine("onehit: Sets all enemies on screen to 1hp");
            Console.WriteLine("Works on bosses too, excluding Sans. You can't hit him anyway.");
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
            Console.WriteLine("changename: Sets the Players name.");
            Console.WriteLine("Saving in game will make the name stick.");
            Console.WriteLine("");
            Console.WriteLine("info: Shows some in-game info");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WIP Commands:");
            Console.WriteLine("");
            Console.WriteLine("changeweapon");
            Console.WriteLine("");
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

    public void SetName(Mem mem, string Name)
    {
        try
        {
            byte[] ba = Encoding.Default.GetBytes(Name);
            var hexString = BitConverter.ToString(ba);
            string namehex = hexString.Replace("-", " ");
            namehex = namehex + " 00";

            if (Debugger.IsAttached)
            {
                l.logWrite(namehex.Length.ToString());
                l.logWrite(namehex);
            }

            else
            mem.WriteMemory(NamePtr, "bytes", "000000000000");
            mem.WriteMemory(NamePtr, "bytes", namehex);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Name has been changed to '" + Name + "'");
            Console.WriteLine("Save the game to keep the change.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }

    public void SetExp(Mem mem, string Exp)
    {
        try
        {
            mem.WriteMemory(ExpPtr, "double", Exp);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("EXP has been set to '" + Exp + "'");
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
            mem.WriteMemory(CurrentLovePtr, "double", Love);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("LOVE has been set to '" + Love + "'");
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
            mem.WriteMemory(CurrentGoldPtr, "double", Gold);
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
    public void OneHit(Mem mem)
    {
        try
        {
            mem.WriteMemory(EnemyOnePtr, "double", null);
            mem.WriteMemory(EnemyTwoPtr, "double", null);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Made all enemies on screen 1 hit.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        catch { }


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
        catch(Exception ex)
        {
            Console.WriteLine($"Unable to kill: " + ex);
        }


    }

    public void UnfreezeHealth(Mem mem)
    {
        try
        {
            mem.UnfreezeValue(CurrentHpPtr);
            Console.WriteLine("");
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
            Console.WriteLine("");
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
        Double gold = mem.ReadDouble(CurrentGoldPtr);
        Double love = mem.ReadDouble(CurrentLovePtr);
        Double weapon = mem.ReadDouble(EquWeapon);
        Double battle = mem.ReadDouble(isInBattle);
        Double exp = mem.ReadDouble(ExpPtr);
        uint room = mem.ReadUInt(RoomPtr);
        string name = mem.ReadString(NamePtr);
        bool isBattling = false;
        if(battle > 0) isBattling = true;
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Player Name: {name}");
        Console.WriteLine($"");
        Console.WriteLine($"Current hp: {hp}/{maxhp}");
        Console.WriteLine($"Current gold: {gold}");
        Console.WriteLine($"Current Room ID: {room}");
        Console.WriteLine($"");
        //Console.WriteLine($"Current weapon: {weapon}");
        Console.WriteLine($"Current love: {love}");
        Console.WriteLine($"Current EXP: {exp}");
        Console.WriteLine($"");
        Console.WriteLine($"Is battling: {isBattling}");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void CheckVersion(Mem mem)
    {

        uint room2 = mem.ReadUInt(RoomPtr);
        uint room1 = mem.ReadUInt("UNDERTALE.exe+50F300");
        if (Debugger.IsAttached)
        {
            try
            {
                Console.WriteLine("1.0 roomid: " + room1);
                Console.WriteLine("1.08 roomid: " + room2);
            }
            catch (Exception ex)
            {
                l.logWrite("idk man: " + ex, "error");
            }
        }
        if (room1 > 0)
        {
            l.logWrite("1.0 - 1.07 detected. This version is not supported.");
            l.logWrite("Press any key to exit..."); 
            Console.ReadKey();
            killProc();
        }
    }
    public void WaitForPlayer(Mem mem)
    {
        bool msgDisplayed = false;
        bool isInGame = false;
        while (!isInGame)
        {
            uint room = mem.ReadUInt(RoomPtr);
            if (room < 4)
            {
                if (!msgDisplayed) {
                    l.logWrite("Waiting for you to load a save first...");
                    l.logWrite("If you load a save and this doesn't continue, your version of UNDERTALE is not 1.08.");
                    msgDisplayed = true;
                }
            }
            else
            {
                if(msgDisplayed) l.logWrite("Continuing...");
                isInGame = true;
                Console.WriteLine("");
            }
            Thread.Sleep(500);
        }
        
    }

}

