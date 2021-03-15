using System;

namespace Timotheus.Utility
{
    /// <summary>
    /// Object that stores the data of a consent form (e.g. the name of the person).
    /// </summary>
    public class ConsentForm
    {
        /// <summary>
        /// Name of the person who signed the form.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Date when the consent was signed.
        /// </summary>
        public DateTime Signed { get; set; }
        /// <summary>
        /// The versions of consent forms are usually identified by the date of printing/publishing.
        /// </summary>
        public DateTime Version { get; set; }
        /// <summary>
        /// This field is used for data not appropriate for the other fields.
        /// </summary>
        public string Comment { get; set; }

        //Constructors
        public ConsentForm(string Name, DateTime Signed, DateTime Version, string Comment)
        {
            this.Name = Name;
            this.Signed = Signed;
            this.Version = Version;
            this.Comment = Comment;
        }
        public ConsentForm(string Name, DateTime Signed, DateTime Version) : this(Name, Signed, Version, string.Empty) { }
    }
}