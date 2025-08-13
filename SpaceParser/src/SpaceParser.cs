


using System.Data;
using System.Runtime.InteropServices;

namespace space_parser;

public class SpaceParser
{
    public Parser parser; //handles the file traversal and sends information to the display to be rendered to the user
    public ParserDisplay display; // handles displaying the list to the user 
    private string[] args;
    private bool running;
    private string working_directory;

    public FileIndex index;
    public object file_index_lock;

    public SpaceParser()
    {
        this.args = new string[] { "" };
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
        // working_directory = args[0];
        // throw new NotImplementedException();
    }
}

