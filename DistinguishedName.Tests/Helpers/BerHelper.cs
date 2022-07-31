namespace DistinguishedName.Tests.Helpers
{
	public class BerHelper
	{
		[Fact]
		public void BerHelper_Decode_Success()
		{
			byte[] encodedValue = new byte[] { 4, 2, 103, 111 };

			Span<byte> decodedValue = DistinguishedName.Helpers.BerHelper.Decode(encodedValue);

			Assert.Equal(2, decodedValue.Length);
			Assert.Equal(103, decodedValue[0]);
			Assert.Equal(111, decodedValue[1]);
		}

		[Theory]
		[InlineData(new byte[] { 4, 3, 103, 111 })]
		[InlineData(new byte[] { 1, 2, 103, 129 })]
		[InlineData(new byte[] { 2, 2, 103, 111 })]
		[InlineData(new byte[] { 4, 5, 103, 111, 0, 11 })]
		public void BerHelper_Decode_Exception(byte[] encodedValue)
		{
			void act() => DistinguishedName.Helpers.BerHelper.Decode(encodedValue);

			Assert.ThrowsAny<Exception>(act);
		}

		[Fact]
		public void BerHelper_Encode_Success()
		{
			byte[] value = new byte[] { 103, 114, 101, 97, 116 };

			Span<byte> encodedValue = DistinguishedName.Helpers.BerHelper.Encode(value);

			Assert.Equal(7, encodedValue.Length);
			Assert.Equal(4, encodedValue[0]);
			Assert.Equal(5, encodedValue[1]);
			Assert.Equal(103, encodedValue[2]);
			Assert.Equal(114, encodedValue[3]);
			Assert.Equal(101, encodedValue[4]);
			Assert.Equal(97, encodedValue[5]);
			Assert.Equal(116, encodedValue[6]);
		}
	}
}
