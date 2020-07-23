using ElasticStack.BinaryPacker.Tasks.Internal;

namespace ElasticStack.BinaryPacker.Tasks
{
    public class UnPackTarBz2 : UnPackTaskBase
    {
        protected override void Extract(
            string sourceFile,
            string destinationFolder)
        {
            using var tarStream = Utilities.ExtractBz2Stream(sourceFile);
            Utilities.ExtractTar(tarStream, destinationFolder);
        }
    }
}
