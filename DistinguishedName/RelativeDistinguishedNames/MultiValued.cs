using DistinguishedName.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Class that holds multiple <see cref="SingleValued"/> objects.
	/// </summary>
	public class MultiValued : Rdn
	{
		private IList<SingleValued> singleValueRdns { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public MultiValued()
		{
			singleValueRdns = new List<SingleValued>();
		}

		/// <summary>
		/// Method that returns the Relative Distinguished Name string.
		/// </summary>
		/// <returns>The Relative Distinguished Name string.</returns>
		public override string GetString()
		{
			return string.Join(SpecialCharacters.PlusChar, singleValueRdns.Select(s => s.GetString()));
		}

		/// <summary>
		/// Method that compares another Relative Distinguished Name object with the current.
		/// </summary>
		/// <param name="toCompare">Relative Distinguished Name object to compare.</param>
		/// <param name="checkRdnOrder">If true enforces that the order of child Relative Distinguished Name objects is the same in both objects.</param>
		/// <returns>True if both objects are the same type and value, false otherwise.</returns>
		public override bool IsEqual(Rdn toCompare, bool checkRdnOrder)
		{
			if (!(toCompare is MultiValued toCompareMultiValued))
			{
				return false;
			}

			IList<SingleValued> toCompareRdns = toCompareMultiValued.singleValueRdns;

			if (singleValueRdns.Count != toCompareRdns.Count)
			{
				return false;
			}

			for (int rdnIndex = 0; rdnIndex < singleValueRdns.Count; rdnIndex++)
			{
				if (checkRdnOrder)
				{
					if (!singleValueRdns[rdnIndex].IsEqual(toCompareRdns[rdnIndex]))
					{
						return false;
					}
				}
				else
				{
					if (toCompareRdns.Any(r => r.IsEqual(singleValueRdns[rdnIndex])))
					{
						continue;
					}

					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Getter that returns a collection of <see cref="SingleValued"/> objects.
		/// </summary>
		public override IList<SingleValued> SingleValuedRdns
		{
			get
			{
				return singleValueRdns;
			}
		}

		/// <summary>
		/// Getter that returns first <see cref="SingleValued"/> object if exists, null otherwise.
		/// </summary>
		public override SingleValued FirstSingleValuedRdn
		{
			get
			{
				return singleValueRdns.FirstOrDefault();
			}
		}
	}
}
