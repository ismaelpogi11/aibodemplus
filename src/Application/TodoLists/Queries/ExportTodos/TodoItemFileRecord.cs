using Medobia.Application.Common.Mappings;
using Medobia.Domain.Entities;

namespace Medobia.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
