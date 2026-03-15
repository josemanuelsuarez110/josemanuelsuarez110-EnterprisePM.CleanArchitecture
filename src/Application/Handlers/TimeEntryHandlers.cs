using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ProjectManagementERP.Application.Commands.TimeEntries;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.DTOs.Reports;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Application.Queries.Reports;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Handlers
{
    public class LogTimeEntryCommandHandler : IRequestHandler<LogTimeEntryCommand, Result<TimeEntryDto>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LogTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository, ITaskRepository taskRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _timeEntryRepository = timeEntryRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TimeEntryDto>> Handle(LogTimeEntryCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                return Result<TimeEntryDto>.Fail("Task not found");
            }

            var entry = _mapper.Map<Domain.Entities.TimeEntry>(request);
            await _timeEntryRepository.AddAsync(entry, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<TimeEntryDto>.Ok(_mapper.Map<TimeEntryDto>(entry));
        }
    }

    public class GetProjectTimeReportQueryHandler : IRequestHandler<GetProjectTimeReportQuery, Result<ProjectTimeReportDto>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IProjectRepository _projectRepository;

        public GetProjectTimeReportQueryHandler(ITimeEntryRepository timeEntryRepository, IProjectRepository projectRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Result<ProjectTimeReportDto>> Handle(GetProjectTimeReportQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                return Result<ProjectTimeReportDto>.Fail("Project not found");
            }

            var entries = await _timeEntryRepository.GetByProjectAsync(request.ProjectId, cancellationToken);
            var grouped = entries.GroupBy(e => e.TaskId)
                .Select(g => new TaskTimeBreakdown
                {
                    TaskId = g.Key,
                    TaskTitle = project.Tasks.FirstOrDefault(t => t.Id == g.Key)?.Title ?? "",
                    Hours = g.Sum(e => e.Hours)
                }).ToList();

            var dto = new ProjectTimeReportDto
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                TotalHours = entries.Sum(e => e.Hours),
                Tasks = grouped
            };
            return Result<ProjectTimeReportDto>.Ok(dto);
        }
    }

    public class GetUserTimeReportQueryHandler : IRequestHandler<GetUserTimeReportQuery, Result<UserTimeReportDto>>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IProjectRepository _projectRepository;

        public GetUserTimeReportQueryHandler(ITimeEntryRepository timeEntryRepository, IProjectRepository projectRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _projectRepository = projectRepository;
        }

        public async Task<Result<UserTimeReportDto>> Handle(GetUserTimeReportQuery request, CancellationToken cancellationToken)
        {
            var entries = await _timeEntryRepository.GetByUserAsync(request.UserId, cancellationToken);
            var projectHours = new List<ProjectHours>();

            foreach (var group in entries.GroupBy(e => e.Task.ProjectId))
            {
                var project = await _projectRepository.GetByIdAsync(group.Key, cancellationToken);
                if (project == null) continue;

                projectHours.Add(new ProjectHours
                {
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    Hours = group.Sum(e => e.Hours)
                });
            }

            var dto = new UserTimeReportDto
            {
                UserId = request.UserId,
                TotalHours = entries.Sum(e => e.Hours),
                Projects = projectHours
            };

            return Result<UserTimeReportDto>.Ok(dto);
        }
    }
}
