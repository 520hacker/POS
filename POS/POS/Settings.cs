using System;
using System.IO;
using Polenter.Serialization;
using Pos.Internals.UndoRedo.Collections.Generic;

namespace POS
{
    public static class Settings
    {
        private static UndoRedoDictionary<string, object> _data = new UndoRedoDictionary<string, object>();

        public static void Load()
        {
            var s = new SharpSerializer();

            if (File.Exists("settings.dat"))
            {
                _data = (UndoRedoDictionary<string, object>)s.Deserialize("settings.dat");
            }
        }

        public static T Get<T>(string name)
        {
            if (_data.ContainsKey(name))
            {
                return (T)_data[name];
            }
            
            return default(T);
        }

        public static void Set(string name, object value)
        {
            if (_data.ContainsKey(name))
            {
                _data[name] = value;
            }
            
            _data.Add(name, value);

            Save();

            return;
        }

        public static void Save()
        {
            var s = new SharpSerializer();

            s.Serialize(_data, "settings.dat");
        }
    }
}