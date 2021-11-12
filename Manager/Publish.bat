@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder
ECHO ***Building to macOS***
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true -p:SelfContained=True
robocopy .\bin\Release\net5.0\osx-x64\publish\Timotheus.app %userprofile%\desktop\Timotheus\macOS\Timotheus.app /E
xcopy Resources\Icon*.icns %userprofile%\desktop\Timotheus\macOS\Timotheus.app\Contents\Resources\ /Y

ECHO ***Building to linux***
dotnet publish --runtime linux-x64 --self-contained true

ECHO ***Building Windows installer***
"C:\Program Files (x86)\Caphyon\Advanced Installer 18.7\bin\x86\AdvancedInstaller.com" /build ../Installer/Windows/Installer.aip
dotnet restore -r win-x64
PAUSE