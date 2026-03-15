using System;
using System.Collections.Generic;

namespace ProjectManagementERP.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string userName, IEnumerable<string> roles);
    }
}
