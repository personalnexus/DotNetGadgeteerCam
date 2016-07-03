using System;
using Microsoft.SPOT;

namespace DotNetGadgeteerCam.Model.Status
{
    public abstract class StatusProviderBase: IStatusProvider
    {
        public StatusProviderBase(IStatusChecker statusChecker, string name)
        {
            Name = name;
            StatusChecker = statusChecker;
            StatusChecker.AddStatusProvider(this);
        }

        protected IStatusChecker StatusChecker { get; private set; }

        protected void Log(string message)
        {
            StatusChecker.Log(message);
        }

        public string Name { get; private set; }

        private bool _isReady;

        public virtual bool IsReady 
        {
            get
            {
                return _isReady;
            }
            set
            {
                _isReady = value;
                StatusChecker.Check();
            }
        }
    }
}
