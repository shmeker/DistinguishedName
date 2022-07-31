namespace DistinguishedName.Tests.Helpers
{
	public class AsciiHelper
	{
		[Theory]
		[InlineData('1')]
		[InlineData('t')]
		[InlineData('0')]
		[InlineData('R')]
		[InlineData('_')]
		[InlineData('!')]
		[InlineData(',')]
		public void AsciiHelper_IsAsciiCharacter_True(char character)
		{
			bool isAscii = DistinguishedName.Helpers.AsciiHelper.IsAsciiCharacter(character);

			Assert.True(isAscii);
		}

		[Theory]
		[InlineData('š')]
		[InlineData('Č')]
		[InlineData('€')]
		[InlineData('©')]
		[InlineData('Á')]
		[InlineData('ß')]
		[InlineData('ü')]
		public void AsciiHelper_IsAsciiCharacter_False(char character)
		{
			bool isAscii = DistinguishedName.Helpers.AsciiHelper.IsAsciiCharacter(character);

			Assert.False(isAscii);
		}
	}
}
