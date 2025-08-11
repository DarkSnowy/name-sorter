using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using name_sorter.interfaces;
using name_sorter.services;
using System.Text;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// Register services for dependency injection
builder.Services.AddScoped<INameReaderService, NameReaderService>();
builder.Services.AddScoped<INameWriterService, NameWriterService>();
using IHost host = builder.Build();

// Start the host environment (this is actually unnessesary for this program)
var hostTask = host.StartAsync();

// Start the main program
var result = await main(host.Services, args);

// Await completion of the program and return its resulting code, which will be 0 on a successful execution
await hostTask;
return result;

static async Task<int> main(IServiceProvider hostProvider, string[] args)
{
    using var serviceScope = hostProvider.CreateAsyncScope();
    // Get the working directory for resolving where to read and write files.
    var workingDirectory = Directory.GetCurrentDirectory();
    string? filename;

    var resolveReletivePath = (string filename) =>
    {
        if (filename.StartsWith("./") || filename.StartsWith(".\\"))
        {
            // We are expecting relative filepaths and will resolve the full path by prefixing the working directory
            return string.Concat(workingDirectory, "\\", filename.AsSpan(2));
        }

        return filename;
    };

    if (args.Length > 0 && args[0].Length > 0)
    {
        // Get the filename from the provided argument.
        filename = args[0];
        filename = resolveReletivePath(filename);
    }
    else
    {
        // No argument was provided so we prompt the user for a filename
        Console.WriteLine("Supply values for the following parameters: \nfilename: ");
        filename = Console.ReadLine();

        if (filename == null || filename.Length == 0)
        {
            Console.WriteLine("Cannot accept filename argument because it is an empty string");
            return -1;
        }

        filename = resolveReletivePath(filename);
    }

    // Make sure the file exists
    if (!File.Exists(filename))
    {
        Console.WriteLine("File does not exist: " + filename);
        return -1;
    }

    // Resolve the services this program will use, as per the Inversion of Control design principle
    var nameReader = serviceScope.ServiceProvider.GetRequiredService<INameReaderService>();
    var nameWriter = serviceScope.ServiceProvider.GetRequiredService<INameWriterService>();

    // As per the Single Responsibility Principle, this method will return the list of names already sorted
    var names = await nameReader.ReadNamesFromFile(filename);

    var stringBuilder = new StringBuilder();

    // Build up the string to be written into the file and to the console. (This is faster than writing lines directly)
    foreach(var lastnameValuePair in names)
    {
        foreach (var firstnamesValuePair in lastnameValuePair.Value)
        {
            for(int index = 0; index < firstnamesValuePair.Value; index++)
            {
                stringBuilder.AppendLine($"{firstnamesValuePair.Key} {lastnameValuePair.Key}");
            }
        }
    }

    // For now the output file is hardcoded, as per project specifications
    var sortedFilepath = workingDirectory + "\\sorted-names-list.txt";

    // Create a task for writing the string to a file
    var writeNamesTask = nameWriter.WriteNamesToFile(sortedFilepath, stringBuilder);
    // Write the string of names to the console at the same time that the file-write task is running
    Console.Write(stringBuilder.ToString());

    try
    {
        // Wait until the file-writing task has completed so that the program doesn't terminate in the middle of writing the file. (Also to bubble any errors)
        await writeNamesTask;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to create or write file {sortedFilepath} with error message: {ex.Message}");
        return -1;
    }

    return 0;
}