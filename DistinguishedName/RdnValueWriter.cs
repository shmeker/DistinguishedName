using DistinguishedName.Helpers;
using System;
using System.Linq;
using System.Text;

namespace DistinguishedName
{
	/// <summary>
	/// Class that writes readable value string into the Attribute value string.
	/// </summary>
	public sealed class RdnValueWriter
	{
		private bool IsBeginning = true;
		private int CurrentSpaceCharCount = 0;

		/// <summary>
		/// Returns Attribute value string.
		/// </summary>
		/// <param name="valueString">Readable value string.</param>
		/// <returns>Attribute value string.</returns>
		public string ToEscapedValue(ReadOnlySpan<char> valueString)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char character in valueString)
			{
				if (IsBeginning && character == SpecialCharacters.SpaceChar)
				{
					sb.Append(SpecialCharacters.BackslashChar);
					sb.Append(SpecialCharacters.SpaceChar);

					continue;
				}
				else if (!IsBeginning && character == SpecialCharacters.SpaceChar)
				{
					CurrentSpaceCharCount++;

					continue;
				}

				if (CurrentSpaceCharCount > 0)
				{
					sb.Append(SpecialCharacters.SpaceChar, CurrentSpaceCharCount);
					CurrentSpaceCharCount = 0;
				}

				if (!AsciiHelper.IsAsciiCharacter(character))
				{
					Span<byte> bytes = Utf8Helper.GetBytesForCharacters(new[] { character });
					foreach (byte singleByte in bytes)
					{
						sb.AppendFormat(SpecialCharacters.BackslashChar + "{0:x2}", singleByte);
					}

					IsBeginning = false;

					continue;
				}
				else if (!SpecialCharacters.IsAcceptableForValue(character, false))
				{
					sb.Append(SpecialCharacters.BackslashChar);
				}

				sb.Append(character);

				IsBeginning = false;
			}

			if (CurrentSpaceCharCount > 0)
			{
				AddSpaces(sb);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Returns Attribute value string for the dotted type.
		/// </summary>
		/// <param name="valueString">Readable value string.</param>
		/// <returns>Attribute value string for the dotted type.</returns>
		public string ToDottedValue(ReadOnlySpan<char> valueString)
		{
			Span<byte> decodedBytes = Utf8Helper.GetBytesForCharacters(valueString);

			Span<byte> encodedBytes = BerHelper.Encode(decodedBytes);

			char[] characters = new char[] { SpecialCharacters.NumberSignChar }.Concat(HexStringHelper.ConvertFromBytes(encodedBytes)).ToArray();

			return new string(characters);
		}

		private void AddSpaces(StringBuilder sb)
		{
			for (int i = 0; i < CurrentSpaceCharCount; i++)
			{
				sb.Append(SpecialCharacters.BackslashChar);
				sb.Append(SpecialCharacters.SpaceChar);
			}
		}
	}
}
