@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder

REM Load info about the application and delete last build
SET VERSION=1.2.5
IF EXIST %userprofile%\desktop\Timotheus (
    rmdir %userprofile%\desktop\Timotheus /s /q
)
CD ..

REM COMPILATION SECTION
ECHO ***Building macOS application***
powershell -Command "(gc Timotheus.cs) -replace 'X.X.X', '%VERSION%' | Out-File -encoding ASCII Timotheus.cs"
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true -p:SelfContained=True -p:CFBundleShortVersionString=%VERSION%

REM MOVE BUNDLE
robocopy .\bin\Release\net6.0\osx-x64\publish\Timotheus.app "%userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app" /E
xcopy Resources\macOS\Icon*.icns "%userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app\Contents\Resources\" /Y
xcopy Resources\macOS\Info*.plist "%userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app\Contents\" /Y
xcopy Resources\macOS\Timotheus*.entitlements "%userprofile%\desktop\Timotheus\macOS\Application\" /Y
powershell -Command "(gc %userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app\Contents\Info.plist) -replace 'X.X.X', '%VERSION%' | Out-File -encoding ASCII %userprofile%\desktop\Timotheus\macOS\Application\Timotheus.app\Contents\Info.plist"

REM CLEAN UP
dotnet restore -r win-x64
powershell -Command "(gc Timotheus.cs) -replace '%VERSION%', 'X.X.X' | Out-File -encoding ASCII Timotheus.cs"
PAUSE