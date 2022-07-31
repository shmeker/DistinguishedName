using System.Collections.Generic;

namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Relative Distinguished Name abstract class.
	/// </summary>
	public abstract class Rdn
	{
		/// <summary>
		/// Abstract method that returns the Relative Distinguished Name string.
		/// </summary>
		/// <returns>The Relative Distinguished Name string.</returns>
		public abstract string GetString();

		/// <summary>
		/// Abstract method that compares another Relative Distinguished Name object with the current.
		/// </summary>
		/// <param name="toCompare">Relative Distinguished Name object to compare.</param>
		/// <param name="checkRdnOrder">If true enforces that the order of child Relative Distinguished Name objects is the same in both objects.</param>
		/// <returns>True if both objects are the same type and value, false otherwise.</returns>
		public abstract bool IsEqual(Rdn toCompare, bool checkRdnOrder);

		/// <summary>
		/// Abstract getter that returns a collection of <see cref="SingleValued"/> objects.
		/// </summary>
		public abstract IList<SingleValued> SingleValuedRdns { get; }

		/// <summary>
		/// Abstract getter that returns the first <see cref="SingleValued"/> object.
		/// </summary>
		public abstract SingleValued FirstSingleValuedRdn { get; }
	}
}
