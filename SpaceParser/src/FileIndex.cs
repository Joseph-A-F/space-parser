using System.Diagnostics;
using System.Runtime.Serialization;
using numbers;

namespace space_parser;

public class FileNode : IComparable
{
    public DateTime date;
    public string path;
    public string filename;
    public ulong filesize;
    public double score;
    public string filepath;
    internal string filetype;

    public FileNode(string filename,
                    string filepath,
                    DateTime last_access_date,
                    long filesize
                    )
    {
        this.filename = filename;
        this.filepath = filepath;
        this.date = last_access_date;
        this.filesize = (ulong)filesize;
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
{
    public List<FileNode> files;
    public SpaceParser spaceParser;
    private ulong max_size;
    private int max_difference;
    public readonly object index_lock = new object();

    public FileIndex(SpaceParser spaceParser)
    {
        this.files = new List<FileNode>();
        this.spaceParser = spaceParser;
    }

    public async void append(string filepath)
    {
        if (!Path.Exists(filepath))
        {
            return;
        }
        if (File.Exists(filepath)) append_file(filepath);
        else if (Directory.Exists(filepath)) append_folder(filepath);
    }

    private void append_folder(string filepath)
    {
        // System.Console.WriteLine(filepath);
        try
        {
            string filename = Path.GetFileName(filepath);

            DateTime last_access_date = File.GetLastWriteTime(filepath);

            long filesize = DirectorySize(filepath);
            // System.Console.WriteLine($"filesize {filesize}");
            FileNode newfile = new FileNode(filename, filepath, last_access_date, filesize);

            newfile.filetype = "Folder";


            // System.Console.WriteLine($"appending {filepath}");

            files.Add(newfile);
            this.rescore();
        }
        catch (Exception)
        {
            System.Console.WriteLine($"oops");
            throw;
        }
    }

    private long DirectorySize(string filepath)
    {
        long answer = 0;
        try
        {
            string[] files = Directory.GetFiles(filepath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                answer += new FileInfo(file).Length;
            }

        }
        catch (System.Exception ex)
        {
            // TODO
        }
        return answer;
        // throw new NotImplementedException();
    }

    private void append_file(string filepath)
    {
        string filename = Path.GetFileName(filepath);
        FileAttributes fileAttributes = File.GetAttributes(filepath);
        DateTime last_access_date = File.GetLastWriteTime(filepath);
        long filesize = new FileInfo(filepath).Length;

        FileNode newfile = new FileNode(filename, filepath, last_access_date, filesize);
        newfile.filetype = "File";
        files.Add(newfile);
        // System.Console.WriteLine($"files count {files.Count}");
        this.rescore();
    }

    public void rescore()
    {
        lock (index_lock)
        {
            max_size = 0;
            max_difference = 0;
            DateTime now = DateTime.Now;
            foreach (FileNode file in files)
            {
                ulong size = (ulong)file.filesize;
                DateTime file_date = file.date;
                long difference = now.CompareTo(file_date);

                if (size > max_size) max_size = size;
                if (difference > max_difference) max_difference = (int)difference;
            }
            foreach (FileNode file in files)
            {
                double normalized_size = file.filesize / (double)max_size;
                double normalized_difference = now.CompareTo(file.date) / max_difference;

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
        int width_of_buffer = Console.BufferWidth;

        string score = string_list.score.ToString("0.00");

        DateTime date = string_list.date;

        string filesize_percentage_str = $" ({(float)string_list.filesize / (float)this.max_size})";
        string filesize = NumberFormat.BytesToHumanReadableFileSize((long)string_list.filesize) + filesize_percentage_str;


        string filetype = string_list.filetype;

        string filepath = string_list.filepath;


        string answer = $"{index_start + 1,-4} | {score,-5} {date,-25} {filesize,-20} {filetype,-6} {filepath}";

        if (answer.Length > width_of_buffer)
        {
            answer = answer.Substring(0, width_of_buffer);
        }

        return answer;
        // throw new NotImplementedException();
    }


    public void RevealFileIndexInExplorer(int file_index)
    {
        if (file_index < 0 || file_index >= this.files.Count) return;
        var file = this.files[file_index];
        var filepath = file.filepath;
        var path_to_open = Path.GetPathRoot(filepath);
        var process = new Process();
        process.StartInfo.FileName = "open";
        if (Directory.Exists(filepath))
        {
            process.StartInfo.Arguments = $"\"{filepath}\"";
        }
        else
        {
            process.StartInfo.Arguments = $"-R \"{filepath}\"";
        }
        process.Start();
    }

    public void DeleteFileIndex(int file_index)
    {
        if (file_index < 0 || file_index >= this.files.Count) return;
        var file = this.files[file_index];
        var filepath = file.filepath;
        try
        {
            if (Directory.Exists(filepath))
            {
                Directory.Delete(filepath, true);
            }
            else if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            this.files.RemoveAt(file_index);
        }
        catch (Exception)
        {
            // Ignore for now
        }
    }
}

