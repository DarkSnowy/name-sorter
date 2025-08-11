using benchmark;

var workingDirectory = Directory.GetCurrentDirectory();
var filename = workingDirectory + "\\..\\..\\..\\..\\1000-names.txt";
List<long> executionTicks = [];

/* ConsoleWithStringBuilder was clear winner, no need to run these benchmarks every time
var allLines = File.ReadAllLines(filename).ToList();

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.ConsoleWriteLine(allLines);
    executionTicks.Add(ticks);
}

var averageConsoleWriteLine = executionTicks.Average();
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.ConsoleWithStringBuilder(allLines);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of ConsoleWriteLine: " + averageConsoleWriteLine.ToString("N0"));
Console.WriteLine("Average ticks of ConsoleWithStringBuilder: " + executionTicks.Average().ToString("N0"));
executionTicks = [];
*/

for (var index = 0; index < 1000; index++) {
    var ticks = BenchmarkMethods.UseConcurrentBag(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseConcurrentBag: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.UseConcurrentDictionaryWithChars(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseConcurrentDictionaryWithChars: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = await BenchmarkMethods.UseFileStreamAndSortedList(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseFileStreamAndSortedList: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.UseSortedList(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseSortedList: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.UseDoubleSortedList(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseDoubleSortedList: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = await BenchmarkMethods.UseDoubleSortedListAsync(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseDoubleSortedListAsync: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.UseSortedListWithChars(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseSortedListWithChars: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = BenchmarkMethods.UseSortedListWithNameCount(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseSortedListWithNameCount: " + executionTicks.Average().ToString("N0"));
executionTicks = [];

for (var index = 0; index < 1000; index++)
{
    var ticks = await BenchmarkMethods.UseSortedListAsync(filename);
    executionTicks.Add(ticks);
}

Console.WriteLine("Average ticks of UseSortedListAsync: " + executionTicks.Average().ToString("N0"));

return 0;