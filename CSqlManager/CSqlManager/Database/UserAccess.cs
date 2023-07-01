using Npgsql;
using System.Security.Cryptography;
using System.Text;
namespace CSqlManager;

public class UserAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, User user)
    {
        user.id = reader.GetInt32(reader.GetOrdinal("id"));
        user.tenant = reader.GetString(reader.GetOrdinal("tenant"));
        user.active = reader.GetBoolean(reader.GetOrdinal("active"));
        user.email = reader.GetString(reader.GetOrdinal("email"));
        user.firstname = reader.GetString(reader.GetOrdinal("firstname"));
        user.lastname = reader.GetString(reader.GetOrdinal("lastname"));
        user.login = reader.GetString(reader.GetOrdinal("login"));
        
        user.profile = reader.GetString(reader.GetOrdinal("profile"));
    }
    
    public List<User> GetUsers()
    {
        List<User> requestResult = new List<User>();
        
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user ORDER BY tenant, login";
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                User current = new User();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }

        }
        
        return requestResult;
    }

    public User GetUserById(int id)
    {
        User user = new User();
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user WHERE id = @id";
            
            command.Parameters.AddWithValue("id", id);
            var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                AddFromReader(reader, user);
            }
        }

        return user;
    }
    
    public Boolean DeleteUserById(int id) {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"DELETE FROM ps_user WHERE id = @id";
            
            command.Parameters.AddWithValue("id", id);
            int count = command.ExecuteNonQuery();
            
            return count > 0;
        }
    }

    public Boolean DeleteUserByTenant(string tenant) {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"DELETE FROM ps_user WHERE tenant = @tenant";
            
            command.Parameters.AddWithValue("tenant", tenant);
            int count = command.ExecuteNonQuery();
            
            return count > 0;
        }
    }

    public List<User> GetUsersByTenant(string tenant)
    {
        List<User> requestResult = new List<User>();
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user WHERE tenant = @tenant";
            
            command.Parameters.AddWithValue("tenant", tenant);
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                User current = new User();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
        }

        return requestResult;
    }
    
    static string ComputeSHA256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public void Create(User user)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = $"INSERT INTO ps_user (id, login, email, firstname, active, lastname, tenant, profile, password)" +
             $" VALUES (nextval('ps_user_id_seq'), @login, @email, @firstname, @active, @lastname, @tenant, @profile, @password);";
            command.Parameters.AddWithValue("login", GetParam(user.login));
            command.Parameters.AddWithValue("email", GetParam(user.email));
            command.Parameters.AddWithValue("firstname",GetParam(user.firstname));
            command.Parameters.AddWithValue("active",GetParam(user.active));
            command.Parameters.AddWithValue("lastname", GetParam(user.lastname));
            command.Parameters.AddWithValue("tenant", GetParam(user.tenant));
            command.Parameters.AddWithValue("profile", GetParam(user.profile.ToString()));
            
            if (!string.IsNullOrEmpty(user.password))
            {
                command.Parameters.AddWithValue("password", ComputeSHA256Hash(user.login+user.password)); 
            }
            else
            {
                command.Parameters.AddWithValue("password", DBNull.Value);
            }
            
            command.ExecuteNonQuery();
        }
    }
    
    public void Update(User user)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_user" +
                $" SET login = @login, email = @email, firstname = @firstname, active = @active, lastname = @lastname, tenant = @tenant, profile = @profile" +
                $" WHERE id = @id;";
            
            command.Parameters.AddWithValue("id", GetParam(user.id));
            command.Parameters.AddWithValue("login", GetParam(user.login));
            command.Parameters.AddWithValue("email", GetParam(user.email));
            command.Parameters.AddWithValue("firstname",GetParam(user.firstname));
            command.Parameters.AddWithValue("active",GetParam(user.active));
            command.Parameters.AddWithValue("lastname", GetParam(user.lastname));
            command.Parameters.AddWithValue("tenant", GetParam(user.tenant));
            command.Parameters.AddWithValue("profile", GetParam(user.profile));
            command.ExecuteNonQuery();
        }
    }

  
    public void UpdatePassword(User user)
    {
        if (string.IsNullOrEmpty(user.password)) {
            return;
        }
        using (NpgsqlCommand command = CreateCommand())
        {
            command.CommandText = 
                "UPDATE ps_user SET password = @password WHERE id = @id;";
            
            command.Parameters.AddWithValue("id", GetParam(user.id));
            command.Parameters.AddWithValue("password", ComputeSHA256Hash(user.login+user.password)); 

            command.ExecuteNonQuery();
        }        
    }

    public User? Login(string tenant, string login, string password)
    {
        using (NpgsqlCommand command = CreateCommand())
        {
            string encrypted = ComputeSHA256Hash(login+password);
            command.Parameters.AddWithValue("login", GetParam(login));
            command.Parameters.AddWithValue("password", GetParam(encrypted));
            command.Parameters.AddWithValue("tenant", GetParam(tenant));

            command.CommandText = "SELECT id, login, firstname, lastname, email, tenant ,active, profile FROM ps_user WHERE login = @login AND tenant = @tenant AND password = @password";
            var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                User user = new User();
                AddFromReader(reader, user);
                Console.WriteLine($"User with id {user.id} was found !");
                return user;
            }
            else
            {
                Console.WriteLine($"No user was found with the given informations");
            }
            
            return null;
            
        }
    }
}