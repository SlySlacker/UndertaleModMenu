using System.Reflection;

namespace UndertaleModMenu.Command;

public static class CommandManager
{
    public static List<Command> Commands = new();
    
    public static void Init()
    {
        Console.Log.WriteLine("CommandManager", "&aLoading commands...");
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Command))).OrderBy(x => x.Name))
        {
            Console.Log.WriteLine("CommandManager", $"&aLoaded command &f{type.Name}");
            Commands.Add((Command)Activator.CreateInstance(type));
        }
        Console.Log.WriteLine("CommandManager", "&aCommands loaded!");
    }
    
    public static void ExecuteCommand(string command)
    {
        command = "." + command; // LOL
        string[] args = command.Trim().Split(' ');
        string cmd = args[0][1..];
        string[] arguments = args[1..];
        
        bool flag = false;
        foreach (Command c in Commands)
        {
            if (c.Name.ToLower() == cmd.ToLower() || c.Aliases.Contains(cmd.ToLower()))
            {
                flag = true;
                c.Execute(arguments);
                break;
            }
        }
        
        if (!flag)
        {
            Console.Log.WriteLine("CommandManager", $"&cUnknown command &f{cmd}", LogLevel.Error);
            Commands.Find(command => command.Name == "help")!.Execute(Array.Empty<string>());
        }
    }
}