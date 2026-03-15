using AutoMapper;
using ProjectManagementERP.Application.Commands.Milestones;
using ProjectManagementERP.Application.Commands.Projects;
using ProjectManagementERP.Application.Commands.Resources;
using ProjectManagementERP.Application.Commands.Tasks;
using ProjectManagementERP.Application.Commands.TimeEntries;
using ProjectManagementERP.Application.DTOs;
using ProjectManagementERP.Application.DTOs.Reports;
using ProjectManagementERP.Domain.Entities;

namespace ProjectManagementERP.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectCommand, Project>();
            CreateMap<Project, ProjectDto>();

            CreateMap<CreateTaskCommand, TaskItem>();
            CreateMap<TaskItem, TaskDto>()
                .ForMember(d => d.ActualHours, o => o.MapFrom(s => s.ActualHours));

            CreateMap<CreateMilestoneCommand, Milestone>();
            CreateMap<UpdateMilestoneCommand, Milestone>();
            CreateMap<Milestone, MilestoneDto>();

            CreateMap<LogTimeEntryCommand, TimeEntry>();
            CreateMap<TimeEntry, TimeEntryDto>();

            CreateMap<AllocateResourceCommand, ResourceAllocation>();
            CreateMap<ResourceAllocation, ResourceAllocationDto>();

            CreateMap<Project, ProjectTimeReportDto>();
        }
    }
}
