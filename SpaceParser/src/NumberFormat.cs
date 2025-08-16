using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices.Marshalling;

namespace numbers;

public class NumberFormat
{
    public static string NumberToHumanReadableSize(long number)
    {
        int formatsep = 1024;
        string[] units = { "B", "KB", "MB", "GB", "TB" };

        string answer = "";
        long size = number;
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
