using BeginEndServer;
using Server.Dot;
using Server.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logic
{
    public class GameHandler : IMessageHandler
    {
        private static GameHandler instance;
        public static GameHandler _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameHandler();
                }
                return instance;
            }
        }

        public void ProcessData(Conn conn, int subCode, object parm)
        {
            switch (subCode)
            {
                case GameSubCode.RequestData:
                    processRequestData(conn, parm);
                    break;

                case GameSubCode.CreateData:
                    processCreateData(conn, parm);
                    break;

                case GameSubCode.GetRoomList:
                    processGetRoomList(conn, parm);
                    break;

                case GameSubCode.CreateRoom:
                    processCreateRoom(conn, parm);
                    break;

                case GameSubCode.EnterRoom:
                    processEnterRoom(conn, parm);
                    break;

                case GameSubCode.GetRoomInfo:
                    processGetRoomInfo(conn, parm);
                    break;

                case GameSubCode.LeaveRoom:
                    processleaveRoom(conn, parm);
                    break;
            }

        }

        /// <summary>
        /// 处理退出房间请求
        /// </summary>
        private void processleaveRoom(Conn conn, object parm)
        {
            Player player = (Player)conn.player;
            PlayerTempData tempData = (PlayerTempData)player.tempdata;
            
            if (tempData.status != PlayerTempData.Status.Room) 
            {
                conn.Send(OpCode.GAME, GameSubCode.LeaveRoom, -1);
                return;
            }

            Room room = tempData.room;
            RoomMgr._Instance.LeaveRoom(player);
            conn.Send(OpCode.GAME, GameSubCode.LeaveRoom, 1);

            if (room != null)
            {
                Message msg = new Message(OpCode.GAME, GameSubCode.GetRoomInfo, room.GetRoomInfo());
                room.Broadcast(msg);
            }
        }

        /// <summary>
        /// 处理获取房间信息请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processGetRoomInfo(Conn conn, object parm)
        {
            Player player = (Player)conn.player;
            PlayerTempData ptd = (PlayerTempData)player.tempdata;

            if (ptd.status != PlayerTempData.Status.Room)
            {
                return;
            }
            Room room = ptd.room;
            conn.Send(OpCode.GAME, GameSubCode.GetRoomInfo, room.GetRoomInfo());

        }

        /// <summary>
        /// 处理创建房间请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processCreateRoom(Conn conn, object parm)
        {
            Player player = (Player)conn.player;
            string roomName = parm as string;
            PlayerTempData tempData = (PlayerTempData)player.tempdata;
            
            if (tempData.status != PlayerTempData.Status.None)
            {
                conn.Send(OpCode.GAME, GameSubCode.CreateRoom, -1);
                return;
            }

            RoomMgr._Instance.CreateRoom(player, roomName);
            
            conn.Send(OpCode.GAME, GameSubCode.CreateRoom, 1);
        }

        /// <summary>
        /// 处理进入房间请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processEnterRoom(Conn conn, object parm)
        {
            int roomid = (int)parm;
            Console.WriteLine(conn.player.username + "试图加入房间" + roomid); 
            if (roomid < 0 || roomid >= RoomMgr._Instance.roomList.Count)
            {
                conn.Send(OpCode.GAME, GameSubCode.EnterRoom, null);
                return;
            }

            Room room = RoomMgr._Instance.roomList[roomid];

            if (room.status != 0)
            {
                conn.Send(OpCode.GAME, GameSubCode.EnterRoom, null);
                return;
            }

            Player player = (Player)conn.player;

            if (!room.AddPlayer(player))
            {
                conn.Send(OpCode.GAME, GameSubCode.EnterRoom, null);
                return;
            }

            conn.Send(OpCode.GAME, GameSubCode.EnterRoom, 1);
            Message msg = new Message(OpCode.GAME, GameSubCode.GetRoomInfo, room.GetRoomInfo());
            room.Broadcast(msg);
        }

        /// <summary>
        /// 处理返回房间列表请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processGetRoomList(Conn conn, object parm)
        {
            RoomListProtocol roomlist = RoomMgr._Instance.ToProtocol(false);
            if (roomlist == null)
            {
                conn.Send(OpCode.GAME, GameSubCode.GetRoomList, null);
                return;
            }
                conn.Send(OpCode.GAME, GameSubCode.GetRoomList, roomlist);

        }

        /// <summary>
        /// 处理创建数据请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processCreateData(Conn conn, object parm)
        {
            PlayerData playerData = DatabaseMgr._Instance.CreatePlayer(conn.player.username, parm as string);
            conn.player.playerdata = playerData;
            conn.player.tempdata = new PlayerTempData();

            conn.Send(OpCode.GAME, GameSubCode.CreateData, playerData.ToProtocol());
        }

        /// <summary>
        /// 处理请求数据请求
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parm"></param>
        private void processRequestData(Conn conn, object parm)
        {
            string username = conn.player.username;
            //获取角色数据
            PlayerData pd = DatabaseMgr._Instance.LoadPlayerData(username);

            try
            {
                if (pd != null)
                {
                    conn.player.playerdata = pd;
                    conn.player.tempdata = new PlayerTempData();
                    conn.Send(OpCode.GAME, GameSubCode.RequestData, pd.ToProtocol());
                    return;
                }
                conn.Send(OpCode.GAME, GameSubCode.RequestData, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
