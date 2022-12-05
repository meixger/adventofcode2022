var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");
var lines = File.ReadAllLines(path);

Dictionary<int, Stack<char>> ParseStacks(string[] lines)
{
    var stackCount = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).Count();
    var stacks = new Dictionary<int, Stack<char>>();
    for (var i = 0; i < stackCount; i++)
        stacks.Add(i, new Stack<char>());
    for (var i = lines.Length - 2; i >= 0; i--)
    {
        var line = lines[i];
        for (var j = 0; j <= line.Length / 4; j++)
        {
            var c = line[j * 4 + 1];
            if (c != ' ') stacks[j].Push(c);
        }
    }

    return stacks;
}

List<(int count, int from, int to)> ParseMoves(string[] lines)
{
    List<(int count, int from, int to)> moves = new();
    foreach (var line in lines)
    {
        var parts = line.Split(' ');
        moves.Add((int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1));
    }

    return moves;
}

var indexOfEmptyLine = Array.FindIndex(lines, _ => _ == "");
var stacks = ParseStacks(lines[..indexOfEmptyLine]);
var moves = ParseMoves(lines[(indexOfEmptyLine + 1)..]);

//foreach (var stack in stacks)
//    Console.WriteLine($"{stack.Key + 1}: {string.Join(' ', stack.Value)}");

foreach (var move in moves)
{
    for (var i = 0; i < move.count; i++)
    {
        var c = stacks[move.from].Pop();
        stacks[move.to].Push(c);
    }
}

var solution1 = string.Join("", stacks.Select(_ => _.Value.ToArray()[0]));
Console.WriteLine($"Solution1: {solution1}");

stacks = ParseStacks(lines[..indexOfEmptyLine]);
foreach (var move in moves)
{
    var items = new List<char>();
    for (var i = 0; i < move.count; i++)
        items.Add(stacks[move.from].Pop());
    items.Reverse();
    foreach (var item in items)
        stacks[move.to].Push(item);
}

var solution2 = string.Join("", stacks.Select(_ => _.Value.ToArray()[0]));
Console.WriteLine($"Solution2: {solution2}");