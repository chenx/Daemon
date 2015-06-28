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

License
======
Apache/BSD/MIT/GPL.V2

Author
======
X. Chen  
Created on: 6/27/2015  
Last modified: 6/27/2015
