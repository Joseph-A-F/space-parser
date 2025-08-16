


namespace space_parser;

public class Parser : IParser
{
    public string working_directory;
    // public List<File> files;
    public SpaceParser spaceParser;
    public bool working = false;

    public string current_file = "";
    public Parser(SpaceParser spaceParser)
    {
        this.spaceParser = spaceParser;
        this.working_directory = Directory.GetCurrentDirectory();
        // this.Run();
    }

    public void Run()
    {
        System.Console.WriteLine(
            "Starting Parser...."
        );
        working = true;
        var working_directory = this.working_directory;
        this.parse_directory(working_directory);
        working = false;
        System.Console.WriteLine("parsing done..");
        // throw new NotImplementedException();
    }

    public void parse_directory(string working_directory)
    {
        if (!CanAccess(working_directory))
            this.spaceParser.index.append(working_directory);
        try
        {
            if (working_directory.Contains(".app")) return;
            string[] files = Directory.GetFiles(working_directory);
            string[] folders = Directory.GetDirectories(working_directory);
            foreach (string file in files)
            {
                // System.Console.WriteLine(file);

                // System.Console.WriteLine($"appending {file}");
                try
                {
                    this.spaceParser.index.append(file);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
            }
            foreach (string folder in folders)
            {
                this.parse_directory(folder);
            }

        }
        catch (UnauthorizedAccessException)
        { }
    }

    private static bool CanAccess(string path)
    {
        if (File.Exists(path))
        {
            return CanAccessFile(path);
        }
        // return false;
        else if (Directory.Exists(path))
        {
            return CanAccessDirectory(path);
        }
        return false;
        // throw new NotImplementedException();
    }

    private static bool CanAccessDirectory(string path)
    {
        try
        {
            var dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles(); // Triggers access check            
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
        catch (OperationCanceledException)
        {
            System.Console.WriteLine($"Operation cancelled checking access for {path}");
            return false;

        }

        // throw new NotImplementedException();
    }

    private static bool CanAccessFile(string path)
    {
        try
        {
            var info = File.Open(path, FileMode.Open);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }

        return false;
        // throw new NotImplementedException();
    }

    public void Start()
    {
        Run();
    }
}

public interface IParser
{

}