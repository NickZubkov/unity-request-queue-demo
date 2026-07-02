using System.Collections.Generic;
using UnityEngine;

namespace RequestQueueDemo.Core.Network
{
    public sealed class TextureCache : ITextureCache
    {
        private const int MaxEntries = 64;

        private readonly Dictionary<string, Texture2D> _cache = new();
        private readonly Queue<string> _order = new();

        public bool TryGet(string url, out Texture2D texture) => _cache.TryGetValue(url, out texture);

        public void Set(string url, Texture2D texture)
        {
            if (_cache.ContainsKey(url))
            {
                _cache[url] = texture;
                return;
            }

            _cache[url] = texture;
            _order.Enqueue(url);

            if (_order.Count <= MaxEntries) return;

            var oldest = _order.Dequeue();
            if (_cache.TryGetValue(oldest, out var evicted))
            {
                _cache.Remove(oldest);
                if (evicted != null) Object.Destroy(evicted);
            }
        }
    }
}
