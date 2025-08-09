namespace space_parser;

public class ParserDisplay
{
    public Parser parser;
    public SpaceParser spaceParser;
    public void Run()
    {
        System.Console.WriteLine("Display Thread Starting...");
        Console.Clear();
        Console.WriteLine("test");

        while (true)
        {
            Loop();
        }
        // throw new NotImplementedException();
    }

    private void Loop()
    {

        this.Update();
        this.Draw();
        Thread.Sleep(1000);
    }

    public void Update()
    {
        // throw new NotImplementedException();
    }

    public void Draw()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        System.Console.WriteLine($"SpaceParser ================================= ");
        System.Console.WriteLine("");
    }
}
