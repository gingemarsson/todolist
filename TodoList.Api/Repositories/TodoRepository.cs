using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;

namespace TodoList.Api.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DatabaseConnectionOptions _databaseConnectionOptions;

        public TodoRepository(IOptionsMonitor<DatabaseConnectionOptions> optionsAccessor)
        {
            _databaseConnectionOptions = optionsAccessor.CurrentValue;
        }

        // Items:

        public TodoItem GetItem(Guid listId, Guid id)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT * FROM TodoItems WHERE Id=@Id AND ListId=@ListId AND Deleted=0", connection)) 
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    command.Parameters.Add(new SqlParameter("ListId", listId));
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new TodoItem {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            Done = (bool)reader["Done"]
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<TodoItem> GetAllItemsofList(Guid listId)
        {
            var todoItems = new List<TodoItem>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM TodoItems WHERE ListId=@ListId AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("ListId", listId));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        todoItems.Add(new TodoItem
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            Done = (bool)reader["Done"]
                        });
                    }
                }
            }
            return todoItems;
        }

        public IEnumerable<TodoItem> GetMatchingItems(string searchString)
        {
            var todoItems = new List<TodoItem>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT * FROM TodoItems WHERE ((Name LIKE @SearchString) OR (Description LIKE @SearchString)) AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("SearchString", "%" + searchString + "%"));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        todoItems.Add(new TodoItem
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            Done = (bool)reader["Done"]
                        });
                    }
                }
            }

            return todoItems;
        }

        public TodoItem SaveItem(Guid listId, TodoItem item)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"INSERT INTO TodoItems(Id, Name, Description, Done, Deleted, ListId) VALUES(@Id, @Name, @Description, @Done, 0, @ListId)", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", item.Id));
                    command.Parameters.Add(new SqlParameter("Name", item.Name));
                    command.Parameters.Add(new SqlParameter("Description", item.Description));
                    command.Parameters.Add(new SqlParameter("Done", item.Done));
                    command.Parameters.Add(new SqlParameter("ListId", listId));

                    command.ExecuteNonQuery();
                }
            }

            return item;
        }

        public TodoItem UpdateItem(Guid listId, TodoItem item)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoItems SET Name = @Name, Description = @Description, Done = @Done WHERE Id = @Id AND ListId=@ListId", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", item.Id));
                    command.Parameters.Add(new SqlParameter("ListId", listId));
                    command.Parameters.Add(new SqlParameter("Name", item.Name));
                    command.Parameters.Add(new SqlParameter("Description", item.Description));
                    command.Parameters.Add(new SqlParameter("Done", item.Done));

                    command.ExecuteNonQuery();
                }
            }

            return item;
        }

        public bool MarkItemAsDeleted(Guid listId, Guid id)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoItems SET Deleted = 1 WHERE Id = @Id AND ListId=@ListId", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    command.Parameters.Add(new SqlParameter("ListId", listId));
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }

        // Lists:

        public TodoItemList GetList(Guid id)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT * FROM TodoLists WHERE Id=@Id AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new TodoItemList
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<TodoItemList> GetAllLists()
        {
            var todoItemLists = new List<TodoItemList>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM TodoLists WHERE Deleted=0", connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        todoItemLists.Add(new TodoItemList
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
            return todoItemLists;
        }

        public IEnumerable<TodoItemList> GetMatchingLists(string searchString)
        {
            var todoItemLists = new List<TodoItemList>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM TodoLists WHERE (Name LIKE @SearchString) AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("SearchString", "%" + searchString + "%"));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        todoItemLists.Add(new TodoItemList
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
            return todoItemLists;
        }

        public TodoItemList SaveList(TodoItemList list)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"INSERT INTO TodoLists VALUES(@Id, @Name, 0)", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", list.Id));
                    command.Parameters.Add(new SqlParameter("Name", list.Name));

                    command.ExecuteNonQuery();
                }
            }

            return list;
        }

        public TodoItemList UpdateList(TodoItemList list)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoLists SET Name = @Name WHERE Id = @Id", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", list.Id));
                    command.Parameters.Add(new SqlParameter("Name", list.Name));

                    command.ExecuteNonQuery();
                }
            }

            return list;
        }

        public bool MarkListAsDeleted(Guid id)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoLists SET Deleted = 1 WHERE Id = @Id", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
    }

    public interface ITodoRepository
    {
        TodoItem GetItem(Guid id, Guid id1);
        IEnumerable<TodoItem> GetAllItemsofList(Guid listId);
        IEnumerable<TodoItem> GetMatchingItems(string searchString);
        TodoItem SaveItem(Guid listId, TodoItem item);
        TodoItem UpdateItem(Guid listId, TodoItem item);
        bool MarkItemAsDeleted(Guid id, Guid id1);


        TodoItemList GetList(Guid id);
        IEnumerable<TodoItemList> GetAllLists();
        IEnumerable<TodoItemList> GetMatchingLists(string searchString);
        TodoItemList SaveList(TodoItemList list);
        TodoItemList UpdateList(TodoItemList list);
        bool MarkListAsDeleted(Guid id);
    }
}
