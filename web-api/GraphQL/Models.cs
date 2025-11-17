namespace Vueboard.Api.GraphQL
{
    public class Project
    {
        public int Id { get; set; }
        public Guid Uid { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DefaultCardFgColor { get; set; }
        public string? DefaultCardBgColor { get; set; }
        public List<ProjectColumn> Columns { get; set; } = new();
    }

    public class ProjectColumn
    {
        public int Id { get; set; }
        public Guid Uid { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public string? FgColor { get; set; }
        public string? BgColor { get; set; }
        public int Index { get; set; }
        public List<WorkItem> WorkItems { get; set; } = new();
    }

    public class WorkItem
    {
        public int Id { get; set; }
        public Guid Uid { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public int ProjectColumnId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? FgColor { get; set; }
        public string? BgColor { get; set; }
        public int Index { get; set; }
        public List<string> Tags { get; set; } = new(); // Abstract tags as strings
    }

    public class CreateProjectInput
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DefaultCardFgColor { get; set; }
        public string? DefaultCardBgColor { get; set; }
    }

    public class CreateColumnInput
    {
        public string Name { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
        public int Index { get; set; } = 0;
    }
}
