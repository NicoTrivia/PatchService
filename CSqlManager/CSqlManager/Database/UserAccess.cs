using Npgsql;
using System.Security.Cryptography;
using System.Text;
namespace CSqlManager;

public class UserAccess : DbAccess
{
    private void AddFromReader(NpgsqlDataReader reader, User user)
    {
        user.id = (int)getInt(reader,"id", true)!;
        user.tenant =  getString(reader, "tenant", true)!;
        user.active =  (bool)getBoolean(reader, "active", true)!;
        user.email = getString(reader, "email");
        user.firstname = getString(reader, "firstname");
        user.lastname = getString(reader, "lastname");
        user.login = getString(reader, "login", true)!;
        
        user.profile = getString(reader, "profile");
    }
    
    public List<User> GetUsers()
    {
        List<User> requestResult = new List<User>();
        
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user ORDER BY tenant, login";
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                User current = new User();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
        }
        
        return requestResult;
    }

    public User GetUserById(int id)
    {
        User user = new User();
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user WHERE id = @id";
            
            command.Parameters.AddWithValue("id", id);
            var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                AddFromReader(reader, user);
            }
            Close(Connection);
        }

        return user;
    }
    
    public Boolean DeleteUserById(int id) {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"DELETE FROM ps_user WHERE id = @id";
            
            command.Parameters.AddWithValue("id", id);
            int count = command.ExecuteNonQuery();
            Close(Connection);            
            return count > 0;
        }
    }

    public Boolean DeleteUserByTenant(string tenant) {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"DELETE FROM ps_user WHERE tenant = @tenant";
            
            command.Parameters.AddWithValue("tenant", tenant);
            int count = command.ExecuteNonQuery();
            Close(Connection);            
            return count > 0;
        }
    }

    public List<User> GetUsersByTenant(string tenant)
    {
        List<User> requestResult = new List<User>();
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = $"SELECT id,login,firstname,lastname,email,tenant,active, profile FROM ps_user WHERE tenant = @tenant";
            
            command.Parameters.AddWithValue("tenant", tenant);
            var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                User current = new User();
                AddFromReader(reader, current);
                requestResult.Add(current);
            }
            Close(Connection);
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
        if (user == null)
        {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
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
            Close(Connection);
        }
    }
    
    public void Update(User user)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
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
            Close(Connection);
        }
    }

  
    public void UpdatePassword(User user)
    {
        if (string.IsNullOrEmpty(user.password)) {
            return;
        }
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
            command.CommandText = 
                "UPDATE ps_user SET password = @password WHERE id = @id;";
            
            command.Parameters.AddWithValue("id", GetParam(user.id));
            command.Parameters.AddWithValue("password", ComputeSHA256Hash(user.login+user.password)); 

            command.ExecuteNonQuery();
            Close(Connection);
        }        
    }

    public User? Login(string tenant, string login, string password)
    {
        using (NpgsqlConnection Connection = GetConnection())
        {
            NpgsqlCommand command = CreateCommand(Connection);
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
                MyLogManager.Debug($"User with id {user.id} was found !");
                Close(Connection);
                return user;
            }
            else
            {
                MyLogManager.Error($"No user was found with the given informations");
            }
            Close(Connection);
            return null;
            
        }
    }
}