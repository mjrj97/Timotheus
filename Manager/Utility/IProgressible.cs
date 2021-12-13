using System.ComponentModel;

namespace Timotheus.Utility
{
    public interface IProgressible
    {
        public BackgroundWorker Worker { get; }
    }
}