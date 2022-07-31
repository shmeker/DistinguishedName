namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// User ID known type.
	/// </summary>
	public sealed class UserId : SingleValued
	{
		/// <summary>
		/// Static getter for name.
		/// </summary>
		public static string Name => "UID";

		/// <summary>
		/// Static getter for dotted-decimal name.
		/// </summary>
		public static string DottedName => "0.9.2342.19200300.100.1.1";

		/// <summary>
		/// Getter for Relative Distinguished Name type.
		/// </summary>
		public override RdnTypes RdnType => RdnTypes.UserId;

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

		private UserId()
		{
			Value = string.Empty;
		}

		/// <summary>
		/// Constructor with value parameter.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		public UserId(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor with value parameter and setting the dotted-decimal type.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		/// <param name="isDottedType">If true, the attribute type is of dotted-decimal format.</param>
		public UserId(string value, bool isDottedType) 
			: base(isDottedType)
		{
			Value = value;
		}
	}
}
