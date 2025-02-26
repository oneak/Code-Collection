:: GNU GENERAL PUBLIC LICENSE v2
:: This script is licensed under the GNU GPL v2
:: You are free to modify and distribute it under the same license
:: Credit: Oneak (https://realmmadness.com/oneak)

@echo off
setlocal enabledelayedexpansion

:: Ask for the folder to scan
set /p "folder=Enter the folder path to scan (e.g., C:\Users\YourName\Documents): "

:: Check if the folder exists
if not exist "%folder%" (
    echo The specified folder does not exist.
    pause
    exit /b
)

:: Show warning and prompt to start scan
echo WARNING: This will scan all files in "%folder%" and its subdirectories.
set /p "scanconfirm=Do you want to proceed with scanning? (y/n): "
if /i not "%scanconfirm%"=="y" (
    echo Exiting script.
    pause
    exit /b
)

:: List all files in the specified directory and subdirectories
echo Scanning files in "%folder%" and its subdirectories:
echo -----------------------------------------------------------
dir /s /b /a:-d "%folder%"
echo -----------------------------------------------------------
echo Total files: 
dir /s /b /a:-d "%folder%" | find /c /v ""
echo.

:: Get the source and target formats from user
set /p "sourceext=Enter the file format you want to convert from (e.g., avif): "
set /p "targetext=Enter the file format you want to convert to (e.g., jpg): "

:: List files that will be renamed
echo.
echo The following files will be renamed from .%sourceext% to .%targetext%:
echo -----------------------------------------------------------------------
set "filecount=0"
for /r "%folder%" %%f in (*.%sourceext%) do (
    echo %%f
    set /a filecount+=1
)

if %filecount% EQU 0 (
    echo No files with the .%sourceext% extension found.
    pause
    exit /b
)

echo -----------------------------------------------------------------------
echo Total files to be renamed: %filecount%
echo.

:: Confirm the operation
set /p "renameconfirm=Are you sure you want to rename these files? (y/n): "
if /i not "%renameconfirm%"=="y" (
    echo Operation cancelled.
    pause
    exit /b
)

:: Rename the files recursively
echo Renaming files...
for /r "%folder%" %%f in (*.%sourceext%) do (
    ren "%%f" "%%~nf.%targetext%"
    echo Renamed "%%f" to "%%~nf.%targetext%"
)

echo.
echo All matching files have been renamed.
echo Total files renamed: %filecount%
pause