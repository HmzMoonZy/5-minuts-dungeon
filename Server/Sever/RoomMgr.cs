using Server.Dot;
using Server.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{



    class RoomMgr
    {
        private static RoomMgr instance;
        public static RoomMgr _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoomMgr();
                }
                return instance;
            }
        }

        private RoomMgr() { }

        public List<Room> roomList = new List<Room>();


        public void CreateRoom(Player player, string roomName)
        {
            Room room = new Room(roomList.Count, roomName);
            lock (roomList)
            {
                roomList.Add(room);
                room.AddPlayer(player);
            }
        }

        public void LeaveRoom(Player player)
        {
            PlayerTempData tempdata = (PlayerTempData)player.tempdata;

            if (tempdata.status == PlayerTempData.Status.None) return;

            lock (roomList)
            {
                tempdata.room.DelPlayer(player);
            }

        }

        public RoomListProtocol ToProtocol(bool testMode = false)
        {
            RoomListProtocol rlp = new RoomListProtocol();

            if (testMode)
            {
                for (int i = 0; i < 10; i++)
                {
                    Room r = new Room(i,"TestRoom" + i);
                    
                    rlp.list.Add(r.ToProtocol());
                }
                return rlp;
            }

            if (roomList.Count <= 0)
            {
                return null;
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomProtocal rp = roomList[i].ToProtocol();
                rlp.list.Add(rp);
            }
            return rlp;
        }

        public void RemoveRoom(Room room)
        {
            roomList.Remove(room);
        }
    }
}
