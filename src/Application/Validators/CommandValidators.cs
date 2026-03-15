using FluentValidation;
using ProjectManagementERP.Application.Commands.Projects;
using ProjectManagementERP.Application.Commands.Tasks;
using ProjectManagementERP.Application.Commands.TimeEntries;
using ProjectManagementERP.Application.Commands.Milestones;
using ProjectManagementERP.Application.Commands.Resources;

namespace ProjectManagementERP.Application.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate ?? x.StartDate.AddYears(5));
        }
    }

    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }

    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.EstimatedHours).GreaterThan(0);
            RuleFor(x => x.ProjectId).NotEmpty();
        }
    }

    public class LogTimeEntryCommandValidator : AbstractValidator<LogTimeEntryCommand>
    {
        public LogTimeEntryCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Hours).GreaterThan(0).LessThanOrEqualTo(24);
        }
    }

    public class CreateMilestoneCommandValidator : AbstractValidator<CreateMilestoneCommand>
    {
        public CreateMilestoneCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProjectId).NotEmpty();
        }
    }

    public class AllocateResourceCommandValidator : AbstractValidator<AllocateResourceCommand>
    {
        public AllocateResourceCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.AllocationPercentage).InclusiveBetween(0, 100);
        }
    }
}
