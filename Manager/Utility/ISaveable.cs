namespace Timotheus.Utility
{
    public interface ISaveable
    {
        void Save(string path);

        bool HasBeenChanged();
    }
}