using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameWorld : JobSerializer
    {
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

                // TODO : 종료 전 DB 저장

                // 본인한테 정보 전송
                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    player.Session.Send(leavePacket);
                }
            }

            // 본인 정보 boradcast
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectId = objectId;
                Broadcast(despawnPacket);
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
            Broadcast(movePacket, myPlayer.Id);
        }

        public void HandleChat(int objectId, C_Chat packet)
        {
            S_Chat chatPacket = new S_Chat()
            {
                ObjectId = objectId,
                Message = packet.Message
            };
            Broadcast(chatPacket, objectId);
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
    }
}
