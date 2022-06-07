using Microsoft.EntityFrameworkCore;
using MusicApi.Authentication;
using MusicApi.Common;
using MusicApi.Entiites;
using MusicApi.JsonModels;
using System.Collections.Generic;
using System.Linq;

namespace MusicApi.Services.Tracks
{
    public class TrackService : ITrackService
    {
        private ApplicationDbContext dbContext;

        public TrackService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CustomResponse<List<TrackJsonModel>> CreateTrack(TrackJsonModel model, string currentUserId)
        {
            var trackType = (Enums.TrackType)model.Type;

            var newTrack = new Track()
            {
                Name = model.Name,
                ArrengedBy = model.ArrengedBy,
                WrittenBy = model.WrittenBy,
                PerformedBy = model.PerformedBy,
                Duration = model.Duration,
                Type = trackType,
            };

            dbContext.Tracks.Add(newTrack);
            dbContext.SaveChanges(currentUserId);

            return GetTracks();
        }

        public CustomResponse<bool> DeleteTrack(int trackId, string currentUserId)
        {
            var track = dbContext.Tracks.FirstOrDefault(t => t.Id == trackId);

            if (track == null)
            {
                return new CustomResponse<bool>
                {
                    Message = "Invalid track id!",
                    Success = false,
                };
            }

            dbContext.Tracks.Remove(track);
            dbContext.SaveChanges(currentUserId);

            return new CustomResponse<bool>
            {
                Message = "Success",
                Success = true,
            };
        }

        public CustomResponse<List<TrackJsonModel>> EditTrack(TrackJsonModel model, string currentUserId)
        {
           var track = dbContext.Tracks.FirstOrDefault(t => t.Id == model.Id);

            if (track == null)
            {
                return new CustomResponse<List<TrackJsonModel>>
                {
                    Status = "Failed",
                    Message = "Invalid Track Id!"
                };
            }

            var trackType = (Enums.TrackType)model.Type;

            track.Name = model.Name;
            track.WrittenBy = model.WrittenBy;
            track.ArrengedBy = model.ArrengedBy;
            track.PerformedBy = model.PerformedBy;
            track.Duration = model.Duration;
            track.Type = trackType;

            dbContext.SaveChanges(currentUserId);

            return GetTracks();

        }

        public CustomResponse<List<TrackJsonModel>> GetTracks()
        {
            var data = dbContext.Tracks
                .Include(x => x.Album)
                .OrderByDescending(x => x.Created)
                .Select(x => new TrackJsonModel
                {
                    Name = x.Name,
                    WrittenBy = x.WrittenBy,
                    ArrengedBy = x.ArrengedBy,
                    PerformedBy = x.PerformedBy,
                    Id = x.Id,
                    Duration = x.Duration,
                    Type = (int)x.Type,
                    TrackType = x.Type.ToString(),
                    CreatedById = x.CreatedById,
                }).ToList();

            return new CustomResponse<List<TrackJsonModel>>
            {
                Data = data,
                Status = "Success",
                Success = true,
            };
        }
    }
}
