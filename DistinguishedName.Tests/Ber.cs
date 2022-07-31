using DistinguishedName.Helpers;

namespace DistinguishedName.Tests
{
	public class Ber
	{
		[Fact]
		public void DistinguishedName_Ber_Success()
		{
			string distinguishedNameString = "1.3.6.1.4.1.1466.0=#04024869,O=Test,C=GB";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(3, distinguishedName.Rdns.Count);
			Assert.IsType<CustomRdn>(distinguishedName.Rdns[0]);
			Assert.IsType<OrganizationName>(distinguishedName.Rdns[1]);
			Assert.IsType<CountryName>(distinguishedName.Rdns[2]);
			Assert.Equal("1.3.6.1.4.1.1466.0=#04024869", distinguishedName.Rdns[0].GetString());
			Assert.Equal("Hi", distinguishedName.Rdns[0].FirstSingleValuedRdn.Value);
			Assert.Equal("1.3.6.1.4.1.1466.0=#04024869,O=Test,C=GB", distinguishedName.GetString());
		}

		[Theory]
		[InlineData("1.3.6.1.4.1.1466.0=#0403434F4D, O=Test  , C=GB")]
		[InlineData("  1.3.6.1.4.1.1466.0=#0403434F4D,O=Test,C=GB")]
		[InlineData(" 1.3.6.1.4.1.1466.0  = #0403434F4D,O=Test,C=GB")]
		[InlineData("1.3.6.1.4.1.1466.0 =   #0403434F4D,O=Test,C=GB")]
		[InlineData("1.3.6.1.4.1.1466.0= #0403434F4D  ,O=Test,C=GB")]
		public void DistinguishedName_Ber_Spaces_Success(string distinguishedNameString)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(3, distinguishedName.Rdns.Count);
			Assert.IsType<CustomRdn>(distinguishedName.Rdns[0]);
			Assert.IsType<OrganizationName>(distinguishedName.Rdns[1]);
			Assert.IsType<CountryName>(distinguishedName.Rdns[2]);
			Assert.Equal("1.3.6.1.4.1.1466.0=#0403434F4D", distinguishedName.Rdns[0].GetString(), true);
			Assert.Equal("COM", distinguishedName.Rdns[0].FirstSingleValuedRdn.Value);
			Assert.Equal("1.3.6.1.4.1.1466.0=#0403434F4D,O=Test,C=GB", distinguishedName.GetString(), true);
		}

		[Theory]
		[InlineData("1.3.6.1.4.1.DVS.0=#04024869,O=Test,C=GB")]
		[InlineData("1.3.6.1.4.1.1466.0=\"#04024869\",O=Test,C=GB")]
		[InlineData("1.3.6.1.4.1.1466.0=#0402URR,O=Test,C=GB")]
		[InlineData("1.3.6.1.4.1.1466.0=Some value,O=Test,C=GB")]
		public void DistinguishedName_Ber_Exception(string distinguishedNameString)
		{
			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			ArgumentException exception = Assert.Throws<ArgumentException>(act);
		}
	}
}
