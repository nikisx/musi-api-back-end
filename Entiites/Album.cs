using System.Collections.Generic;

namespace MusicApi.Entiites
{
    public class Album : BaseEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
    }
}
