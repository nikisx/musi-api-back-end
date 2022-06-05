using Microsoft.EntityFrameworkCore;
using MusicApi.Authentication;
using MusicApi.Common;
using MusicApi.Entiites;
using MusicApi.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicApi.Services.Playlists
{
    public class PlayListService : IPlayListService
    {
        private ApplicationDbContext dbContext;

        public PlayListService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CustomResponse<bool> AddTrack(int trackId, int playlistId, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists
                .Include(x => x.Tracks)
                .FirstOrDefault(x => x.Id == playlistId);

            if (currentPlaylist == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid playlist id",
                };
            }

            var trackInPlaylist = currentPlaylist.Tracks.Any(x => x.TrackId == trackId);

            if (trackInPlaylist)
            {
                return new CustomResponse<bool>
                {
                    Message = "Track already added in playlist",
                };
            }

            var track = dbContext.Tracks.FirstOrDefault(x => x.Id == trackId);

            if (track == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid track id",
                };
            }

            if (currentPlaylist.Duration == 200 || currentPlaylist.Duration + track.Duration > 200)
            {
                return new CustomResponse<bool>
                {
                    Message = "Playlist cannot be longer than 2 hours",
                };
            }

            var trackPlaylist = new TrackPlaylist
            {
                TrackId = trackId,
                PlaylistId = playlistId,
                TrackPosition = currentPlaylist.Tracks.Count,
            
            };

            currentPlaylist.Duration += track.Duration;

            dbContext.TrackPlaylists.Add(trackPlaylist);
            dbContext.SaveChanges(currentUserId);


            return new CustomResponse<bool>
            {
                Success = true,
            };
        }

        public CustomResponse<PlayListJsonModel> ChangeTrackPosition(int trackId, int playlistId, int positiom, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists
                .Include(x => x.Tracks)
                .FirstOrDefault(x => x.Id == playlistId);

            if (currentPlaylist == null)
            {
                return new CustomResponse<PlayListJsonModel>
                {
                    Message = "Invalid playlist id",
                };
            }

            var trackInPlaylist = currentPlaylist.Tracks.Any(x => x.TrackId == trackId);

            if (!trackInPlaylist)
            {
                return new CustomResponse<PlayListJsonModel>
                {
                    Message = "Track not added in playlist",
                };
            }

            var currentTrack = currentPlaylist.Tracks.FirstOrDefault(x => x.TrackId == trackId);
            var otherTrack = currentPlaylist.Tracks.FirstOrDefault(x => x.TrackPosition == positiom);

            if (currentTrack.TrackPosition > otherTrack.TrackPosition)
            {
                otherTrack.TrackPosition = otherTrack.TrackPosition + 1;
                        
            }
            else
            {
                otherTrack.TrackPosition = otherTrack.TrackPosition - 1;
            }

            currentTrack.TrackPosition = positiom;

            dbContext.SaveChanges(currentUserId);

            return GetPlaylist(playlistId, currentUserId);
        }

        public CustomResponse<List<PlayListJsonModel>> CreatePlayList(PlayListJsonModel model, string currentUserId)
        {
            var newPlaylist = new Playlist()
            {
                Name = model.Name,
                IsPublic = model.IsPublic,
            };

            dbContext.Playlists.Add(newPlaylist);

            dbContext.SaveChanges(currentUserId);

            return GetPlayLists(currentUserId);
        }

        public CustomResponse<bool> DeletePlayList(int albumId, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists.FirstOrDefault(x => x.Id == albumId);

            if (currentPlaylist == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid playlist id",
                };
            }

            dbContext.Playlists.Remove(currentPlaylist);
            dbContext.SaveChanges(currentUserId);

            return new CustomResponse<bool>
            {
                Success = true, 
            };
        }

        public CustomResponse<List<PlayListJsonModel>> EditPlayList(PlayListJsonModel model, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists.FirstOrDefault(x => x.Id == model.Id);

            if (currentPlaylist == null)
            {
                return new CustomResponse<List<PlayListJsonModel>>
                {
                    Message = "Invalid playlist id",
                };
            }

            currentPlaylist.IsPublic = model.IsPublic;
            currentPlaylist.Name = model.Name;

            dbContext.SaveChanges(currentUserId);

            return GetPlayLists(currentUserId);
        }

        public CustomResponse<PlayListJsonModel> GetPlaylist(int playlistId, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists
                .Include(x => x.Tracks)
                .ThenInclude(x => x.Track)
                .FirstOrDefault(x => x.Id == playlistId);

            if (currentPlaylist == null)
            {
                return new CustomResponse<PlayListJsonModel>
                {
                    Message = "Invalid playlist id",
                };
            }

            var res = new PlayListJsonModel
            {
                Id = currentPlaylist.Id,
                Name = currentPlaylist.Name,
                CreatedById = currentPlaylist.CreatedById,
                Duration = Math.Round(currentPlaylist.Duration, 2),
                IsPublic = currentPlaylist.IsPublic,
                Tracks = currentPlaylist.Tracks.OrderBy(x => x.TrackPosition).Select(x => new TrackJsonModel{
                    Id = x.TrackId,
                    Name = x.Track.Name,
                    Duration = x.Track.Duration,
                    WrittenBy = x.Track.WrittenBy,
                    ArrengedBy = x.Track.ArrengedBy,
                    PerformedBy = x.Track.PerformedBy,
                    Type = (int)x.Track.Type,
                    TrackType = x.Track.Type.ToString(),
                }).ToList()
            };

            return new CustomResponse<PlayListJsonModel>
            {
                Success = true,
                Data = res,
            };
        }

        public CustomResponse<List<PlayListJsonModel>> GetPlayLists(string userId)
        {
            var res = dbContext.Playlists
                .Include(x => x.Tracks)
                .OrderByDescending(x => x.Created)
                .Where(x => x.IsPublic || x.CreatedById == userId)
                .Select(x => new PlayListJsonModel
                {
                    Name = x.Name,
                    IsPublic = x.IsPublic,
                    Id = x.Id,
                    Duration = Math.Round(x.Duration, 2),
                    TrackCount = x.Tracks.Count,
                    CreatedById = x.CreatedById,
                }).ToList();

            return new CustomResponse<List<PlayListJsonModel>>
            {
                Data = res,
                Success = true,
            };
        }

        public CustomResponse<bool> RemoveTrackFromPlaylist(int trackId, int playlistId, string currentUserId)
        {
            var currentPlaylist = dbContext.Playlists
               .Include(x => x.Tracks)
               .FirstOrDefault(x => x.Id == playlistId);

            if (currentPlaylist == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid playlist id",
                };
            }

            var trackInPlaylist = currentPlaylist.Tracks.Any(x => x.TrackId == trackId);

            if (!trackInPlaylist)
            {
                return new CustomResponse<bool>
                {
                    Message = "Track not added in playlist",
                };
            }

            var track = dbContext.Tracks.FirstOrDefault(x => x.Id == trackId);

            if (track == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid track id",
                };
            }

           var trackPlaylist = dbContext.TrackPlaylists.FirstOrDefault(x => x.TrackId == trackId && x.PlaylistId == playlistId);
            dbContext.Remove(trackPlaylist);

            currentPlaylist.Duration -= track.Duration;

            dbContext.SaveChanges(currentUserId);

            return new CustomResponse<bool>()
            {
                Success = true,
            };
        }
    }
}
