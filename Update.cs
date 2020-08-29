using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace pcrjjc_log
{
    public class Update
    {
        #region 字段

        #endregion

        #region 公有方法

        public void UpdateCharaList(SQLiteConnection con)
        {
            //TODO

            //using SQLiteCommand cmd = new SQLiteCommand(con);
            //cmd.CommandText = "INSERT INTO chara_list(name) VALUES(@name)";

            //StreamReader sr = new StreamReader(Common.path_charalist);
            //string str;

            //while ((str = sr.ReadLine()) != null)
            //{
            //    string[] tmp = str.Split(',');
            //    cmd.Parameters.AddWithValue("@name", tmp[0]);
            //    cmd.Prepare();
            //    cmd.ExecuteNonQuery();
            //}
        }

        public void UpdateCharaLocation(SQLiteConnection con)
        {
            //TODO

            //using SQLiteCommand cmd = new SQLiteCommand(con);
            //cmd.CommandText = "INSERT INTO chara_location(chara_id,location) VALUES(@chara_id,@location)";

            //StreamReader sr = new StreamReader(Common.path_charalocation);
            //string str;

            //while ((str = sr.ReadLine()) != null)
            //{
            //    string[] tmp = str.Split(',');
            //    cmd.Parameters.AddWithValue("@chara_id", tmp[0]);
            //    cmd.Parameters.AddWithValue("@location", tmp[2]);
            //    cmd.Prepare();
            //    cmd.ExecuteNonQuery();
            //}
        }

        public void UpdateCharaAlias(SQLiteConnection con)
        {
            //TODO

            //using SQLiteCommand cmd = new SQLiteCommand(con);
            //cmd.CommandText = "INSERT INTO chara_alias(name,chara_id) VALUES(@name,@chara_id)";

            //StreamReader sr = new StreamReader(Common.path_alias);
            //string str;

            //while ((str = sr.ReadLine()) != null)
            //{
            //    string[] tmp = str.Split(',');
            //    cmd.Parameters.AddWithValue("@name", tmp[0]);
            //    cmd.Parameters.AddWithValue("@chara_id", tmp[1]);
            //    cmd.Prepare();
            //    cmd.ExecuteNonQuery();
            //}

            //using SQLiteCommand cmd = new SQLiteCommand(con);
            //cmd.CommandText = "INSERT INTO chara_alias(name,chara_id) VALUES(@name,@chara_id)";

            //StreamReader sr = new StreamReader(Common.path_charalist);
            //string str;

            //====================================================================//

            //while ((str = sr.ReadLine()) != null)
            //{
            //    try
            //    {
            //        string[] tmp = str.Split(',');
            //        cmd.Parameters.AddWithValue("@name", tmp[1]);
            //        cmd.Parameters.AddWithValue("@chara_id", tmp[0]);
            //        cmd.Prepare();
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (SQLiteException)
            //    {
            //        continue;
            //    }
            //}

            //Console.WriteLine("over");
        }

        #endregion


        #region 私有方法

        #endregion
    }
}
