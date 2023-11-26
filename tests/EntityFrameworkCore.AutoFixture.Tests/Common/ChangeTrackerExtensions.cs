using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore;

internal static class ChangeTrackerExtensions
{
    public static void Clear(this ChangeTracker changeTracker)
    {
        changeTracker.Entries().Where(e => e.State != EntityState.Detached)
            .ToList().ForEach(static e => e.State = EntityState.Detached);
    }
}
