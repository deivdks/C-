using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace TaskProject.Classes
{
    public class SqlConn
    {
        private static string _connectionString = @"Data Source=.\data\dbTask.db; Version=3;";

        /*public static SQLiteConnection DbConnection()
        {
            SQLiteConnection dbConn = new SQLiteConnection("Data Source=C:\\Users\\deivd.silva\\Documents\\Deivd Krug\\C#\\TaskProject\\TaskProject\\TaskProject\\data\\dbTask.db; Version=3;");
            dbConn.Open();
            return dbConn;
        }*/

        public static void ExecuteNonQuery(String sql, Dictionary<string, object> parameters)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(sql, connection);
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                command.ExecuteNonQuery(); 
            }            
        }

        public static int ExecuteScalar(string sql, Dictionary<string, object> parameters)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(sql, connection);
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static DataTable Read(String sql)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                DataTable dt = new DataTable();
                SQLiteDataAdapter ap = new SQLiteDataAdapter(command);
                ap.Fill(dt);
                return dt;
            }
        }
    }
}
