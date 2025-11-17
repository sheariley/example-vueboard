using HotChocolate;
using HotChocolate.Types;

namespace Vueboard.Api.GraphQL
{
    public class Mutation
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectColumnRepository _columnRepo;
        private readonly IWorkItemRepository _workItemRepo;
        public Mutation(IProjectRepository projectRepo, IProjectColumnRepository columnRepo, IWorkItemRepository workItemRepo)
        {
            _projectRepo = projectRepo;
            _columnRepo = columnRepo;
            _workItemRepo = workItemRepo;
        }

        public Project CreateProject(CreateProjectInput input)
        {
            var project = new Project
            {
                Title = input.Title,
                Description = input.Description,
                DefaultCardFgColor = input.DefaultCardFgColor,
                DefaultCardBgColor = input.DefaultCardBgColor,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false,
                Uid = Guid.NewGuid(),
                UserId = Guid.NewGuid() // TODO: Replace with actual user context
            };
            return _projectRepo.Add(project);
        }

        public Project? UpdateProject(string uid, CreateProjectInput input)
        {
            var project = _projectRepo.GetByUid(uid);
            if (project == null) return null;
            project.Title = input.Title;
            project.Description = input.Description;
            project.DefaultCardFgColor = input.DefaultCardFgColor;
            project.DefaultCardBgColor = input.DefaultCardBgColor;
            project.Updated = DateTime.UtcNow;
            _projectRepo.Update(project);
            return project;
        }

        public bool DeleteProject(string uid)
        {
            return _projectRepo.Delete(uid);
        }

        public ProjectColumn CreateProjectColumn(int projectId, CreateColumnInput input)
        {
            var column = new ProjectColumn
            {
                Name = input.Name,
                IsDefault = input.IsDefault,
                Index = input.Index,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false,
                Uid = Guid.NewGuid(),
                ProjectId = projectId
            };
            return _columnRepo.Add(projectId, column);
        }

        public ProjectColumn? UpdateProjectColumn(int id, CreateColumnInput input)
        {
            var column = _columnRepo.GetById(id);
            if (column == null) return null;
            column.Name = input.Name;
            column.IsDefault = input.IsDefault;
            column.Index = input.Index;
            column.Updated = DateTime.UtcNow;
            _columnRepo.Update(column);
            return column;
        }

        public bool DeleteProjectColumn(int id)
        {
            return _columnRepo.Delete(id);
        }

        public WorkItem CreateWorkItem(int projectColumnId, string title, List<string> tags, Guid userId)
        {
            var workItem = new WorkItem
            {
                Title = title,
                ProjectColumnId = projectColumnId,
                Tags = tags,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                IsDeleted = false,
                Uid = Guid.NewGuid()
            };
            return _workItemRepo.CreateWorkItem(workItem, userId);
        }

        public bool UpdateWorkItem(WorkItem item, Guid userId)
        {
            return _workItemRepo.UpdateWorkItem(item, userId);
        }

        public bool DeleteWorkItem(int workItemId)
        {
            return _workItemRepo.DeleteWorkItem(workItemId);
        }
    }
}
