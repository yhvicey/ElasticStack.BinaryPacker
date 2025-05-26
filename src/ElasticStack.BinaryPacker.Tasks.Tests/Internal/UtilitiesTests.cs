using System;
using System.IO;
using System.Reflection;

using Xunit;

namespace ElasticStack.BinaryPacker.Tasks.Internal;

public class UtilitiesTests
{
    [Fact]
    public void TarFileShouldBeExtractedCorrectly()
    {
        // Arrange
        var archiveFilePath = GetArchiveFilePath("archive.tar");
        var stream = File.OpenRead(archiveFilePath);

        // Act
        var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Utilities.ExtractTar(stream, tempFolder);

        // Assert
        AssertFilesExtractedCorrectly(tempFolder);
    }

    [Fact]
    public void TarGzFileShouldBeExtractedCorrectly()
    {
        // Arrange
        var archiveFilePath = GetArchiveFilePath("archive.tar.gz");

        // Act
        var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        using var tarStream = Utilities.ExtractGzStream(archiveFilePath);
        Utilities.ExtractTar(tarStream, tempFolder);

        // Assert
        AssertFilesExtractedCorrectly(tempFolder);
    }

    [Fact]
    public void TarBz2FileShouldBeExtractedCorrectly()
    {
        // Arrange
        var archiveFilePath = GetArchiveFilePath("archive.tar.bz2");

        // Act
        var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        using var tarStream = Utilities.ExtractBz2Stream(archiveFilePath);
        Utilities.ExtractTar(tarStream, tempFolder);

        // Assert
        AssertFilesExtractedCorrectly(tempFolder);
    }

    private string GetArchiveFilePath(string fileName)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyLocation == null)
        {
            throw new DirectoryNotFoundException("Assembly location could not be determined.");
        }
        
        return Path.Combine(assemblyLocation, "Archives", fileName);
    }

    private void AssertFilesExtractedCorrectly(string extractFolder)
    {
        var dir1Path = Path.Combine(extractFolder, "1");
        var dir2Path = Path.Combine(dir1Path, "2");
        var fileAPath = Path.Combine(extractFolder, "a");
        var fileBPath = Path.Combine(dir1Path, "b");
        var fileCPath = Path.Combine(dir2Path, "c");

        Assert.True(Directory.Exists(extractFolder), "Extract folder should be created correctly");
        Assert.True(Directory.Exists(dir1Path), $"Subdirectory 1 should be created correctly in {extractFolder}");
        Assert.True(Directory.Exists(dir2Path), $"Subdirectory 2 should be created correctly in {dir1Path}");
        Assert.True(File.Exists(fileAPath), $"File 'a' should be extracted correctly in {extractFolder}");
        Assert.True(File.Exists(fileBPath), $"File 'b' should be extracted correctly in {dir1Path}");
        Assert.True(File.Exists(fileCPath), $"File 'c' should be extracted correctly in {dir2Path}");
    }
}
