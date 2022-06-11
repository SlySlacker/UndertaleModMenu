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
    
    public void Cons(Mem mem)
    {

        while (true)
        {
            string? input = Console.ReadLine();
            if (input != null)
            {
                input = input.ToLower().Trim();
                if(input.Contains("sethp ")){
                    input = input.Replace("sethp ", "");
                    SetHp(mem, input);
                } else if(input.Contains("setmaxhp ")){
                    input = input.Replace("setmaxhp ", "");
                    SetMaxHp(mem, input);
                } else if (input.Contains("readall")){
                    ReadValues(mem);
                } else if (input.Contains("freezehp")){
                    FreezeHealth(mem);
                } else if (input.Contains("unfreezehp")){ 
                    UnfreezeHealth(mem);
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    logging.logWrite("Unknown command!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }
    }
    public void SetHp(Mem mem, string Hp)
    {
        try
        {
            mem.WriteMemory(CurrentHpPtr, "double", Hp);
            logging.logWrite("Hp has been set to '" + Hp + "'");
        }
        catch (Exception ex)
        {
            logging.logWrite($"Couldn't write memory: {ex}");
        }
    }
    public bool HealthFrozen = false;
    public void FreezeHealth(Mem mem)
    {
        try
        {
            mem.FreezeValue(CurrentHpPtr, "double", mem.ReadDouble(CurrentHpPtr).ToString());
            logging.logWrite("Hp has been frozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());
            HealthFrozen = true;
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
            logging.logWrite("Hp has been unfrozen with value " + mem.ReadDouble(CurrentHpPtr).ToString());
            HealthFrozen = false;
        }
        catch (Exception ex)
        {
            logging.logWrite($"Unable to freeze health: " + ex);
        }
    }

    public void SetMaxHp(Mem mem, string Hp)
    {
        try
        {
            mem.WriteMemory(MaxHpPtr, "double", Hp);
            logging.logWrite("MaxHp has been set to '" + Hp + "'");
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
        if (HealthFrozen)
        {
            logging.logWrite("Current hp: '" + hp + "' (Frozen)");
        }
        else
        {
            logging.logWrite("Current hp: '" + hp + "'");
        }
        logging.logWrite("Max hp: '" + maxhp + "'");

    }
}
