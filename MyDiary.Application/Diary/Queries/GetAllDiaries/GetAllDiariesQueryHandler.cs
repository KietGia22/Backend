using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyDiary.Application.Common;
using MyDiary.Application.Diary.Dtos;
using MyDiary.Domain.Repositories;
using System.Security.Claims;

namespace MyDiary.Application.Diary.Queries.GetAllDiaries
{
    public class GetAllDiariesQueryHandler(ILogger<GetAllDiariesQueryHandler> logger, 
        IDiaryRepository diaryRepository, 
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetAllDiariesQuery, PagedResult<DiaryDtos>>
    {
        public async Task<PagedResult<DiaryDtos>> Handle(GetAllDiariesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all diaries by userId");

            var user = httpContextAccessor?.HttpContext?.User;
            string userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var (diaries, totalCount) = await diaryRepository.GetAllMatchingAsync(query.SearchPhrase,
                    query.PageSize,
                    query.PageNumber,
                    userId
                );

            var diaryDtoList = mapper.Map<IEnumerable<DiaryDtos>>(diaries);

            var result = new PagedResult<DiaryDtos>(diaryDtoList, totalCount, query.PageSize, query.PageNumber);
            return result;
        }
    }
}
