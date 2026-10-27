using PepperDash.Core;
using PepperDash.Essentials.Core;

namespace PepperDash.Essentials.Plugins.Utilities
{
    public class EpiphanCommunicationMonitor:StatusMonitorBase
    {
        private bool _isStarted;

        public EpiphanCommunicationMonitor(IKeyed parent, long warningTime, long errorTime) : base(parent, warningTime, errorTime)
        {
        }

        public override void Start()
        {
            _isStarted = true;

            if (Status == MonitorStatus.IsOk)
                StopErrorTimers();
            else
                StartErrorTimers();
        }

        public override void Stop()
        {
            _isStarted = false;
            StopErrorTimers();
        }

        /// <summary>
        /// Report the outcome of a request.
        /// </summary>
        /// <remarks>
        /// A failure starts the warning and error timers rather than changing the status itself, so
        /// one slow reply does not take the device offline but a Pearl that stays silent does. This
        /// used to stop the timers whenever the status was already OK — which, after the first good
        /// reply, it always was — so the Pearl could never be reported offline again.
        /// </remarks>
        public void SetOnlineStatus(bool isOnline)
        {
            if (isOnline)
            {
                Status = MonitorStatus.IsOk;
                StopErrorTimers();
                return;
            }

            // StartErrorTimers leaves timers that are already running alone, so repeated failures
            // count from the first one.
            if (_isStarted)
                StartErrorTimers();
        }
    }
}
