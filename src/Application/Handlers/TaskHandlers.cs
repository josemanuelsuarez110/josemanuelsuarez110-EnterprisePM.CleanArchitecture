using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ProjectManagementERP.Application.Commands.Tasks;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Application.Queries.Tasks;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                return Result<TaskDto>.Fail("Project not found");
            }

            var entity = _mapper.Map<Domain.Entities.TaskItem>(request);
            await _taskRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<TaskDto>.Ok(_mapper.Map<TaskDto>(entity));
        }
    }

    public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                return Result.Fail("Task not found");
            }

            task.AssignedUserId = request.UserId;
            task.Touch();
            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }

    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Result>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                return Result.Fail("Task not found");
            }

            task.Status = request.Status;
            task.Touch();
            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }

    public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, Result<PagedResult<TaskDto>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTasksByProjectQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<TaskDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var paged = await _taskRepository.GetByProjectAsync(request.ProjectId, request.Page, request.PageSize, cancellationToken);
            var dto = new PagedResult<TaskDto>
            {
                Items = _mapper.Map<System.Collections.Generic.IEnumerable<TaskDto>>(paged.Items),
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };
            return Result<PagedResult<TaskDto>>.Ok(dto);
        }
    }
}
