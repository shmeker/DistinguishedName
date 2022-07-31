using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DistinguishedName.Helpers
{
	internal static class HexStringHelper
	{
		private static readonly char[] hexValidCharacters = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9'
		};

		public static bool IsAcceptableForHexValue(char character)
		{
			return hexValidCharacters.Any(c => c == character);
		}

		public static IEnumerable<byte> ConvertToBytes(ReadOnlySpan<char> stringValue)
		{
			List<byte> bytes = new List<byte>(stringValue.Length / 2);

			for (int charIndex = 0; charIndex < stringValue.Length; charIndex += 2)
			{
				int intValue = int.Parse(stringValue.Slice(charIndex, 2), NumberStyles.HexNumber);

				bytes.Add((byte)intValue);
			}

			return bytes;
		}

		public static string ConvertFromBytes(ReadOnlySpan<byte> bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);

			foreach (byte singleByte in bytes)
			{
				stringBuilder.AppendFormat("{0:x2}", singleByte);
			}

			return stringBuilder.ToString();
		}
	}
}
