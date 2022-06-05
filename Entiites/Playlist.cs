﻿using System.Collections.Generic;

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

        public bool IsPublic { get; set; }

        ICollection<TrackPlaylist> Tracks { get; set; } = new HashSet<TrackPlaylist>();
    }
}