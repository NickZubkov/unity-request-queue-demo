using System.Threading;
using Cysharp.Threading.Tasks;

namespace RequestQueueDemo.Core.Queue
{
    // Единица работы: один сетевой вызов. Реализуется операциями сетевого слоя.
    // Контракт: операция ОБЯЗАНА слушать cancellationToken и прерываться при отмене
    // (реальные — через UnityWebRequest.WithCancellation). На этом строится отмена
    // выполняющегося запроса: очередь отменяет токен — операция прерывается сама.
    public interface IRequestOperation<T>
    {
        public UniTask<T> ExecuteAsync(CancellationToken cancellationToken);
    }
}
