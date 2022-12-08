var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");
var lines = File.ReadAllLines(path);

var root = new DirEntry("root");

Parse(lines);

void Parse(string[] lines)
{
    DirEntry current = root;
    foreach (var line in lines)
    {
        if (line == "$ cd /")
            continue;

        var parts = line.Split(' ');
        switch (parts[0])
        {
            case "$" when parts[1] == "cd" && parts[2] == "..":
            {
                if (current.Parent == null) throw new InvalidOperationException($"cannot 'cd ..' on ${current.Name}");
                current = current.Parent;
                break;
            }
            case "$" when parts[1] == "cd" && parts[2] != "..":
            {
                var dirname = parts[2];
                current = current.Directories.Single(_ => _.Name == dirname);
                break;
            }
            case "$" when parts[1] == "ls": break;
            case "dir":
            {
                var dirname = parts[1];
                current.AddDir(dirname);
                break;
            }
            case not null when int.TryParse(parts[0], out var size):
            {
                var filename = parts[1];
                current.AddFile(filename, size);
                break;
            }
            default: throw new ArgumentException($"cannot handle {parts[0]}");
        }
    }
}

//Print(root);
//void Print(Entry e, int level = 0)
//{
//    var indent = new string(' ', level * 2);
//    foreach (var d in e.Directories)
//    {
//        Console.WriteLine($"DIR  :{indent} <{d.Name.ToUpper()}> {d.Size()}");
//        Print(d, level + 1);
//    }

//    foreach (var f in e.Files)
//    {
//        Console.WriteLine($"FILE :{indent} {f.Name.ToLower()} {f.Size}");
//    }
//}

var sumOfDirectoriesGreater100000 = FindSumOfDirectoriesGreater(root, 100_000);
Console.WriteLine($"Solution1: {sumOfDirectoriesGreater100000} is the size of all directories > 100000");

int FindSumOfDirectoriesGreater(DirEntry root, int maxSize)
{
    var sum = 0;

    void Recurse(DirEntry dir, int level)
    {
        foreach (var d in dir.Directories)
        {
            var sizeWithSubDirectories = d.CalcSizeWithSubDirectiries();
            if (sizeWithSubDirectories <= maxSize)
            {
                //Console.WriteLine($"DIR  :{indent} <{d.Name.ToUpper()}> {sizeWithSubDirectories}");
                sum += sizeWithSubDirectories;
            }

            Recurse(d, level + 1);
        }
    }

    root.CalcSizeWithSubDirectiries();
    Recurse(root, 0);
    return sum;
}


int fsSize = 70_000_000;
var fsUsed = root.SizeWithSubDirectories;
int fsFree = fsSize - fsUsed;
int minFreeSizeNeeded = 30_000_000;
int minSpaceToBeReleased = minFreeSizeNeeded - fsFree;
//Console.WriteLine($"{fsUsed:0,000} of {fsSize:0,000} used - need to delete min {minSpaceToBeReleased:0,000} to have {minFreeSizeNeeded:0,000} free space");

var allDirs = FindAllDirsWithSize().ToList();
var candidate = allDirs.OrderBy(_ => _.SizeWithSubDirectories).First(_ => _.SizeWithSubDirectories >= minSpaceToBeReleased);
Console.WriteLine($"Solution2: {candidate.SizeWithSubDirectories} bytes will be freed by deleting <{candidate.Name.ToUpper()}>");

IEnumerable<DirEntry> FindAllDirsWithSize()
{
    return Recurse(root);

    IEnumerable<DirEntry> Recurse(DirEntry e)
    {
        foreach (var d in e.Directories)
        {
            //allDirs.Add(d);
            yield return d;
            foreach (var _ in Recurse(d))
                yield return _;
        }
    }
}

public abstract class Entry
{
    public readonly string Name;

    protected Entry(string name)
    {
        Name = name;
    }

    public List<Entry> Childrens { get; } = new();

    public List<FileEntry> Files => Childrens.OfType<FileEntry>().ToList();
    public List<DirEntry> Directories => Childrens.OfType<DirEntry>().ToList();

    public DirEntry? Parent { get; init; }
}

public class DirEntry : Entry
{
    public DirEntry(string name) : base(name)
    {
    }

    public void AddFile(string filename, int size) => Childrens.Add(new FileEntry(filename, size) { Parent = this });

    public void AddDir(string dirname) => Childrens.Add(new DirEntry(dirname) { Parent = this });

    public int CalcSizeWithSubDirectiries()
    {
        var sizeThis = Childrens.OfType<FileEntry>().Sum(_ => _.Size);
        var sizeOfChildren = Childrens.OfType<DirEntry>().Sum(_ => _.CalcSizeWithSubDirectiries());
        SizeWithSubDirectories = sizeThis + sizeOfChildren;
        return SizeWithSubDirectories;
    }

    public int SizeWithSubDirectories { get; set; }
}

public class FileEntry : Entry
{
    public FileEntry(string filename, int size) : base(filename)
    {
        Size = size;
    }

    public readonly int Size;
}