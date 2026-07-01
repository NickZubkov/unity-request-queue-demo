using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using RequestQueueDemo.Core.Queue;

namespace RequestQueueDemo.Tests.EditMode
{
    public sealed class RequestQueueTests
    {
        [Test]
        public void Processes_Operations_Sequentially()
        {
            var queue = new RequestQueue();
            var op1 = new FakeOperation<int>();
            var op2 = new FakeOperation<int>();

            _ = queue.EnqueueAsync(op1);
            _ = queue.EnqueueAsync(op2);

            Assert.IsTrue(op1.Started, "Первая стартует сразу");
            Assert.IsFalse(op2.Started, "Вторая ждёт завершения первой");

            op1.Complete(1);
            Assert.IsTrue(op2.Started, "Вторая стартует после первой");
        }

        [Test]
        public void Returns_Operation_Result()
        {
            var queue = new RequestQueue();
            var op = new FakeOperation<int>();
            var task = queue.EnqueueAsync(op);
            op.Complete(42);
            // Очередь завершается синхронно в детерминированных EditMode-тестах,
            // поэтому результат читаем без await (UTF 1.1.33 не поддерживает async Task-тесты).
            Assert.IsTrue(task.Status.IsCompletedSuccessfully());
            Assert.AreEqual(42, task.GetAwaiter().GetResult());
        }

        [Test]
        public void Error_Does_Not_Break_Queue()
        {
            var queue = new RequestQueue();
            var op1 = new FakeOperation<int>();
            var op2 = new FakeOperation<int>();
            var t1 = queue.EnqueueAsync(op1);
            var t2 = queue.EnqueueAsync(op2);

            op1.Fail(new InvalidOperationException("boom"));
            Assert.IsTrue(op2.Started, "Вторая исполняется несмотря на ошибку первой");

            op2.Complete(7);
            Assert.IsTrue(t2.Status.IsCompletedSuccessfully());
            Assert.AreEqual(7, t2.GetAwaiter().GetResult());
            Assert.IsTrue(t1.Status.IsFaulted());
        }

        [Test]
        public void Cancelling_Running_Operation_Cancels_And_Advances()
        {
            var queue = new RequestQueue();
            var cts = new CancellationTokenSource();
            var op1 = new FakeOperation<int>();
            var op2 = new FakeOperation<int>();

            var t1 = queue.EnqueueAsync(op1, cts.Token);
            _ = queue.EnqueueAsync(op2);

            cts.Cancel();

            Assert.IsTrue(op1.ObservedCancellation, "Операция получила отмену по токену");
            Assert.IsTrue(t1.Status.IsCanceled(), "Результат первого — Canceled");
            Assert.IsTrue(op2.Started, "Следующая стартует после отмены");
        }

        [Test]
        public void Cancelling_Pending_Operation_Removes_It()
        {
            var queue = new RequestQueue();
            var cts2 = new CancellationTokenSource();
            var op1 = new FakeOperation<int>();
            var op2 = new FakeOperation<int>();

            _ = queue.EnqueueAsync(op1);
            var t2 = queue.EnqueueAsync(op2, cts2.Token);

            Assert.IsFalse(op2.Started, "Вторая ещё Pending");
            cts2.Cancel();

            Assert.IsTrue(t2.Status.IsCanceled(), "Удалённый из очереди → Canceled");
            op1.Complete(1);
            Assert.IsFalse(op2.Started, "Удалённая операция не стартует");
        }

        [Test]
        public void Dispose_Cancels_Pending_And_Running()
        {
            var queue = new RequestQueue();
            var op1 = new FakeOperation<int>();
            var op2 = new FakeOperation<int>();
            var t1 = queue.EnqueueAsync(op1);
            var t2 = queue.EnqueueAsync(op2);

            queue.Dispose();

            Assert.IsTrue(t1.Status.IsCanceled(), "Выполнявшийся отменён при Dispose");
            Assert.IsTrue(t2.Status.IsCanceled(), "Ожидавший отменён при Dispose");
        }
    }
}
