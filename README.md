# Binary Packer for ElasticStack components

This project aims at packing ElasticStack component binaries into NuGet packages so people can download them using NuGet.

## Package list

### Filebeat

| Name                    | Operation System | Platform | NuGet Package                                                                                                                  |
| ----------------------- | ---------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------ |
| filebeat-windows-x86_64 | Windows          | x86_64   | [![NuGet](https://img.shields.io/nuget/v/filebeat-windows-x86_64.svg)](https://www.nuget.org/packages/filebeat-windows-x86_64) |
| filebeat-linux-x86_64   | Linux            | x86_64   | [![NuGet](https://img.shields.io/nuget/v/filebeat-linux-x86_64.svg)](https://www.nuget.org/packages/filebeat-linux-x86_64)     |

## Available MSBuild props

| Name                             | Required? | Default Value | Description                                                                         |
| -------------------------------- | --------- | ------------- | ----------------------------------------------------------------------------------- |
| BinaryPackageDownloadUrlTemplate | Yes       | -             | Url template to download binary package from. Support placeholders\*.               |
| BinaryPackageNameTemplate        | Yes       | -             | Name to be used as packed package's id. Support placeholders\*.                     |
| BinaryPackageVersion             | Yes       | -             | Version of component binary package, will also be used as packed package's version. |
| BinaryPackageOS                  | No        | -             | Target operation system of component binary package.                                |
| BinaryPackagePlatform            | No        | -             | Target platform of component binary package.                                        |
| BinaryPackageExtension           | No        | -             | File extension of component binary package.                                         |

\* Supported placeholders: `{BinaryPackageOS}`, `{BinaryPackagePlatform}`, `{BinaryPackageVersion}`, `{BinaryPackageExtension}`.

## Add new component package

1. Create package common props under `packages/<component>/Directory.Build.props` with below template:

```xml
<Project>
    <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory).., Directory.Build.props))/Directory.Build.props" />
    <PropertyGroup>
        <BinaryPackageDownloadUrlTemplate>COMPONENT_BINARY_PACKAGE_DOWNLOAD_URL_TEMPLATE</BinaryPackageDownloadUrlTemplate>
        <BinaryPackageNameTemplate>COMPONENT_BINARY_PACKAGE_NAME_TEMPLATE</BinaryPackageNameTemplate>
    </PropertyGroup>
</Project>
```

## Add new version/os/platform of existing component package

1. Create package project under `packages/<component>/<os>/<platform>/<version>/package.proj` with below template:

   ```xml
   <Project Sdk="Microsoft.Build.NoTargets">
       <PropertyGroup>
           <BinaryPackageVersion>COMPONENT_BINARY_PACKAGE_VERSION</BinaryPackageVersion>
           <BinaryPackageOS>COMPONENT_BINARY_PACKAGE_OS</BinaryPackageOS>
           <BinaryPackagePlatform>COMPONENT_BINARY_PACKAGE_PLATFORM</BinaryPackagePlatform>
           <BinaryPackageExtension>COMPONENT_BINARY_PACKAGE_EXTENSION</BinaryPackageExtension>
       </PropertyGroup>
   </Project>
   ```

2. Modify `README.md` to add newly added components to list
3. Commit and trigger CI/CD process to publish new package
