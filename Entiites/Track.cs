using MusicApi.Enums;
using System.Collections.Generic;

namespace MusicApi.Entiites
{
    public class Track : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string WrittenBy { get; set; }
        public string PerformedBy { get; set; }
        public string ArrengedBy { get; set; }
        public double Duration { get; set; }
        public TrackType Type { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
        public ICollection<TrackPlaylist> Playlists { get; set; } = new HashSet<TrackPlaylist>();
    }
}
