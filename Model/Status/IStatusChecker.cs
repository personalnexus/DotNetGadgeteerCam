using System;
using System.Collections;

namespace DotNetGadgeteerCam.Model.Status
{
    public interface IStatusChecker
    {
        void AddStatusProvider(IStatusProvider statusProvider);
        bool Check();
        void Log(string message);

        IEnumerable StatusProviders { get; }
    }
}