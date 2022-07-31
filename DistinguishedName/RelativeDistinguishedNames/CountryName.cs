namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Country name known type.
	/// </summary>
	public sealed class CountryName : SingleValued
	{
		/// <summary>
		/// Static getter for name.
		/// </summary>
		public static string Name => "C";

		/// <summary>
		/// Static getter for dotted-decimal name.
		/// </summary>
		public static string DottedName => "2.5.4.6";

		/// <summary>
		/// Getter for Relative Distinguished Name type.
		/// </summary>
		public override RdnTypes RdnType => RdnTypes.CountryName;

		/// <summary>
		/// Getter for name.
		/// </summary>
		public override string NameString => Name;

		/// <summary>
		/// Getter for dotted-decimal name.
		/// </summary>
		public override string NameDotted => DottedName;

		/// <summary>
		/// Getter for value.
		/// </summary>
		public override string Value { get; }

		private CountryName()
		{
			Value = string.Empty;
		}

		/// <summary>
		/// Constructor with value parameter.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		public CountryName(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor with value parameter and setting the dotted-decimal type.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		/// <param name="isDottedType">If true, the attribute type is of dotted-decimal format.</param>
		public CountryName(string value, bool isDottedType) 
			: base(isDottedType)
		{
			Value = value;
		}
	}
}
