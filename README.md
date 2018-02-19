# Simple-Recovery
A simple password recovery tool written in C# that recovers passwords from various sources

## How does it work?
When a session is launched vulnerabilities in the security of the storage of specific passwords are exploited and thus the passwords are recovered in clear text. It then saves these passwords to text files, to the folder specified in App.Config. The default path is C:\Data.

## Supported sources
* Google Chrome saved passwords
* Wireless networks that the computer has signed in to at some point

## How is the Google Chrome password storage exploited?
Google Chrome stores its saved passwords in an encrypted SQLite database. It uses the Windows cryptography library (crypto.dll) to encrypt this database, specifically the method "CryptProtectedData". This method of encryption uses the user instance as its encryption key and therefore can only be decrypted using the user instance that it was encrypted with. This essentially means that as long as you have access to the computer that the database was encrypted on, you can decrypt the database relativly easily.

## How is the wireless network password storage exploited?
Windows stores the passwords of the wireless networks that you have connected to in an encrypted file (that requires admin rights to access.) It uses the Windows cryptography library (crypto.dll) to encrypt this file, specifically the method "CryptProtectedData". There is however a way to access this database without admin rights using the command line. This is achieved by using the built in scripting utility "netsh" to retrieve the wireless profiles and access the saved passwords in clear text. 

## How do I use Simple Recovery?
* Compile the project (Change debug to release)
* Run the Simple Recovery.exe file it creates in the release directory (Simple-Recovery\Simple Recovery\bin\Release)

#### Please note I do not condone you using Simple Recovery for anything that may be considered illegal.
