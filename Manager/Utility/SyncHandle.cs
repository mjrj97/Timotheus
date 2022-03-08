namespace Timotheus.Utility
{
    /// <summary>
    /// Enum that tells the software how to handle a file on sync.
    /// </summary>
    public enum SyncHandle
    {
        Nothing,
        Synchronize,
        NewDownload,
        Download,
        NewUpload,
        Upload,
        DeleteLocal,
        DeleteRemote
    }
}