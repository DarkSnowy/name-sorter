To generate the executable file:
- Restore NuGet packages
- Select Release build configuration 
- Build the name-sorter project.

Before PowerShell can execute "name-sorter" the module must first be imported to the current session. This can be done by opening a PowerShell terminal in the project directory and executing:
  Import-Module .\name-sorter.psm1

Once imported the program can be executed with:
	name-sorter .\unsorted-names-list.txt

Please note that the PS module will look for the executable in the Release bin. So it won't work if there is no build or only a Debug build.

If you don't want to need to import the command every session then the import command can be added to your PowerShell user profile script. Usually found at:
  C:\Users\USER_NAME\Documents\WindowsPowerShell\Microsoft.PowerShell_profile.
Making sure to include the full filepath for name-sorter.psm1 on your own system.