namespace Timotheus.ViewModels
{
	public class TabViewModel : ViewModel
	{
		private string _path = string.Empty;
		/// <summary>
		/// Path of that the viewmodel uses.
		/// </summary>
		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
				NotifyPropertyChanged(nameof(Path));
			}
		}
	}
}
