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

    public string CurrentHpPtr = "Undertale.exe+00408950,44,10,D0,460";                // Current health pointer
    public string MaxHpPtr = "Undertale.exe+00408950,44,10,D0,450";                    // Max health pointer
    public string EquWeapon = "Undertale.exe+0040894C,44,10,700,120";                  // Current Weapon 
    public string EquArmor = "Undertale.exe+0040894C,44,10,700,110";                   // Armor
    public string CurrentGoldPtr = "Undertale.exe+003F9F44,0,44,10,364,400";           // Gold
    public string CurrentLovePtr = "Undertale.exe+003F9F44,0,44,10,364,3E0";           // Love 
    public string isInBattle = "Undertale.exe+003F9F44,0,44,10,1A8,30";                // Battle Check
    public string ExpPtr = "Undertale.exe+003F9F44,0,44,10,364,3F0";                   // EXP pointer
    public string EnemyOnePtr = "Undertale.exe+004099B4,48,8,50,14,5C0";               // Enemy One pointer
    public string EnemyTwoPtr = "Undertale.exe+004099B4,48,8,50,14,5D0";               // Enemy Two pointer
    public string EnemyThreePtr = "Undertale.exe+004099B4,48,8,50,14,5E0";             // Enemy Three pointer
    public string RoomPtr = "Undertale.exe+618EA0";                                    // Room Pointer
    public string NamePtr = "Undertale.exe+003FC5EC,0,14,0,8,18,10,2B0";               // Name Pointer
    public string Item1 = "Undertale.exe+004099B4,330,8,50,18,300";                    // Slot 1
    public string Item2 = "Undertale.exe+004099B4,330,8,50,18,310";                    // Slot 2  
    public string Item3 = "Undertale.exe+004099B4,330,8,50,18,320";                    // Slot 3
    public string Item4 = "Undertale.exe+004099B4,330,8,50,18,330";                    // Slot 4
    public string Item5 = "Undertale.exe+004099B4,330,8,50,18,340";                    // Slot 5
    public string Item6 = "Undertale.exe+004099B4,330,8,50,18,350";                    // Slot 6
    public string Item7 = "Undertale.exe+004099B4,330,8,50,18,360";                    // Slot 7
    public string Item8 = "Undertale.exe+004099B4,330,8,50,18,370";                    // Slot 8
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
                else if (input.Contains("setweapon "))
                {
                    input = input.Replace("setweapon ", "");
                    setweapon(mem, input);
                }
                else if (input.Contains("setarmor "))
                {
                    input = input.Replace("setarmor ", "");
                    setarmor(mem, input);
                }
                else if (input.Contains("setgold"))
                {
                    input = input.Replace("setgold ", "");
                    SetGold(mem, input);
                }
                else if (input.Contains("setitem "))
                {
                    input = input.Replace("setitem ", "");
                    setitem(mem, input);
                }
                else if (input == "kill")
                {
                    Kill(mem);
                }
                else if (input.Contains("help"))
                {
                    Help(mem);
                }
                else if (input == "info")
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
                else if (input == "crash")
                {
                    Crash(mem);
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
            Console.WriteLine("sethp <value>: Sets the current players HP.");
            Console.WriteLine("This will reset if you SAVE or leave a battle.");
            Console.WriteLine("Leaving a battle will slightly raise the HP if the user has some left over.");
            Console.WriteLine("");
            Console.WriteLine("setmaxhp <value>: Sets the currents players max HP.");
            Console.WriteLine("Does not reset if you save or gain LOVE.");
            Console.WriteLine("Restarting the game will not keep this score.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--- Currently being debugged, May not work ---");
            Console.WriteLine("onehit: Sets all enemies on screen to 1hp");
            Console.WriteLine("Works on some bosses, too.");
            Console.WriteLine("--- Currently being debugged, May not work ---");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("freezehealth/unfreezehealth: Freezes the health at its current state.");
            Console.WriteLine("Nothing can change the value (except for restarting) unless you unfreeze it.");
            Console.WriteLine("If an attack does more damage than the amount set, you will die.");
            Console.WriteLine("");
            Console.WriteLine("kill: Kills the player if you are in battle.");
            Console.WriteLine("");
            Console.WriteLine("setlove <value>: Sets the current LOVE value.");
            Console.WriteLine("Your health will be updated if you go into battle, unless set otherwise.");
            Console.WriteLine("Saving in game will make the value persist.");
            Console.WriteLine("");
            Console.WriteLine("setgold <value>: Sets the current Gold value.");
            Console.WriteLine("Saving in game will make the value persist.");
            Console.WriteLine("");
            Console.WriteLine("changename <name>: Sets the Players name.");
            Console.WriteLine("Saving in game will make the name persist.");
            Console.WriteLine("Be careful! If you make it too long you will have to fix it manually, this is at least 50 characters though.");
            Console.WriteLine("");
            Console.WriteLine("crash: Crashes the game, anything unsaved will be lost.");
            Console.WriteLine("");
            Console.WriteLine("info: Shows some in-game info");
            Console.WriteLine("");
            Console.WriteLine("setweapon <item>: Sets the weapon, only visually.");
            Console.WriteLine("");
            Console.WriteLine("setarmor <item>: Sets the armor, only visually.");
            Console.WriteLine("");
            Console.WriteLine("setitem <slot> <item>: sets the item in selected slot.");
            Console.WriteLine("You can use this to actually equip weapons/armor i.e setitem 1 temy_armor.");
            Console.WriteLine("Saving will make the change persist.");
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
                l.logWrite("ex string length:" + namehex.Length.ToString());
                l.logWrite("Hex: " + namehex);
            }

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
    public void Crash(Mem mem)
    {
        try
        {

            mem.WriteMemory(NamePtr, "string", "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
        killProc();
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

    public void setweapon(Mem mem, string Weap)
    {

        try
        {
            Weap = getItemId(Weap);
            mem.WriteMemory(EquWeapon, "double", Weap);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Weapon has been set to '" + Weap + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }

    public void setarmor(Mem mem, string armor)
    {

        try
        {

            armor = getItemId(armor);
            mem.WriteMemory(EquArmor, "double", armor);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Armor has been set to '" + armor + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}");
        }
    }

    public void setitem(Mem mem, string item1)
    {

        try
        {
            string[] args = item1.Split(" ");
            if (args[1] == null) throw new Exception("You must specify the item."); 
            string slot = args[0];
            string item = args[1];
            item = getItemId(item);
            item = item.Trim();
            if (slot == "1") mem.WriteMemory(Item1, "double", item);
            if (slot == "2") mem.WriteMemory(Item2, "double", item);
            if (slot == "3") mem.WriteMemory(Item3, "double", item);
            if (slot == "4") mem.WriteMemory(Item4, "double", item);
            if (slot == "5") mem.WriteMemory(Item5, "double", item);
            if (slot == "6") mem.WriteMemory(Item6, "double", item);
            if (slot == "7") mem.WriteMemory(Item7, "double", item);
            if (slot == "8") mem.WriteMemory(Item8, "double", item);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Item slot {slot} has been set to '" + item + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            l.logWrite($"Couldn't write memory: {ex}", "error");
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
            mem.WriteMemory(EnemyOnePtr, "double", "1");
            mem.WriteMemory(EnemyTwoPtr, "double", "1");
            mem.WriteMemory(EnemyThreePtr, "double", "1");
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
        catch (Exception ex)
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("MaxHp has been set to '" + Hp + "'");
            Console.ForegroundColor = ConsoleColor.White;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Couldn't write memory: {ex}");
        }
    }

    public void Equi(Mem mem, string Hp)
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
        Double gold = mem.ReadDouble(CurrentGoldPtr);
        Double love = mem.ReadDouble(CurrentLovePtr);
        Double weapon = mem.ReadDouble(EquWeapon);
        Double armor = mem.ReadDouble(EquArmor);
        Double battle = mem.ReadDouble(isInBattle);
        Double exp = mem.ReadDouble(ExpPtr);
        Double i1 = mem.ReadDouble(Item1);
        Double i2 = mem.ReadDouble(Item2);
        Double i3 = mem.ReadDouble(Item3);
        Double i4 = mem.ReadDouble(Item4);
        Double i5 = mem.ReadDouble(Item5);
        Double i6 = mem.ReadDouble(Item6);
        Double i7 = mem.ReadDouble(Item7);
        Double i8 = mem.ReadDouble(Item8);

        uint room = mem.ReadUInt(RoomPtr);
        string name = mem.ReadString(NamePtr);
        bool isBattling = false;
        if (battle > 0) isBattling = true;
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Player Name: {name}");
        Console.WriteLine($"");
        Console.WriteLine($"Current HP: {hp}/{maxhp}");
        Console.WriteLine($"Current Room ID: {room}");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Current Weapon ID: {weapon}");
        Console.WriteLine($"Current Armor ID: {armor}");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Current LOVE: {love}");
        Console.WriteLine($"Current EXP: {exp}");
        Console.WriteLine($"");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Current gold: {gold}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Item slot ID 1: {i1}");
        Console.WriteLine($"Item slot ID 2: {i2}");
        Console.WriteLine($"Item slot ID 3: {i3}");
        Console.WriteLine($"Item slot ID 4: {i4}");
        Console.WriteLine($"Item slot ID 5: {i5}");
        Console.WriteLine($"Item slot ID 6: {i6}");
        Console.WriteLine($"Item slot ID 7: {i7}");
        Console.WriteLine($"Item slot ID 8: {i8}");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Blue;
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
                if (msgDisplayed) l.logWrite("Continuing...");
                isInGame = true;
                Console.WriteLine("");
            }
            Thread.Sleep(500);
        }

    }
    public string getItemId(string item)
    {
        item = item.ToLower().Trim();
        if (item == "nothing") item = "0";
        if (item == "monster_candy") item = "1";
        if (item == "croquet_roll") item = "2";
        if (item == "stick") item = "3";
        if (item == "bandage") item = "4";
        if (item == "rock_candy") item = "5";
        if (item == "pumpkin_rings") item = "6";
        if (item == "spider_donut") item = "7";
        if (item == "stoic_onion") item = "8";
        if (item == "ghost_fruit") item = "9";
        if (item == "spider_cider") item = "10";
        if (item == "butterscotch_pie") item = "11";
        if (item == "faded_ribbon") item = "12";
        if (item == "toy_knife") item = "13";
        if (item == "tough_glove") item = "14";
        if (item == "manly_bandana") item = "15";
        if (item == "snowman_piece") item = "16";
        if (item == "nice_cream") item = "17";
        if (item == "puppydough_icecream") item = "18";
        if (item == "bisicle") item = "19";
        if (item == "unisicle") item = "20";
        if (item == "cinnamon_bun") item = "21";
        if (item == "temmie_flakes") item = "22";
        if (item == "abandoned_quiche") item = "23";
        if (item == "old_tutu") item = "24";
        if (item == "ballet_shoes") item = "25";
        if (item == "punch_card") item = "26";
        if (item == "annoying_dog") item = "27";
        if (item == "dog_salad") item = "28";
        if (item == "dog_residue1") item = "29";
        if (item == "dog_residue2") item = "30";
        if (item == "dog_residue3") item = "31";
        if (item == "dog_residue4") item = "32";
        if (item == "dog_residue5") item = "33";
        if (item == "dog_residue6") item = "34";
        if (item == "astronaut_food") item = "35";
        if (item == "instant_noodles") item = "36";
        if (item == "crab_apple") item = "37";
        if (item == "hot_dog...?") item = "38";
        if (item == "hot_cat") item = "39";
        if (item == "glamburger") item = "40";
        if (item == "sea_tea") item = "41";
        if (item == "starfait") item = "42";
        if (item == "legendary_hero") item = "43";
        if (item == "cloudy_glasses") item = "44";
        if (item == "torn_notebook") item = "45";
        if (item == "stained_apron") item = "46";
        if (item == "burnt_pan") item = "47";
        if (item == "cowboy_hat") item = "48";
        if (item == "empty_gun") item = "49";
        if (item == "heart_locket") item = "50";
        if (item == "worn_dagger") item = "51";
        if (item == "real_knife") item = "52";
        if (item == "the_locket") item = "53";
        if (item == "bad_memory") item = "54";
        if (item == "dream") item = "55";
        if (item == "undynes_letter") item = "56";
        if (item == "undynes_letter_ex") item = "57";
        if (item == "potato_chisps") item = "58";
        if (item == "junk_food") item = "59";
        if (item == "mystery_key") item = "60";
        if (item == "face_steak") item = "61";
        if (item == "hush_puppy") item = "62";
        if (item == "snail_pie") item = "63";
        if (item == "temy_armor") item = "64";
        try
        {
            Double.Parse(item);
        } catch(Exception ex)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Something went wrong: " + ex);
            } else
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
            item = "0";
        }
        return item;
    }


}

