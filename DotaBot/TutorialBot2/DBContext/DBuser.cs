using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialBot2.Data
{
    class DBuser
    {
        private string connectionString = "Data Source=DESKTOP-9P4S0UL\\SQLEXPRESS;Initial Catalog=Dota;Trusted_Connection=yes;" + "User=sa;Password=";


        public List<users> Get(string discordID)
        {
            List<users> user = new List<users>();
            string query = "SELECT discord_id, steam_id, dota_id FROM users WHERE discord_id = @discord_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, connection);

                connection.Open();
                cmd.Parameters.AddWithValue("@discord_id", discordID);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string discord_id = reader.GetString(0);
                    string steam_id = reader.GetString(1);
                    int dota_id = reader.GetInt32(2);
                    var usuario = new users(discord_id, steam_id, dota_id);
                    user.Add(usuario);
                }
                
                reader.Close();
                connection.Close();
            }
            return user;
        }

        public bool Add(users user)
        {
            string checkQuery = "SELECT discord_id FROM users where discord_id = @discord_id";
            using (var connection = new SqlConnection(connectionString))
            {
                var cmdQuery = new SqlCommand(checkQuery, connection);
                cmdQuery.Parameters.AddWithValue("@discord_id", user.discord_id);
                connection.Open();
                SqlDataReader reader = cmdQuery.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return true;
                }
                else
                {
                    {
                        reader.Close();
                        string query = "insert into users(discord_id, steam_id, dota_id) " + "values(@discord_id, @steam_id, @dota_id)";
                        var cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@discord_id", user.discord_id);
                        cmd.Parameters.AddWithValue("@steam_id", user.steam_id);
                        cmd.Parameters.AddWithValue("@dota_id", user.dota_id);
                        cmd.ExecuteNonQuery();

                        connection.Close();
                        return false;
                    }
                }
            }

        }
    }
}
