using Google.Protobuf;
using Google.Protobuf.Protocol;
using Microsoft.EntityFrameworkCore;
using Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server.Game
{
    public class GameWorld : JobSerializer
    {
        // all online players
        Dictionary<int, Player> _players = new Dictionary<int, Player>();


        public void Update()
        {
            Flush();
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            if (type == GameObjectType.Player)
            {
                Player myPlayer = gameObject as Player;
                _players.Add(gameObject.Id, myPlayer);

                // my player -> my client
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.MyPlayer = myPlayer.ObjectInfo;
                    enterPacket.MyPlayer.PlayerInfo = myPlayer.PlayerInfo;
                    foreach (PlayerInfo friend in myPlayer.Friends.Values)
                    {
                        enterPacket.Friends.Add(friend);
                    }
                    myPlayer.Session.Send(enterPacket);
                }

                // other players -> my client
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player player in _players.Values)
                    {
                        if (player.PlayerDbId != myPlayer.PlayerDbId)
                        {
                            ObjectInfo objectInfo = player.ObjectInfo;
                            objectInfo.PlayerInfo = player.PlayerInfo;
                            spawnPacket.Objects.Add(objectInfo);
                        }
                    }
                    myPlayer.Session.Send(spawnPacket);
                }

                // my player -> other clients
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    ObjectInfo objectInfo = myPlayer.ObjectInfo;
                    objectInfo.PlayerInfo = myPlayer.PlayerInfo;
                    spawnPacket.Objects.Add(gameObject.ObjectInfo);
                    Broadcast(spawnPacket, gameObject.Id);
                }
            }

            // TODO : 다른 오브젝트들 입장
        }

        public void LeaveGame(int objectId)
        {
            GameObjectType type = ObjectManager.GetObjectTypeById(objectId);

            if (type == GameObjectType.Player)
            {
                Player player = null;
                if (_players.Remove(objectId, out player) == false)
                    return;

                // Update token validation
                string tokenKey;
                using (AppDbContext db = new AppDbContext())
                {
                    AccountDb findAccount = db.Accounts
                        .AsNoTracking()
                        .Where(a => a.Name == player.Name)
                        .FirstOrDefault();
                    tokenKey = findAccount.AccountId;
                }
                using (RedisDb cache = new RedisDb())
                {
                    cache.Set($"{tokenKey}Where", "end");
                }

                // TODO : 종료 전 DB 저장

                // my player -> my client 
                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    player.Session.Send(leavePacket);
                }

                // my player -> other clients
                {
                    if (player.PlayerInfo.Place == Place.Outdoor)
                    {
                        S_Despawn despawnPacket = new S_Despawn();
                        despawnPacket.ObjectId = objectId;
                        despawnPacket.Name = player.Name;
                        Broadcast(despawnPacket, objectId);
                    }
                    else
                    {
                        S_DespawnIndoor despawnIndoorPacket = new S_DespawnIndoor();
                        despawnIndoorPacket.ObjectId = objectId;
                        despawnIndoorPacket.Name = player.Name;
                        Broadcast(despawnIndoorPacket, objectId);
                    }
                }
            }
        }

        public void HandleMove(Player myPlayer, C_Move packet)
        {
            myPlayer.Position.X = packet.Position.X;
            myPlayer.Position.Y = packet.Position.Y;
            myPlayer.Position.Z = packet.Position.Z;
            myPlayer.RotationY = packet.RotationY;

            S_Move movePacket = new S_Move();
            movePacket.ObjectId = myPlayer.Id;
            movePacket.Position = myPlayer.Position;
            movePacket.RotationY = myPlayer.RotationY;
            movePacket.TargetSpeed = packet.TargetSpeed;
            movePacket.TargetRotation = packet.TargetRotation;
            movePacket.Jump = packet.Jump;
            BroadcastSamePlace(movePacket, myPlayer.Id);
        }

        public void HandleChat(ClientSession session, C_Chat packet)
        {
            S_Chat chatPacket = new S_Chat()
            {
                ObjectId = session.MyPlayer.Id,
                Message = packet.Message
            };
            BroadcastSamePlace(chatPacket, session.MyPlayer.Id);
        }

        public void HandleEmotion(int objectId, C_Emotion packet)
        {
            S_Emotion emotionPacket = new S_Emotion()
            {
                ObjectId = objectId,
                EmotionNum = packet.EmotionNum
            };
            Broadcast(emotionPacket, objectId);
        }

        public void HandleEnterIndoor(ClientSession session, C_EnterIndoor packet)
        {
            // my player -> other clients
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectId = session.MyPlayer.Id;
                despawnPacket.Name = session.MyPlayer.Name;
                Broadcast(despawnPacket, session.MyPlayer.Id);
            }

            session.MyPlayer.Place = packet.Place;
            session.MyPlayer.Position.X = -13;
            session.MyPlayer.Position.Y = -1.5f;
            session.MyPlayer.Position.Z = -10;
            
            // my player -> my client
            {
                S_EnterIndoor enterIndoorPacket = new S_EnterIndoor();
                {
                    enterIndoorPacket.MyPlayer = session.MyPlayer.ObjectInfo;
                    enterIndoorPacket.MyPlayer.PlayerInfo = session.MyPlayer.ObjectInfo.PlayerInfo;
                    foreach (PlayerInfo friend in session.MyPlayer.Friends.Values)
                    {
                        enterIndoorPacket.Friends.Add(friend);
                    }
                }
                session.Send(enterIndoorPacket);
            }

            // other players -> my client
            {
                S_SpawnIndoor spawnIndoorPacket = new S_SpawnIndoor();
                foreach (Player player in _players.Values)
                {
                    if (player.PlayerDbId != session.MyPlayer.PlayerDbId)
                    {
                        ObjectInfo objectInfo = player.ObjectInfo;
                        objectInfo.PlayerInfo = player.PlayerInfo;
                        spawnIndoorPacket.Objects.Add(objectInfo);
                    }
                }
                session.Send(spawnIndoorPacket);
            }

            // my player -> other clients
            {
                S_SpawnIndoor spawnIndoorPacket = new S_SpawnIndoor();
                ObjectInfo objectInfo = session.MyPlayer.ObjectInfo;
                objectInfo.PlayerInfo = session.MyPlayer.PlayerInfo;
                spawnIndoorPacket.Objects.Add(objectInfo);
                Broadcast(spawnIndoorPacket, session.MyPlayer.Id);
            }
        }

        public void HandleMoveIndoor(ClientSession session, C_MoveIndoor packet)
        {
            Player myPlayer = session.MyPlayer;

            myPlayer.Position.X = packet.PosX;

            S_MoveIndoor moveIndoorPacket = new S_MoveIndoor()
            {
                ObjectId = myPlayer.Id,
                PosX = myPlayer.Position.X,
                MoveX = packet.MoveX
            };
            BroadcastSamePlace(moveIndoorPacket, myPlayer.Id);
        }

        public void HandleLeaveIndoor(ClientSession session)
        {
            // my player -> other clients
            {
                S_DespawnIndoor despawnIndoorPacket = new S_DespawnIndoor();
                despawnIndoorPacket.ObjectId = session.MyPlayer.Id;
                despawnIndoorPacket.Name = session.MyPlayer.Name;
                Broadcast(despawnIndoorPacket, session.MyPlayer.Id);
            }

            // Set player's outdoor position
            switch (session.MyPlayer.Place)
            {
                case Place.IndoorBima:
                    session.MyPlayer.Position.X = 53;
                    session.MyPlayer.Position.Y = 20;
                    session.MyPlayer.Position.Z = -72;
                    break;
                case Place.IndoorHanwool:
                    session.MyPlayer.Position.X = -225;
                    session.MyPlayer.Position.Y = 5;
                    session.MyPlayer.Position.Z = 142;
                    break;
                case Place.IndoorHwado:
                    session.MyPlayer.Position.X = 30;
                    session.MyPlayer.Position.Y = 24;
                    session.MyPlayer.Position.Z = 68;
                    break;
                case Place.IndoorLibrary:
                    session.MyPlayer.Position.X = -71;
                    session.MyPlayer.Position.Y = 7;
                    session.MyPlayer.Position.Z = -5;
                    break;
                case Place.IndoorOgui:
                    session.MyPlayer.Position.X = -25;
                    session.MyPlayer.Position.Y = 14;
                    session.MyPlayer.Position.Z = -144;
                    break;
                case Place.IndoorSaebit:
                    session.MyPlayer.Position.X = 205;
                    session.MyPlayer.Position.Y = 30;
                    session.MyPlayer.Position.Z = -11;
                    break;
            }
            session.MyPlayer.Place = Place.Outdoor;

            // my player -> my client
            {
                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.MyPlayer = session.MyPlayer.ObjectInfo;
                enterPacket.MyPlayer.PlayerInfo = session.MyPlayer.PlayerInfo;
                foreach (PlayerInfo friend in session.MyPlayer.Friends.Values)
                {
                    enterPacket.Friends.Add(friend);
                }
                session.MyPlayer.Session.Send(enterPacket);
            }

            // other players -> my client
            {
                S_Spawn spawnPacket = new S_Spawn();
                foreach (Player player in _players.Values)
                {
                    if (player.PlayerDbId != session.MyPlayer.PlayerDbId)
                    {
                        ObjectInfo objectInfo = player.ObjectInfo;
                        objectInfo.PlayerInfo = player.PlayerInfo;
                        spawnPacket.Objects.Add(objectInfo);
                    }
                }
                session.MyPlayer.Session.Send(spawnPacket);
            }

            // my player -> other clients
            {
                S_Spawn spawnPacket = new S_Spawn();
                ObjectInfo objectInfo = session.MyPlayer.ObjectInfo;
                objectInfo.PlayerInfo = session.MyPlayer.PlayerInfo;
                spawnPacket.Objects.Add(objectInfo);
                Broadcast(spawnPacket, session.MyPlayer.Id);
            }
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player player in _players.Values)
            {
                player.Session.Send(packet);
            }
        }
        public void Broadcast(IMessage packet, int id)
        {
            foreach (Player player in _players.Values)
            {
                if (player.Id == id)
                    continue;
                player.Session.Send(packet);
            }
        }
        public void BroadcastSamePlace(IMessage packet, int id)
        {
            Player myPlayer = _players[id];
            foreach (Player player in _players.Values)
            {
                if (player.Id == id)
                    continue;
                if (player.Place == myPlayer.Place)
                    player.Session.Send(packet);
            }
        }
    }
}
