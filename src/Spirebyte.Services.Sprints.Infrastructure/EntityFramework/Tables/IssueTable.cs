using Convey.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables
{
    public sealed class IssueTable : IIdentifiable<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public ProjectTable Project { get; set; }
        public string? SprintId { get; set; }
        public SprintTable Sprint { get; set; }
    }
}
