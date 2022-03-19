using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class ObjectManager
    {
        #region Singleton
        static ObjectManager _instance = new ObjectManager();
        public static ObjectManager Instance { get { return _instance; } }
        #endregion

        // [UNUSED(1)|TYPE(7)|ID(24)]
        int _id = 0;
        Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

        object _lock = new object();


        int GenerateId(GameObjectType type)
        {
            lock (_lock)
            {
                return ((int)type << 24) | (_id++);
            }
        }

        public static GameObjectType GetObjectTypeById(int id)
        {
            int type = (id >> 24) & 0x7F;
            return (GameObjectType)type;
        }

        public T Add<T>() where T : GameObject, new()
        {
            T gameObject = new T();
            gameObject.Id = GenerateId(gameObject.ObjectType);

            lock (_lock)
            {
                _objects.Add(gameObject.Id, gameObject);
            }

            return gameObject;
        }

        public bool Remove(int objectId)
        {
            lock (_lock)
            {
                return _objects.Remove(objectId);
            }
        }

        public T Find<T>(int objectId) where T : GameObject
        {
            lock (_lock)
            {
                GameObject gameObject = null;
                if (_objects.TryGetValue(objectId, out gameObject))
                    return gameObject as T;
            }

            return null;
        }
    }
}
