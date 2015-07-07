 #!/usr/bin/perl
use Proc::Daemon;
use Proc::PID::File;

Proc::Daemon::Init; # Demonize.
if (Proc::PID::File->running()) { exit(0); } # Exit if already running, so only 1 instance can run.
for (;;) { 
    # print "do something every 5 seconds..\n"; # This won't print because STDOUT is closed.
    sleep(5); 
}
