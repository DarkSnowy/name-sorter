To generate the executable file:
- Restore NuGet packages
- Select Release build configuration 
- Build the name-sorter project.

<p>
Before PowerShell can execute "name-sorter" the module must first be imported to the current session. This can be done by opening a PowerShell terminal in the project directory and executing:<br />
  <b>Import-Module .\name-sorter.psm1</b>
</p><p>
Once imported the program can be executed with:<br />
	<b>name-sorter .\unsorted-names-list.txt</b>
</p><p>
Please note that the PS module will look for the executable in the Release bin. So it won't work if there is no build or only a Debug build.<br />
</p><p>
If you don't want to need to import the command every session then the import command can be added to your PowerShell user profile script. Usually found at:<br />
  <b>C:\Users\USER_NAME\Documents\WindowsPowerShell\Microsoft.PowerShell_profile.</b>
  <br />
<i>*Make sure to include the full filepath for name-sorter.psm1 on your own system.</i>
</p>
