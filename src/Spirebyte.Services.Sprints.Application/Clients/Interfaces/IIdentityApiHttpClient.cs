using System;
using System.Threading.Tasks;
using Spirebyte.Services.Sprints.Application.Clients.DTO;

namespace Spirebyte.Services.Sprints.Application.Clients.Interfaces
{
    public interface IIdentityApiHttpClient
    {
        Task<UserDto> GetUserAsync(Guid userId);
    }
}
