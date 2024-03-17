using System.Data.SqlClient;
using System.Xml.Linq;

namespace GiveAwayWithLoginHw.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Ad
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }

    public class Repisotory
    {
        private string _connection;
        public Repisotory(string connection)
        {
            _connection = connection;
        }

        public List<Ad> GetAllAds()
        {
            List<Ad> adds = new();
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT p.Name, p.PhoneNumber ,a.* FROM Person p
                                JOIN Adds a
                                ON a.UserId=p.Id";

            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adds.Add(new()
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"],
                    UserPhoneNumber = (string)reader["PhoneNumber"],
                    Date = (DateTime)reader["Date"],
                    Text = (string)reader["Text"]
                });
            }
            return adds;
        }

        public List<Ad> GetAdsByUserId(int userId)
        {
            List<Ad> ads = new();
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT a.* FROM Person p
                                JOIN Adds a
                                ON a.UserId=p.Id
                                WHERE a.UserId=@userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new()
                {
                    Date = (DateTime)reader["Date"],
                    Text = (string)reader["Text"]
                });
            }
            return ads;
        }

        public void CreateAccount(User user)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = @"INSERT Person
                                VALUES(@name, @email, @password, @num)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@num", user.PhoneNumber);
            con.Open();
            var reader = cmd.ExecuteNonQuery();
        }

        public User GetUserByEmail(string email)
        {
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT TOP 1 * FROM Person
                                WHERE Email= @email";
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                PhoneNumber = (string)reader["PhoneNumber"],
                Email = email,
                Password = (string)reader["Password"]
            };
        }

        public bool Authenticated(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return false;
            }

            return true;
        }

        public void DeleteAd(int id)
        {
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM Adds WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void NewAd(string text, int userId)
        {
            SqlConnection con = new(_connection);
            var cmd = con.CreateCommand();
            cmd.CommandText = @"INSERT INTO Adds
                                VALUES(@userId, @date, @text)";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@text", text);
            con.Open();
            cmd.ExecuteNonQuery();
        }


    }
}