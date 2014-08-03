/**
 * This program creates a daemon in C++.
 *
 * @compile: g++ daemon.cpp -o daemon
 * @from: function daemonize() is from [1].
 * @references:
 *    [1] http://blog2.emptycrate.com/content/making-linux-daemon
 *
 * @since: 8/2/2014
 */

#include <iostream>
#include <string>
#include <stdexcept>  // for std::runtime_error()
#include <sys/stat.h> // for umask
#include <fcntl.h>    // for open, O_RDONLY, O_WRONLY, O_CREAT, O_APPEND
#include <errno.h>    // for errno
#include <syslog.h>   // for openlog, closelog, syslog
using namespace std;


//
// To see the output, type: tail -f /var/log/system.log
//
//void do_log(const char * identity, const char * msg) {
void do_log(const string &identity, const string &msg) {
    // Open a connection to the syslog server 
    openlog(identity.c_str(),LOG_NOWAIT|LOG_PID,LOG_USER); 
 
    // Sends a message to the syslog daemon 
    syslog(LOG_NOTICE, "%s", msg.c_str());
 
    // this is optional and only needs to be done when your daemon exits 
    closelog();
}


//From: http://blog2.emptycrate.com/content/making-linux-daemon
//! daemonize the currently running programming
//! Note: the calls to strerror are not thread safe, but that should not matter
//! as the application is only just starting up when this function is called
//! \param[in] dir which dir to ch to after becoming a daemon
//! \param[in] stdinfile file to redirect stdin to
//! \param[in] stdoutfile file to redirect stdout from
//! \param[in] stderrfile file to redirect stderr to
//void System::daemonize(const string &dir = "/",
void daemonize(const string &dir = "/",
               const std::string &stdinfile = "/dev/null",
               const std::string &stdoutfile = "/dev/null",
               const std::string &stderrfile = "/dev/null")
{
  umask(0);
  rlimit rl;
  if (getrlimit(RLIMIT_NOFILE, &rl) < 0) 
  {
    //can't get file limit
    throw std::runtime_error(strerror(errno));
  }

  pid_t pid;
  if ((pid = fork()) < 0) 
  {
    //Cannot fork!
    throw std::runtime_error(strerror(errno));
  } else if (pid != 0) { //parent
    exit(0);
  }

  setsid();

  if (!dir.empty() && chdir(dir.c_str()) < 0) 
  {
    // Oops we couldn't chdir to the new directory
    throw std::runtime_error(strerror(errno));
  }

  if (rl.rlim_max == RLIM_INFINITY) 
  {
    rl.rlim_max = 1024;
  }

  // Close all open file descriptors: 0, 1
  for (unsigned int i = 0; i < rl.rlim_max; i++) 
  {
    close(i);
  }

  // let stdin/out/err all go to /dev/null
  int fd0 = open(stdinfile.c_str(), O_RDONLY);
  int fd1 = open(stdoutfile.c_str(),
      O_WRONLY|O_CREAT|O_APPEND, S_IRUSR|S_IWUSR);
  int fd2 = open(stderrfile.c_str(),
      O_WRONLY|O_CREAT|O_APPEND, S_IRUSR|S_IWUSR);

  if (fd0 != STDIN_FILENO || fd1 != STDOUT_FILENO || fd2 != STDERR_FILENO) 
  {
    //Unexpected file descriptors
    throw runtime_error("new standard file descriptors were not opened as expected");
  }
}


//
// Do whatever task here.
//
void do_something() {
  while (1) {
    // do something.

    do_log("C++ daemon", "I am alive");
    sleep(5);
  }
}


int main() {
  daemonize();
  do_something();
  return 0;
}

