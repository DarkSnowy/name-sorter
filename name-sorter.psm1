
# Declare a PowerShell module from which the name-sorter command can be imported
function global:name-sorter {
    # Declare filename parameter and make it mandatory
    [CmdletBinding()]
    param(
        [Parameter(Position=0,mandatory=$true)]
        [string] $filename
    )

	# Get the file path for the release version of the name-sorter project executable
	$nameSorterExe = "$PSScriptRoot\name-sorter\bin\Release\net8.0\name-sorter.exe"
	# Run the name-sorter executable with the provided arguments
	#& $nameSorterExe "$PWD\$filename" 
    & $nameSorterExe $filename
}

# Export the name-sorter module
Export-ModuleMember -Function name-sorter