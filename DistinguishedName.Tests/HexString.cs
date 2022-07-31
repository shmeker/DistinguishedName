namespace DistinguishedName.Tests
{
	public class HexString
	{
		[Fact]
		public void DistinguishedName_HexString_Success()
		{
			string distinguishedNameString = @"CN=Lu\C4\8Di\C4\87,OU=Sales,DC=Fabrikam,DC=COM";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(4, distinguishedName.Rdns.Count);
			Assert.IsType<CommonName>(distinguishedName.Rdns[0]);
			Assert.IsType<OrganizationalUnitName>(distinguishedName.Rdns[1]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[2]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[3]);
			Assert.Equal("CN", distinguishedName.Rdns[0].FirstSingleValuedRdn.NameString);
			Assert.Equal("Lučić", distinguishedName.Rdns[0].FirstSingleValuedRdn.Value);
			Assert.Equal(@"CN=Lu\c4\8di\c4\87", distinguishedName.Rdns[0].GetString());
			Assert.Equal("OU=Sales", distinguishedName.Rdns[1].GetString());
			Assert.Equal("DC=Fabrikam", distinguishedName.Rdns[2].GetString());
			Assert.Equal("DC=COM", distinguishedName.Rdns[3].GetString());
			Assert.Equal(@"CN=Lu\c4\8di\c4\87,OU=Sales,DC=Fabrikam,DC=COM", distinguishedName.GetString());
		}

		[Theory]
		[InlineData(@"CN=AB\43", "ABC")]
		[InlineData(@"CN=Jeff \22 the Beast \22 Smith", "Jeff \" the Beast \" Smith")]
		[InlineData(@"CN=L\C3\ADdia", "Lídia")]
		[InlineData(@"CN=10 \E3\8E\9D", "10 ㎝")]
		[InlineData(@"CN=Before\0DAfter", "Before\rAfter")]
		[InlineData(@"CN=Some space\20\20\20", "Some space   ")]
		public void DistinguishedName_HexString_Values_Success(string distinguishedNameString, string expectedValue)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(expectedValue, distinguishedName.Rdns[0].FirstSingleValuedRdn.Value);
		}

		[Theory]
		[InlineData(@"CN=Jeff\f Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Hey \C2,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Hey \C235,OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_HexString_Failure(string distinguishedNameString)
		{
			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			Assert.Throws<ArgumentException>(act);
		}
	}
}
