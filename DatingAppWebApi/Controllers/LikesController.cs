using AutoMapper;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppWebApi.Controllers
{
    [Authorize]
    public class LikesController(ILikesRepository likesRepository, IMapper mapper) : BaseController
    {
        


        [HttpPost("{targetUserId}")]
        public async Task<IActionResult> ToggleLike(string targetUserId) { 
        var sourceUserId = User.GetUserId();
            if(sourceUserId==targetUserId) return BadRequest("You cannot like yourself");
            var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);
            if (existingLike == null) {
                var userLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };
                likesRepository.AddLike(userLike);
            }
            else
            {
                likesRepository.RemoveLike(existingLike);
            }
            if (await likesRepository.SaveAllChanges())
            {
                return Ok();
            }
            return BadRequest("Failed to update like status");


        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentUserLikeIds() { 
        
        return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetUserDTO>>> GetUserLikes([FromQuery]LikesParams likesParams) {
            likesParams.UserId = User.GetUserId();
            var users = await likesRepository.GetUserLikes(likesParams);
            var mappedUsers = mapper.Map<IReadOnlyList<GetUserDTO>>(users.Items);
            var usersToReturn = new PaginatedResult<GetUserDTO>
            {
                Metadata = users.Metadata,
                Items = mappedUsers.ToList()
            };

            return Ok(usersToReturn);
        
        }
    }
}
