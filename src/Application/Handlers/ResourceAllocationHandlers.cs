using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ProjectManagementERP.Application.Commands.Resources;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Handlers
{
    public class AllocateResourceCommandHandler : IRequestHandler<AllocateResourceCommand, Result<ResourceAllocationDto>>
    {
        private readonly IResourceAllocationRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AllocateResourceCommandHandler(IResourceAllocationRepository repository, IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ResourceAllocationDto>> Handle(AllocateResourceCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                return Result<ResourceAllocationDto>.Fail("Project not found");
            }

            var existing = (await _repository.GetByProjectAsync(request.ProjectId, cancellationToken))
                .FirstOrDefault(r => r.UserId == request.UserId);
            if (existing != null)
            {
                existing.AllocationPercentage = request.AllocationPercentage;
                _repository.Update(existing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<ResourceAllocationDto>.Ok(_mapper.Map<ResourceAllocationDto>(existing));
            }
            else
            {
                var allocation = _mapper.Map<Domain.Entities.ResourceAllocation>(request);
                await _repository.AddAsync(allocation, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result<ResourceAllocationDto>.Ok(_mapper.Map<ResourceAllocationDto>(allocation));
            }
        }
    }

    public class UpdateResourceAllocationCommandHandler : IRequestHandler<UpdateResourceAllocationCommand, Result<ResourceAllocationDto>>
    {
        private readonly IResourceAllocationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateResourceAllocationCommandHandler(IResourceAllocationRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ResourceAllocationDto>> Handle(UpdateResourceAllocationCommand request, CancellationToken cancellationToken)
        {
            var allocation = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (allocation == null)
            {
                return Result<ResourceAllocationDto>.Fail("Allocation not found");
            }

            allocation.AllocationPercentage = request.AllocationPercentage;
            allocation.Touch();
            _repository.Update(allocation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ResourceAllocationDto>.Ok(_mapper.Map<ResourceAllocationDto>(allocation));
        }
    }
}
