using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Tests.EditMode
{
    internal sealed class FakeOperation<T> : IRequestOperation<T>
    {
        private readonly UniTaskCompletionSource<T> _completion = new();
        public bool Started { get; private set; }
        public bool ObservedCancellation { get; private set; }

        public async UniTask<T> ExecuteAsync(CancellationToken ct)
        {
            Started = true;
            try
            {
                return await _completion.Task.AttachExternalCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                ObservedCancellation = true;
                throw;
            }
        }

        public void Complete(T value) => _completion.TrySetResult(value);
        public void Fail(Exception e) => _completion.TrySetException(e);
    }
}
