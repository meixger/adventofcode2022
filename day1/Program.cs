using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

new Solutions().Run();

Console.WriteLine("Press any key to run benchmarks or ^c to exit");
Console.ReadKey();
BenchmarkRunner.Run<Solutions>();

[MemoryDiagnoser]
public class Solutions
{
    string path = Path.Combine(Environment.CurrentDirectory, @".\input.txt");

    [Benchmark]
    public int SolutionA()
    {
        var caloriesPerElve = File
            .ReadAllText(path)
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(e => Array.ConvertAll(e.Split(Environment.NewLine), int.Parse))
            .Select(_ => _.Sum());

        var top1 = caloriesPerElve
            .Max();

        return top1;
    }

    // faster, less memory allocations
    [Benchmark]
    public int SolutionAStreaming()
    {
        using var stream = File.OpenRead(path);
        using var sr = new StreamReader(stream);
        var highestCalories = 0;
        while (!sr.EndOfStream)
        {
            var currentCalories = 0;
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentCalories > highestCalories)
                        highestCalories = currentCalories;
                    currentCalories = 0;
                }
                else
                    currentCalories += int.Parse(line);
            }
        }

        return highestCalories;
    }

    [Benchmark]
    public int SolutionB()
    {
        var caloriesPerElve = System.IO.File
            .ReadAllText(path)
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(e => Array.ConvertAll(e.Split(Environment.NewLine), int.Parse))
            .Select(_ => _.Sum());

        var top3Sum = caloriesPerElve
            .OrderByDescending(_ => _)
            .Take(3)
            .Sum();

        return top3Sum;
    }

    public void Run()
    {
        ExecuteAndMeasure(SolutionA, nameof(SolutionA));
        ExecuteAndMeasure(SolutionAStreaming, nameof(SolutionAStreaming));
        ExecuteAndMeasure(SolutionB, nameof(SolutionB));
    }

    void ExecuteAndMeasure(Func<int> func, string name)
    {
        for (var i = 0; i < 10; i++) func();
        var sw = new Stopwatch();
        sw.Start();
        var solution = func();
        sw.Stop();
        Console.WriteLine($"Result of {name}: {solution} - {sw.ElapsedMilliseconds} ms - {sw.ElapsedTicks} ticks");
    }
}