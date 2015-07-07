#
# This script demonstates:
# 1) running a Perl script as daemon in background, using the Daemon module.
# 2) only one instance of the daemon can run by checking "Proc::PID::File->running()".
# 3) implementation of daemon commands: start, stop, status.
# 4) use of Getopt::Long, $0 (name of this file).
#
# Note:
# 1) to run as daemon, "sudo" should be used for non-admin user.
# 2) parameters that can change: $LOG_FILE, $USE_OPT.
#
# @By: X.C.
# @Created on: 6/28/2014
# @Last modified: 6/28/2014
#

#!/usr/bin/perl

use strict;
use warnings;
use Getopt::Long;
use Proc::Daemon;
use Proc::PID::File;

#
# Location of log file.
#
my $LOG_FILE = "/Users/chenx/tmp/dmon.log";

#
# If $USE_OPT = 1, use GetOptions.
# 0 is better here because if an arg does not start with "--",
# it will be ignored and no usage information is printed.
#
my $USE_OPT = 0;


my $len = @ARGV;
if ($len == 0) {
    show_usage();
} else {
    if ($USE_OPT) {
        GetOptions(
            "start" => \&do_start,
            "status" => \&show_status,
            "stop" => \&do_stop,
            "help" => \&show_usage
        ) or show_usage();
    } else {
        my $cmd = $ARGV[0];
        if ($cmd eq "start") { do_start(); }
        elsif ($cmd eq "stop") { do_stop(); }
        elsif ($cmd eq "status") { show_status(); }
        else { show_usage(); }
    }
}


#
# 1 at the end of a module means that the module returns true to use/require statements.
# It can be used to tell if module initialization is successful.
# Otherwise, use/require will fail.
#
# 1;


sub show_usage {
    if ($USE_OPT) {
        print "Usage: sudo perl $0 --[start|stop|status|help]\n";
    } else {
        print "Usage: sudo $0 [start|stop|status]\n";
    }
    exit(0);
}


sub show_status {
    if (Proc::PID::File->running()) {
        print "daemon is running..\n";
    } else {
        print "daemon is stopped\n";
    }
}


sub do_stop {
    my $pid = Proc::PID::File->running();
    if ($pid == 0) {
        print "daemon is not running\n";
    } else {
        #print "stop daemon now ..\n";
        kill(9, $pid);
        print "daemon is stopped\n";
    }
}


sub do_start {
    print "start daemon now\n";

    Proc::Daemon::Init();

    #
    # To use this, you need to start the daemon with "sudo" to have the permission
    # to PID file. To kill the daemon, "sudo" is also needed.
    #
    if (Proc::PID::File->running()) {
        do_log( "A copy of this daemon is already running, exit" );
        exit(0);
    }

    my $continue = 1;
    $SIG{TERM} = sub { $continue = 0; };

    while ($continue) {
        #print "continue ..\n"; # this won't work, since STDOUT is closed.
        do_log("continuing ..");

        sleep(2);
    }
}


sub do_log {
    my ($msg) = @_;

    my ($sec,$min,$hour,$mday,$mon,$year,$wday,$yday,$isdst) = localtime();
    $year += 1900;

    open FILE, ">>$LOG_FILE" or die "cannot open log file $!\n";
    print FILE "$year-$mon-$mday $hour:$min:$sec  $msg\n";
    close FILE;
}
