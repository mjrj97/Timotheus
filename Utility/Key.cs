namespace Timotheus.Utility
{
    /// <summary>
    /// A key is named and has a corresponding value. Can be used as holding a password ie. Password,12345 or localization etc.
    /// </summary>
    public class Key
    {
        /// <summary>
        /// Name of the key, could be "Password"
        /// </summary>
        public readonly string name;
        
        /// <summary>
        /// Value of the key, could be a password.
        /// </summary>
        public readonly string value;

        /// <summary>
        /// Constructor to create a key with a name and value.
        /// </summary>
        /// <param name="name">Designates the name of the key. Is used when trying to get the value.</param>
        /// <param name="value">The value assigned to this key.</param>
        public Key(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}