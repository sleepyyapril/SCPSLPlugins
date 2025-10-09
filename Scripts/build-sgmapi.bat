@echo off

IF NOT DEFINED SL_REFERENCES (
    echo Missing SL_REFERENCES environment variable.
    pause >nul
    exit
)

IF NOT DEFINED SL_PLUGINS (
    echo Missing SL_PLUGINS environment variable.
    pause >nul
    exit
)

echo Beginning build.
cd ..\SleepyGamemodeAPI
dotnet build -c Release

cd bin\Release
copy SleepyGamemodeAPI.dll /B "%SL_PLUGINS%\SleepyGamemodeAPI.dll"
copy SleepyGamemodeAPI.dll /B "%SL_REFERENCES%\SleepyGamemodeAPI.dll"
del SleepyGamemodeAPI.dll
cls
echo Build complete.
echo SL plugins folder: %SL_PLUGINS%
echo SL references folder: %SL_REFERENCES%
pause >nul