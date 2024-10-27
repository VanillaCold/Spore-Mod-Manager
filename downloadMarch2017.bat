@echo off

REM Setup the location and gameloc variables, by reading from the registry.
REM location is the location of Steam, while gameloc is the game's EP1 Data directory.

for /f "usebackq tokens=2,* skip=2" %%L in (`reg query HKLM\SOFTWARE\WOW6432Node\Valve\Steam /v InstallPath `) do set "location=%%M"

for /f "usebackq tokens=2,* skip=2" %%L in (`reg query "HKLM\SOFTWARE\WOW6432Node\Electronic Arts\SPORE_EP1" /v DataDir `) do set "gameloc=%%M"

echo Looking for existing download...

SET LookForFile="%location%/steamapps/content/app_24720/depot_24721/SporebinEP1/SporeApp.exe"

IF EXIST %LookForFile% GOTO FoundIt

echo No existing download found. Prompting Steam to download.

cd %location%
steam "steam://open/console/+download_depot 24720 24721 7407510787032991484"

echo Waiting for download to complete...

:CheckForFile
IF EXIST %LookForFile% GOTO FoundIt

REM If we get here, the file is not found.

REM Wait 20 seconds and then recheck.
TIMEOUT /T 20 >nul

GOTO CheckForFile

:FoundIt

echo SporeApp.exe found. Copying to SporebinEP1.

cd %gameloc%..\SporebinEP1\
echo f | xcopy /s /y "%location%\steamapps\content\app_24720\depot_24721\SporebinEP1\SporeApp.exe" "%cd%\SporeApp_ModAPIFix.exe"

TIMEOUT /T 1 >nul

