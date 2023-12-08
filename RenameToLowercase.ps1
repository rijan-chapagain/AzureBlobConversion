Write-Host "Renaming started"
# Get the path to the folder
$folderPath = "C:\Users\RijanChapagain\Desktop\mediaTest\documents"
Write-Host "folder found, $folderPath"
# Get all files in the folder
$files = Get-ChildItem -Path $folderPath

# Loop through each file and rename it to lowercase
foreach ($file in $files) {
	Write-Host "Curr file, $file $file.FullName"
    $newName = $file.FullName.ToLower()
    Write-Host "Curr file, $newFile"
	Rename-Item -Path $file.FullName -NewName $newName -Force

}

Write-Host "File renaming to lowercase completed for all files in $folderPath."
