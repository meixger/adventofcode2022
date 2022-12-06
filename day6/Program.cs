var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");
var input = File.ReadAllText(path);

int FindUniqueSequence(string input, int uniqueLength)
{
    for (var i = 0; i < input.Length; i++)
        if (input[i..(i + uniqueLength)].Distinct().Count() == uniqueLength)
            return i + uniqueLength;
    return -1;
}

Console.WriteLine($"Solution1: {FindUniqueSequence(input, 4)}");
Console.WriteLine($"Solution2: {FindUniqueSequence(input, 14)}");
