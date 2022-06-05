using System.Collections.Generic;

namespace MusicApi.JsonModels
{
    public class PlayListJsonModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }
        public int TrackCount { get; set; }

        public double Duration { get; set; }
        public string CreatedById { get; set; }


        public IEnumerable<TrackJsonModel> Tracks { get; set; }
    }
}
