@echo off

ssh -i hongik_SE.pem ubuntu@3.35.30.92 "make"
ssh -i hongik_SE.pem ubuntu@3.35.30.92 "pkill dotnet"

timeout /t 2