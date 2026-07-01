using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    internal sealed class QueueItem<T> : IQueueItem
    {
        private readonly IRequestOperation<T> _operation;
        private readonly CancellationToken _externalToken;
        private readonly UniTaskCompletionSource<T> _tcs = new();
        private CancellationTokenRegistration _registration;
        private LinkedListNode<IQueueItem> _node;
        private bool _settled;

        public bool Started { get; set; }
        public UniTask<T> Task => _tcs.Task;

        public QueueItem(IRequestOperation<T> operation, CancellationToken externalToken)
        {
            _operation = operation;
            _externalToken = externalToken;
        }

        public void Attach(LinkedListNode<IQueueItem> node, Action<IQueueItem> onExternalCancel)
        {
            _node = node;
            _registration = _externalToken.Register(() => onExternalCancel(this));
        }

        public async UniTask RunAsync(CancellationToken shutdownToken)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_externalToken, shutdownToken);
            try
            {
                var result = await _operation.ExecuteAsync(linked.Token);
                Settle(() => _tcs.TrySetResult(result));
            }
            catch (OperationCanceledException)
            {
                Settle(() => _tcs.TrySetCanceled());
            }
            catch (Exception e)
            {
                Settle(() => _tcs.TrySetException(e));
            }
            finally
            {
                _registration.Dispose();
            }
        }

        public void CancelPending()
        {
            _registration.Dispose();
            Settle(() => _tcs.TrySetCanceled());
        }

        public void RemoveFromList()
        {
            if (_node?.List != null) _node.List.Remove(_node);
        }

        private void Settle(Action action)
        {
            if (_settled) return;
            _settled = true;
            action();
        }
    }
}
