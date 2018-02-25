# Simple-Recovery
A simple password recovery tool written in C# that recovers passwords from various sources

## How does it work?
When a session is launched vulnerabilities in the security of the storage of specific passwords are exploited and thus the passwords are recovered in clear text. It then saves these passwords to text files, to the folder specified in App.Config. The default path is C:\Data.

## Supported sources
* Google Chrome saved passwords
* Wireless networks that the computer has signed in to at some point

## How do I use Simple Recovery?
* Compile the project (Change debug to release)
* Run the Simple Recovery.exe file it creates in the release directory (Simple-Recovery\Simple Recovery\bin\Release)

#### Please note I do not condone you using Simple Recovery for anything that may be considered illegal.
