using System.Collections.Generic;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskItemsCollection : HashSet<StartupTaskItem>, IStartupTaskItemsCollection
    {
    }
}