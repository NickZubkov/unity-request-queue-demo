namespace RequestQueueDemo.Core.Domain
{
    public sealed class BreedFacts
    {
        public string Name { get; }
        public string Description { get; }
        public BreedFacts(string name, string description) { Name = name; Description = description; }
    }
}
