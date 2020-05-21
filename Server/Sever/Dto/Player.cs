using BeginEndServer;
using BeginEndServer.Dto;
using Server.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Dot
{
    /// <summary>
    /// 玩家虚拟角色类
    /// </summary>
    public class Player : PlayerBase
    {

        public Player(string username, Conn conn)
        {
            base.username = username;
            base.conn = conn;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        public void LogOut()
        {
            //保存数据
            DatabaseMgr._Instance.SavePlayer(this);
            //下线
            conn.Disconnect();
        }
    }
}
