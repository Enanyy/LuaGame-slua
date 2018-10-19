using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public enum ConnectID
    {
        Logic,
    }

 
    public class NetworkManager
    {
        #region Singleton
        private NetworkManager() { }
        static NetworkManager mInstance;
        public static NetworkManager GetSingleton()
        {
            if(mInstance==null)
            {
                mInstance = new NetworkManager();
            }
            return mInstance;
        }
        #endregion
        private Dictionary<int, Connection> mConnectionDic = new Dictionary<int, Connection>();

        private List<Connection> mConnectionList = new List<Connection>();

        public void Connect(ConnectID varConnectID, string ip, int port, OnConnectionHandler onConnect, OnConnectionHandler onDisconnect)
        {
            int id = (int)varConnectID;

            if(mConnectionDic.ContainsKey(id))
            {
                Connection client = mConnectionDic[id];
                mConnectionDic.Remove(id);
                client.Close(true);
            }

            Connection c = new Connection(varConnectID);

            c.onConnect += onConnect;
            c.onDisconnect += onDisconnect;
            c.onDebug += OnDebug;

            c.onReceive += OnMessage;

            c.Connect(ip, port);

            mConnectionDic.Add(id, c);
        }

        public Connection GetConnection(ConnectID clientID)
        {
            int id = (int)clientID;
            if(mConnectionDic.ContainsKey(id))
            {
                return mConnectionDic[id];
            }
            return null;
        }
       
        
        public void Update()
        {
            mConnectionList.AddRange(mConnectionDic.Values);
            for(int i = 0; i < mConnectionList.Count; ++i)
            {
                if(mConnectionList[i]!=null)
                {
                    mConnectionList[i].Update();
                }
            }
            mConnectionList.Clear();
        }    



        /// <summary>
        /// 通过TCP协议发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageId"></param>
        /// <param name="data"></param>
        public void Send<T>(ConnectID clientID, int id, T data) where T : class, ProtoBuf.IExtensible
        {
            var client = GetConnection(clientID);

            if (client == null)
            {
                return;
            }

            byte[] bytes = ProtoTransfer.SerializeProtoBuf<T>(data);

            client.Send(bytes, (ushort)bytes.Length);
        }



        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close(ConnectID connectID)
        {
            if(mConnectionDic.ContainsKey((int)connectID))
            {
                var c = mConnectionDic[(int)connectID];
                mConnectionDic.Remove((int)connectID);
                c.Close(true);
            }
        }

        /// <summary>
        /// 断开所有连接
        /// </summary>
        public void Close()
        {
            foreach(var  v in mConnectionDic.Values)
            {
                v.Close(true);
            }
            mConnectionDic.Clear();
        }

        private void OnMessage(ConnectID id, byte[] data)
        {
            if(data == null)
            {
                return;
            }
            //if (Debuger.EnableLog)
            //{
            //    OnDebug(string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(Color.green), "Receive:" + (ClientMsg.CLIENT_MSG_ID)msg.ID()));
            //}
            //MessageDispatch.Dispatch(msg);
        }

        private void OnDebug(string message)
        {
            //if (Debuger.EnableLog)
            {
                Debug.Log(message);
            }
        }
    }
}
