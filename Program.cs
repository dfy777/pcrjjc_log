using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;

namespace pcrjjc_log
{

    class Program
    {

        static void Main(string[] args)
        {
            //连接数据库
            using SQLiteConnection con = new SQLiteConnection(Common.path_database);
            con.Open();


            using SQLiteCommand cmd = new SQLiteCommand(con);

            Display display = new Display(con);
            display.MainStage();

            //DbTest dbtest = new DbTest();
            //dbtest.ReaderTest(con);

            con.Close();
            Console.WriteLine("over");

        }

        
    }
}
