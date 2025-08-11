using System.Text;

namespace name_sorter.interfaces;

/// <summary>
/// Defines a service for writing names to a file.
/// </summary>
public interface INameWriterService
{
    /// <summary>
    /// Writes the provided StringBuilder to a file at the specified file path.
    /// </summary>
    /// <remarks>This method overwrites the file if it already exists.</remarks>
    /// <param name="filepath">The full path of the file to be written.</param>
    /// <param name="names">A <see cref="StringBuilder"/> containing the names to write to the file.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
    public Task WriteNamesToFile(string filepath, StringBuilder names, CancellationToken cancellationToken = default);
}
