using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Common;
using MusicApi.JsonModels;
using MusicApi.Services.Albums;
using System.Collections.Generic;
using System.Security.Claims;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [Route("get-all")]
        public CustomResponse<List<AlbumJsonModel>> GetAll()
        {
            var result = this.albumService.GetAlbums();

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("create-album")]
        public CustomResponse<List<AlbumJsonModel>> CreateAlbum([FromBody] AlbumJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.albumService.CreateAlbum(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("edit-album")]
        public CustomResponse<List<AlbumJsonModel>> EditAlbum([FromBody] AlbumJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.albumService.EditAlbum(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public CustomResponse<bool> DeleteAlbum([FromBody] AlbumJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.albumService.DeleteAlbum(model.Id, userId);

            return result;
        }
    }
}
