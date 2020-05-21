using Server.Dot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Dto
{
    public class Room
    {
        //public string id = "000000";
        public int roomid;
        public string roomName = "TestName";

        public int maxNumber = 2;

        //public int level = 0;

        public int status = 0;  //0:等待 1:开始

        public Dictionary<string, Player> list = new Dictionary<string, Player>();

        public Room(int id) {
            this.roomid = id;
        }

        public Room(int id, string name)
        {
            this.roomid = id;
            this.roomName = name;
            this.maxNumber = 2;
            this.status = 0;
        }

        public bool AddPlayer(Player player)
        {
            lock (list)
            {
                if (list.Count >= maxNumber) return false;

                PlayerTempData tempData = (PlayerTempData)player.tempdata;
                tempData.room = this;
                tempData.status = PlayerTempData.Status.Room;
                player.tempdata = tempData;

                if (list.Count == 0) tempData.isRoomMaster = true;

                list.Add(player.username, player);
            }
            return true;
        }

        public void DelPlayer(Player player)
        {
            lock (list)
            {
                //已经不在房间
                if (!list.ContainsKey(player.username)) return;

                PlayerTempData td = (PlayerTempData)player.tempdata;
                td.status = PlayerTempData.Status.None;
                player.tempdata = td;
                list.Remove(player.username);
                if (list.Count > 0)
                {
                    //更新房主
                    if (td.isRoomMaster)
                    { 
                        Player p = list.Values.First();
                        PlayerTempData temp = (PlayerTempData)p.tempdata;
                        temp.isRoomMaster = true;
                        p.tempdata = temp;
                    }
                }
                else
                {
                    RoomMgr._Instance.RemoveRoom(this);
                }
            }
        }

        public void Broadcast(Message msg)
        {
            foreach (Player player in list.Values)
            {
                player.conn.Send(msg);
            }
        }

        public RoomProtocal ToProtocol()
        {
            RoomProtocal rp = new RoomProtocal();
            rp.maxNumber = this.maxNumber;
            rp.roomName = this.roomName;
            rp.status = this.status;
            rp.current = this.list.Count;
            rp.roomid = this.roomid;

            return rp;
        }

        public List<PlayerDataProtocol> GetRoomInfo()
        {
            List<PlayerDataProtocol> pds = new List<PlayerDataProtocol>();

            foreach (Player player in list.Values)
            {
                PlayerData data = player.playerdata as PlayerData;
                pds.Add(data.ToProtocol());
            }

            return pds;
        }
    }
}
