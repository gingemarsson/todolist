using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;

namespace TodoList.Api.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly string _connectionString;
        public TodoItemRepository()
        {
            _connectionString = "Server=localhost\\SQLEXPRESS;Database=TodoList;Trusted_Connection=True;"; // TODO: Remove hardcoded string
        }

        public TodoItem Get(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT * FROM TodoItems WHERE Id=@Id AND Deleted=0", connection)) 
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
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

        public IEnumerable<TodoItem> GetAll()
        {
            var todoItems = new List<TodoItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM TodoItems WHERE Deleted=0", connection))
                {
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

        public TodoItem Save(TodoItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"INSERT INTO TodoItems VALUES(@Id, @Name, @Description, @Done, 0)", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", item.Id));
                    command.Parameters.Add(new SqlParameter("Name", item.Name));
                    command.Parameters.Add(new SqlParameter("Description", item.Description));
                    command.Parameters.Add(new SqlParameter("Done", item.Done));

                    command.ExecuteNonQuery();
                }
            }

            return item;
        }

        public TodoItem Update(TodoItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoItems SET Name = @Name, Description = @Description, Done = @Done WHERE Id = @Id", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", item.Id));
                    command.Parameters.Add(new SqlParameter("Name", item.Name));
                    command.Parameters.Add(new SqlParameter("Description", item.Description));
                    command.Parameters.Add(new SqlParameter("Done", item.Done));

                    command.ExecuteNonQuery();
                }
            }

            return item;
        }

        public bool MarkAsDeleted(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"UPDATE TodoItems SET Deleted = 1 WHERE Id = @Id", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", id));
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
    }

    public interface ITodoItemRepository
    {
        TodoItem Get(Guid id);
        IEnumerable<TodoItem> GetAll();
        TodoItem Save(TodoItem item);
        TodoItem Update(TodoItem item);
        bool MarkAsDeleted(Guid id);
    }
}
