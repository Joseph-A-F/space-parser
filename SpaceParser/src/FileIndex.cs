namespace space_parser;

public struct File
{
    string path;
    string filename;
    DateTime creation_date;
    ulong filesize;

}

public class FileCollection
{
    public List<File> files;
    public SpaceParser spaceParser;

}