using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateReportProfile
{
    public class CreateReportProfileCommandHandler : ICommandHandler<CreateReportProfileCommand, CreateReportProfileCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateReportProfileCommandHandler(fptforumCommandContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateReportProfileCommandResult>> Handle(CreateReportProfileCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportProfile = new ReportProfile();
            reportProfile.ReportProfileId = _helper.GenerateNewGuid();
            reportProfile.ReportTypeId = request.ReportTypeId;
            reportProfile.GroupId = request.GroupId;
            reportProfile.UserId = request.UserId;
            reportProfile.ReportById = (Guid) request.ReportById;
            reportProfile.ReportStatus = null;
            reportProfile.CreatedDate = DateTime.Now;
            reportProfile.Processing = true;

            await _context.ReportProfiles.AddAsync(reportProfile);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CreateReportProfileCommandResult>(reportProfile);
            return Result<CreateReportProfileCommandResult>.Success(result);
        }
    }
}
