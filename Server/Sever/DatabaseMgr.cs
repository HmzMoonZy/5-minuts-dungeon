using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BeginEndServer.DataBase;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Dot;
using Server.Dto;

namespace Server
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class DatabaseMgr : DataMgr
    {
        //单例
        private static DatabaseMgr instance;
        public static DatabaseMgr _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseMgr();
                }
                return instance;
            }
        }

        //
        string mainTable = "maintable";

        private DatabaseMgr()
        {
            base.dbhost = "127.0.0.1";
            base.dbname = "five_minutes_dungeon";
            base.dbport = "3306";
            base.userid = "root";
            base.password = "000000";

            this.mainTable = "maintable";
        }

        /// <summary>
        /// 检查用户输入的字符是否可以注册
        /// -2:未知错误
        /// -1:字符非法
        /// 0 :已被注册
        /// 1 : 可以注册
        /// </summary>
        private int canRegister(string username)
        {
            //字符非法
            if (!IsSafeStr(username))
                return -1;

            //查询库
            string cmdStr = string.Format("SELECT * from {0} WHERE username = \"{1}\";", mainTable, username);
            MySqlCommand msc = new MySqlCommand(cmdStr, base.sqlConn);
            try
            {
                MySqlDataReader dr = msc.ExecuteReader();
                bool hasRows = dr.HasRows;
                dr.Close();

                //已被注册
                if (hasRows)
                    return 0;
                //可以注册
                else
                    return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -2;
            }
        }

        /// <summary>
        /// 注册操作 向数据库写入信息
        /// -2:未知错误
        /// -1:字符非法
        /// 0 :已被注册
        /// 1 : 可以注册
        /// </summary>
        /// <param name="account"></param>
        public int Register(string username, string password)
        {
            int result = canRegister(username);
            Console.WriteLine(result);
            if (result == 1)
            {
                string cmd = string.Format("insert into {0} set username =\"{1}\", password=\"{2}\";", mainTable, username, password);
                MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);
                try
                {
                    msc.ExecuteNonQuery();
                    Console.WriteLine(username + "注册成功");
                    return result;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return result;

        }

        /// <summary>
        /// 登录检查
        /// </summary>
        /// <param name="account"></param>
        public bool LoginCheck(string username, string password)
        {
            Console.WriteLine("用户试图登录,用户名为" + username);
            if (!IsSafeStr(username))
                //TODO Send();
                return false;

            string cmd = string.Format("select * from {0} where username=\"{1}\" and password=\"{2}\";", mainTable, username, password);
            MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);

            try
            {
                MySqlDataReader dr = msc.ExecuteReader();
                bool result = dr.HasRows;
                dr.Close();
                if (!result) return false;

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建游戏角色 -> 初始化一个Playdata并序列化至数据库中
        /// </summary>
        /// <param name="username"></param>
        public PlayerData CreatePlayer(string username, string nickName)
        {
            if (!IsSafeStr(username) || !IsSafeStr(nickName))
                return null;


            IFormatter formatter = new BinaryFormatter();
            PlayerData playerData = new PlayerData(nickName, 100);
            using (MemoryStream ms = new MemoryStream())
            {
                //将玩家数据序列化成内存字节流
                formatter.Serialize(ms, playerData);
                //将字节流转换会字节数组
                byte[] dataBuff = ms.ToArray();
                //写入数据库
                //string cmd = string.Format("insert into player set user_name=\"{0}\",data=\"{1}\";");
                //string cmd = string.Format("insert into {0} set username=\"{1}\",playerdata=@data;", mainTable, username);
                string cmd = string.Format("update {0} set playerdata=@data where username=\"{1}\";", mainTable, username);
                MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);
                msc.Parameters.Add("@data", MySqlDbType.Blob);   //定义一个名为@data,类型为Blob的字段
                msc.Parameters[0].Value = dataBuff;             //为该字段赋值
                msc.ExecuteNonQuery();                         //执行 => cmd中@data为上一句所赋值
            }
            return playerData;
        }

        public string GetUserID(string username)
        {
            if (!IsSafeStr(username))
                return null;
            string id = null;
            string cmd = string.Format("select id from \"{0}\" where username=\"{1}\";", mainTable, username, password);
            MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);
            try
            {
                MySqlDataReader dr = msc.ExecuteReader();
                bool result = dr.HasRows;
                if (!result)
                    return null;
                while (dr.Read())
                {
                    id = $"{dr["id"]}";
                }

                return id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取游戏角色数据
        /// </summary>
        public PlayerData LoadPlayerData(string username)
        {
            //if (!IsSafeStr(username))
            //    return playerData;

            PlayerData playerData = null;

            //查询
            string cmd = string.Format("select * from {0} where username=\"{1}\";", mainTable, username);
            MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);
            byte[] buffer = new byte[1];
            try
            {
                MySqlDataReader dr = msc.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    return null;
                }

                dr.Read();                  //读取下一行(第一行)

                //返回最大长度
                if (dr[3] == DBNull.Value)    //
                {
                    dr.Close();
                    return null;
                }

                long len = dr.GetBytes(3, 0, null, 0, 0); //不复制, 仅获取长度.
                buffer = new byte[len];
                dr.GetBytes(3, 0, buffer, 0, (int)len);
                dr.Close();

                //反序列化
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    playerData = bf.Deserialize(ms) as PlayerData;
                    return playerData;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public void SavePlayer(Player player)
        {
            string username = player.username;
            PlayerData playerData = player.playerdata as PlayerData;
            //序列化
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, playerData);
                    byte[] buffer = ms.ToArray();

                    //数据库
                    string cmd = string.Format("update {0} set playerdata=@data where username=\"{1}\";", mainTable, player.username);
                    MySqlCommand msc = new MySqlCommand(cmd, base.sqlConn);
                    msc.Parameters.Add("@data", MySqlDbType.Blob);
                    msc.Parameters[0].Value = buffer;
                    msc.ExecuteNonQuery();
                }

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    } 
}
