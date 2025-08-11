using name_sorter.interfaces;
using System.Text;

namespace name_sorter.services;

/// <inheritdoc/>
public class NameWriterService : INameWriterService
{
    /// <inheritdoc/>
    public async Task WriteNamesToFile(string filepath, StringBuilder names, CancellationToken cancellationToken = default)
    {
        // Create or overrite the file at the given filepath
        var file = File.Create(filepath);

        // Open a filestream
        using TextWriter textWriter = new StreamWriter(file, Encoding.UTF8, 65536);
        // Write the new content to the file
        await textWriter.WriteAsync(names, cancellationToken);
    }
}
