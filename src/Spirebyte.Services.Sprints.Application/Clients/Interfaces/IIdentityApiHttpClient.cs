using Spirebyte.Services.Sprints.Application.Clients.DTO;
using System;
using System.Threading.Tasks;

namespace Spirebyte.Services.Sprints.Application.Clients.Interfaces
{
    public interface IIdentityApiHttpClient
    {
        Task<UserDto> GetUserAsync(Guid userId);
    }
}
