var path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");

var rounds = File
    .ReadAllLines(path)
    .Select(_ => _.Split(" ")
                .Select(Convert.ToChar)
                .ToArray()
    );

int Result(char opponent, char me)
{
    if (me == opponent) return 0;
    if (opponent == 'A' && me == 'C') return -1;
    if (opponent == 'C' && me == 'A') return 1;
    return opponent < me ? 1 : -1;
}

int PointsOfGesture(char value) => value - 'A' + 1;

int PointsOfResult(int result) =>
    result switch
    {
        -1 => 0,
        0 => 3,
        1 => 6,
    };

var totalPointsA = 0;
foreach (var round in rounds)
{
    var (opponent, me) = (round[0], (char)(round[1] - 23));

    var result = Result(opponent, me);
    var points = PointsOfGesture(me) + PointsOfResult(result);
    totalPointsA += points;
}
Console.WriteLine($"SolutionA: {totalPointsA}");

char GuestureWhen(char opponent, char result) =>
    result switch
    {
        // draw
        'Y' => opponent,
        // loose
        'X' => (char)(opponent - 1 < 'A' ? 'C' : opponent - 1),
        // win
        'Z' => (char)(opponent + 1 > 'C' ? 'A' : opponent + 1),
    };

var totalPointsB = 0;
foreach (var round in rounds)
{
    var (opponent, result) = (round[0], round[1]);

    var guesture = GuestureWhen(opponent, result);
    var points = PointsOfGesture(guesture) + PointsOfResult(result == 'X' ? -1 : result == 'Y' ? 0 : 1);
    totalPointsB += points;
}
Console.WriteLine($"SolutionB: {totalPointsB}");