@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder

REM Load info about the application and delete last build
SET VERSION=1.3.0
IF EXIST %userprofile%\desktop\Timotheus (
    rmdir %userprofile%\desktop\Timotheus /s /q
)
CD ..

REM COMPILATION SECTION
powershell -Command "(gc Timotheus.cs) -replace 'X.X.X', '%VERSION%' | Out-File -encoding ASCII Timotheus.cs"
dotnet publish -c Release --runtime win-x64 --self-contained true -p:PublishReadyToRun=true --output %userprofile%\desktop\Timotheus\Windows\Application\

REM INSTALLER SECTION
REM "C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /edit %userprofile%/OneDrive/Dokumenter/GitHub/Timotheus/Installer/Windows/Installer.aip /SetVersion %VERSION%
REM "C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /build %userprofile%/OneDrive/Dokumenter/GitHub/Timotheus/Installer/Windows/Installer.aip
REM xcopy ..\Installer\Windows\Installer-SetupFiles\Installer*.msi %userprofile%\desktop\Timotheus\Windows\Installer\ /Y
REM move %userprofile%\desktop\Timotheus\Windows\Installer\Installer.msi %userprofile%\desktop\Timotheus\Windows\Installer\Installer-%VERSION%.msi

REM CLEAN UP
dotnet restore -r win-x64
powershell -Command "(gc Timotheus.cs) -replace '%VERSION%', 'X.X.X' | Out-File -encoding ASCII Timotheus.cs"
PAUSE