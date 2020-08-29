using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace pcrjjc_log
{
    public class LoadFunc
    {
        #region 字段

        private List<string> attack_members_name = new List<string>(5);
        private List<string> defend_members_name = new List<string>(5);
        private List<int> attack_members_id = new List<int>(5);
        private List<int> defend_members_id = new List<int>(5);
        private Dictionary<int, int> chara_location = new Dictionary<int, int>();   //<角色id,角色站位>
        private Dictionary<int, string> chara_id_list = new Dictionary<int, string>(); //<角色id,角色名称>

        private string battle_msg;
        private char SPLIT_CHAR = '-';
        private int TEAM_MEMBER_NUM = 5;
        private int QUIT_INSTRUCTION_LENGTH = 1;

        #endregion



        #region 构造函数

        public LoadFunc(SQLiteConnection con)
        {
            ReadCharaIdList(con);
            ReadCharaLocation(con);
        }

        #endregion



        #region 公有方法



        #endregion



        #region Debug



        #endregion



        #region 私有方法

        /// <summary>
        /// 清理之前记录的队伍信息
        /// </summary>
        private void CleanAttackInfo()
        {
            attack_members_name.Clear();
            attack_members_id.Clear();
        }

        /// <summary>
        /// 清理之前记录的队伍信息
        /// </summary>
        private void CleanDefendInfo()
        {
            defend_members_name.Clear();
            defend_members_id.Clear();
        }

        /// <summary>
        /// 清理之前记录的战斗备注
        /// </summary>
        private void CleanBattleMessage()
        {
            battle_msg = "";
        }

        private void ReadCharaLocation(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select * from chara_location";
            SQLiteDataReader reader = cmd.ExecuteReader();

            //在字典中添加角色id和角色站位(已排好序)
            while (reader.Read())
            {
                chara_location.Add(reader.GetInt32(1), reader.GetInt32(2));
            }
        }

        private void ReadCharaIdList(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select * from chara_list";
            SQLiteDataReader reader = cmd.ExecuteReader();

            //在字典中添加角色id和角色名称
            while (reader.Read())
            {
                chara_id_list.Add(reader.GetInt32(0), reader.GetString(1)); ;
            }
        }

        #endregion
    }
}
