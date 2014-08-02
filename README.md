Daemon
======

Collection of Unix/Linux daemons and Windows services


Why Daemon?
-----------

Linux/Unix daemon and windows service are important. They reside in memory as long-running processes, often start up automatically when the machine is booted.

Any piece of software, either a database or web/file/mail/etc server, is a linux/unix daemon or windows service. NoSQL applications Hadoop, mongoDB, memcached, etc., all are daemons/services or make use of it.

For this reason, it is good to write templates for linux/unix daemon and windows service. When need a relevant utility, it is possible to quickly add the functionality into the daemon/service template and make a product.

Socket programming is another technology tightly coupled and integrated with Daemon/Service. This comes from the need of communication between the daemon/service and their clients.  Protocol and port are two key players.  Different servers use different protocols, and listen on different ports.

Most enterprise IT and software jobs today are on information system, used to be C/S, now B/S. Most of them use databases, process data, implement business logic, have back end and front end. Back end use database and web server, front end use web programming languages. Database + Web/Mobile UI + business logic is the mainstream of IT and software jobs today. Technologies outside this realm feels exotic to many programmers.

However, when it comes to creating new software tools, Daemon/Service and Socket programming play vital roles.

About Daemon
------------

Text below on features of a deamon are copied from [1].

<i><font color="#666666">
A daemon is a program that runs in the background. A daemon will usually be started at system startup and end at system shutdown. The exceptions to this rule are programs like the Bluetooth SDP daemon, which is activated when a new Bluetooth HCI is found,, and ends when it is removed. Daemons run transparently and do not normally interact with the user directly.

Daemons start as ordinary processes but they eventually ‘fork and die’ to start running in the background. Some daemons do only the ‘fork and die’ step but ignore other important steps. Here is a list of what a daemon should do:

- Fork to create a child, and exit the parent process.
- Change the umask so that we aren’t relying on the one set in the parent.
- Open logs to write to in the case of an error.
- Create a new session id and detach from the current session.
- Change the working directory to somewhere that won’t get unmounted.
- Close STDIN, STDOUT and STDERR.
</font></i>


References
----------

[1] <a href="http://www.danielhall.me/2010/01/writing-a-daemon-in-c/">Writing a daemon in C</a>  
[2] <a href="http://www.netzmafia.de/skripten/unix/linux-daemon-howto.html">Linux Daemon Writing HOWTO</a>


