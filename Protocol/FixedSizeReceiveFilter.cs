using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSocket.Protocol
{
    public abstract class FixedSizeReceiveFilter<TPackageInfo> : IReceiveFilter<TPackageInfo>, IPackageResolver<TPackageInfo>
        where TPackageInfo : IPackageInfo
    {
        private int m_OriginalSize;
        private int m_Size;

        public FixedSizeReceiveFilter(int size)
        {
            m_OriginalSize = size;
            m_Size = size;
        }

        public virtual TPackageInfo Filter(ReceivedData data, out int rest)
        {
            rest = 0;
            var total = data.Total;

            //Haven't received a full request package
            if (total < m_Size)
                return default(TPackageInfo);

            if (total > m_Size)
                rest = total - m_Size;

            data.SetLastLength(m_Size - (data.Current.Count - rest));

            if (!CanResolvePackage(data.PackageData))
                return default(TPackageInfo);

            return ResolvePackage(data.PackageData);
        }

        public IReceiveFilter<TPackageInfo> NextReceiveFilter { get; protected set; }

        public FilterState State { get; protected set; }

        protected void ResetSize(int newSize)
        {
            m_Size = newSize;
        }

        public virtual void Reset()
        {
            if (m_Size != m_OriginalSize)
                m_Size = m_OriginalSize;
        }

        protected virtual bool CanResolvePackage(IList<ArraySegment<byte>> packageData)
        {
            return true;
        }

        public abstract TPackageInfo ResolvePackage(IList<ArraySegment<byte>> packageData);
    }
}
