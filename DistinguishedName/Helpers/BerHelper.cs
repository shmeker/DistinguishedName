using System.Formats.Asn1;
using System;

namespace DistinguishedName.Helpers
{
	internal static class BerHelper
	{
		public static Span<byte> Decode(ReadOnlySpan<byte> encodedValue)
		{
			Asn1Tag tag = AsnDecoder.ReadEncodedValue(encodedValue, AsnEncodingRules.BER, out int _, out int _, out int _);

			if (tag.TagValue != (int)UniversalTagNumber.OctetString)
			{
				throw new NotSupportedException("BER encoded value is of unsupported type. Only OCTET STRING is supported.");
			}

			return AsnDecoder.ReadOctetString(encodedValue, AsnEncodingRules.BER, out int _).AsSpan();
		}

		public static Span<byte> Encode(ReadOnlySpan<byte> valueForEncode)
		{
			AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.BER);
			asnWriter.WriteOctetString(valueForEncode);

			int encodedLength = asnWriter.GetEncodedLength();
			Span<byte> bytes = new byte[encodedLength];
			asnWriter.Encode(bytes);

			return bytes;
		}
	}
}
