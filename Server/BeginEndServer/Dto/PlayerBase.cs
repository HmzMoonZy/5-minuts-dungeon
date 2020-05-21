using BeginEndServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer.Dto
{
    /// <summary>
    /// 虚拟角色类
    /// 代表一个玩家的虚拟角色
    /// </summary>
    public class PlayerBase
    {
        public string username;

        public PlayerDataBase playerdata;   //角色需要保存的游戏数据

        public PlayerTempDataBase tempdata; //角色需要保存的游戏数据

        public Conn conn;                 //与框架中的Conn相互持有
    }
}
