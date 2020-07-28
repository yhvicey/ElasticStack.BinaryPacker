using System.Linq;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ElasticStack.BinaryPacker.Tasks
{
    public class IsPackageVersionExists : Task
    {
        [Required]
        public string NuGetFeed { get; set; }

        [Required]
        public string PackageId { get; set; }

        [Required]
        public string PackageVersion { get; set; }

        public bool IsV2Feed { get; set; } = false;

        [Output]
        public bool Exists { get; set; } = false;

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
            if (!versions.Any())
            {
                Log.LogMessage(MessageImportance.High, "Package {0} doesn't exist in feed {1}", PackageId, NuGetFeed);
                return true;
            }

            var nugetVersion = new NuGetVersion(PackageVersion);
            var exists = versions.Any(v => v == nugetVersion);
            if (!exists)
            {
                Log.LogMessage(MessageImportance.High, "Version {0} of package {1} doesn't exist in feed {2}", PackageVersion, PackageId, NuGetFeed);
            }
            else
            {
                Log.LogMessage(MessageImportance.High, "Version {0} of package {1} exists in feed {2}", PackageVersion, PackageId, NuGetFeed);
                Exists = true;
            }
            return true;
        }
    }
}
