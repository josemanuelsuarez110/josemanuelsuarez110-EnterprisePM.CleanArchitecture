using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ProjectManagementERP.Application.Commands.Projects;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.Interfaces.Repositories;
using ProjectManagementERP.Application.Interfaces.Services;
using ProjectManagementERP.Application.Queries.Projects;
using ProjectManagementERP.Application.Utilities;
using ProjectManagementERP.Domain.Entities;
using ProjectManagementERP.Shared.Utilities;

namespace ProjectManagementERP.Application.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
    {
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public CreateProjectCommandHandler(IProjectRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Project>(request);
            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.ProjectList());
            return Result<ProjectDto>.Ok(_mapper.Map<ProjectDto>(entity));
        }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
    {
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public UpdateProjectCommandHandler(IProjectRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (project == null)
            {
                return Result<ProjectDto>.Fail("Project not found");
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.Status = request.Status;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.Touch();

            _repository.Update(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Project(project.Id));
            await _cache.RemoveAsync(CacheKeys.ProjectList());

            return Result<ProjectDto>.Ok(_mapper.Map<ProjectDto>(project));
        }
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
    {
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public DeleteProjectCommandHandler(IProjectRepository repository, IUnitOfWork unitOfWork, ICacheService cache)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (project == null)
            {
                return Result.Fail("Project not found");
            }

            _repository.Delete(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Project(project.Id));
            await _cache.RemoveAsync(CacheKeys.ProjectList());
            return Result.Ok();
        }
    }

    public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Result>
    {
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public ArchiveProjectCommandHandler(IProjectRepository repository, IUnitOfWork unitOfWork, ICacheService cache)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (project == null)
            {
                return Result.Fail("Project not found");
            }

            project.Archive();
            _repository.Update(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Project(project.Id));
            await _cache.RemoveAsync(CacheKeys.ProjectList());
            return Result.Ok();
        }
    }

    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Result<PagedResult<ProjectDto>>>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public GetProjectsQueryHandler(IProjectRepository repository, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<PagedResult<ProjectDto>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.ProjectList(request.Page, request.PageSize, request.Search, request.Status);
            if (request.UseCache)
            {
                var cached = await _cache.GetAsync<PagedResult<ProjectDto>>(cacheKey);
                if (cached != null)
                {
                    return Result<PagedResult<ProjectDto>>.Ok(cached);
                }
            }

            var paged = await _repository.GetPagedAsync(request.Page, request.PageSize, request.Search, request.Status, cancellationToken);
            var dto = new PagedResult<ProjectDto>
            {
                Items = _mapper.Map<System.Collections.Generic.IEnumerable<ProjectDto>>(paged.Items),
                Page = paged.Page,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount
            };

            if (request.UseCache)
            {
                await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
            }

            return Result<PagedResult<ProjectDto>>.Ok(dto);
        }
    }

    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public GetProjectByIdQueryHandler(IProjectRepository repository, IMapper mapper, ICacheService cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.Project(request.Id);
            var cached = await _cache.GetAsync<ProjectDto>(cacheKey);
            if (cached != null)
            {
                return Result<ProjectDto>.Ok(cached);
            }

            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (project == null)
            {
                return Result<ProjectDto>.Fail("Project not found");
            }

            var dto = _mapper.Map<ProjectDto>(project);
            await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(10));
            return Result<ProjectDto>.Ok(dto);
        }
    }
}
