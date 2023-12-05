namespace UndertaleModMenu.Command;

public class Command
{
    public readonly string Name;
    public readonly string Description;
    public readonly List<string> Aliases;
    public readonly Dictionary<string, Type> Arguments = new();
    
    protected Command(string name, string description, Dictionary<string, Type> arguments, List<string> aliases = null)
    {
        Name = name;
        Description = description;
        Arguments = arguments;
        Aliases = aliases ?? new List<string>();
    }

    protected virtual void OnExecute(List<object> args)
    {
        
    }
    

    public void Execute(string[] args)
    {
        //List<object> arguments = args.Select((arg, i) => Convert.ChangeType(arg, Arguments.Values.ElementAt(i))).ToList();
        List<object> arguments = new();

        for (int i = 0; i < args.Length; i++)
        {
            if (i < Arguments.Count - 1)
            {
                try
                {
                    arguments.Add(Convert.ChangeType(args[i], Arguments.Values.ElementAt(i)));
                }
                catch
                {
                    //CommandLogger($"&cInvalid argument &f(&6{args[i]} &4is not &6{Arguments.Values.ElementAt(i)}&f)");
                    Console.Log.WriteLine(ToString(), $"&cInvalid argument &f(&6{args[i]} &4is not &6{Arguments.Values.ElementAt(i)}&f)");
                    return;
                }
            }
            else
            {
                if (args.Length > Arguments.Count)
                {
                    if (Arguments.Any() && Arguments.ElementAt(Arguments.Count - 1).Value == typeof(string))
                    {
                        //concatenate the rest of the arguments into a single string
                        string concat = "";
                        for (int j = i; j < args.Length; j++)
                        {
                            concat += args[j] + " ";
                        }

                        concat = concat.TrimEnd();
                        arguments.Add(concat);
                    }
                    else
                    {
                        //Logger.Log(ToString(), $"&cToo many arguments &f(&c{args.Length} &4given, &c{Arguments.Count} &4expected&f)"); Obsolete
                        Console.Log.WriteLine(ToString(), $"&cToo many arguments &f(&c{args.Length} &4given, &c{Arguments.Count} &4expected&f)");
                        return;
                    }
                }
                else
                {
                    arguments.Add(args[i]);
                }
            }
        }

        OnExecute(arguments);
    }
}