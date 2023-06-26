# How to run
## Runtime prepare
DotNet 7 runtime installation https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website
## Download application
Download the archive from [latest release](https://github.com/consoleworld/Miaocrosoft.GPT/releases) and **extract** into your directory.
## Run application
### Simply start up
```
dotnet Miaocrosoft.GPT.dll
```
### Optional parameters
```
dotnet Miaocrosoft.GPT.dll --urls "http://localhost:8000;http://localhost:8001", --kestrel:Certificates:Default:Password={{password}} --kestrel:Certificates:Default:Path={{.pfx}}
```