using DistinguishedName.RelativeDistinguishedNames;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistinguishedName
{
	/// <summary>
	/// Static class that creates Relative Distinguished Name objects by assigned name.
	/// </summary>
	public static class RdnFactory
	{
		/// <summary>
		/// Creates a <see cref="SingleValued"/> object based upon the <paramref name="nameString"/>.
		/// </summary>
		/// <param name="nameString">The name string of the <see cref="SingleValued"/> object to create.</param>
		/// <param name="value">The value string of the <see cref="SingleValued"/> object to create.</param>
		/// <returns>The <see cref="SingleValued"/> object.</returns>
		/// <exception cref="ArgumentNullException">Throws if <paramref name="nameString"/> is not set.</exception>
		public static SingleValued Create(string nameString, string value)
		{
			if (string.IsNullOrEmpty(nameString))
			{
				throw new ArgumentNullException(nameof(nameString));
			}

			if (string.Compare(nameString, CommonName.Name, true) == 0)
			{
				return new CommonName(value);
			}
			else if (string.Compare(nameString, CountryName.Name, true) == 0)
			{
				return new CountryName(value);
			}
			else if (string.Compare(nameString, DomainComponent.Name, true) == 0)
			{
				return new DomainComponent(value);
			}
			else if (string.Compare(nameString, LocalityName.Name, true) == 0)
			{
				return new LocalityName(value);
			}
			else if (string.Compare(nameString, OrganizationalUnitName.Name, true) == 0)
			{
				return new OrganizationalUnitName(value);
			}
			else if (string.Compare(nameString, OrganizationName.Name, true) == 0)
			{
				return new OrganizationName(value);
			}
			else if (string.Compare(nameString, StateOrProvinceName.Name, true) == 0)
			{
				return new StateOrProvinceName(value);
			}
			else if (string.Compare(nameString, StreetAddress.Name, true) == 0)
			{
				return new StreetAddress(value);
			}
			else if (string.Compare(nameString, UserId.Name, true) == 0)
			{
				return new UserId(value);
			}

			return new CustomRdn(nameString, value);
		}

		/// <summary>
		/// Creates a <see cref="SingleValued"/> object based upon the <paramref name="dottedNumberString"/>.
		/// </summary>
		/// <param name="dottedNumberString">The dotted number string of the <see cref="SingleValued"/> object to create.</param>
		/// <param name="value">The value string of the <see cref="SingleValued"/> object to create.</param>
		/// <returns>The <see cref="SingleValued"/> object.</returns>
		/// <exception cref="ArgumentNullException">Throws if <paramref name="dottedNumberString"/> is not set.</exception>
		public static SingleValued CreateFromDottedNumberString(string dottedNumberString, string value)
		{
			if (string.IsNullOrEmpty(dottedNumberString))
			{
				throw new ArgumentNullException(nameof(dottedNumberString));
			}

			if (string.Compare(dottedNumberString, CommonName.DottedName, true) == 0)
			{
				return new CommonName(value, true);
			}
			else if (string.Compare(dottedNumberString, CountryName.DottedName, true) == 0)
			{
				return new CountryName(value, true);
			}
			else if (string.Compare(dottedNumberString, DomainComponent.DottedName, true) == 0)
			{
				return new DomainComponent(value, true);
			}
			else if (string.Compare(dottedNumberString, LocalityName.DottedName, true) == 0)
			{
				return new LocalityName(value, true);
			}
			else if (string.Compare(dottedNumberString, OrganizationalUnitName.DottedName, true) == 0)
			{
				return new OrganizationalUnitName(value, true);
			}
			else if (string.Compare(dottedNumberString, OrganizationName.DottedName, true) == 0)
			{
				return new OrganizationName(value, true);
			}
			else if (string.Compare(dottedNumberString, StateOrProvinceName.DottedName, true) == 0)
			{
				return new StateOrProvinceName(value, true);
			}
			else if (string.Compare(dottedNumberString, StreetAddress.DottedName, true) == 0)
			{
				return new StreetAddress(value, true);
			}
			else if (string.Compare(dottedNumberString, UserId.DottedName, true) == 0)
			{
				return new UserId(value, true);
			}

			return new CustomRdn(dottedNumberString, value, true);
		}
	}
}
