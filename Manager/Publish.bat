@ECHO OFF
REM This .bat script builds for macOS and creates a an .app on the Desktop in a Timotheus folder
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:UseAppHost=true
robocopy .\bin\Release\net5.0\osx-x64\publish\Timotheus.app %userprofile%\desktop\Timotheus\macOS\Timotheus.app /E
xcopy .\Resources\Icon*.icns %userprofile%\desktop\Timotheus\macOS\Timotheus.app\Contents\Resources\ /Y
PAUSE