using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices.Marshalling;

namespace numbers;

public class NumberFormat
{
    // only goes up to TB
    public static string BytesToHumanReadableFileSize(long number_bytes)
    {
        int formatsep = 1024;
        string[] units = { "B", "KB", "MB", "GB", "TB" };

        string answer = "";
        long size = number_bytes;
        int index = 0;


        while (size >= formatsep && index < units.Length)
        {
            size /= formatsep;
            index++;
        }
        answer = $"{size}{units[index]}";
        return answer;
    }


}
