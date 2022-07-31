using DistinguishedName.Helpers;
using DistinguishedName.RelativeDistinguishedNames;
using System.Collections.Generic;
using System.Linq;

namespace DistinguishedName
{
	/// <summary>
	/// Class that holds Relative Distinguished Name objects.
	/// </summary>
	public sealed class Dn
	{
		/// <summary>
		/// List of <see cref="Rdn">Relative Distinguished Name</see> objects.
		/// </summary>
		public List<Rdn> Rdns { get; set; }

		/// <summary>
		/// Returns true if there are no  <see cref="Rdn">Relative Distinguished Name</see> objects set, false otherwise.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return Rdns is null || !Rdns.Any();
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Dn()
		{
			Rdns = new List<Rdn>();
		}

		/// <summary>
		/// Returns the string representation of the Distinguished Name.
		/// </summary>
		/// <returns>String representation of Relative Distinguished Name sequence if set, empty string otherwise.</returns>
		public string GetString()
		{
			if (IsEmpty)
			{
				return string.Empty;
			}

			return string.Join(SpecialCharacters.CommaChar, Rdns.Select(s => s.GetString()));
		}

		/// <summary>
		/// Compares values with the other Distinguished Name object.
		/// </summary>
		/// <param name="toCompare"> <see cref="Dn">Distinguished Name</see> object to compare.</param>
		/// <param name="checkRdnOrder">If true, the comparison will return false when Relative Distinguished Name are not in the same order.</param>
		/// <returns>True if both objects have  <see cref="Rdn">Relative Distinguished Name</see> objects with the same type and value, false otherwise.</returns>
		public bool IsEqual(Dn toCompare, bool checkRdnOrder = false)
		{
			if (IsEmpty && toCompare.IsEmpty)
			{
				return true;
			}

			IList<Rdn> toCompareRdns = toCompare.Rdns;

			if (Rdns.Count != toCompareRdns.Count)
			{
				return false;
			}

			for (int rdnIndex = 0; rdnIndex < Rdns.Count; rdnIndex++)
			{
				if (checkRdnOrder)
				{
					if (!Rdns[rdnIndex].IsEqual(toCompareRdns[rdnIndex], checkRdnOrder))
					{
						return false;
					}
				}
				else
				{
					if (toCompareRdns.Any(r => r.IsEqual(Rdns[rdnIndex], checkRdnOrder)))
					{
						continue;
					}

					return false;
				}
			}

			return true;
		}
	}
}
