using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace pcrjjc_log
{
    public class DbTest
    {

        public void SqliteGrammerTest(SQLiteConnection con)
        {
            ////string path = @".\battle_info.db";
            //string path = "Data Source=battle_info.db";

            //using SQLiteConnection con = new SQLiteConnection(path);
            //con.Open();

            //using SQLiteCommand cmd = new SQLiteCommand(con);

            ////cmd.CommandText = "DROP TABLE IF EXISTS attack";
            ////cmd.ExecuteNonQuery();

            ////cmd.CommandText = "CREATE TABLE attack(id INTEGER PRIMARY KEY, chara_1 VARCHAR(8))";
            ////cmd.ExecuteNonQuery();

            ////cmd.CommandText = "INSERT INTO attack VALUES(3, \"羊驼\")";
            ////cmd.ExecuteNonQuery();

            //cmd.CommandText = "SELECT * FROM attack";
            //SQLiteDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //    Console.WriteLine("id: " + reader["id"] + "   chara_1: " + reader["chara_1"]);
            //Console.ReadLine();

            //string s = Console.ReadLine();//输入一行
            //Console.WriteLine(s);
        }

        public void ReaderTest(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select id from test_table where index1 = 22;";

            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0));
            }
            reader.Close();

            cmd.CommandText = "select id from test_table where index1 = 55;";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0));
            }
            reader.Close();
        }
    }
}
