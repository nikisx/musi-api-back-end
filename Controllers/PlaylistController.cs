using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Common;
using MusicApi.JsonModels;
using MusicApi.Services.Playlists;
using System.Collections.Generic;
using System.Security.Claims;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private IPlayListService playListService;

        public PlaylistController(IPlayListService playListService)
        {
            this.playListService = playListService;
        }

        [Route("get-all")]
        public CustomResponse<List<PlayListJsonModel>> GetAll()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = this.playListService.GetPlayLists(userId);

            return result;
        }

        [Route("get-playlist")]
        public CustomResponse<PlayListJsonModel> GetPlaylist([FromQuery] int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = this.playListService.GetPlaylist(id, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("create-playlist")]
        public CustomResponse<List<PlayListJsonModel>> CreatePlaylist([FromBody] PlayListJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.CreatePlayList(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("edit-playlist")]
        public CustomResponse<List<PlayListJsonModel>> EditPlaylist([FromBody] PlayListJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.EditPlayList(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public CustomResponse<bool> Delete([FromBody] PlayListJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.DeletePlayList(model.Id, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("add-track-to-playlist")]
        public CustomResponse<bool> AddTrack([FromForm] int trackId, [FromForm] int playlistId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.AddTrack(trackId, playlistId, userId);

            return result;
        }


        [Authorize]
        [HttpPost]
        [Route("change-track-position")]
        public CustomResponse<PlayListJsonModel> ChangeTrackPosition([FromForm] int trackId, [FromForm] int playlistId, [FromForm] int position)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.ChangeTrackPosition(trackId, playlistId,position, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("remove-track")]
        public CustomResponse<bool> RemoveTackFromPlaylist([FromForm] int trackId, [FromForm] int playlistId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.playListService.RemoveTrackFromPlaylist(trackId, playlistId, userId);

            return result;
        }
    }
}
