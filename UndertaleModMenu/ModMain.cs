using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;

class UndertaleMod
{
    public string CurrentHpPtr = "Undertale.exe+00408950,44,10,D0,460"; // Current health pointer
    public string MaxHpPtr = "Undertale.exe+00408950,44,10,D0,450"; // Max health pointer
    
    public void Cons(Mem mem)
    {

        while (true)
        {
            string? input = Console.ReadLine();
            Console.Title = input;
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
                    Console.WriteLine("Unknown command!");
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
            Console.WriteLine("Hp has been set to '" + Hp + "'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Couldn't write memory: {ex}");
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
            Console.WriteLine($"Unable to freeze health: " + ex);
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
            Console.WriteLine($"Couldn't write memory: {ex}");
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
