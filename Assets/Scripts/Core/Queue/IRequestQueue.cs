using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    public interface IRequestQueue
    {
        public UniTask<T> EnqueueAsync<T>(IRequestOperation<T> operation,
                                          CancellationToken cancellationToken = default);
    }
}
