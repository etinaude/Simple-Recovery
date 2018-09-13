# Simple-Recovery
A Windows recovery tool written in C# that supports both saved Chrome network passwords.

### How does it work?
When a session is launched vulnerabilities in the security of the storage of specific passwords are exploited and thus the passwords are recovered in clear text. It then saves these passwords to text files, to the folder specified in App.Config. The default path is C:\Data.

### Supported sources
* Google Chrome saved passwords
* Wireless networks that the computer has signed in to at some point

#### Please note I do not condone the useage of Simple Logger for anything that may be considered illegal.
