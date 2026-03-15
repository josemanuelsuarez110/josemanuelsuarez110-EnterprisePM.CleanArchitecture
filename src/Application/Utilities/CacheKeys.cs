using System;
using ProjectManagementERP.Domain.Enums;

namespace ProjectManagementERP.Application.Utilities
{
    public static class CacheKeys
    {
        public static string ProjectList(int page = 1, int pageSize = 20, string? search = null, ProjectStatus? status = null)
            => $"projects:{page}:{pageSize}:{search}:{status}";

        public static string Project(Guid id) => $"project:{id}";
    }
}
