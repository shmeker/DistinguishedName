namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Custom (unknown) Relative Distinguished Name type.
	/// </summary>
	public sealed class CustomRdn : SingleValued
	{
		private readonly string name;
		private readonly string value;

		/// <summary>
		/// Getter for Relative Distinguished Name type.
		/// </summary>
		public override RdnTypes RdnType => RdnTypes.CustomRdn;

		/// <summary>
		/// Getter for name.
		/// </summary>
		public override string NameString => name;

		/// <summary>
		/// Getter for dotted-decimal name.
		/// </summary>
		public override string NameDotted => name;

		/// <summary>
		/// Getter for value.
		/// </summary>
		public override string Value => value;

		private CustomRdn()
		{
			name = string.Empty;
			value = string.Empty;
		}

		/// <summary>
		/// Constructor with name and value parameters.
		/// </summary>
		/// <param name="name">String form of attribute type.</param>
		/// <param name="value">Readable string value.</param>
		public CustomRdn(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		/// <summary>
		/// Constructor with name and value parameters and setting the dotted-decimal type.
		/// </summary>
		/// <param name="name">String or dotted-decimal form of attribute type.</param>
		/// <param name="value">Readable string value.</param>
		/// <param name="isDottedType">If true, the attribute type is of dotted-decimal format.</param>
		public CustomRdn(string name, string value, bool isDottedType)
			: base(isDottedType)
		{
			this.name = name;
			this.value = value;
		}
	}
}
