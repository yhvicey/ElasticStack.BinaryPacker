using System.Linq;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace ElasticStack.BinaryPacker.Tasks
{
    public class ListLatestNuGetPackageVersion : Task
    {
        [Required]
        public string NuGetFeed { get; set; }

        [Required]
        public string PackageId { get; set; }

        public bool IsV2Feed { get; set; } = false;

        [Output]
        public bool Exists { get; set; }

        [Output]
        public string LatestVersion { get; set; }

        public override bool Execute()
        {
            var cache = new SourceCacheContext();
            var repo = IsV2Feed
                ? Repository.Factory.GetCoreV3(NuGetFeed)
                : Repository.Factory.GetCoreV2(new NuGet.Configuration.PackageSource(NuGetFeed));
            var resource = repo.GetResource<FindPackageByIdResource>();
            var versions = resource
                .GetAllVersionsAsync(PackageId, cache, NullLogger.Instance, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
            var latestVersion = versions.OrderByDescending(v => v.Version).FirstOrDefault();
            if (latestVersion == null)
            {
                Log.LogMessage("Package {0} doesn't exist in feed {1}", PackageId, NuGetFeed);
                Exists = false;
                LatestVersion = null;
            }
            else
            {
                Log.LogMessage("Package {0} exists in feed {1}, latest version is {2}", PackageId, NuGetFeed, latestVersion.Version);
                Exists = true;
                LatestVersion = latestVersion.Version.ToString();
            }
            return true;
        }
    }
}
