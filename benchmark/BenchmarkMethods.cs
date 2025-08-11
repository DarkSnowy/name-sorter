using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace benchmark;

public static class BenchmarkMethods
{
    public static long UseConcurrentBag(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        ConcurrentBag<string> names = [];

        Parallel.ForEach(File.ReadLines(filename), line =>
        {
            line = line.Trim();
            var indexOf = line.LastIndexOf(' ');
            char startingChar = line[0];
            string lastName = line[indexOf..];
            string otherNames = line[..indexOf];

            names.Add(lastName + " " + otherNames);
        });

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        var sortedNames = names.Order().ToList();

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + readtime.ToString("N0") + " sort:" + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static long UseConcurrentDictionaryWithChars(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var buckets = new ConcurrentDictionary<char, ConcurrentBag<string>>();

        Parallel.ForEach(File.ReadLines(filename), line =>
        {
            line = line.Trim();
            var indexOf = line.LastIndexOf(' ');
            char startingChar = line[0];
            string lastName = line[indexOf..];
            string otherNames = line[..indexOf];

            var bucket = buckets.GetOrAdd(startingChar, []);
            bucket.Add($"{lastName} {otherNames}");
        });

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        List<string> names = [];

        foreach (var startingChar in buckets.Keys.Order()) {
            foreach (var line in buckets[startingChar].Order()) {
                var indexOf = line.LastIndexOf(' ');
                string lastName = line[..indexOf];
                string otherNames = line[(indexOf + 1)..];

                names.Add($"{otherNames} {lastName}");
            }
        }

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + readtime.ToString("N0") + " sort:" + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static async Task<long> UseFileStreamAndSortedList(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Initialize a new MemoryStream with the byte array
        FileStream filePath = new(path: filename, mode: FileMode.Open);
        using var reader = new StreamReader(filePath);

        SortedList<string, List<string>> sortedNames = [];
        string? line = await reader.ReadLineAsync();

        while (line != null)
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(lastName, out List<string>? value))
            {
                value.Add(otherNames);
            }
            else
            {
                sortedNames.Add(lastName, [otherNames]);
            }

            line = await reader.ReadLineAsync();
        } 

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        foreach (var valuePair in sortedNames)
        {
            valuePair.Value.Sort();
        }

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + readtime.ToString("N0") + " sort: " + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static long UseSortedList(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedList<string, List<string>> sortedNames = [];

        foreach (string line in File.ReadLines(filename))
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(lastName, out List<string>? value))
            {
                value.Add(otherNames);
            }
            else
            {
                sortedNames.Add(lastName, [otherNames]);
            }
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        foreach (var valuePair in sortedNames)
        {
            valuePair.Value.Sort();
        }

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + readtime.ToString("N0") + " sort time: " + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static long UseDoubleSortedList(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedList<string, SortedList<string, int>> sortedNames = [];

        foreach (string line in File.ReadLines(filename))
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(lastName, out SortedList<string, int>? sortedOtherNames))
            {
                var count = sortedOtherNames.GetValueOrDefault(otherNames, 0);
                count++;
                sortedNames[lastName][otherNames] = count;
            }
            else
            {
                sortedNames[lastName] = new SortedList<string, int> { { otherNames, 1 } };
            }
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read and sort: " + readtime.ToString("N0"));
        return readtime;
    }

    public static async Task<long> UseDoubleSortedListAsync(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedList<string, SortedList<string, int>> sortedNames = [];

        await foreach (string line in File.ReadLinesAsync(filename))
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf + 1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(lastName, out SortedList<string, int>? sortedOtherNames))
            {
                var count = sortedOtherNames.GetValueOrDefault(otherNames, 0);
                count++;
                sortedNames[lastName][otherNames] = count;
            }
            else
            {
                sortedNames[lastName] = new SortedList<string, int> { { otherNames, 1 } };
            }
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read and sort: " + readtime.ToString("N0"));
        return readtime;
    }

    public static long UseSortedListWithChars(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedDictionary<char, List<string>> sortedNames = [];

        foreach (string line in File.ReadLines(filename))
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            char startingChar = line[0];
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(startingChar, out List<string>? value))
            {
                value.Add($"{lastName} {otherNames}");
            }
            else
            {
                sortedNames.Add(startingChar, [$"{lastName} {otherNames}"]);
            }
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        List<string> names = [];

        foreach (var valuePair in sortedNames)
        {
            foreach (var name in valuePair.Value.Order())
            {
                var indexOf = name.IndexOf(' ');
                string lastName = name[..indexOf];
                string otherNames = name[indexOf..];

                names.Add($"{otherNames} {lastName}");
            }
        }

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read and : " + (readtime + sortTime).ToString("N0") + " sort: " + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static long UseSortedListWithNameCount(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedList<string, int> sortedNames = [];

        foreach (string line in File.ReadLines(filename))
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];
            name = $"{lastName} {otherNames}";

            var count = sortedNames.GetValueOrDefault(name, 0);
            count++;
            sortedNames[name] = count;
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        var stringBuilder = new StringBuilder();

        foreach (var valuePair in sortedNames)
        {
            for(int index = 0; index < valuePair.Value; index++)
            {
                var name = valuePair.Key;
                var indexOf = name.IndexOf(' ');
                string otherNames = name[indexOf..];
                string lastName = name[..indexOf];
                name = $"{otherNames} {lastName}";

                stringBuilder.AppendLine(name);
            }
        }

        stopwatch.Stop();
        var sort = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + readtime.ToString("N0") + " sort: " + sort.ToString("N0"));
        return readtime + sort;
    }

    public static async Task<long> UseSortedListAsync(string filename)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        SortedList<string, List<string>> sortedNames = [];

        var readLinesAsync = File.ReadLinesAsync(filename);

        await foreach (var line in readLinesAsync)
        {
            var name = line.Trim();
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf+1)..];
            string otherNames = name[..indexOf];

            if (sortedNames.TryGetValue(lastName, out List<string>? value))
            {
                value.Add(otherNames);
            }
            else
            {
                sortedNames.Add(lastName, [otherNames]);
            }
        }

        stopwatch.Stop();
        var readtime = stopwatch.ElapsedTicks;
        stopwatch.Restart();

        foreach (var valuePair in sortedNames)
        {
            valuePair.Value.Sort();
        }

        stopwatch.Stop();
        var sortTime = stopwatch.ElapsedTicks;

        //Console.WriteLine("Read: " + (readtime + sortTime).ToString("N0") + " sort: " + sortTime.ToString("N0"));
        return readtime + sortTime;
    }

    public static long ConsoleWriteLine(List<string> names)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var name in names)
        {
            Console.WriteLine(name);
        }

        stopwatch.Stop();
        return stopwatch.ElapsedTicks;
    }

    public static long ConsoleWithStringBuilder(List<string> names)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var stringBuilder = new StringBuilder();

        foreach (var name in names)
        {
            stringBuilder.AppendLine(name);
        }

        Console.Write(stringBuilder.ToString());

        stopwatch.Stop();
        return stopwatch.ElapsedTicks;
    }
}
