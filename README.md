# RdxCopy

## Running
1. Clone the project
2. Open the project in your preferred IDE
3. Restore NuGet packages
4. Build the project
5. Start the ~\RdxCopy\bin\[Debug || Release]\net6.0\RdxCopy.exe file either by double clicking it or from CLI
   * CLI arguments are supported too



## Parameters
### To copy a directory and files within it: 
```-s <soure directory path>```:   
The source directoy that will be copied to the destination.  
Must be valid and existing directory.  
*Alias*:  
--src  
--source  
  
```-d <destination directory path>```:  
The destination directory where the source will be copied.  
Must be valid and existing directory.  
*Alias*:  
--dest  
--destination  
  
```[-r]```:  
If set, the source directoy will be recursively looked for files within all nested directories.  
If not set, only the files within the root of the source direcoty will be copied.  
Optional parameter, default value: false.  
*Alias*:  
--recurse  
  
```[-o]```:  
If set, the destination directory already contains one of the files, the file will be overwritten.  
If not set, the file copy will be skipped and the original file remains.  
Optional parameter, default value: false.  
*Alias*:  
--override  
  

 
----
### To exit the program:  
  
```-e```:  
Exits the program.  
*Alias*:  
--exit  
  

 
----
### To get help about valid arguments:  
  
```-h```:
Provides help about arguments.  
*Alias*:  
-?  
--help  
  

 
----
### To clear the console:  
  
```-c```:  
Clears the console.  
*Alias*:  
--clear  
