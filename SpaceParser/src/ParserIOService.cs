
namespace space_parser;

public class ParserConsoleInput
{


    private SpaceParser spaceParser;
    private bool running;

    public ParserConsoleInput(SpaceParser spaceParser)
    {
        this.spaceParser = spaceParser;
    }

    public void Run()
    {
        running = true;
        System.Console.WriteLine("starting IO service....");
        while (running)
        {
            Update();
        }
    }


    public void Update()
    {
        System.Console.WriteLine("key available");
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Q)
        {
            this.spaceParser.exit();
        }
        if (key.Key == ConsoleKey.F)
        {
            this.spaceParser.display.filequery_mode = true;
            Thread.Sleep(150); // wait for display loop to render prompt
            var new_key = Console.ReadLine();
            int file_index;
            if (int.TryParse(new_key, out file_index))
            {
                lock (this.spaceParser.index.index_lock)
                {
                    this.spaceParser.index.RevealFileIndexInExplorer(file_index - 1);
                }
            }
            this.spaceParser.display.filequery_mode = false;
        }
        else if (key.Key == ConsoleKey.D)
        {
            this.spaceParser.display.filequery_mode = true;
            Thread.Sleep(150); // wait for display loop to render prompt
            var new_key = Console.ReadLine();
            int file_index;
            if (int.TryParse(new_key, out file_index))
            {
                lock (this.spaceParser.index.index_lock)
                {
                    this.spaceParser.index.DeleteFileIndex(file_index - 1);
                }
            }
            this.spaceParser.display.filequery_mode = false;
        }
    }
}