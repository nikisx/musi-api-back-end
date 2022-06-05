using Microsoft.EntityFrameworkCore;
using MusicApi.Authentication;
using MusicApi.Common;
using MusicApi.Entiites;
using MusicApi.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicApi.Services.Albums
{
    public class AlbumService : IAlbumService
    {
        private ApplicationDbContext dbContext;

        public AlbumService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CustomResponse<List<AlbumJsonModel>> CreateAlbum(AlbumJsonModel model, string currentUserId)
        {
            var newAlbum = new Album
            {
                Name = model.Name,
                Year = model.Year,
                ImageUrl = model.ImageUrl,
            };

            dbContext.Albums.Add(newAlbum);

            dbContext.SaveChanges(currentUserId);

            return GetAlbums();
        }

        public CustomResponse<bool> DeleteAlbum(int albumId, string currentUserId)
        {
            var album = dbContext.Albums
                .Include(x => x.Tracks).FirstOrDefault(a => a.Id == albumId);

            foreach (var t in album.Tracks)
            {
                t.AlbumId = null;
            }

            if (album == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid album Id",
                };
            }

            dbContext.Albums.Remove(album);
            dbContext.SaveChanges(currentUserId);

            return new CustomResponse<bool>
            {
                Success = true,
            };
        }

        public CustomResponse<List<AlbumJsonModel>> EditAlbum(AlbumJsonModel model, string currentUserId)
        {
            var album = dbContext.Albums.FirstOrDefault(a => a.Id == model.Id);

            if (album == null)
            {
                return new CustomResponse<List<AlbumJsonModel>>
                {
                    Message = "Invalid album Id",
                };
            }

            album.Name = model.Name;
            album.Year = model.Year;
            album.ImageUrl = model.ImageUrl;

            dbContext.SaveChanges(currentUserId);
            return GetAlbums();
        }

        public CustomResponse<List<AlbumJsonModel>> GetAlbums()
        {
            var albums = dbContext.Albums
                .Include(x => x.Tracks)
                .OrderByDescending(a => a.Created)
                .Select(a => new AlbumJsonModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Duration = Math.Round(a.Duration, 2),
                    ImageUrl = a.ImageUrl,
                    Tracks = a.Tracks.Select(t => new TrackJsonModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Duration = t.Duration,
                    }),
                    CreatedById = a.CreatedById,
                    Year = a.Year,
                    
                }).ToList();

            return new CustomResponse<List<AlbumJsonModel>>
            {
                Data = albums,
                Success = true,
            };
        }
    }
}
