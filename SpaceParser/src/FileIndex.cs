using System.Runtime.Serialization;
using numbers;

namespace space_parser;

public class FileNode : IComparable
{
    public DateTime date;
    public string path;
    public string filename;
    public long filesize;
    public double score;
    public string filepath;

    public FileNode(string filename,
                    string filepath,
                    DateTime last_access_date,
                    long filesize)
    {
        this.filename = filename;
        this.filepath = filepath;
        this.date = last_access_date;
        this.filesize = filesize;
    }

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

public class FileIndex // needs to be in a lock in order to be written to.
//  perhaps this class could have its own lock to abstract it 
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
        if (Directory.Exists(filepath)) append_folder(filepath);
    }

    private void append_folder(string filepath)
    {
        string filename = Path.GetFileName(filepath);
        FileAttributes fileAttributes = File.GetAttributes(filepath);
        DateTime last_access_date = File.GetLastAccessTime(filepath);
        long filesize = DirectorySize(new DirectoryInfo(filepath));
        FileNode newfile = new FileNode(filename, filepath, last_access_date, filesize);


        newfile.date = last_access_date;
        newfile.filesize = filesize;

        System.Console.WriteLine($"appending {filepath}");

        files.Add(newfile);
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

    private void append_file(string filepath)
    {
        string filename = Path.GetFileName(filepath);
        FileAttributes fileAttributes = File.GetAttributes(filepath);
        DateTime last_access_date = File.GetCreationTime(filepath);
        long filesize = new FileInfo(filepath).Length;

        FileNode newfile = new FileNode(filename, filepath, last_access_date, filesize);

        files.Add(newfile);
        // System.Console.WriteLine($"files count {files.Count}");
        this.rescore();
    }

    public void rescore()
    {
        lock (this.index_lock)
        {
            ulong max_size = 0;
            long max_difference = 0;
            DateTime now = DateTime.Now;
            foreach (var file in files)
            {
                ulong size = (ulong)file.filesize;
                DateTime file_date = file.date;
                long difference = now.CompareTo(file_date);

                if (size > max_size) max_size = size;
                if (difference > max_difference) max_difference = difference;
            }
            foreach (var file in files)
            {
                double normalized_size = (file.filesize / (double)max_size);
                double normalized_difference = (now.CompareTo(file.date) / max_difference);
                double new_score = 0.5 * normalized_difference + 0.5 * normalized_size;
                file.score = new_score;
            }

            this.files.Sort();
            this.files.Reverse();
        }

    }

    public string getfiles(int index_start, int index_end)
    {
        lock (this.index_lock)
        {
            string answer = "";
            // TODO: implement 
            for (int i = index_start; i < index_end; i++)
            {
                if (i < this.files.Count)
                {
                    string file_string = generate_string_for_index(i);
                    // System.Console.WriteLine(file_string);
                    answer += file_string + "\n";
                }
            }
            return answer;
        }
    }

    private string generate_string_for_index(int index_start)
    {
        // System.Console.WriteLine($"index start {index_start}");
        FileNode string_list = files[index_start];


        string answer = $"{index_start + 1,-4} | {string_list.score.ToString("0.00"),-5} {string_list.date,-25} {NumberFormat.NumberToHumanReadableSize((long)string_list.filesize),-9} {string_list.filepath}";
        return answer;
        // throw new NotImplementedException();
    }
}