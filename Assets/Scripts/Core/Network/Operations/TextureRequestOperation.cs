using System.Threading;
using Cysharp.Threading.Tasks;
using RequestQueueDemo.Core.Queue;
using UnityEngine;

namespace RequestQueueDemo.Core.Network.Operations
{
    public sealed class TextureRequestOperation : IRequestOperation<Texture2D>
    {
        private readonly IWebRequestRunner _runner;
        private readonly string _url;

        public TextureRequestOperation(IWebRequestRunner runner, string url)
        {
            _runner = runner; _url = url;
        }

        public UniTask<Texture2D> ExecuteAsync(CancellationToken ct) => _runner.GetTextureAsync(_url, ct);
    }
}
