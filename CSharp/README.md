README
=====

About
======
This is a windows service written in C#. 

Functions
==========
It does two things:

1) In every 5 seconds, write a message "Test Event: I'm alive" to windows event log. This is similar to a cronjob in Unix.  
2) Listen on port 9090, if contacted by a TCP client, invoke a local vbscript to display a dialog box.   
   
This can be used as a template for more complex functions.

Deployment
========
- Open this project in VS2008, compile it into csharp_win_svc.exe.  
- Store csharp_win_svc.exe in a place, say C:\windows, move resources\csharp_win_svc.exe.config there too.  
- Move recourses\test.vbs to C:\\.   
- Install the windows service:
  - To install, type this in DOS window:  
    InstallUtil /LogToConsole=true csharp_win_svc.exe

    You will be prompted for an account name/password by which the windows services will be run as.  
    If it is a local user account, it should start with ".\", e.g., ".\Administrator".   
    If it is a domain user, it should start with the domain name:  "[domain name]\username".  

  - To uninstall, type this in DOS window:  
    InstallUtil /LogToConsole=true /u csharp_win_svc.exe


License
======
Apache/BSD/MIT/GPL.V2

Author
======
X. Chen  
Created on: 6/27/2015  
Last modified: 6/27/2015
