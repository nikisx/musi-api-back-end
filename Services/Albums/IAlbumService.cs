using MusicApi.Common;
using MusicApi.JsonModels;
using System.Collections.Generic;

namespace MusicApi.Services.Albums
{
    public interface IAlbumService
    {
        CustomResponse<List<AlbumJsonModel>> GetAlbums();
        CustomResponse<List<AlbumJsonModel>> CreateAlbum(AlbumJsonModel model, string currentUserId);
        CustomResponse<List<AlbumJsonModel>> EditAlbum(AlbumJsonModel model, string currentUserId);
        CustomResponse<bool> DeleteAlbum(int albumId, string currentUserId);
    }
}
