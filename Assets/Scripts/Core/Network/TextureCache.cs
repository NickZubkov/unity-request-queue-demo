using System.Collections.Generic;
using UnityEngine;

namespace RequestQueueDemo.Core.Network
{
    // Тонкое хранилище по URL. Загрузку оркестрирует презентер (через очередь + TextureRequestOperation).
    public sealed class TextureCache : ITextureCache
    {
        private readonly Dictionary<string, Texture2D> _cache = new();
        public bool TryGet(string url, out Texture2D texture) => _cache.TryGetValue(url, out texture);
        public void Set(string url, Texture2D texture) => _cache[url] = texture;
    }
}
