

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Configurations
{
    public class FeatureManagementConfiguration
    {
        public Dictionary<FeatureManagement, bool> Features { get; set; } = new();

        public static FeatureManagementConfiguration FromConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("FeatureManagement");
            var dict = section.GetChildren()
                .ToDictionary(
                    x => Enum.Parse<FeatureManagement>(x.Key, ignoreCase: true),
                    x => bool.Parse(x.Value!)
                );

            return new FeatureManagementConfiguration { Features = dict };
        }

        public bool IsEnabled(FeatureManagement feature)
            => Features.TryGetValue(feature, out var enabled) && enabled;
    }
}
