ECHO Backing up world ...
rmdir backups\backup_before /Q /S
robocopy world backups\backup_before /e

start /min BackUpper.bat
ECHO Starting server ...
java -Xmx3G -Xms2G -XX:PermSize=128m -jar modpack.jar nogui

ECHO Backing up world ...
rmdir backups\backup_after /Q /S
robocopy world backups\backup_after /e

exit