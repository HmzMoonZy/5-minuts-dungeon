using BeginEndServer;
using BeginEndServer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Dto
{
    /// <summary>
    /// 玩家临时数据, 无需保存在数据库中
    /// </summary>
    public class PlayerTempData : PlayerTempDataBase
    {
        public enum Status { None, Room, Game }

        public Status status;
        public Room room;
        public bool isRoomMaster = false;

        public PlayerTempData()
        {
            status = Status.None;
        }




    }
}
