﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Snapper.Core
{
    public interface ISnapStore
    {
        object GetSnap(string path);

        void StoreSnap(string path, object value);
    }

    public class ByteSnapStore : ISnapStore
    {
        public object GetSnap(string path)
            => File.Exists(path) ? StringToObject(File.ReadAllText(path)) : null;

        public void StoreSnap(string path, object value)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, ObjectToString(value));
        }

        private static object StringToObject(string data)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }

        private static string ObjectToString(object obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            var bytes = ms.ToArray();
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}