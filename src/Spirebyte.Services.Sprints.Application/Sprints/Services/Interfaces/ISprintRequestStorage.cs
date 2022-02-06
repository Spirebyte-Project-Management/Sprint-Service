using System;
using Spirebyte.Services.Sprints.Application.Sprints.DTO;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.Sprints.Services.Interfaces;

public interface ISprintRequestStorage
{
    void SetSprint(Guid referenceId, Sprint sprint);
    SprintDto GetSprint(Guid referenceId);
}