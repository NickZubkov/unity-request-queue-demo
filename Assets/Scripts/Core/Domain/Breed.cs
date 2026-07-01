namespace RequestQueueDemo.Core.Domain
{
    public sealed class Breed
    {
        public string Id { get; }
        public string Name { get; }
        public Breed(string id, string name) { Id = id; Name = name; }
    }
}
