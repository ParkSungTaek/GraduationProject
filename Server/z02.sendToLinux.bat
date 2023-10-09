@echo off

cd Server
if exist psftp.txt (
    del psftp.txt
)

echo put Server.zip >> psftp.txt
echo quit >> psftp.txt

sftp -i ..\hongik_SE.pem -b psftp.txt ubuntu@3.35.30.92

del Server.zip
del psftp.txt

timeout /t 2