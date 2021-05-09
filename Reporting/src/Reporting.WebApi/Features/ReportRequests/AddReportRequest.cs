namespace Reporting.WebApi.Features.ReportRequests
{
    using Reporting.Core.Entities;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Exceptions;
    using Reporting.Infrastructure.Contexts;
    using Reporting.WebApi.Features.ReportRequests.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using MassTransit;
    using Messages;

    public class AddReportRequest
    {
        public class AddReportRequestCommand : IRequest<ReportRequestDto>
        {
            public ReportRequestForCreationDto ReportRequestToAdd { get; set; }

            public AddReportRequestCommand(ReportRequestForCreationDto reportRequestToAdd)
            {
                ReportRequestToAdd = reportRequestToAdd;
            }
        }

        public class CustomCreateReportRequestValidation : ReportRequestForManipulationDtoValidator<ReportRequestForCreationDto>
        {
            public CustomCreateReportRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<AddReportRequestCommand, ReportRequestDto>
        {
            private readonly ReportingDbContext _db;
            private readonly IMapper _mapper;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ReportingDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper;
                _db = db;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<ReportRequestDto> Handle(AddReportRequestCommand request, CancellationToken cancellationToken)
            {
                if (await _db.ReportRequests.AnyAsync(r => r.ReportRequestId == request.ReportRequestToAdd.ReportRequestId))
                {
                    throw new ConflictException("ReportRequest already exists with this primary key.");
                }

                var reportRequest = _mapper.Map<ReportRequest>(request.ReportRequestToAdd);
                _db.ReportRequests.Add(reportRequest);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    var message = new
                    {
                        ReportId = reportRequest.ReportRequestId,
                        reportRequest.Provider,
                        reportRequest.Target,
                        reportRequest.IsPublic
                    };
                    await _publishEndpoint.Publish<ISendReportRequest>(message);

                    return await _db.ReportRequests
                        .ProjectTo<ReportRequestDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(r => r.ReportRequestId == reportRequest.ReportRequestId);
                }
                else
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }
            }
        }
    }
}