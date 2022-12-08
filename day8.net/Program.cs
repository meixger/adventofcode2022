var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");
var lines = File.ReadAllLines(path);

var rows = lines.ToList().Select(_ => _.ToCharArray().ToArray()).ToList();

int FindVisibleTrees()
{
    int Test(char[] seq, int pos)
    {
        var before = seq[..pos];
        var current = seq[pos];
        var after = seq[(pos + 1)..];
        return before.Max() < current || current > after.Max() ? pos : -1;
    }

    HashSet<(int x, int y)> visibleOnInside = new();
    for (var y = 1; y < rows.Count - 1; y++)
    {
        var row = rows[y];
        for (var x = 1; x < row.Length - 1; x++)
        {
            var col = rows.Select(_ => _[x]).ToArray();
            var foundX = Test(row, x);
            var foundY = Test(col, y);
            if (foundX > -1 || foundY > -1) visibleOnInside.Add((x, y));
        }
    }

    return visibleOnInside.Count;
}

var visibleOnEdge = (rows[0].Length + rows.Count) * 2 - 4;
var visibleOnInside = FindVisibleTrees();
Console.WriteLine($"Solution1: {visibleOnEdge + visibleOnInside}");

int CalcScenicScore()
{
    List<int> scores = new();

    IEnumerable<int> CalcOneDimension(char[] row, int pos)
    {
        var current = row[pos];

        var left = row[..pos];
        var l = 0;
        foreach (var c in left.Reverse())
        {
            l++;
            if (c >= current) break;
        }

        yield return l;

        var right = row[(pos + 1)..];
        var r = 0;
        foreach (var c in right)
        {
            r++;
            if (c >= current) break;
        }

        yield return r;
    }

    for (var y = 1; y < rows.Count - 1; y++)
    {
        var row = rows[y];
        for (var x = 1; x < row.Length - 1; x++)
        {
            var col = rows.Select(_ => _[x]).ToArray();
            var score = CalcOneDimension(row, x)
                .Concat(CalcOneDimension(col, y))
                .Aggregate(1, (a, b) => a * b);
            scores.Add(score);
        }
    }

    return scores.Max();
}

var scenicScore = CalcScenicScore();
Console.WriteLine($"Solution2: {scenicScore}");