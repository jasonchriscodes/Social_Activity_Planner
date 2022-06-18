using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
 public class List
 {
  public class Query : IRequest<Result<List<CommentDto>>>
  {
   public Guid ActivityId { get; set; }
  }

  public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
  {
   private readonly DataContext context;
   private readonly IMapper mapper;
   public Handler(DataContext context, IMapper mapper)
   {
    this.context = context;
    this.mapper = mapper;
   }

   public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
   {
    var comments = await this.context.Comments
    .Where(x => x.Activity.Id == request.ActivityId)
    .OrderBy(c => c.CreatedAt)
    .ProjectTo<CommentDto>(this.mapper.ConfigurationProvider)
    .ToListAsync();

    return Result<List<CommentDto>>.Success(comments);
   }
  }
 }
}