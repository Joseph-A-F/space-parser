


using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace space_parser;

public class SpaceParser
{

    public FileIndex index;
    public Parser parser; //handles the file traversal and sends information to the display to be rendered to the user
    public ParserDisplay display; // handles displaying the list to the user 
    public DisplayUserInput input;
    private string[] args;
    private bool running;
    private string working_directory;

    public object file_index_lock;

    public SpaceParser(string[] args)
    {
        // args = args;
        this.args = args;
        this.TranscribeArguments();
        this.display = new ParserDisplay(this);
        this.index = new FileIndex(this);
        this.parser = new Parser(this);
        this.running = true;
        this.Run();
    }

    private void Run()
    {
        Thread display_thread = new Thread(new ThreadStart(display.Run));
        Thread parser_thread = new Thread(new ThreadStart(parser.Run));
        parser_thread.Start();
        display_thread.Start();

    }

    public void TranscribeArguments()
    {
        // TODO 
        if (args.Length == 1)
        {
            Directory.SetCurrentDirectory(args[0]);
        }
    }
}

