using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace ElasticStack.BinaryPacker.Tasks.Internal
{
    internal static class Utilities
    {
        internal static int Execute(
            string fileName,
            string arguments,
            string workingDirectory,
            out string stdout,
            out string stderr)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Directory.GetCurrentDirectory();
            }
            workingDirectory = Path.GetFullPath(workingDirectory);
            var ps = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
            };

            using (var process = Process.Start(ps))
            {
                process.WaitForExit();
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                return process.ExitCode;
            }
        }

        internal static Stream ExtractBz2Stream(
            string sourceFile)
        {
            using var bz2Stream = File.OpenRead(sourceFile);
            var decompressedStream = new MemoryStream();
            BZip2.Decompress(bz2Stream, decompressedStream, false);
            decompressedStream.Seek(0, SeekOrigin.Begin);
            return decompressedStream;
        }

        internal static Stream ExtractGzStream(
            string sourceFile)
        {
            using var gzStream = File.OpenRead(sourceFile);
            var decompressedStream = new MemoryStream();
            GZip.Decompress(gzStream, decompressedStream, false);
            decompressedStream.Seek(0, SeekOrigin.Begin);
            return decompressedStream;
        }

        internal static void ExtractTar(
            string sourceFile,
            string destinationFolder)
        {
            using var tarStream = File.OpenRead(sourceFile);
            ExtractTar(tarStream, destinationFolder);
        }

        internal static void ExtractTar(
            Stream tarStream,
            string destinationFolder)
        {
            var archive = TarArchive.CreateInputTarArchive(tarStream, Encoding.UTF8);
            archive.ExtractContents(destinationFolder);
        }

        internal static string GetFullPath(
            string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            var extensions = Environment.GetEnvironmentVariable("PATHEXT") ?? string.Empty;
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPathWithoutExtension = Path.Combine(path, fileName);
                if (File.Exists(fullPathWithoutExtension))
                {
                    return fullPathWithoutExtension;
                }
                foreach (var extension in extensions.Split(Path.PathSeparator))
                {
                    var fullPath = Path.Combine(path, $"{fileName}{extension}");
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }
            return null;
        }
    }
}
