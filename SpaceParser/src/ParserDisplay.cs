using System.ComponentModel;

namespace space_parser;

public class ParserDisplay
{
    public Parser parser;
    public SpaceParser spaceParser;

    public ParserDisplay(SpaceParser spaceParser)
    {
        this.spaceParser = spaceParser;
    }

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
        System.Console.WriteLine($"SpaceParser ================================= {this.spaceParser.parser.working} ");

        int line_space_available_to_list = Console.BufferWidth - 2;

        int line_start = 1;
        int line_end = line_start + line_space_available_to_list;
        System.Console.WriteLine($"line start {line_start} line end {line_end}");
        System.Console.WriteLine($"length of list {this.spaceParser.index.files.Count}");
        string list_lines = this.spaceParser.index.getfiles(0, line_end);
        System.Console.WriteLine(list_lines);
    }
}
