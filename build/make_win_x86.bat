@echo off

:: lua paths define
set LUAJIT_PATH=luajit-2.1.0-beta3
set STANDARD_LUA_PATH=lua-5.1.5

:: deciding whether to use luajit or not
:: set USE_STANDARD_LUA=%1%
set USE_LUA_PATH=%LUAJIT_PATH%
:: if "%USE_STANDARD_LUA%"=="YES" (set USE_LUA_PATH=%STANDARD_LUA_PATH%)

:: get visual studio tools path
set VS_TOOL_VER=vs140
set VCVARS="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\"
goto build


:build
set ENV32="%VCVARS%vcvars32.bat"
set ENV64="%VCVARS%vcvars64.bat"

copy /Y slua.c "%USE_LUA_PATH%\src\"
copy /Y luasocket-mini\*.* "%USE_LUA_PATH%\src\"
copy /Y protoc-gen-lua\protobuf\pb.c "%USE_LUA_PATH%\src\"

call "%ENV32%"
echo Swtich to x86 build env(%VS_TOOL_VER%)
cd %USE_LUA_PATH%\src
call msvcbuild.bat
copy /Y lua51.dll ..\..\..\Assets\Plugins\x86\slua.dll
copy /Y lua51.dll ..\..\..\jit\win\x86\lua51.dll
copy /Y luajit.exe ..\..\..\jit\win\x86\luajit.exe
cd ..\..



goto :eof

:missing
echo Can't find Visual Studio, compilation fails!

goto :eof
