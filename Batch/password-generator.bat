:: ===========================================================
:: Password Generator
:: Version: 1.0
:: License: GNU GPL v2
:: Author: Oneak (https://realmmadness.com/oneak)
:: 
:: This script generates a specified number of random numeric bytes.
:: Licensed under the GNU General Public License v2.
:: ===========================================================

@echo off
setlocal EnableDelayedExpansion

cls
title Password Generator
echo ===========================================================
echo  Password Generator
echo  Oneak RealmMadness.com
echo ===========================================================
echo  Generate random passwords
echo ===========================================================

:GenerateLoop
set /p length=Enter the length of the password (or type 'exit' to quit): 

if /i "%length%" equ "exit" goto :EOF

if %length% lss 1 set length=1
if %length% gtr 5000 set length=5000

set "chars=ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%%^&*()-+=<>?{}[]"

set "password="

for /L %%i in (1,1,%length%) do (
    call :GetRandomChar char
    set "password=!password!!char!"
)

echo Your password is: !password!
echo.
goto GenerateLoop

:GetRandomChar
set /a index=!random! %% 76
set "char=!chars:~%index%,1!"
set "char=!char:^=^^!"
exit /b