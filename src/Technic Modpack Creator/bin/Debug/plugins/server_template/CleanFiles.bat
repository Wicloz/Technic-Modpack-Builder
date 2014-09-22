@ECHO OFF
title Reverting to Template ...

rmdir config /Q /S
rmdir coremods /Q /S
rmdir Flan /Q /S
rmdir hats /Q /S
rmdir mods /Q /S
rmdir ServerFiles /Q /S
rmdir world /Q /S
del Files.zip
del *.txt /Q
del *.log /Q
del *.lck /Q
del versionew.dat
del currentversion.dat

rmdir backups\backup_before /Q /S
rmdir backups\backup_after /Q /S

exit