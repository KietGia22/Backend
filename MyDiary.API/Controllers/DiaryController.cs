using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyDiary.API.Factories;
using MyDiary.API.Models;
using MyDiary.Application.Common;
using MyDiary.Application.Diary.Commands.CreateDiary;
using MyDiary.Application.Diary.Commands.DeleteDiary;
using MyDiary.Application.Diary.Commands.UpdateDiary;
using MyDiary.Application.Diary.Dtos;
using MyDiary.Application.Diary.Queries.GetAllDiaries;
using MyDiary.Application.Diary.Queries.GetDiaryById;
using System.Net;
using System.Security.Claims;

namespace MyDiary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiaryController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{diaryId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse<DiaryDtos>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiaryById([FromRoute] Guid diaryId)
        {
            var result = await mediator.Send(new GetDiaryByIdQuery(diaryId));
            return Ok(APIResponseFactory.Create<DiaryDtos>(HttpStatusCode.OK, true, result));
        }

        [HttpGet("all-diaries")]
        [Authorize]
        [ProducesResponseType(typeof(APIResponse<PagedResult<DiaryDtos>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <IActionResult> GetAllDiaries([FromQuery] GetAllDiariesQuery query)
        {
            var diaries = await mediator.Send(query);
            return Ok(APIResponseFactory.Create<PagedResult<DiaryDtos>>(HttpStatusCode.OK, true, diaries));
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<PagedResult<DiaryDtos>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDiary(CreateDiaryCommand request)
        {
            Guid diaryId = await mediator.Send(request);
            var result = APIResponseFactory.Create(HttpStatusCode.Created, true, new {Diary = diaryId});
            return CreatedAtAction(nameof(GetDiaryById), new { diaryId }, result);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDiary(UpdateDiaryCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{diaryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDiary([FromRoute] Guid diaryId)
        {
            await mediator.Send(new DeleteDiaryCommand(diaryId));
            return NoContent();
        }
    }
}
