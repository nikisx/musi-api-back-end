namespace MusicApi.Entiites
{
    public class TrackPlaylist : BaseEntity
    {
        public int Id { get; set; }
        public int TrackPosition { get; set; }

        public int TrackId { get; set; }
        public Track Track { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
