namespace DistinguishedName.Tests
{
	public class EscapedValue
	{
		[Fact]
		public void DistinguishedName_EscapedValue_Success()
		{
			string distinguishedNameString = @"CN=""Jeff Smith"",OU=Sales,DC=Fabrikam,DC=COM";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(4, distinguishedName.Rdns.Count);
			Assert.IsType<CommonName>(distinguishedName.Rdns[0]);
			Assert.IsType<OrganizationalUnitName>(distinguishedName.Rdns[1]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[2]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[3]);
			Assert.Equal("CN=Jeff Smith", distinguishedName.Rdns[0].GetString());
			Assert.Equal("OU=Sales", distinguishedName.Rdns[1].GetString());
			Assert.Equal("DC=Fabrikam", distinguishedName.Rdns[2].GetString());
			Assert.Equal("DC=COM", distinguishedName.Rdns[3].GetString());
			Assert.Equal("CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM", distinguishedName.GetString());
		}

		[Theory]
		[InlineData(@"CN=""Jeff Smith, jr."",OU=Sales,DC=Fabrikam,DC=COM", "CN=Jeff Smith\\, jr.,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=""Jeff \"" the Beast \"" Smith"",OU=Sales,DC=Fabrikam,DC=COM", "CN=Jeff \\\" the Beast \\\" Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"OU=Sales,DC=Fabrikam,DC=COM,CN=""Jeff + Jane""", "OU=Sales,DC=Fabrikam,DC=COM,CN=Jeff \\+ Jane")]
		[InlineData(@"DC=Fabrikam,DC=COM,CN=""Football > Crime"",OU=Sales", "DC=Fabrikam,DC=COM,CN=Football \\> Crime,OU=Sales")]
		[InlineData(@"CN=""Small < Big"",OU=Sales,DC=Fabrikam,DC=COM", "CN=Small \\< Big,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=""Break; return"",OU=Sales,DC=Fabrikam,DC=COM", "CN=Break\\; return,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN=""Peace=Love"",OU=Sales,DC=Fabrikam,DC=COM", "CN=Peace\\=Love,OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_EscapedValue_SpecialCharacters_Success(string distinguishedNameString, string distinguishedNameStringToCompare)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.True(areEqual);
		}

		[Theory]
		[InlineData(@"CN=""Jeff"" Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= Jeff ""Smith"",OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= Jeff ""jr"" Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= Jeff "" Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= ""Jeff Smith""d,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= ""Jeff Smith""\,,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= e""Jeff Smith"",OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= ""Jeff Smith""=,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= ""Jeff Smith""\s,OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= >""Jeff Smith"",OU=Sales,DC=Fabrikam,DC=COM")]
		[InlineData(@"CN= \\""Jeff Smith"",OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_EscapedValue_Failure(string distinguishedNameString)
		{
			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			Assert.Throws<ArgumentException>(act);
		}
	}
}
