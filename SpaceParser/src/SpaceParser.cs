


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

    public FileCollection fileCollection;
    public object file_index_lock;

    public SpaceParser()
    {
        this.args = new string[] { "" };
        this.TranscribeArguments();
        this.parser = new Parser();
        this.display = new ParserDisplay();
        this.fileCollection = new FileCollection();
        this.running = true;
        this.Run();
    }

    private void Run()
    {
        Thread display_thread = new Thread(new ThreadStart(display.Run));
        display_thread.Start();
    }

    public void TranscribeArguments()
    {
        // working_directory = args[0];
        // throw new NotImplementedException();
    }
}

