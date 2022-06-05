using MusicApi.Common;
using MusicApi.JsonModels;
using System.Collections.Generic;

namespace MusicApi.Services.Tracks
{
    public interface ITrackService
    {
        Response<List<TrackJsonModel>> GetTracks();
        Response<List<TrackJsonModel>> CreateTrack(TrackJsonModel model, string currentUserId);
        Response<List<TrackJsonModel>> EditTrack(TrackJsonModel model, string currentUserId);
        Response<bool> DeleteTrack(int trackId, string currentUserId);
    }
}