@echo off

cd Server
if exist psftp.txt (
    del psftp.txt
)

echo put Server.zip >> psftp.txt
echo quit >> psftp.txt

psftp 3.35.30.92 -i E:\hongik_SE.ppk -l ubuntu -b psftp.txt

del Server.zip
del psftp.txt
pause