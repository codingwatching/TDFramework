﻿using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TDFramework.Resource;

namespace TDFramework.Table
{
    public abstract class AbstractDataTable<T,P>where T:class,new() where P:TableDataBase 
    {
        protected List<P> mDataList = new List<P>();
        protected Dictionary<int, P> mDataDic = new Dictionary<int, P>();

        public AbstractDataTable()
        {
            Load();
        }


        public abstract string FileName { get; }

        public abstract P ReadData(JsonData data);

        private void Load()
        {
            var handle = GameEntry.Res.LoadAssetAsync<TextAsset>(FileName);
            handle.Completed += p =>
            {
                if (p.Result != null)
                {
                    string jsonData = Tool.StringEncryption.DecryptDES(p.Result.text);
                    JsonData data = JsonMapper.ToObject(jsonData);
                    foreach (JsonData item in data)
                    {
                        P tableData = ReadData(item);
                        mDataList.Add(tableData);
                        mDataDic.Add(tableData.ID, tableData);
                    }
                }
                UnityEngine.AddressableAssets.Addressables.Release(handle);
            };

        }


        public List<P> GetList()
        {
            return mDataList;
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="isCopy">是否序列化出来一份新的</param>
        /// <returns></returns>
        public P Get(int id,bool isCopy=true)
        {
            if (mDataDic.ContainsKey(id))
            {
                if (isCopy)
                    return Copy<P>(mDataDic[id]);
                else
                    return mDataDic[id];
            }
                
            else
                return null;
        }


        

        public  P Copy<P>(P RealObject)where P:TableDataBase
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (P)formatter.Deserialize(objectStream);
            }
        }

       
    }
}
