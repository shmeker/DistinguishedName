using System.Collections.Generic;

namespace DistinguishedName.RelativeDistinguishedNames
{
	/// <summary>
	/// Class that holds a single type and value.
	/// </summary>
	public abstract class SingleValued : Rdn
	{
		/// <summary>
		/// Abstract getter of the Relative Distinguished Name type.
		/// </summary>
		public abstract RdnTypes RdnType { get; }

		/// <summary>
		/// Abstract getter of the name in string format.
		/// </summary>
		public abstract string NameString { get; }

		/// <summary>
		/// Abstract getter of the name in dotted-decimal format.
		/// </summary>
		public abstract string NameDotted { get; }

		/// <summary>
		/// Abstract getter of the value.
		/// </summary>
		public abstract string Value { get; }

		/// <summary>
		/// Getter that returns bool value if this Relative Distinguished Name is in a dotted-decimal format.
		/// </summary>
		public bool IsDottedType { get; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SingleValued()
		{
			IsDottedType = false;
		}

		/// <summary>
		/// Constructor that sets if the Relative Distinguished Name is in a dotted-decimal format.
		/// </summary>
		/// <param name="isDottedType"></param>
		public SingleValued(bool isDottedType)
		{
			IsDottedType = isDottedType;
		}

		/// <summary>
		/// Method that returns the Relative Distinguished Name string.
		/// </summary>
		/// <returns>The Relative Distinguished Name string.</returns>
		public override string GetString()
		{
			RdnValueWriter dnWriter = new RdnValueWriter();

			if (IsDottedType)
			{
				return GetStringFormatted(NameDotted, dnWriter.ToDottedValue(Value));
			}

			return GetStringFormatted(NameString, dnWriter.ToEscapedValue(Value));
		}

		/// <summary>
		/// Method that compares another Relative Distinguished Name object with the current.
		/// </summary>
		/// <param name="toCompare">Relative Distinguished Name object to compare.</param>
		/// <param name="checkRdnOrder">Has no impact on this object because it has no child objects.</param>
		/// <returns>True if both objects are the same type and value, false otherwise.</returns>
		public override bool IsEqual(Rdn toCompare, bool checkRdnOrder = false)
		{
			if (!(toCompare is SingleValued toCompareSingleValued))
			{
				return false;
			}

			return string.Compare(NameString, toCompareSingleValued.NameString, true) == 0
				&& string.Compare(Value, toCompareSingleValued.Value, true) == 0;
		}

		/// <summary>
		/// Getter that returns a collection of <see cref="SingleValued"/> objects with only this object.
		/// </summary>
		public override IList<SingleValued> SingleValuedRdns
		{
			get
			{
				return new List<SingleValued>()
				{
					this
				};
			}
		}

		/// <summary>
		/// Getter that returns this <see cref="SingleValued"/> object.
		/// </summary>
		public override SingleValued FirstSingleValuedRdn
		{
			get
			{
				return this;
			}
		}

		private string GetStringFormatted(string name, string value)
		{
			return name + "=" + value;
		}
	}
}
