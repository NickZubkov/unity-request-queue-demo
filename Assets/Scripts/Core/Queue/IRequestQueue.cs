using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    public interface IRequestQueue
    {
        // Ставит операцию в очередь и возвращает её результат.
        // cancellationToken отменяет КОНКРЕТНО этот запрос:
        //   не стартовал — удаляется из очереди; выполняется — прерывается.
        public UniTask<T> EnqueueAsync<T>(IRequestOperation<T> operation,
                                          CancellationToken cancellationToken = default);
    }
}
