namespace RequestQueueDemo.App.Features.Clicker
{
    public readonly struct TapResult
    {
        public bool Success { get; }
        public long Reward { get; }

        private TapResult(bool success, long reward)
        {
            Success = success;
            Reward = reward;
        }

        public static TapResult Collected(long reward) => new(true, reward);
        public static TapResult NoEnergy => new(false, 0);
    }
}
