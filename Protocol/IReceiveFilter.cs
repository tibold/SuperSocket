using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSocket.Protocol
{
    public interface IReceiveFilter<out TPackageInfo>
        where TPackageInfo : IPackageInfo
    {
        TPackageInfo Filter(ReceivedData data, out int rest);

        IReceiveFilter<TPackageInfo> NextReceiveFilter { get; }

        FilterState State { get; }

        void Reset();
    }
}
