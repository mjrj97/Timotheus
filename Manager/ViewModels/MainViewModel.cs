using Timotheus.IO;

namespace Timotheus.ViewModels
{
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Index of the currently open tab.
        /// </summary>
        public int CurrentTab { get; set; }

        private Register _keys = new();
        /// <summary>
        /// Register containing all the keys loaded at startup or manually from a key file (.tkey or .txt)
        /// </summary>
        public Register Keys
        {
            get
            {
                return _keys;
            }
            private set
            {
                _keys = value;
            }
        }

        private static MainViewModel s_instance;
        public static MainViewModel Instance
        {
            get
            {
                return s_instance;
            }
            private set
            {
                s_instance = value;
            }
        }

        /// <summary>
        /// Creates an instance of the MainViewModel
        /// </summary>
        public MainViewModel() {
            Instance = this;
        }

        public void NewProject(Register register)
        {
            Keys = register;
        }

        /// <summary>
        /// Loads the key from the path. Saves the path and password to the registry.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        /// <param name="password">The password used to decrypt the key.</param>
        public void LoadKey(string path, string password)
        {
            Keys = new Register(path, password, ':');
            Timotheus.Registry.Update("KeyPath", path);
        }
        /// <summary>
        /// Loads the key from the path.
        /// </summary>
        /// <param name="path">Path to the key file.</param>
        public void LoadKey(string path)
        {
            Keys = new Register(path, ':');
            Timotheus.Registry.Update("KeyPath", path);
            Timotheus.Registry.Delete("KeyPassword");
        }

        /// <summary>
        /// Save the key to the path.
        /// </summary>
        public void SaveKey(string path)
        {
            Keys.Save(path);
        }
        /// <summary>
        /// Save the encrypted key to the path.
        /// </summary>
        public void SaveKey(string path, string password)
        {
            Keys.Save(path, password);
        }
    }
}