using Medobia.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Medobia.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
