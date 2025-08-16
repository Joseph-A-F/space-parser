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

        bool draw = true;
        this.Update();
        if (draw)
        {
            this.Draw();
        }
        Thread.Sleep(100);
    }

    public void Update()
    {
        // throw new NotImplementedException();
    }

    public void Draw()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        bool working = this.spaceParser.parser.working;
        string status;
        if (working) status = "currently parsing";
        else status = "done";

        System.Console.WriteLine($"SpaceParser ================================= {status} ");

        int line_space_available_to_list = Console.BufferHeight - 2;
        int line_start = 2;
        int line_end = line_start + line_space_available_to_list;

        // System.Console.WriteLine($"line start {line_start} line end {line_end}");
        System.Console.WriteLine($"files indexed {this.spaceParser.index.files.Count}");
        string list_lines = this.spaceParser.index.getfiles(0, line_end - 5);
        System.Console.WriteLine(list_lines);
        System.Console.WriteLine($"Type the file's number to reveal the file your system's file explorer");
    }
}
