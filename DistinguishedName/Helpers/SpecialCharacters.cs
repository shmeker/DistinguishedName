using System.Linq;

namespace DistinguishedName.Helpers
{
	internal static class SpecialCharacters
	{
		public static char SpaceChar = ' ';
		public static char DoubleQuotesChar = '"';
		public static char PlusChar = '+';
		public static char CommaChar = ',';
		public static char SemicolonChar = ';';
		public static char LessThanChar = '<';
		public static char EqualChar = '=';
		public static char GreaterThanChar = '>';
		public static char HyphenChar = '-';
		public static char BackslashChar = '\\';
		public static char NumberSignChar = '#';

		public static char DotChar = '.';
		public static char NewLine = '\n';

		private static readonly char[] charactesToEscape = new char[]
		{
			DoubleQuotesChar,
			PlusChar,
			CommaChar,
			SemicolonChar,
			LessThanChar,
			EqualChar,
			GreaterThanChar,
			BackslashChar,
			NumberSignChar
		};

		public static bool IsAcceptableForType(char character, bool isFirstChar, bool isDottedType, bool wasPreviousDotChar)
		{
			if (isFirstChar)
			{
				return isDottedType ? char.IsDigit(character) : char.IsLetter(character);
			}
			else
			{
				if (!isDottedType && (char.IsLetterOrDigit(character) || character == HyphenChar))
				{
					return true;
				}
				else if (isDottedType && (char.IsDigit(character) || character == DotChar && !wasPreviousDotChar))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsAcceptableForDottedType(char character)
		{
			return character == DotChar || char.IsDigit(character);
		}

		public static bool IsAcceptableForValue(char character, bool isEscaped)
		{
			return isEscaped || !charactesToEscape.Any(c => c == character);
		}
	}
}
