# SimpleSync
A simple windows tool for synchronising files and directories, which can be configured to synchronise for example a USB-Stick folder to a Local folder and reverse.

The console program recieves two paths:

-> SimpleSync.exe (Source Path) (Destination Path)

You can configure two shortcuts "Upload" and "Download" one for each synchronising direction.
For example:

  Upload ->    F:\SimpleSync.exe %USERPROFILE%\Documents\SyncFolder F:\\SyncFolder
  Download ->  F:\SimpleSync.exe F:\\SyncFolder %USERPROFILE%\Documents\SyncFolder
  
The program takes the source folders directory and file tree as reference. 
This Application:
 - creates all directories in the Destination that are present in the Source
 - deletes directories in the Destination when the parent Source directory is more recent and they are not present in Source
 - copies files from Source that are not existent in Destination
 - copies the more recent of the files from Source or Destination
 - deletes files in the Destination when the parent Source directory is more recent and they are not present in Source
