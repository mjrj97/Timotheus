@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder
ECHO ***Building macOS app***
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true
robocopy .\bin\Release\net5.0\osx-x64\publish\Timotheus.app %userprofile%\desktop\Timotheus\macOS\Timotheus.app /E
xcopy Icon*.icns %userprofile%\desktop\Timotheus\macOS\Timotheus.app\Contents\Resources\ /Y

ECHO ***Building Windows installer***
"C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /build ../Installer/Windows/Installer.aip
PAUSE