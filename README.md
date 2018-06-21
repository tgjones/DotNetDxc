# DotNetDxc

This is a .NET wrapper for Microsoft's open-source [DirectXShaderCompiler](https://github.com/Microsoft/DirectXShaderCompiler). All code comes from the upstream DirectXShaderCompiler project, and this project only exists separately so that it can be easily published as a NuGet package.

## Usage

``` csharp
var path = "shader.hlsl";

var dxcIntelliSense = HlslDxcLib.CreateDxcIntelliSense();

var index = dxcIntelliSense.CreateIndex();

var commandLineArgs = new string[0];

var unsavedFiles = new IDxcUnsavedFile[]
{
    new TrivialDxcUnsavedFile(path, File.ReadAllText(path))
};

var translationUnit = index.ParseTranslationUnit(
    path,
    commandLineArgs,
    commandLineArgs.Length,
    unsavedFiles,
    (uint) unsavedFiles.Length,
    (uint) DxcTranslationUnitFlags.DxcTranslationUnitFlags_UseCallerThread);

var numDiagnostics = translationUnit.GetNumDiagnostics();
```

## Binaries

DotNetDxc is available as a NuGet package compatible with .NET Standard 2.0: [![NuGet](https://img.shields.io/nuget/v/DotNetDxc.svg)](https://www.nuget.org/packages/DotNetDxc/)

The NuGet package includes native DirectXShaderCompiler DLLs for all supported platforms - currently, Windows x86 and Windows x64.

## License

This software is released under the [University of Illinois Open Source License](https://github.com/Microsoft/DirectXShaderCompiler/blob/master/LICENSE.TXT), the same licence as the upstream DirectXShaderCompiler project.