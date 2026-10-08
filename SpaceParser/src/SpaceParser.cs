


using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace space_parser;

public class SpaceParser
{

    public FileIndex index;
    public Parser parser; //handles the file traversal and sends information to the display to be rendered to the user
    public ParserDisplay display; // handles displaying the list to the user 
    public ParserConsoleInput iOService;

    private string[] args;
    private bool running;
    private string working_directory;

    public object file_index_lock;
    public bool wait;
    private Thread display_thread;
    private Thread parser_thread;
    private Thread io_thread;

    public SpaceParser(string[] args)
    {
        // args = args;
        this.args = args;
        this.TranscribeArguments();
        this.display = new ParserDisplay(this);
        this.index = new FileIndex(this);
        this.parser = new Parser(this);
        this.iOService = new ParserConsoleInput(this);
        this.running = true;
        this.Run();
    }

    private void Run()
    {
        this.display_thread = new Thread(new ThreadStart(display.Run));
        this.parser_thread = new Thread(new ThreadStart(parser.Run));
        this.io_thread = new Thread(new ThreadStart(iOService.Run));

        parser_thread.Start();
        display_thread.Start();
        io_thread.Start();

    }

    public void TranscribeArguments()
    {
        // TODO 
        if (args.Length == 1)
        {
            Directory.SetCurrentDirectory(args[0]);
        }
    }

    public void exit()
    {
        // throw new NotImplementedException();
        var output_buffer = this.index.getfiles(0, 50);
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Environment.Exit(0);
    }

    public void PauseParsing()
    {
        // this.parser_thread.Suspend();
        // throw new NotImplementedException();
    }
}
