:::==============================================================
:: This file is here for reference.
::==============================================================
::@set dotnet4=%windir%\Microsoft.Net\Framework\v4.0.30319
::@set dotnet35=%windir%\Microsoft.Net\Framework\v3.5
::@set dotnet2=%windir%\Microsoft.Net\Framework\v2.0.50727
::==============================================================
::%programfiles%\Microsoft SDKs\Windows
::x64
::@set x64sdk=%programfiles%\Microsoft SDKs\Windows
::@set sdk=%programfiles%\Microsoft SDKs\Windows\V6.0A\Bin
::@set sdk=%programfiles%\Microsoft SDKs\Windows\V6.1\Bin
::@set sdk=%programfiles%\Microsoft SDKs\Windows\V7.0\Bin
::@set sdk=%programfiles%\Microsoft SDKs\Windows\V7.1\Bin
::x86
@set x86sdk=%programfiles(x86)%\Microsoft SDKs\Windows
::@set sdk=%x86sdk%\V5.0\Bin
::@set sdk=%x86sdk%\V6.0A\Bin
::@set sdk=%x86sdk%\V7.0A\Bin
::@set sdk=%x86sdk%\V8.0\Bin
::@set sdk=%x86sdk%\V8.0A\Bin
::==============================================================
:: using NET4
::@set path=%dotnet4%;%sdk%;%path%
::==============================================================
@set path=%x86sdk%\V7.0A\Bin;%windir%\microsoft.net\framework\v4.0.30319;%path%
@xsd /?
xsd /c /e:GeneratorTags /language:cs %1
@pause
