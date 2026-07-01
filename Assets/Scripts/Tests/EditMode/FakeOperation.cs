using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Tests.EditMode
{
    // Операция с управляемым завершением: тест сам решает, когда она закончится.
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
                // AttachExternalCancellation эмулирует контракт «прерваться по токену»
                // (как реальный UnityWebRequest.WithCancellation).
                return await _completion.Task.AttachExternalCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                // Ловим отмену здесь (а не через ct.Register): при синхронном возобновлении
                // await продолжение освобождало бы регистрацию раньше, чем она сработает.
                ObservedCancellation = true;
                throw;
            }
        }

        public void Complete(T value) => _completion.TrySetResult(value);
        public void Fail(Exception e) => _completion.TrySetException(e);
    }
}
