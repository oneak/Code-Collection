:: ===========================================================
:: Random Byte Generator
:: Version: 1.0
:: License: GNU GPL v2
:: Author: Oneak (https://realmmadness.com/oneak)
:: 
:: This script generates a specified number of random numeric bytes.
:: Licensed under the GNU General Public License v2.
:: ===========================================================

@echo off
setlocal enabledelayedexpansion

cls
title Random Byte Generator
echo ===========================================================
echo  Random Byte Generator v1.0
echo  Oneak RealmMadness.com
echo ===========================================================
echo  Generate random numeric bytes
echo ===========================================================

:generate
set /p num_bytes="Enter the length of the password (or type 'exit' to quit): "

:: Check if the user typed 'exit' to quit
if /i "!num_bytes!"=="exit" exit

:: Check if the input is a valid number
for /f "delims=" %%a in ('echo !num_bytes! ^| findstr "^[0-9]*$"') do set "valid_num=%%a"
if not defined valid_num (
    echo Invalid input. Please enter a valid number.
    goto generate
)

:: Initialize the key variable
set "key="

:: Generate random numeric bytes
for /L %%i in (1,1,!num_bytes!) do (
    set /a num=!random! %% 10
    set "key=!key!!num!"
)

echo Generated Key: !key!

:: Ask for new length or exit
goto generate

endlocal