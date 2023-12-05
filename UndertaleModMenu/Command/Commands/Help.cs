namespace UndertaleModMenu.Command.Commands;

public class Help : Command
{
    public Help() : base("help", "Displays this help message", new Dictionary<string, Type>(), new List<string>() { "?" })
    {
        
    }

    protected override void OnExecute(List<object> args)
    {
        Console.Log.WriteLine("Help", "&aCommands:");
        foreach (Command c in CommandManager.Commands)
        {
            Console.Log.WriteLine("Help", $"&f{c.Name} &7- &f{c.Description}");
        }
    }
}