using System.Runtime.Serialization;

namespace space_parser;

public class FileNode : IComparable
{
    public DateTime last_access_date;
    public string path;
    public string filename;
    public long filesize;
    public float score;

    public int CompareTo(object? obj)
    {
        int answer = 0;
        if (obj.GetType() != this.GetType()) return 0;
        FileNode obj_as_file = (FileNode)obj;
        answer = score.CompareTo(obj_as_file.score);
        return answer;
        // throw new NotImplementedException();
    }
}

public class FileIndex // needs to be in a lock in order to be written to. perhaps this class could have its own lock to abstract it 
{
    public List<FileNode> files;
    public SpaceParser spaceParser;

    public readonly object index_lock = new object();


    public FileIndex(SpaceParser spaceParser)
    {
        this.files = new List<FileNode>();
        this.spaceParser = spaceParser;
    }

    public void append(string filepath)
    {
        if (!Path.Exists(filepath))
        {
            return;
        }
        if (File.Exists(filepath)) append_file(filepath);
        // if (Directory.Exists(filepath)) append_folder(filepath);
    }

    private void append_folder(string filepath)
    {
        FileAttributes fileAttributes = File.GetAttributes(filepath);

        DateTime last_access_date = File.GetLastAccessTime(filepath);
        long filesize = DirectorySize(new DirectoryInfo(filepath));
        FileNode newfile = new FileNode();
        newfile.last_access_date = last_access_date;
        newfile.filesize = filesize;
        System.Console.WriteLine($"appending {filepath}");
        files.Append(newfile);
        this.rescore();
    }

    private long DirectorySize(DirectoryInfo directoryInfo)
    {
        long answer = 0;
        FileInfo[] files;
        DirectoryInfo[] dirs;

        try
        {
            files = directoryInfo.GetFiles();
            dirs = directoryInfo.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return 0;

        }
        catch (IOException)
        {
            return 0;

        }
        foreach (var file in files)
        {
            answer += file.Length;
        }
        foreach (var dir in dirs)
        {
            answer += DirectorySize(dir);
        }

        return answer;
    }

    private static long DirectorySize(string filepath)
    {
        throw new NotImplementedException();
        long answer = 0;
        var files = Directory.GetFiles(filepath);
        var dirs = Directory.GetDirectories(filepath);
        foreach (var file in files)
        {

        }

        return answer;

    }

    private void append_file(string filepath)
    {
        FileAttributes fileAttributes = File.GetAttributes(filepath);

        DateTime last_access_date = File.GetLastAccessTime(filepath);
        long filesize = new FileInfo(filepath).Length;
        FileNode newfile = new FileNode();
        newfile.last_access_date = last_access_date;
        newfile.filesize = filesize;

        files.Append(newfile);
        // System.Console.WriteLine($"files count {files.Count}");
        this.rescore();
    }

    public void rescore()
    {
        ulong max_size = 0;
        long max_difference = 0;
        DateTime now = DateTime.Now;
        foreach (var file in files)
        {
            ulong size = (ulong)file.filesize;
            DateTime file_date = file.last_access_date;
            long difference = Math.Abs(now.CompareTo(file_date));

            if (size > max_size) max_size = size;
            if (difference > max_difference) max_difference = difference;
        }
        foreach (var file in files)
        {
            var normalized_size = ((double)file.filesize / (double)max_size);
            var normalized_difference = ((double)now.CompareTo(file.last_access_date) / (double)max_difference);
        }
        // this.files.Sort();

    }

    public string getfiles(int index_start, int index_end)
    {
        string answer = "";
        // TODO: implement 
        for (int i = index_start; i < index_end; i++)
        {
            if (i < this.files.Count)
            {
                string file_string = generate_string_for_index(index_start);
                // System.Console.WriteLine(file_string);
                answer += file_string + "\n";
            }
        }
        return answer;
    }

    private string generate_string_for_index(int index_start)
    {
        System.Console.WriteLine($"index start {index_start}");
        FileNode string_list = files[index_start];
        return string_list.filename;
        // throw new NotImplementedException();
    }
}