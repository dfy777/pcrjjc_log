using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace pcrjjc_log
{
    public class SaveFunc
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
        //private SQLiteConnection my_sqlcon;

        #endregion



        #region 属性

        #endregion



        #region 构造函数

        public SaveFunc(SQLiteConnection con) 
        {
            ReadCharaIdList(con);
            ReadCharaLocation(con);
        }

        #endregion



        #region 公有函数

        /// <summary>
        /// 处理进攻队伍信息
        /// </summary>
        /// <param name="team">用户输入的进攻队伍字符串,其中应包含5个人名</param>
        /// <param name="con">数据库链接</param>
        public void HandleAttackMember(string team, SQLiteConnection con)
        {
            //清楚队伍数据
            CleanAttackInfo();

            //检测长度
            string[] tmp = team.Split(SPLIT_CHAR);

            if (team.Length == QUIT_INSTRUCTION_LENGTH || tmp.Length != TEAM_MEMBER_NUM)
            {
                throw new FormatException();
            }

            //通过角色外号读取角色id

            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select chara_id from chara_alias where name = (@name1)" +
                                "or name = (@name2)" +
                                "or name = (@name3)" +
                                "or name = (@name4)" +
                                "or name = (@name5)";

            cmd.Parameters.AddWithValue("@name1", tmp[0]);
            cmd.Parameters.AddWithValue("@name2", tmp[1]);
            cmd.Parameters.AddWithValue("@name3", tmp[2]);
            cmd.Parameters.AddWithValue("@name4", tmp[3]);
            cmd.Parameters.AddWithValue("@name5", tmp[4]);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                attack_members_id.Add(reader.GetInt32(0));
            }

            //检测是否5名角色都被识别
            if (attack_members_id.Count != TEAM_MEMBER_NUM)
            {
                throw new FormatException();
            }

            //角色id排序
            attack_members_id.Sort();

            //检测id是否重复
            for(int i = 0; i < TEAM_MEMBER_NUM - 1; i++)
            {
                if (attack_members_id[i] == attack_members_id[i+1])
                {
                    throw new ArgumentException();
                }
            }

            //角色排序
            AttackTeamSort();

            //确定角色名称
            ReadAttackMembersNameById();

            //DEBUG
            //PrintAttackMemberInfo();
        }


        /// <summary>
        /// 同HandleAttackMember
        /// </summary>
        /// <param name="team"></param>
        /// <param name="con"></param>
        public void HandleDefendMember(string team, SQLiteConnection con)
        {
            //清楚队伍数据
            CleanDefendInfo();

            //检测长度
            string[] tmp = team.Split(SPLIT_CHAR);

            if (team.Length == QUIT_INSTRUCTION_LENGTH || tmp.Length != TEAM_MEMBER_NUM)
            {
                throw new FormatException();
            }

            //通过角色外号读取角色id

            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select chara_id from chara_alias where name = (@name1)" +
                                "or name = (@name2)" +
                                "or name = (@name3)" +
                                "or name = (@name4)" +
                                "or name = (@name5)";

            cmd.Parameters.AddWithValue("@name1", tmp[0]);
            cmd.Parameters.AddWithValue("@name2", tmp[1]);
            cmd.Parameters.AddWithValue("@name3", tmp[2]);
            cmd.Parameters.AddWithValue("@name4", tmp[3]);
            cmd.Parameters.AddWithValue("@name5", tmp[4]);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                defend_members_id.Add(reader.GetInt32(0));
            }

            //检测是否5名角色都被识别
            if (defend_members_id.Count != TEAM_MEMBER_NUM)
            {
                throw new FormatException();
            }

            //角色id排序
            defend_members_id.Sort();

            //检测角色是否有重复
            for(int i = 0; i < TEAM_MEMBER_NUM - 1; i++)
            {
                if (defend_members_id[i] == defend_members_id[i+1])
                {
                    throw new ArgumentException();
                }
            }

            //角色排序
            DefendTeamSort();

            //确定角色名称
            ReadDefendMembersNameById();

            //DEBUG
            //PrintDefendMemberInfo();
        }

        /// <summary>
        /// 同HandleAttackMember
        /// </summary>
        /// <param name="msg"></param>
        public void HandleMessage(string msg)
        {
            CleanBattleMessage();

            battle_msg = msg;
        }

        /// <summary>
        /// 处理数据库存储战斗信息的过程
        /// </summary>
        /// <param name="con"></param>
        public void ExcuteSaveFunc(SQLiteConnection con)
        {
            int attack_team_id = SearchAttackTeamId(con);
            int defend_team_id = SearchDefendTeamId(con);
            int msg_id = AddMsgId(con);

            if ( attack_team_id > 0 &&
                 defend_team_id > 0 &&
                 msg_id > 0 )
            {
                int tmp_id = AddAttackWithDefence(con, attack_team_id, defend_team_id, msg_id);
                Console.WriteLine($"atatck_id:{attack_team_id}\ndefend_id:{defend_team_id}" + 
                                     $"\nmsg_id:{msg_id}\nAWD:{tmp_id}");
            }
            else
            {
                throw new ArgumentException();
            }
        }

        #endregion



        #region debug

        /// <summary>
        /// Debug用函数
        /// 打印进攻队队员角色id和名字
        /// </summary>
        public void PrintAttackMemberInfo()
        {
            for (int i = 0; i < TEAM_MEMBER_NUM; i++)
            {
                Console.WriteLine($"id:{attack_members_id[i]}   name:{attack_members_name[i]}");
            }
        }

        /// <summary>
        /// Debug用函数
        /// 打印防守队队员角色id和名字
        /// </summary>
        public void PrintDefendMemberInfo()
        {
            for (int i = 0; i < TEAM_MEMBER_NUM; i++)
            {
                Console.WriteLine($"id:{defend_members_id[i]}   name:{defend_members_name[i]}");
            }
        }

        /// <summary>
        /// Debug用函数
        /// 打印战斗信息备注
        /// </summary>
        public void PrintBattleMessage()
        {
            Console.WriteLine(battle_msg);
        }

        /// <summary>
        /// Debug用函数
        /// 打印所有信息
        /// </summary>
        public void PrintAllLog()
        {
            PrintAttackMemberInfo();
            PrintDefendMemberInfo();
            PrintBattleMessage();
        }

        #endregion



        #region 私有函数
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

        /// <summary>
        /// 对队伍信息进行排列
        /// 进攻队站位由后到前
        /// </summary>
        private void AttackTeamSort()
        {
            bool[] tmp_array = new bool[chara_location.Count+5];

            Dictionary<int, int>.Enumerator enumerator = chara_location.GetEnumerator();

            for(int i = 0, j = 0; i < chara_location.Count && j < TEAM_MEMBER_NUM; i++)
            {
                if (enumerator.MoveNext())
                {
                    if(attack_members_id[j] == enumerator.Current.Key)
                    {
                        tmp_array[i + 1] = true;
                        j++;
                    }
                }
            }

            for (int i = 0, j = 0; i < tmp_array.Length && j < TEAM_MEMBER_NUM; i++)
            {
                if (tmp_array[i])
                {
                    attack_members_id[j++] = i;
                }
            }
        }


        /// <summary>
        /// 对队伍信息进行排列
        /// 防守队站位由后到前 
        /// </summary>
        private void DefendTeamSort()
        {
            bool[] tmp_array = new bool[chara_location.Count];

            Dictionary<int, int>.Enumerator enumerator = chara_location.GetEnumerator();

            for (int i = 0, j = 0; i < chara_location.Count && j < TEAM_MEMBER_NUM; i++)
            {
                if (enumerator.MoveNext())
                {
                    if (defend_members_id[j] == enumerator.Current.Key)
                    {
                        tmp_array[i + 1] = true;
                        j++;
                    }
                }
            }

            for (int i = 0, j = 0; i < tmp_array.Length && j < TEAM_MEMBER_NUM; i++)
            {
                if (tmp_array[i])
                {
                    defend_members_id[j++] = i;
                }
            }
        }


        /// <summary>
        /// 寻找数据库中是否已有队伍信息
        /// 如果没有，创建新的队伍记录
        /// </summary>
        /// <param name="con">sqlconnection</param>
        /// <returns>attack表的id，如果找不到返回-1</returns>
        private int SearchAttackTeamId(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select id from attack where chara_1 = (@id1)" +
                                "and chara_2 = (@id2)" +
                                "and chara_3 = (@id3)" +
                                "and chara_4 = (@id4)" +
                                "and chara_5 = (@id5)";

            cmd.Parameters.AddWithValue("@id1", attack_members_id[0]);
            cmd.Parameters.AddWithValue("@id2", attack_members_id[1]);
            cmd.Parameters.AddWithValue("@id3", attack_members_id[2]);
            cmd.Parameters.AddWithValue("@id4", attack_members_id[3]);
            cmd.Parameters.AddWithValue("@id5", attack_members_id[4]);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                return reader.GetInt32(0);
            }
            reader.Close();

            //如果没有对应的队伍
            cmd.CommandText = "insert into attack" +
                                "(chara_1, chara_2, chara_3, chara_4, chara_5) " +
                                "values(@id1, @id2, @id3, @id4, @id5);" +
                                "select last_insert_rowid();";

            cmd.Parameters.AddWithValue("@id1", attack_members_id[0]);
            cmd.Parameters.AddWithValue("@id2", attack_members_id[1]);
            cmd.Parameters.AddWithValue("@id3", attack_members_id[2]);
            cmd.Parameters.AddWithValue("@id4", attack_members_id[3]);
            cmd.Parameters.AddWithValue("@id5", attack_members_id[4]);
            cmd.Prepare();

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }

            return -1;
        }

        /// <summary>
        /// 同SearchAttackTeamId
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private int SearchDefendTeamId(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "select id from defence where chara_1 = (@id1)" +
                                "and chara_2 = (@id2)" +
                                "and chara_3 = (@id3)" +
                                "and chara_4 = (@id4)" +
                                "and chara_5 = (@id5)";

            cmd.Parameters.AddWithValue("@id1", defend_members_id[0]);
            cmd.Parameters.AddWithValue("@id2", defend_members_id[1]);
            cmd.Parameters.AddWithValue("@id3", defend_members_id[2]);
            cmd.Parameters.AddWithValue("@id4", defend_members_id[3]);
            cmd.Parameters.AddWithValue("@id5", defend_members_id[4]);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            reader.Close();

            //如果没有对应的队伍
            cmd.CommandText = "insert into defence" +
                                "(chara_1, chara_2, chara_3, chara_4, chara_5) " +
                                "values(@id1, @id2, @id3, @id4, @id5);" +
                                "select last_insert_rowid();";

            cmd.Parameters.AddWithValue("@id1", defend_members_id[0]);
            cmd.Parameters.AddWithValue("@id2", defend_members_id[1]);
            cmd.Parameters.AddWithValue("@id3", defend_members_id[2]);
            cmd.Parameters.AddWithValue("@id4", defend_members_id[3]);
            cmd.Parameters.AddWithValue("@id5", defend_members_id[4]);
            cmd.Prepare();

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }
            reader.Close();

            return -1;
        }

        /// <summary>
        /// 向数据库中添加战斗备注
        /// </summary>
        /// <param name="con"></param>
        /// <returns>备注条目对应表的id</returns>
        private int AddMsgId(SQLiteConnection con)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "insert into description(msg) values(@battle_msg);" +
                                "select last_insert_rowid();";
            cmd.Parameters.AddWithValue("@battle_msg", battle_msg);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }

            return -1;
        }

        /// <summary>
        /// 将战斗队伍联系添加到数据库中
        /// </summary>
        /// <param name="con"></param>
        /// <param name="attack_team_id">进攻队id</param>
        /// <param name="defend_team_id">防守队id</param>
        /// <param name="msg_id">战斗备注id</param>
        /// <returns>战斗队伍联系条目对应表的id</returns>
        private int AddAttackWithDefence(SQLiteConnection con, int attack_team_id, 
                                            int defend_team_id, int msg_id)
        {
            using SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = "insert into attack_with_defence(attack_id, defence_id, description_id)" +
                              "values(@id1, @id2, @id3);" +
                              "select last_insert_rowid();";
            cmd.Parameters.AddWithValue("@id1", attack_team_id);
            cmd.Parameters.AddWithValue("@id2", defend_team_id);
            cmd.Parameters.AddWithValue("@id3", msg_id);
            cmd.Prepare();

            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetInt32(0);
            }

            return -1;
        }
      

        private void ReadAttackMembersNameById()
        {
            for(int i = 0; i < TEAM_MEMBER_NUM; i++)
            {
                attack_members_name.Add(chara_id_list[attack_members_id[i]]);
            }
        }

        private void ReadDefendMembersNameById()
        {
            for(int i = 0; i < TEAM_MEMBER_NUM; i++)
            {
                defend_members_name.Add(chara_id_list[defend_members_id[i]]);
            }
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
