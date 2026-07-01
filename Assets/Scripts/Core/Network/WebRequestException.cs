using System;

namespace RequestQueueDemo.Core.Network
{
    public sealed class WebRequestException : Exception
    {
        public int Code { get; }
        public WebRequestException(int code, string message) : base($"HTTP {code}: {message}") => Code = code;
    }
}
