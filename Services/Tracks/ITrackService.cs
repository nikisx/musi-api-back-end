using MusicApi.Common;
using MusicApi.JsonModels;
using System.Collections.Generic;

namespace MusicApi.Services.Tracks
{
    public interface ITrackService
    {
        CustomResponse<List<TrackJsonModel>> GetTracks();
        CustomResponse<List<TrackJsonModel>> CreateTrack(TrackJsonModel model, string currentUserId);
        CustomResponse<List<TrackJsonModel>> EditTrack(TrackJsonModel model, string currentUserId);
        CustomResponse<bool> DeleteTrack(int trackId, string currentUserId);
    }
}
