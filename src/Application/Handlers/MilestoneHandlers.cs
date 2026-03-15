using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ProjectManagementERP.Application.Commands.Milestones;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Application.Queries.Projects;
using ProjectManagementERP.Shared.Utilities;
using System.Linq;

namespace ProjectManagementERP.Application.Handlers
{
    public class CreateMilestoneCommandHandler : IRequestHandler<CreateMilestoneCommand, Result<MilestoneDto>>
    {
        private readonly IMilestoneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMilestoneCommandHandler(IMilestoneRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MilestoneDto>> Handle(CreateMilestoneCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Milestone>(request);
            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<MilestoneDto>.Ok(_mapper.Map<MilestoneDto>(entity));
        }
    }

    public class UpdateMilestoneCommandHandler : IRequestHandler<UpdateMilestoneCommand, Result<MilestoneDto>>
    {
        private readonly IMilestoneRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMilestoneCommandHandler(IMilestoneRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MilestoneDto>> Handle(UpdateMilestoneCommand request, CancellationToken cancellationToken)
        {
            var milestone = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (milestone == null)
            {
                return Result<MilestoneDto>.Fail("Milestone not found");
            }

            milestone.Name = request.Name;
            milestone.DueDate = request.DueDate;
            milestone.Touch();
            _repository.Update(milestone);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<MilestoneDto>.Ok(_mapper.Map<MilestoneDto>(milestone));
        }
    }

    public class GetProjectTimelineQueryHandler : IRequestHandler<GetProjectTimelineQuery, Result<IEnumerable<MilestoneDto>>>
    {
        private readonly IMilestoneRepository _repository;
        private readonly IMapper _mapper;

        public GetProjectTimelineQueryHandler(IMilestoneRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MilestoneDto>>> Handle(GetProjectTimelineQuery request, CancellationToken cancellationToken)
        {
            var milestones = await _repository.GetByProjectAsync(request.ProjectId, cancellationToken);
            var dto = _mapper.Map<IEnumerable<MilestoneDto>>(milestones);
            return Result<IEnumerable<MilestoneDto>>.Ok(dto);
        }
    }
}
