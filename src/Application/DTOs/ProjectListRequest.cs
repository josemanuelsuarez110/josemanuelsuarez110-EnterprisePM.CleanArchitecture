namespace ProjectManagementERP.Application.DTOs
{
    public class ProjectListRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Status { get; set; }
    }
}
