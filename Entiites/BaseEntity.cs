using System;

namespace MusicApi.Entiites
{
    public class BaseEntity
    {
        public string CreatedById { get; set; }
        public DateTime? Created { get; set; }
    }
}
