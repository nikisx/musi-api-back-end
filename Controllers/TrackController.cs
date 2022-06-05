using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Common;
using MusicApi.JsonModels;
using MusicApi.Services.Tracks;
using System.Collections.Generic;
using System.Security.Claims;

namespace MusicApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService trackService;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }


        [Route("get-all")]
        public Response<List<TrackJsonModel>> GetAll()
        {
            var result = this.trackService.GetTracks();

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("create-track")]
        public Response<List<TrackJsonModel>> CreateTrack([FromBody] TrackJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.trackService.CreateTrack(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("edit-track")]
        public Response<List<TrackJsonModel>> EditTrack([FromBody] TrackJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.trackService.EditTrack(model, userId);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("delete")]
        public Response<bool> DeleteTrack([FromBody] TrackJsonModel model)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = this.trackService.DeleteTrack(model.Id, userId);

            return result;
        }
    }
}
