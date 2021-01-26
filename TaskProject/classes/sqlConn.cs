using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace TaskProject.classes
{
    public class sqlConn
    {
        public static SQLiteConnection DbConnection()
        {
            //sqliteConnection = new SQLiteConnection("Data Source=C:\\Users\\deivd.silva\\Documents\\Deivd Krug\\C#\\TaskProject\\TaskProject\\TaskProject\\data\\dbTask.db; Version=3;");
            //sqliteConnection.Open();
           // return sqliteConnection;

            SQLiteConnection dbConn = new SQLiteConnection("Data Source=C:\\Users\\deivd.silva\\Documents\\Deivd Krug\\C#\\TaskProject\\TaskProject\\TaskProject\\data\\dbTask.db; Version=3;");
            dbConn.Open();
            return dbConn;
        }

        public static void Execute(String sql, SQLiteConnection dbConn)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            command.ExecuteNonQuery();
        }

        public static DataTable Read(String sql, SQLiteConnection dbConn)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbConn);
            DataTable dt = new DataTable();
            SQLiteDataAdapter ap = new SQLiteDataAdapter(command);
            ap.Fill(dt);
            return dt;
        }
    }
}
