namespace DistinguishedName.Helpers
{
	internal static class AsciiHelper
	{
		public static bool IsAsciiCharacter(char character)
		{
			return character < 128;
		}
	}
}
