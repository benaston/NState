To push a new version to NuGet:

1. Build the solution in debug mode (mnakes for easier debugging by users).
2. Pack the NuGet package. Run `build\build.bat`, then choose `npack` as the option and press return.
3. Open powershell and cd to the location of the NState source root folder on your machine.
4. Set your NuGet API key (login to NuGet.org to find this out). `./build/nuget.exe setApiKey <key>`
5. .\build\Nuget.exe push .\NState.<version-number-of-your-package>.nupkg