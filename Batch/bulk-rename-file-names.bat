:: GNU General Public License v2
:: This script is licensed under the GNU GPL v2
:: You are free to modify and distribute it under the same license
:: Credit: Oneak (https://realmmadness.com/oneak)

@echo off
setlocal enabledelayedexpansion

:: Ask for the filename to search
set /p filename=Enter the filename to search (e.g., cmd.bat): 

:: Initialize file counter
set count=0

:: Search for files
for /f "delims=" %%a in ('dir /s /b /a-d "%filename%" 2^>nul') do (
    set /a count+=1
    set "filePath[!count!]=%%a"
    echo Found: %%a
)

:: Check if files were found
if %count%==0 (
    echo No files found matching "%filename%".
    pause
    exit /b
)

:: Ask for the new filename
set /p newname=Enter the new filename (e.g., cmd.py): 

:: Rename all found files
for /l %%i in (1,1,%count%) do (
    set "oldFile=!filePath[%%i]!"
    set "newFile=!oldFile:%filename%=%newname%!"
    ren "!oldFile!" "!newname!"
    echo Renamed "!oldFile!" to "!newFile!"
)

echo Rename operation completed.
pause