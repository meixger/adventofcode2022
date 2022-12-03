var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");

var rucksacks = File.ReadAllLines(path);

var itemTypes = new List<char>();
itemTypes.AddRange(Enumerable.Range('a', 26).Select(Convert.ToChar));
itemTypes.AddRange(Enumerable.Range('A', 26).Select(Convert.ToChar));

var solutionA = 0;
foreach (var rucksack in rucksacks)
{
    var length = rucksack.Length;
    var (compartement1, compartement2) = (rucksack.Substring(0, length / 2), rucksack.Substring(length / 2, length / 2));

    foreach (var itemType in itemTypes)
        if (compartement1.Contains(itemType) && compartement2.Contains(itemType))
            solutionA += itemTypes.IndexOf(itemType) + 1;
}

Console.WriteLine($"SolutionA: {solutionA}");

int solutionB = 0;
foreach (var group in rucksacks.Chunk(3))
{
    foreach (var itemType in itemTypes)
        if (group.All(g => g.Contains(itemType)))
            solutionB += itemTypes.IndexOf(itemType) + 1;
}

Console.WriteLine($"SolutionB: {solutionB}");