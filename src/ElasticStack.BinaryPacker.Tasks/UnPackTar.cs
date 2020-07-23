using ElasticStack.BinaryPacker.Tasks.Internal;

namespace ElasticStack.BinaryPacker.Tasks
{
    public class UnPackTar : UnPackTaskBase
    {
        protected override void Extract(
            string sourceFile,
            string destinationFolder)
        {
            Utilities.ExtractTar(sourceFile, destinationFolder);
        }
    }
}
