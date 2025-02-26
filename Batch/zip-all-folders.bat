@echo off
:: GNU GENERAL PUBLIC LICENSE v2
:: This script is licensed under the GNU GPL v2
:: You are free to modify and distribute it under the same license
:: Credit: Oneak (https://realmmadness.com/oneak)

chcp 65001 >nul
setlocal enabledelayedexpansion

:: Set the working directory to where the batch script is located
cd /d "%~dp0"

:: Define the backup folder
set "backupDir=%CD%\backup"

:: Create the backup directory if it doesn't exist
if not exist "%backupDir%" mkdir "%backupDir%"

:: Loop through all directories except "backup"
for /d %%F in (*) do (
    if /I not "%%F"=="backup" (
        set "folder=%%F"
        set "baseName=%%F"
        set "zipFile=%backupDir%\%%F.zip"
        set "count=1"

        :: Check for existing zip and increment if needed
        if exist "!zipFile!" (
            :increment
            set "zipFile=%backupDir%\!baseName!_!count!.zip"
            if exist "!zipFile!" (
                set /a count+=1
                goto increment
            )
        )

        :: Ensure the directory exists before compressing
        if exist "!folder!" (
            :: Pass the folder and zip file paths directly to PowerShell
            powershell.exe -ExecutionPolicy Bypass -NoProfile -Command "Compress-Archive -Path '!folder!' -DestinationPath '!zipFile!'"
            echo Created !zipFile!
        ) else (
            echo Directory !folder! does not exist. Skipping.
        )
    )
)

echo Backup completed.