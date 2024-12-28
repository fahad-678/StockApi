using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Extensions;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using api.Models;

namespace api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentsDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            return comment == null ? NotFound() : Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentRequestDto commentDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _stockRepo.StockExists(stockId)) return BadRequest("Stock does not exist");

            var comment = commentDto.ToCommentFromCreate(stockId, user);
            await _commentRepo.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var commentModel = await _commentRepo.GetByIdAsync(id);
            if (commentModel == null) return NotFound();
            if (commentModel.AppUserId != user.Id) return BadRequest("Unauthorized");

            var comment = await _commentRepo.UpdateAsync(id, commentDto.ToCommentFromUpdate());

            return comment == null ? NotFound() : Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            var commentModel = await _commentRepo.GetByIdAsync(id);
            if (commentModel == null) return NotFound();
            if (commentModel.AppUserId != user.Id) return BadRequest("Unauthorized");

            var comment = await _commentRepo.DeleteAsync(id);

            return comment == null ? NotFound() : NoContent();
        }
    }
}