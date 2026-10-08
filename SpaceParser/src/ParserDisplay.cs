namespace space_parser;

public class ParserDisplay
{
    public Parser parser;
    public SpaceParser spaceParser;
    public bool filequery_mode;
    private string list_buffer;

    public ParserDisplay(SpaceParser spaceParser)
    {
        this.spaceParser = spaceParser;
        this.filequery_mode = false;
        this.list_buffer = "";
    }

    public void Run()
    {
        Console.WriteLine("Display Thread Starting...");
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

    }

    private bool drawn_query_prompt = false;

    public void Draw()
    {
        if (filequery_mode && drawn_query_prompt)
        {
            return;
        }

        Console.Clear();
        Console.SetCursorPosition(0, 0);
        bool working = this.spaceParser.parser.working;
        string status;
        if (working) status = "currently parsing";
        else status = "done";

        Console.WriteLine($"SpaceParser ================================= {status} ");

        int line_space_available_to_list = Console.BufferHeight - 2;
        int line_start = 2;
        int line_end = line_start + line_space_available_to_list;

        // System.Console.WriteLine($"line start {line_start} line end {line_end}");
        Console.WriteLine($"files indexed {this.spaceParser.index.files.Count}");

        this.list_buffer = this.spaceParser.index.getfiles(0, line_end - 5);
        Console.WriteLine(list_buffer);

        if (filequery_mode)
        {
            Console.WriteLine($"Type Number to perform action:");
            drawn_query_prompt = true;
        }
        else
        {
            Console.WriteLine($"[Q] Quit || [F] select a file from the list to reveal it your system's file explorer || [D] select a file to delete it");
            drawn_query_prompt = false;
        }

    }
}
