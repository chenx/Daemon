
#!/usr/bin/perl

#
# This demonstrates running a Perl program in background as daemon without using Proc::Daemon.
# More than 1 instance can run at the same time.
# From: [2] 
#

use POSIX qw(setsid);

chdir '/';
umask 0;
open STDIN, '/dev/null';
#open STDOUT, '>/Users/chenx/tmp/dmon.log';
open STDERR, '>/dev/null';

defined(my $pid = fork);
exit if $pid;
setsid;

while(1)
{
    sleep(2);
    #print "Hello...\n";
    do_log();
}

# note: cannot use "log" as it's preserved.
sub do_log {
    open LOGFILE, '>>', '/Users/chenx/tmp/dmon.log' or die "cannot open file"; # $!;
    print LOGFILE "continue on ..\n";
    close LOGFILE;
}
