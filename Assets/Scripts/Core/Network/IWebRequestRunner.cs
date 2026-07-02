using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RequestQueueDemo.Core.Network
{
    public interface IWebRequestRunner
    {
        public UniTask<string> GetTextAsync(string url, CancellationToken ct, string accept = null);
        public UniTask<Texture2D> GetTextureAsync(string url, CancellationToken ct);
    }
}
