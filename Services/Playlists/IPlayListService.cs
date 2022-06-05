using MusicApi.Common;
using MusicApi.JsonModels;
using System.Collections.Generic;

namespace MusicApi.Services.Playlists
{
    public interface IPlayListService
    {
        CustomResponse<List<PlayListJsonModel>> GetPlayLists(string userId);
        CustomResponse<List<PlayListJsonModel>> CreatePlayList(PlayListJsonModel model, string currentUserId);
        CustomResponse<List<PlayListJsonModel>> EditPlayList(PlayListJsonModel model, string currentUserId);
        CustomResponse<bool> DeletePlayList(int albumId, string currentUserId);

        CustomResponse<bool> AddTrack(int trackId, int playlistId, string currentUserId);

        CustomResponse<bool> RemoveTrackFromPlaylist(int trackId, int playlistId, string currentUserId);

        CustomResponse<PlayListJsonModel> GetPlaylist(int playlistId, string currentUserId);

        CustomResponse<PlayListJsonModel> ChangeTrackPosition(int trackId, int playlistId, int positiom, string currentUserId);
    }
}
