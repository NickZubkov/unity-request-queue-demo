using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RequestQueueDemo.Core.Network
{
    public sealed class UnityWebRequestRunner : IWebRequestRunner
    {
        private readonly IApiConfig _config;

        public UnityWebRequestRunner(IApiConfig config) => _config = config;

        public async UniTask<string> GetTextAsync(string url, CancellationToken ct)
        {
            using var request = UnityWebRequest.Get(url);
            ApplyHeaders(request);
            await SendAsync(request, ct);
            return request.downloadHandler.text;
        }

        public async UniTask<Texture2D> GetTextureAsync(string url, CancellationToken ct)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);
            ApplyHeaders(request);
            await SendAsync(request, ct);
            return DownloadHandlerTexture.GetContent(request);
        }

        private void ApplyHeaders(UnityWebRequest request)
        {
            if (!string.IsNullOrEmpty(_config.UserAgent))
                request.SetRequestHeader("User-Agent", _config.UserAgent);
            if (!string.IsNullOrEmpty(_config.AcceptHeader))
                request.SetRequestHeader("Accept", _config.AcceptHeader);
        }

        private static async UniTask SendAsync(UnityWebRequest request, CancellationToken ct)
        {
            await request.SendWebRequest().WithCancellation(ct);
            if (request.result != UnityWebRequest.Result.Success)
                throw new WebRequestException((int)request.responseCode, request.error);
        }
    }
}
