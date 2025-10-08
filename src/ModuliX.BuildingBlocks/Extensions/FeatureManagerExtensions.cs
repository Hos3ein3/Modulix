

using Microsoft.FeatureManagement;
using ModuliX.BuildingBlocks.Enums;

namespace ModuliX.BuildingBlocks.Extensions;

public static class FeatureManagerExtensions
{
    public static Task<bool> IsEnabledAsync(this IFeatureManager featureManager, FeatureManagement feature)
        => featureManager.IsEnabledAsync(feature.ToString());
}
