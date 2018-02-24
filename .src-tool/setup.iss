; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Generator Application (tfw.io)"
#define MyAppVersion "1.0"
#define MyAppPublisher "tfwroble@gmail.com"
#define MyAppURL "mailto:tfwroble@gmail.com"
#define MyAppExeName "Gen.App.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{F5E08013-E8DB-431A-AA00-75A71FF46E3E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=.\bin
OutputBaseFilename=setup-generator-20180224
SetupIconFile=Source\_rc\appbar.shuffle.ico
Compression=lzma
SolidCompression=yes
ShowLanguageDialog=auto
UninstallDisplayName=Generator Application (github/tfwio)
UninstallDisplayIcon={app}\Gen.App.exe
MinVersion=0,6.0

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "bin\Release\FirstFloor.ModernUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\FirstFloor.ModernUI.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Gen.App.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Gen.App.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Gen.Lib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Gen.Lib.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\ICSharpCode.AvalonEdit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\ICSharpCode.AvalonEdit.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\ICSharpCode.SharpDevelop.Widgets.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\System.Data.SQLite.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\AvalonDock.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\AvalonDock.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\AvalonDock.Themes.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\AvalonDock.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\x64\SQLite.Interop.dll"; DestDir: "{app}\x64\"; Flags: ignoreversion
Source: "bin\Release\x86\SQLite.Interop.dll"; DestDir: "{app}\x86\"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

