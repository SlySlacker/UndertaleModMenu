namespace UndertaleModMenu.Command.Commands;

public class Test : Command
{
    public Test() : base("test", "Test command", new Dictionary<string, Type>(), new List<string>() { "t" })
    {
        
    }

    protected override void OnExecute(List<object> args)
    {
        Console.Log.WriteLine("Test", "&aTest command executed!");
    }
}