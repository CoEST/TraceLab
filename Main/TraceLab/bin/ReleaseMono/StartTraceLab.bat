@echo off

if "%TraceLab_Home%"=="" (
      echo TraceLab_Home is not defined
      pause
)

if "%~x1"==".teml" (
	start mono "%TraceLab_Home%"\TraceLab.exe -base:"%TraceLab_Home%" ^"-o:%~1^"
	goto eof
)

if "%~x1"==".temlx" (
	start mono "%TraceLab_Home%"\TraceLab.exe -base:"%TraceLab_Home%" ^"-o:%~1^"
	goto eof
)


if "%~x1" == ".tpkg" (
	start mono "%TraceLab_Home%"\TraceLab.exe -base:"%TraceLab_Home%" ^"-installpackage:%~1^"
	goto eof
)

if "%1"=="" (
	start mono "%TraceLab_Home%"\TraceLab.exe -base:"%TraceLab_Home%"
	goto eof
)

:eof