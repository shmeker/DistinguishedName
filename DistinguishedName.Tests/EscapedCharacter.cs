namespace DistinguishedName.Tests
{
	public class EscapedCharacter
	{
		[Fact]
		public void DistinguishedName_EscapedCharacter_Success()
		{
			string distinguishedNameString = @"CN=Smith\, Jeff,OU=Sales,DC=Fabrikam,DC=COM";

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
			Assert.Equal("Smith, Jeff", distinguishedName.Rdns[0].FirstSingleValuedRdn.Value);
		}

		[Theory]
		[InlineData(@"CN=Jeff Smith\, jr.,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Jeff \"" the Beast \"" Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"OU=Sales,DC=Fabrikam,DC=COM,CN=Jeff \+ Jane")]
		[InlineData(@"DC=Fabrikam,DC=COM,CN=Football \> Crime,OU=Sales")]
		[InlineData(@"CN=Small \< Big,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Break\; return,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Peace\=Love,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Jeff Smith,OU=Sales\\Finance,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Jeff Smith\ ,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=Jeff Smith\ \ ,OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_EscapedCharacter_SpecialCharacters_Success(string distinguishedNameString)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(4, distinguishedName.Rdns.Count);
		}

		[Theory]
		[InlineData(@"CN=Jeff Smith\f,OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_EscapedCharacter_SpecialCharacters_Failure(string distinguishedNameString)
		{
			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			Assert.Throws<ArgumentException>(act);
		}
	}
}
