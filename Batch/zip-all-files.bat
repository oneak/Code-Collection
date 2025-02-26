@echo off
setlocal enabledelayedexpansion

:: Define paths
set "DIR=%~dp0"
set "OUTPUT=FileList.txt"
set "TEMP_DIR=%DIR%temp_backup"
set "BACKUP_DIR=%DIR%backup"

:: Find the next available incremental backup number
set "COUNTER=1"
:CHECK_BACKUP
if exist "%BACKUP_DIR%\backup_!COUNTER!.zip" (
    set /a COUNTER+=1
    goto CHECK_BACKUP
)

set "ZIPFILE=%BACKUP_DIR%\backup_!COUNTER!.zip"

:: Cleanup any old temporary directory
if exist "%TEMP_DIR%" rd /s /q "%TEMP_DIR%"
mkdir "%TEMP_DIR%"

:: List all files and directories recursively (excluding script & backup folder)
(for /r "%DIR%" %%F in (*) do (
    echo %%F | findstr /V /I "%~nx0" | findstr /V /I "\\backup\\"
)) > "%OUTPUT%"

:: Display message
echo File list saved to %OUTPUT%

:: Show the contents of the file
type "%OUTPUT%"

echo Proceeding...

:: Create backup directory if not exists
if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%"

:: Copy files to temporary folder while preserving structure
for /f "delims=" %%i in (%OUTPUT%) do (
    set "REL_PATH=%%i"
    set "REL_PATH=!REL_PATH:%DIR%=!"
    mkdir "%TEMP_DIR%\!REL_PATH!\.." 2>nul
    copy "%%i" "%TEMP_DIR%\!REL_PATH!" >nul
)

:: Zip the temporary folder
powershell.exe -ExecutionPolicy Bypass -NoProfile -Command ^
    "Compress-Archive -Path '%TEMP_DIR%\*' -DestinationPath '%ZIPFILE%' -Force"

:: Cleanup temp directory
rd /s /q "%TEMP_DIR%"

:: Remove the file list
del "%OUTPUT%"

echo Backup completed.