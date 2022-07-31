using System;
using System.Collections;
using System.Text;

namespace DistinguishedName.Helpers
{
	internal static class Utf8Helper
	{
		private static readonly Encoding utf8Encoding = Encoding.UTF8;

		public static int GetNumberOfBytesByFirstByte(byte firstByte)
		{
			BitArray bitArray = new BitArray(new byte[] { firstByte });

			if (!bitArray[7])
			{
				return 1;
			}
			else if (bitArray[7] && bitArray[6] && !bitArray[5])
			{
				return 2;
			}
			else if (bitArray[7] && bitArray[6] && bitArray[5] && !bitArray[4])
			{
				return 3;
			}
			else if (bitArray[7] && bitArray[6] && bitArray[5] && bitArray[4] && !bitArray[3])
			{
				return 4;
			}

			return 0;
		}

		public static Span<char> ToReadableChars(ReadOnlySpan<byte> bytes)
		{
			int count = utf8Encoding.GetMaxCharCount(bytes.Length);
			Span<char> chars = new char[count];
			int read = utf8Encoding.GetChars(bytes, chars);

			return chars[..read];
		}

		public static Span<byte> GetBytesForCharacters(ReadOnlySpan<char> characters)
		{
			int count = utf8Encoding.GetMaxByteCount(characters.Length);
			Span<byte> bytes = new byte[count];
			int read = utf8Encoding.GetBytes(characters, bytes);

			return bytes[..read];
		}
	}
}
