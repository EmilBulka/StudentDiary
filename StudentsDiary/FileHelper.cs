using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
   public class FileHelper<T> where T : new()
    {
        private string _filePath;

        public FileHelper(string _filePath)
        {
           this._filePath = _filePath;
        }

        public void SerializeToFile(T Students)
        {

            var serializer = new XmlSerializer(typeof(T));
            StreamWriter streamWriter = null;

            try
            {

                streamWriter = new StreamWriter(_filePath);
                serializer.Serialize(streamWriter, Students);
                streamWriter.Close();
                streamWriter.Dispose();
            }

            finally
            {
                streamWriter.Dispose();
            }

        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new T();
            }

            var deserializer = new XmlSerializer(typeof(T));
            StreamReader streamReader = null;

            try
            {

                streamReader = new StreamReader(_filePath);
                var students = (T)deserializer.Deserialize(streamReader);
                streamReader.Close();
                return students;



            }

            finally
            {
                streamReader.Dispose();

            }
        }



        

    }
}
