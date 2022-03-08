@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder

REM Load info about the application and delete last build
SET VERSION=1.2.0
IF EXIST %userprofile%\desktop\Timotheus (
    rmdir %userprofile%\desktop\Timotheus /s /q
)

REM MACOS SECTION
ECHO ***Building macOS application***
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:CFBundleVersion=%VERSION% -p:CFBundleShortVersionString=%VERSION% -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true -p:SelfContained=True
robocopy .\bin\Release\net5.0\osx-x64\publish\Timotheus.app %userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app /E
xcopy Resources\Icon*.icns %userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app\Contents\Resources\ /Y

REM LINUX SECTION
ECHO ***Building Linux application***
dotnet publish -c Release --runtime linux-x64 --self-contained true -p:PublishReadyToRun=true -p:PublishTrimmed=true --output %userprofile%\desktop\Timotheus\Linux\Application\

REM WINDOWS SECTION
ECHO ***Building Windows application***
dotnet publish -c Release --runtime win-x64 --self-contained true -p:PublishReadyToRun=true -p:PublishTrimmed=true --output %userprofile%\desktop\Timotheus\Windows\Application\

ECHO ***Building Windows installer***
"C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /edit %userprofile%/OneDrive/Dokumenter/GitHub/Timotheus/Installer/Windows/Installer.aip /SetVersion %VERSION%
"C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /build %userprofile%/OneDrive/Dokumenter/GitHub/Timotheus/Installer/Windows/Installer.aip
xcopy ..\Installer\Windows\Installer-SetupFiles\Installer*.msi %userprofile%\desktop\Timotheus\Windows\Installer\ /Y
move %userprofile%\desktop\Timotheus\Windows\Installer\Installer.msi %userprofile%\desktop\Timotheus\Windows\Installer\Installer-%VERSION%.msi
dotnet restore -r win-x64
PAUSE