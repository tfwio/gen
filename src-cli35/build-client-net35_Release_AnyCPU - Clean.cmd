@echo off
set path1=C:\Program Files (x86)\msbuild\14.0\bin
REM set path1=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%PATH%;%path1%
msbuild "..\\.sln\\generator.sln" "/t:gen-client35:Clean" "/p:Configuration=Release;Platform=Any CPU"