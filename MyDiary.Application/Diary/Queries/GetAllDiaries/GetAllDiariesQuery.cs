using MediatR;
using MyDiary.Application.Common;
using MyDiary.Application.Diary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Queries.GetAllDiaries
{
    public class GetAllDiariesQuery : IRequest<PagedResult<DiaryDtos>>
    {
        public string? SearchPhrase { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
