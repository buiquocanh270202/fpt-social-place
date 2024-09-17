using Application.Commands.CreateGender;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportType
{
    public class CreateReportTypeCommandHandler : ICommandHandler<CreateReportTypeCommand, CreateReportTypeCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReportTypeCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateReportTypeCommandResult>> Handle(CreateReportTypeCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var listreporttype = await _querycontext.ReportTypes.FirstOrDefaultAsync(x => x.ReportTypeName.Equals(request.ReportTypeName));
            if (listreporttype != null)
            {
                throw new ErrorException(StatusCodeEnum.RT01_ReportType_Existed);
            }
            var reporttype = new Domain.CommandModels.ReportType();
            reporttype.ReportTypeId = _helper.GenerateNewGuid();
            reporttype.ReportTypeName = request.ReportTypeName;
            reporttype.CreatedAt = DateTime.Now;
            await _context.ReportTypes.AddAsync(reporttype);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateReportTypeCommandResult>(reporttype);
            return Result<CreateReportTypeCommandResult>.Success(reporttype);
        }
    }
}
