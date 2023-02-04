@ECHO OFF
SET ORIG=%PATH%
SET MMODE=Release

if EXIST Strings.resx (
  if EXIST ..\build\%MMODE%-AnyCPU\bin\gen.exe (
    CALL:update_path AnyCPU
  ) else if EXIST  ..\build\%MMODE%-Win32\bin\gen.exe (
    CALL:update_path Win32
  ) else if EXIST  ..\build\%MMODE%-Win64\bin\gen.exe (
    CALL:update_path Win64
  )
)
CALL:do_gen
GOTO:EOF

:update_path
  echo - UPDATING PATH %~1
  PATH=%ORIG%;%~dp0..\build\%MMODE%-%~1\bin
  GOTO:EOF

:do_gen
  ECHO - Calling Generator
  gen -it "Schematics/my.xtpl" -is "Schematics/my.xdata" ^
    -dbn "Calibre" ^
    -tbln "books" ^
    -tpln "js.calibre-models" ^
    -o "%~dp1\model.js"

  ECHO.
  ECHO the file 'model.js' has been generated in the path
  ECHO you executed this script.
  ECHO.
  ECHO --------------
  ECHO.
pause
  GOTO:EOF
pause