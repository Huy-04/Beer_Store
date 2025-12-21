namespace Api.Core.Logging
{
    public sealed class SerilogOptions
    {
        public string BeTextPath { get; set; } = "logs/be/beerstore-be-.txt";
        public string FeJsonPath { get; set; } = "logs/fe/beerstore-fe-.clef";
        public int? RetainedFileCountLimit { get; set; } = 30;
        public string? TextTemplate { get; set; }
    }
}