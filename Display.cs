using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace pcrjjc_log
{
    public class Display
    {
        #region 字段

        private SQLiteConnection con;
        private SaveFunc savefunc;
        private LoadFunc loadfunc;
        #endregion



        #region 构造函数

        public Display() { }

        public Display(SQLiteConnection _con)
        {
            con = _con;
            savefunc = new SaveFunc(con);
            loadfunc = new LoadFunc(con);
        }

        #endregion



        #region 公有方法

        public void MainStage()
        {
            bool isquit = false;

            while(true)
            {
                PrintMainChoice();

                isquit = ServiceChooseStage();

                PrintCrossingLine();

                if (isquit)
                {
                    break;
                }
            }
        }


        #endregion



        #region 私有方法

        /// <summary>
        /// 选择功能界面
        /// </summary>
        /// <returns>返回布尔值，决定是否退出整个程序</returns>
        private bool ServiceChooseStage()
        {
            bool isquit = false;

            while (true)
            {
                if (isquit)
                {
                    //回到上层,但不退出整个程序
                    return false;
                }

                string user_input_str = Console.ReadLine();

                try
                {
                    int user_input_num = int.Parse(user_input_str);

                    switch (user_input_num)
                    {
                        case 1:
                            AddBattleLogStage();
                            Console.WriteLine("处理事务");
                            isquit = true;
                            break;
                        case 2:
                            FindBattleLogStage();
                            Console.WriteLine("处理事务");
                            isquit = true;
                            break;
                        default:
                            throw new FormatException();
                    }
                }
                catch (ArgumentNullException)
                {
                    continue;
                }
                catch (FormatException)
                {
                    if (IsQuit(user_input_str))
                    {
                        //返回上层，并退出整个程序
                        return true;
                    }
                    PrintCrossingLine();
                    PrintErrorMsg("请输入正确选项:");
                }
            }
        }

        private void AddBattleLogStage()
        {
            bool isquit = false;
            string attack = null, defend = null, msg = null;

            while(true)
            {
                if (isquit)
                {
                    return;
                }

                PrintCrossingLine();
                try
                {
                    //读取对战记录
                    Console.WriteLine("--------请输入防守队伍--------:");
                    defend = Console.ReadLine();
                    savefunc.HandleDefendMember(defend, con);

                    Console.WriteLine("--------请输入进攻队伍--------:");
                    attack = Console.ReadLine();
                    savefunc.HandleAttackMember(attack, con);

                    Console.WriteLine("--------请输入战斗备注--------:");
                    msg = Console.ReadLine();
                    savefunc.HandleMessage(msg);

                    savefunc.PrintAllLog();

                    savefunc.ExcuteSaveFunc(con);

                    isquit = true;
                    continue;
                }
                catch (FormatException ex1)
                { 
                    //如果用户选择退出
                    if ( (!string.IsNullOrEmpty(attack) && attack.ToLower().Equals("q")) ||
                         (!string.IsNullOrEmpty(defend) && defend.ToLower().Equals("q")) ||
                         (!string.IsNullOrEmpty(msg)    && msg.ToLower().Equals("q")) )
                    {
                        isquit = true;
                        continue;
                    }

                    PrintErrorMsg("请输入正确格式");
                    Console.WriteLine(ex1);
                    continue;
                }
                catch (ArgumentException ex2)
                {
                    Console.WriteLine(ex2);
                    continue;
                }
                catch (Exception ex3)
                {
                    Console.WriteLine(ex3);
                    continue;
                }
            }
        }

        private void FindBattleLogStage()
        {
            bool isquit = false;
            string defend = null;

            while (true)
            {
                if (isquit)
                {
                    return;
                }
                PrintCrossingLine();

                try
                {
                    //读取对战记录
                    Console.WriteLine("--------请输入防守队伍--------:");
                    defend = Console.ReadLine();
                    savefunc.HandleDefendMember(defend, con);
                }
            }
        }

        private void DeleteBattleLogStage()
        {
            //TODO

        }

        private void PrintMainChoice()
        {
            Console.WriteLine("请选择要进行的操作:");
            Console.WriteLine("1.添加战斗记录");
            Console.WriteLine("2.查询战斗记录");
            Console.WriteLine("----q or Q退出");
        }

        private void PrintCrossingLine()
        {
            Console.WriteLine("\n===============================\n");
        }

        private void PrintErrorMsg(string str)
        {
            Console.WriteLine(str);
        }

        private void PrintEndMsg()
        {
            Console.WriteLine("谢谢使用");
            Console.ReadLine();
        }

        private bool IsQuit(string str)
        {
            if (str.Equals("q") || str.Equals("Q"))
            {
                return true;
            }

            return false;
        }

        private void PrintChara()
        {
            //TODO
        }

        #endregion
    }
}
