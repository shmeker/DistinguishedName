namespace DistinguishedName.Tests.Helpers
{
	public class SpecialCharacters
	{
		[Theory]
		[InlineData('p', true, false, false)]
		[InlineData('2', false, true, true)]
		[InlineData('5', false, true, false)]
		[InlineData('.', false, true, false)]
		[InlineData('c', false, false, false)]
		[InlineData('A', true, false, false)]
		public void SpecialCharacters_IsAcceptableForType_True(char character, bool isFirstChar, bool isDottedType, bool wasPreviousDotChar)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForType(character, isFirstChar, isDottedType, wasPreviousDotChar);

			Assert.True(isAcceptable);
		}

		[Theory]
		[InlineData('1', true, false, false)]
		[InlineData('-', false, true, true)]
		[InlineData('n', false, true, false)]
		[InlineData('.', false, true, true)]
		[InlineData('>', false, false, false)]
		[InlineData(' ', true, false, false)]
		public void SpecialCharacters_IsAcceptableForType_False(char character, bool isFirstChar, bool isDottedType, bool wasPreviousDotChar)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForType(character, isFirstChar, isDottedType, wasPreviousDotChar);

			Assert.False(isAcceptable);
		}

		[Theory]
		[InlineData('1')]
		[InlineData('0')]
		[InlineData('.')]
		public void SpecialCharacters_IsAcceptableForDottedType_True(char character)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForDottedType(character);

			Assert.True(isAcceptable);
		}

		[Theory]
		[InlineData('a')]
		[InlineData(',')]
		[InlineData(';')]
		[InlineData('E')]
		public void SpecialCharacters_IsAcceptableForDottedType_False(char character)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForDottedType(character);

			Assert.False(isAcceptable);
		}

		[Theory]
		[InlineData('1', false)]
		[InlineData('c', false)]
		[InlineData('R', true)]
		[InlineData('/', true)]
		[InlineData('>', true)]
		public void SpecialCharacters_IsAcceptableForValue_True(char character, bool isEscaped)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForValue(character, isEscaped);

			Assert.True(isAcceptable);
		}

		[Theory]
		[InlineData('"', false)]
		[InlineData('=', false)]
		[InlineData('<', false)]
		[InlineData('+', false)]
		[InlineData('>', false)]
		public void SpecialCharacters_IsAcceptableForValue_False(char character, bool isEscaped)
		{
			bool isAcceptable = DistinguishedName.Helpers.SpecialCharacters.IsAcceptableForValue(character, isEscaped);

			Assert.False(isAcceptable);
		}
	}
}
