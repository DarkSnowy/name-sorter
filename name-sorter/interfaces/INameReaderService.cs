namespace name_sorter.interfaces;

/// <summary>
/// Defines a service for reading and processing names from a file.
/// </summary>
public interface INameReaderService
{
    /// <summary>
    /// Loads each line from file into a sorted list with last names as keys and for values another sorted list with other names, as well as a count of how many times they occured
    /// </summary>
    /// <param name="filepath">Path to the file to read the list of names from</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Sorted list with lastnames as keys and, as values, another sorted list with first names and a count of how many times they occured</returns>
    public Task<SortedList<string, SortedList<string, int>>> ReadNamesFromFile(string filepath, CancellationToken cancellationToken = default);
}
