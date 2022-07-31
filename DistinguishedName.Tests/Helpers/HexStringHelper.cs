namespace DistinguishedName.Tests.Helpers
{
	public class HexStringHelper
	{
		[Theory]
		[InlineData('1')]
		[InlineData('c')]
		[InlineData('D')]
		[InlineData('0')]
		[InlineData('9')]
		[InlineData('E')]
		[InlineData('F')]
		public void HexStringHelper_IsAcceptableForHexValue_True(char character)
		{
			bool isAscii = DistinguishedName.Helpers.HexStringHelper.IsAcceptableForHexValue(character);

			Assert.True(isAscii);
		}

		[Theory]
		[InlineData('š')]
		[InlineData('Č')]
		[InlineData('g')]
		[InlineData('G')]
		[InlineData('-')]
		[InlineData('_')]
		[InlineData(';')]
		public void HexStringHelper_IsAcceptableForHexValue_False(char character)
		{
			bool isAscii = DistinguishedName.Helpers.HexStringHelper.IsAcceptableForHexValue(character);

			Assert.False(isAscii);
		}

		[Theory]
		[InlineData("", new byte[] { })]
		[InlineData("6A", new byte[] { 106 })]
		[InlineData("626f6f6b", new byte[] { 98, 111, 111, 107 })]
		[InlineData("322b313d33", new byte[] { 50, 43, 49, 61, 51 })]
		public void HexStringHelper_ConvertToBytes_Success(string hexValue, IEnumerable<byte> expectedValues)
		{
			IEnumerable<byte> byteValues = DistinguishedName.Helpers.HexStringHelper.ConvertToBytes(hexValue);

			Assert.Equal(expectedValues, byteValues);
		}

		[Theory]
		[InlineData("_3")]
		[InlineData("4H")]
		[InlineData("/gg")]
		[InlineData("#44")]
		public void HexStringHelper_ConvertToBytes_Exception(string hexValue)
		{
			void act() => DistinguishedName.Helpers.HexStringHelper.ConvertToBytes(hexValue);

			Assert.Throws<FormatException>(act);
		}

		[Theory]
		[InlineData(new byte[] { }, "")]
		[InlineData(new byte[] { 106 }, "6A")]
		[InlineData(new byte[] { 98, 111, 111, 107 }, "626f6f6b")]
		[InlineData(new byte[] { 50, 43, 49, 61, 51 }, "322b313d33")]
		public void HexStringHelper_ConvertFromBytes_Success(byte[] bytes, string expectedValue)
		{
			string stringValue = DistinguishedName.Helpers.HexStringHelper.ConvertFromBytes(bytes);

			Assert.Equal(expectedValue, stringValue, true);
		}
	}
}
