using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace ElasticStack.BinaryPacker.Tasks
{
    public abstract class UnPackTaskBase : Task
    {
        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        [Required]
        public ITaskItem DestinationFolder { get; set; }

        public string TarExecutablePath { get; set; }

        public override bool Execute()
        {
            var destinationFolderPath = Path.GetFullPath(DestinationFolder.ItemSpec);
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            foreach (var sourceFile in SourceFiles)
            {
                var sourceFilePath = Path.GetFullPath(sourceFile.ItemSpec);
                var arguments = $"xzvf {sourceFilePath} -C {destinationFolderPath}";
                Log.LogMessage("Extracting {0} to {1}", sourceFilePath, destinationFolderPath);
                try
                {
                    Extract(sourceFilePath, destinationFolderPath);
                }
                catch (Exception ex)
                {
                    Log.LogError("Failed to extract {0}", sourceFilePath);
                    Log.LogError(ex.Message);
                    return false;
                }
            }

            return true;
        }

        protected abstract void Extract(string sourceFile, string destinationFolder);
    }
}
