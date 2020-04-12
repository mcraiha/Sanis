namespace Generoija
{
	public sealed class Translation
	{
		/// <summary>
		/// Translations
		/// </summary>
		/// <value></value>
		public string[] t { get; set; }

		/// <summary>
		/// Links, o (original only), t (translation only) and b (both)
		/// </summary>
		/// <value></value>
		public string[] l { get; set; }
	} 
}