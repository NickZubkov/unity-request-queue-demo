using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    public sealed class RequestQueue : IRequestQueue, IDisposable
    {
        private readonly LinkedList<IQueueItem> _items = new();
        private readonly CancellationTokenSource _shutdownCts = new();
        private bool _isProcessing;
        private bool _disposed;

        public UniTask<T> EnqueueAsync<T>(IRequestOperation<T> operation,
                                          CancellationToken cancellationToken = default)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (_disposed) throw new ObjectDisposedException(nameof(RequestQueue));

            var item = new QueueItem<T>(operation, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                item.CancelPending();
                return item.Task;
            }

            var node = _items.AddLast((IQueueItem)item);
            item.Attach(node, OnExternalCancel);

            if (!_isProcessing)
            {
                _isProcessing = true;
                ProcessAsync().Forget();
            }
            return item.Task;
        }

        private void OnExternalCancel(IQueueItem item)
        {
            if (item.Started) return;
            item.RemoveFromList();
            item.CancelPending();
        }

        private async UniTaskVoid ProcessAsync()
        {
            while (true)
            {
                if (_items.Count == 0)
                {
                    _isProcessing = false;
                    return;
                }

                var node = _items.First;
                var item = node.Value;
                item.Started = true;

                await item.RunAsync(_shutdownCts.Token);

                if (node.List != null) _items.Remove(node);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _shutdownCts.Cancel();
            foreach (var item in _items) item.CancelPending();
            _items.Clear();
            _shutdownCts.Dispose();
        }
    }
}
