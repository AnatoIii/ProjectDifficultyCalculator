namespace ProjectDifficultyCalculator.Serializers
{
    public interface IFileSerializer<T>
    {
        void Save(string path, T value);
        T Load(string path);
    }
}
