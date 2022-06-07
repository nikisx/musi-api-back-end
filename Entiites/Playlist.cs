using System.Collections.Generic;

namespace MusicApi.Entiites
{
    public class Playlist : BaseEntity
    {
        public Playlist()
        {
            this.IsPublic = true;
        }
        public int Id { get; set; }

        public string Name { get; set; }
        
        public double Duration { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<TrackPlaylist> Tracks { get; set; } = new HashSet<TrackPlaylist>();
    }
}
