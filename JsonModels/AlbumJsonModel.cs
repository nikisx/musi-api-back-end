using System.Collections.Generic;

namespace MusicApi.JsonModels
{
    public class AlbumJsonModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double Duration { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string CreatedById { get; set; }

        public IEnumerable<TrackJsonModel> Tracks { get; set; }
    }
}
