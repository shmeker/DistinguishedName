namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Enumerator with Relative Distinguished Name types.
	/// </summary>
	public enum RdnTypes
	{
		/// <summary>
		/// Unknown type.
		/// </summary>
		CustomRdn = 0,

		/// <summary>
		/// Common name - CN.
		/// </summary>
		CommonName = 1,

		/// <summary>
		/// Country name - C.
		/// </summary>
		CountryName = 2,

		/// <summary>
		/// Domain component - DC.
		/// </summary>
		DomainComponent = 3,

		/// <summary>
		/// Locality name - L.
		/// </summary>
		LocalityName = 4,

		/// <summary>
		/// Organizational unit name - OU.
		/// </summary>
		OrganizationalUnitName = 5,

		/// <summary>
		/// Organization name - O.
		/// </summary>
		OrganizationName = 6,

		/// <summary>
		/// State or province name - ST.
		/// </summary>
		StateOrProvinceName = 7,

		/// <summary>
		/// Street address - STREET.
		/// </summary>
		StreetAddress = 8,

		/// <summary>
		/// User ID - UID.
		/// </summary>
		UserId = 9,
	}
}
