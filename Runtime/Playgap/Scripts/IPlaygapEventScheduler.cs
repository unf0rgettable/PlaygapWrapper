using System;

namespace Playgap
{
    internal interface IPlaygapEventScheduler
    {
        void ScheduleOnUpdate(Action action);
    }
}
