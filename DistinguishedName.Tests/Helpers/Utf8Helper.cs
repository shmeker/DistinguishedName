namespace DistinguishedName.Tests.Helpers
{
	public class Utf8Helper
	{
		[Theory]
		[InlineData(120, 1)]
		[InlineData(192, 2)]
		[InlineData(224, 3)]
		[InlineData(240, 4)]
		public void Utf8Helper_IsAcceptableForDottedType_Success(byte firstByte, int expectedValue)
		{
			int numberOfBytes = DistinguishedName.Helpers.Utf8Helper.GetNumberOfBytesByFirstByte(firstByte);

			Assert.Equal(expectedValue, numberOfBytes);
		}

		[Theory]
		[InlineData(new byte[] { }, new char[] { })]
		[InlineData(new byte[] { 76, 73, 77, 69 }, new char[] { 'L', 'I', 'M', 'E' })]
		[InlineData(new byte[] { 197, 161, 101, 196, 135, 101, 114 }, new char[] { 'š', 'e', 'ć', 'e', 'r' })]
		public void Utf8Helper_ToReadableChars_Success(byte[] bytes, char[] expectedValues)
		{
			char[] readableChars = DistinguishedName.Helpers.Utf8Helper.ToReadableChars(bytes).ToArray();

			Assert.Equal(expectedValues, readableChars);
		}

		[Fact]
		public void Utf8Helper_ToReadableChars_Fail()
		{
			byte[] bytes = new byte[] { 1, 61, 101, 1, 7, 101, 114 };
			char[] exptectedValue = new char[] { 'š', 'e', 'ć', 'e', 'r' };

			char[] readableChars = DistinguishedName.Helpers.Utf8Helper.ToReadableChars(bytes).ToArray();

			Assert.NotEqual(exptectedValue, readableChars);
		}

		[Theory]
		[InlineData(new char[] { }, new byte[] { })]
		[InlineData(new char[] { 'L', 'I', 'M', 'E' }, new byte[] { 76, 73, 77, 69 })]
		[InlineData(new char[] { 'š', 'e', 'ć', 'e', 'r' }, new byte[] { 197, 161, 101, 196, 135, 101, 114 })]
		public void Utf8Helper_GetBytesForCharacters_Success(char[] characters, byte[] expectedValues)
		{
			byte[] bytes = DistinguishedName.Helpers.Utf8Helper.GetBytesForCharacters(characters).ToArray();

			Assert.Equal(expectedValues, bytes);
		}

		[Fact]
		public void Utf8Helper_GetBytesForCharacters_Fail()
		{
			char[] chars = new char[] { 'š', 'e', 'ć', 'e', 'r' };
			byte[] exptectedValue = new byte[] { 1, 61, 101, 1, 7, 101, 114 };

			byte[] bytes = DistinguishedName.Helpers.Utf8Helper.GetBytesForCharacters(chars).ToArray();

			Assert.NotEqual(exptectedValue, bytes);
		}
	}
}
