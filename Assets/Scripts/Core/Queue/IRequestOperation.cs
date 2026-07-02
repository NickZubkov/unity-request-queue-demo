using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    public interface IRequestOperation<T>
    {
        public UniTask<T> ExecuteAsync(CancellationToken cancellationToken);
    }
}
