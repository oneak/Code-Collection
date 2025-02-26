@echo off
:: GNU GENERAL PUBLIC LICENSE v2
:: This script is licensed under the GNU GPL v2
:: You are free to modify and distribute it under the same license
:: Credit: Oneak (https://realmmadness.com/oneak)

:: Prevents the command prompt from displaying each command as it runs
cd /d %~dp0
:: Changes the working directory to the script's location
:: %~dp0 expands to the full path of the script's directory
:: The /d switch allows changing the drive as well

powershell -Command "Start-Process cmd -ArgumentList '/k cd /d %~dp0' -Verb RunAs"
:: Runs a new Command Prompt window as an administrator using PowerShell
:: - `Start-Process cmd` starts a new command prompt process
:: - `-ArgumentList '/k cd /d %~dp0'` keeps the window open and sets its directory to the script's location
:: - `-Verb RunAs` ensures the new command prompt is elevated (runs as administrator)
