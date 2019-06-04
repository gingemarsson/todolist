using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Entities;

namespace TodoList.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnectionOptions _databaseConnectionOptions;

        public UserRepository(IOptionsMonitor<DatabaseConnectionOptions> optionsAccessor)
        {
            _databaseConnectionOptions = optionsAccessor.CurrentValue;
        }

        public User GetUser(Guid Id)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Users WHERE Id=@Id AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", Id));

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        };
                    }
                }
            }

            return null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Users WHERE Deleted=0", connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
            return users;
        }

        public IEnumerable<User> GetMatchingUsers(string searchString)
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Users WHERE (Name LIKE @SearchString) AND Deleted=0", connection))
                {
                    command.Parameters.Add(new SqlParameter("SearchString", "%" + searchString + "%"));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = Guid.Parse((string)reader["Id"]),
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }
            return users;
        }

        public User SaveUser(User user)
        {
            using (var connection = new SqlConnection(_databaseConnectionOptions.connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Users VALUES(@Id, @Name, 0)", connection))
                {
                    command.Parameters.Add(new SqlParameter("Id", user.Id));
                    command.Parameters.Add(new SqlParameter("Name", user.Name));

                    command.ExecuteNonQuery();
                }
            }

            return user;
        }



    }

    public interface IUserRepository
    {
        User GetUser(Guid id);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetMatchingUsers(string searchString);
        User SaveUser(User user);
    }
}