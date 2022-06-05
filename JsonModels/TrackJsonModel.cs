namespace MusicApi.JsonModels
{
    public class TrackJsonModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string CreatedById { get; set; }
        public string WrittenBy { get; set; }
        public string PerformedBy { get; set; }
        public string ArrengedBy { get; set; }
        public double Duration { get; set; }
        public int Type { get; set; }

        public string TrackType { get; set; }
    }
}
