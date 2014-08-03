/**
 * This is the skeleton of a daemon in C.
 *
 * @compile: gcc daemon.c -o daemon
 *
 * @from: http://www.netzmafia.de/skripten/unix/linux-daemon-howto.html
 * @reference: http://www.danielhall.me/2010/01/writing-a-daemon-in-c/
 *
 * @since 8/1/2014
 */

#include <sys/types.h>
#include <sys/stat.h>
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <errno.h>
#include <unistd.h>
#include <syslog.h>
#include <string.h>

//
// To see the output, type: tail -f /var/log/system.log
//
void do_log(const char * identity, const char * msg) {
    //static int ct = 0;
    //printf("%s %d\n", msg, ++ ct);

    /* Open a connection to the syslog server */
    openlog(identity,LOG_NOWAIT|LOG_PID,LOG_USER);

    /* Sends a message to the syslog daemon */
    syslog(LOG_NOTICE, "%s", msg);

    /* this is optional and only needs to be done when your daemon exits */
    closelog();
}


//
// Call this function will daemonize the current program.
//
void daemonize() {
    /* Our process ID and Session ID */
    pid_t pid, sid;
    
    /* Fork off the parent process */
    pid = fork();
    if (pid < 0) {
        exit(EXIT_FAILURE);
    }
    /* If we got a good PID, then
       we can exit the parent process. */
    if (pid > 0) {
        exit(EXIT_SUCCESS);
    }

    /* Change the file mode mask */
    umask(0);
        
    /* Open any logs here */    
        
    /* Create a new SID for the child process */
    sid = setsid();
    if (sid < 0) {
        /* Log the failure */
        exit(EXIT_FAILURE);
    }
    
    /* Change the current working directory */
    if ((chdir("/")) < 0) {
        /* Log the failure */
        exit(EXIT_FAILURE);
    }
    
    /* Close out the standard file descriptors */
    close(STDIN_FILENO);
    close(STDOUT_FILENO);
    close(STDERR_FILENO);
    
    /* Daemon-specific initialization goes here */
    
    /* The Big Loop */
    /*
    while (1) {
        // Do some task here ... 
        do_log("C Daemon", "I am alive");
       
        sleep(5); 
    }
    exit(EXIT_SUCCESS);
    */
}


int main(int argc, char * argv[]) {
    daemonize(); // daemonize this program.

    // now do something.
    while (1) {
        /* Do some task here ... */
        do_log(argv[0], "Daemon is alive");
   
        sleep(5); /* wait some time */
    }

    return 0;
}
