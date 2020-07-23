using ElasticStack.BinaryPacker.Tasks.Internal;

namespace ElasticStack.BinaryPacker.Tasks
{
    public class UnPackTarGz : UnPackTaskBase
    {
        protected override void Extract(
            string sourceFile,
            string destinationFolder)
        {
            using var tarStream = Utilities.ExtractGzStream(sourceFile);
            Utilities.ExtractTar(tarStream, destinationFolder);
        }
    }
}
