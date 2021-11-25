using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebAppOOP.Core.ModelDTOS;
using WebAppOOP.Core.ModelDTOS.Interfaces;
using WebbAppOOP.Data.DataAccess.Interfaces;

namespace WebbAppOOP.Data.DataAccess
{
    public class EntityDataAccess_JSON<T> : IDataAccess<T> where T : IEntity
    {
        private readonly IConfiguration _configuration;

        public EntityDataAccess_JSON(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Add(T item)
        {
            var list = Read();
            list.Add(item);
            Write(list);
        }

        public List<T> GetAll() => Read().OrderBy(x => x.Id).ToList();

        public T GetById(int id) => Read().FirstOrDefault(x => x.Id == id);


        public T GetByKey(string key) => Read().FirstOrDefault(x => x.Key == key);

        public int GetNewID()
        {
            var tmp = Read();
            if (tmp?.Count > 0)
            {
                return tmp.Max(x => x.Id) + 1;
            }
            return 1;
        }


        public void Remove(T item)
        {
            var list = Read();
            list.RemoveAll(x => x.Id == item.Id);
            Write(list);
        }

        public void Update(T item)
        {
            var list = Read();
            var itemToUpdate = list.FirstOrDefault(x => x.Id == item.Id);
            if (itemToUpdate is not null)
            {
                list.Remove(itemToUpdate);
                list.Add(item);
                Write(list);
            }         
        }

        private void Write(List<T> list)
        {
            var jsonString = JsonConvert.SerializeObject(list);
            File.WriteAllText(GetPath(), jsonString);
        }
        private List<T> Read()
        {
            try
            {
                var jsonResponse = File.ReadAllText(GetPath());
                var jsonObjectList = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
                if (jsonObjectList is not null) return jsonObjectList;
                return new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
        private string GetPath()
        {
            Type type = typeof(T);
            string path;
            if (type == typeof(User))
            {
                path = _configuration["UserPath"];
            }
            else if (type == typeof(Order))
            {
                path = _configuration["OrderPath"];
            }
            else if (type == typeof(Reciept))
            {
                path = _configuration["RecieptPath"];
            }
            else
            {
                path = _configuration["ShoppingCartPath"];
            }

            return path;
        }
    }
}
