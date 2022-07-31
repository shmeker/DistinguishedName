namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Domain component known type.
	/// </summary>
	public sealed class DomainComponent : SingleValued
	{
		/// <summary>
		/// Static getter for name.
		/// </summary>
		public static string Name => "DC";

		/// <summary>
		/// Static getter for dotted-decimal name.
		/// </summary>
		public static string DottedName => "0.9.2342.19200300.100.1.25";

		/// <summary>
		/// Getter for Relative Distinguished Name type.
		/// </summary>
		public override RdnTypes RdnType => RdnTypes.DomainComponent;

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

		private DomainComponent()
		{
			Value = string.Empty;
		}

		/// <summary>
		/// Constructor with value parameter.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		public DomainComponent(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor with value parameter and setting the dotted-decimal type.
		/// </summary>
		/// <param name="value">Readable string value.</param>
		/// <param name="isDottedType">If true, the attribute type is of dotted-decimal format.</param>
		public DomainComponent(string value, bool isDottedType) 
			: base(isDottedType)
		{
			Value = value;
		}
	}
}
