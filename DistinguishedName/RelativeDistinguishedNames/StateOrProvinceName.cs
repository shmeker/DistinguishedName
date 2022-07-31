namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// State or province name known type.
	/// </summary>
	public sealed class StateOrProvinceName : SingleValued
	{
		/// <summary>
		/// Static getter for name.
		/// </summary>
		public static string Name => "ST";

		/// <summary>
		/// Static getter for dotted-decimal name.
		/// </summary>
		public static string DottedName => "2.5.4.8";

		/// <summary>
		/// Getter for Relative Distinguished Name type.
		/// </summary>
		public override RdnTypes RdnType => RdnTypes.StateOrProvinceName;

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

		private StateOrProvinceName()
		{
			Value = string.Empty;
		}

		/// <summary>
		/// Constructor with value parameter.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		public StateOrProvinceName(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor with value parameter and setting the dotted-decimal type.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		/// <param name="isDottedType">If true, the attribute type is of dotted-decimal format.</param>
		public StateOrProvinceName(string value, bool isDottedType) 
			: base(isDottedType)
		{
			Value = value;
		}
	}
}
