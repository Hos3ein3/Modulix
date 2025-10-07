

namespace ModuliX.BuildingBlocks.Extensions;

public static class ExceptionExtensions
{
    public static string GetInnermostMessage(this Exception ex)
    {
        while (ex.InnerException != null)
            ex = ex.InnerException;
        return ex.Message;
    }

    public static string ToLogMessage(this Exception ex)
        => $"{ex.GetType().Name}: {ex.GetInnermostMessage()}";
}
