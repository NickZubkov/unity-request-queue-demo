using UnityEngine;

namespace RequestQueueDemo.Core.Network
{
    public interface ITextureCache
    {
        public bool TryGet(string url, out Texture2D texture);
        public void Set(string url, Texture2D texture);
    }
}
