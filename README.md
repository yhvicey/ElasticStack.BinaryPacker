# Binary Packer for ElasticStack components

This project aims at packing ElasticStack component binaries into NuGet packages so people can download them using NuGet.

## Package list

### Elasticsearch

| Name                                | Operation System | Platform | NuGet Package                                                                                                                                          |
| ----------------------------------- | ---------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| elasticsearch-windows-x86_64-binary | Windows          | x86_64   | [![NuGet](https://img.shields.io/nuget/v/elasticsearch-windows-x86_64-binary.svg)](https://www.nuget.org/packages/elasticsearch-windows-x86_64-binary) |
| elasticsearch-linux-x86_64-binary   | Linux            | x86_64   | [![NuGet](https://img.shields.io/nuget/v/elasticsearch-linux-x86_64-binary.svg)](https://www.nuget.org/packages/elasticsearch-linux-x86_64-binary)     |

### Filebeat

| Name                           | Operation System | Platform | NuGet Package                                                                                                                                |
| ------------------------------ | ---------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| filebeat-windows-x86_64-binary | Windows          | x86_64   | [![NuGet](https://img.shields.io/nuget/v/filebeat-windows-x86_64-binary.svg)](https://www.nuget.org/packages/filebeat-windows-x86_64-binary) |
| filebeat-linux-x86_64-binary   | Linux            | x86_64   | [![NuGet](https://img.shields.io/nuget/v/filebeat-linux-x86_64-binary.svg)](https://www.nuget.org/packages/filebeat-linux-x86_64-binary)     |

### Kibana

| Name                         | Operation System | Platform | NuGet Package                                                                                                                            |
| ---------------------------- | ---------------- | -------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| kibana-windows-x86_64-binary | Windows          | x86_64   | [![NuGet](https://img.shields.io/nuget/v/kibana-windows-x86_64-binary.svg)](https://www.nuget.org/packages/kibana-windows-x86_64-binary) |
| kibana-linux-x86_64-binary   | Linux            | x86_64   | [![NuGet](https://img.shields.io/nuget/v/kibana-linux-x86_64-binary.svg)](https://www.nuget.org/packages/kibana-linux-x86_64-binary)     |

### Logstash

| Name            | Operation System | Platform | NuGet Package                                                                                                  |
| --------------- | ---------------- | -------- | -------------------------------------------------------------------------------------------------------------- |
| filebeat-binary | -                | -        | [![NuGet](https://img.shields.io/nuget/v/logstash-binary.svg)](https://www.nuget.org/packages/logstash-binary) |

## Available MSBuild props

| Name                             | Required? | Default Value | Description                                                                                         |
| -------------------------------- | --------- | ------------- | --------------------------------------------------------------------------------------------------- |
| BinaryPackageDownloadUrlTemplate | Yes       | -             | Url template to download binary package from. Support placeholders\*.                               |
| BinaryPackageNameTemplate        | Yes       | -             | Name to be used as packed package's id. Support placeholders\*.                                     |
| BinaryPackageVersion             | Yes       | -             | Version of component binary package, will also be used as packed package's version.                 |
| BinaryPackageOS                  | No        | -             | Target operation system of component binary package.                                                |
| BinaryPackagePlatform            | No        | -             | Target platform of component binary package.                                                        |
| BinaryPackageExtension           | No        | -             | File extension of component binary package.                                                         |
| IsNestedPackage                  | No        | False         | Indicates if the package is nested package (e.g. the archive file contains a folder with same name) |

\* Supported placeholders: `{BinaryPackageOS}`, `{BinaryPackagePlatform}`, `{BinaryPackageVersion}`, `{BinaryPackageExtension}`.

## Add new component package

1. Create package common props under `packages/<component>/Directory.Build.props` with below template:

   ```xml
   <Project>
       <Import Project="$([MSBuild]::GetDirectoryNameOfFileAbove($(MSBuildThisFileDirectory).., Directory.Build.props))/Directory.Build.props" />
       <PropertyGroup>
           <BinaryPackageDownloadUrlTemplate>COMPONENT_BINARY_PACKAGE_DOWNLOAD_URL_TEMPLATE</BinaryPackageDownloadUrlTemplate>
           <BinaryPackageNameTemplate>COMPONENT_BINARY_PACKAGE_NAME_TEMPLATE</BinaryPackageNameTemplate>
           <IsNestedPackage>True</IsNestedPackage>
       </PropertyGroup>
   </Project>
   ```

1. Add traversal project for sub projects with below template:

   ```xml
   <Project Sdk="Microsoft.Build.Traversal">
       <ItemGroup>
           <ProjectReference Include="**/package.proj" />
       </ItemGroup>
   </Project>
   ```

1. Add component traversal project to root traversal project (`packages/dirs.proj`):

   ```xml
   <Project Sdk="Microsoft.Build.Traversal">
       <ItemGroup>
           <!-- ... -->
           <ProjectReference Include="<component>/dirs.proj" />
       </ItemGroup>
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
