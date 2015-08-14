@echo off

reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath > nul 2>&1
if ERRORLEVEL 1 goto MissingMSBuildRegistry

for /f "skip=2 tokens=2,*" %%A in ('reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath') do SET MSBUILDDIR=%%B

IF NOT EXIST %MSBUILDDIR%nul goto MissingMSBuildToolsPath
IF NOT EXIST %MSBUILDDIR%msbuild.exe goto MissingMSBuildExe

::BUILD
"tools\nuget.exe" restore Aliencube.WebApi.Hal.sln
"%MSBUILDDIR%msbuild.exe" "Aliencube.WebApi.Hal\Aliencube.WebApi.Hal.csproj" /t:ReBuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE;NET45";OutPutPath=bin\Release\net45\;DocumentationFile=bin\Release\net45\Aliencube.WebApi.Hal.xml

mkdir build
del "build\*.nupkg"

IF [%1]==[] GOTO MissingVersion

::PACK
"tools\nuget.exe" pack "Aliencube.WebApi.Hal\Aliencube.WebApi.Hal.nuspec" -OutputDirectory build -Version %1

IF [%2]==[] GOTO MissingApiKey

::SET API KEY
"tools\nuget.exe" setApiKey %2


::DEPLOY
"tools\nuget.exe" push "build\*.nupkg"

goto:eof
::ERRORS
::---------------------
:MissingMSBuildRegistry
echo Cannot obtain path to MSBuild tools from registry
goto:eof
:MissingMSBuildToolsPath
echo The MSBuild tools path from the registry '%MSBUILDDIR%' does not exist
goto:eof
:MissingMSBuildExe
echo The MSBuild executable could not be found at '%MSBUILDDIR%'
goto:eof
:MissingVersion
echo Version not found
goto:eof
:MissingApiKey
echo API Key not found
goto:eof
