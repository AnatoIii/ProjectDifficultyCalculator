using System.IO;
using Newtonsoft.Json;

namespace ProjectDifficultyCalculator.Serializers
{
    public class JsonFileSerializer<T> : IFileSerializer<T>
    {
        public void Save(string path, T value)
        {
            using var streamWriter = new StreamWriter(path);
            var serialized = JsonConvert.SerializeObject(value);
            streamWriter.WriteLine(serialized);
        }

        public T Load(string path)
        {
            using var streamReader = new StreamReader(path);
            return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
        }
    }
}
