using name_sorter.interfaces;

namespace name_sorter.services;

/// <inheritdoc/>
public class NameReaderService: INameReaderService
{
    /// <inheritdoc/>
    public async Task<SortedList<string, SortedList<string, int>>> ReadNamesFromFile(string filename, CancellationToken cancellationToken = default)
    {
        // Create a sorted list to store last names in, which contains a second sorted list for storing first names. It will also maintain a count of how many times a name repeats
        SortedList<string, SortedList<string, int>> sortedNames = [];

        // Read each line in the file. This is done asynchronously, as per good IO standards
        await foreach (string line in File.ReadLinesAsync(filename, cancellationToken))
        {
            // Trim any starting or trailing white space so that it does not interfear with the sorting
            var name = line.Trim();
            // Extract the last name from the line so that the list can sort via last names
            var indexOf = name.LastIndexOf(' ');
            string lastName = name[(indexOf + 1)..];
            string otherNames = name[..indexOf];

            // Check if anyone with the same last name has already been read into the list
            if (sortedNames.TryGetValue(lastName, out SortedList<string, int>? sortedOtherNames))
            {
                // Check if anyone exists with the same first names
                var count = sortedOtherNames.GetValueOrDefault(otherNames, 0);
                // Increment the count of how many people there are with the same full name
                count++;
                sortedNames[lastName][otherNames] = count;
            }
            else
            {
                // Add the name to the list for the first time
                sortedNames[lastName] = new SortedList<string, int> { { otherNames, 1 } };
            }
        }

        return sortedNames;
    }
}
