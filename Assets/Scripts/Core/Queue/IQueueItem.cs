using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    internal interface IQueueItem
    {
        public bool Started { get; set; }
        public UniTask RunAsync(CancellationToken shutdownToken);
        public void CancelPending();
        public void Attach(LinkedListNode<IQueueItem> node, Action<IQueueItem> onExternalCancel);
        public void RemoveFromList();
    }
}
