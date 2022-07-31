namespace DistinguishedName.Tests
{
	public class Basic
	{
		[Fact]
		public void DistinguishedName_Basic_Success()
		{
			string distinguishedNameString = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM";

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

		[Fact]
		public void DistinguishedName_Empty_Success()
		{
			string distinguishedNameString = string.Empty;

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.True(distinguishedName.IsEmpty);
			Assert.Empty(distinguishedName.Rdns);
			Assert.Equal(string.Empty, distinguishedName.GetString());
		}

		[Fact]
		public void DistinguishedName_SemicolonSeparator_Success()
		{
			string distinguishedNameString = "CN=Jeff Smith;OU=Sales;DC=Fabrikam;DC=COM";

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
		[InlineData("CN= Jeff Smith ,OU  =Sales,  DC=Fabrikam   ,DC =COM")]
		[InlineData(" CN=  Jeff Smith,OU  = Sales,DC= Fabrikam   ,DC =  COM")]
		[InlineData("  CN=Jeff Smith,   OU =  Sales ,DC   =Fabrikam  , DC =  COM ")]
		[InlineData(" CN= Jeff Smith , OU=Sales, DC = Fabrikam ,DC =COM   ")]
		[InlineData(" CN =  Jeff Smith , OU =Sales, DC=Fabrikam  , DC =COM  ")]
		public void DistinguishedName_Basic_Spaces_Success(string distinguishedNameString)
		{
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

		[Fact]
		public void DistinguishedName_Basic_Uncompleted_Exception()
		{
			string distinguishedNameString = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC";

			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			ArgumentException exception = Assert.Throws<ArgumentException>(act);
			Assert.Equal("The last relative distuinguished name is incomplete.", exception.Message);
		}

		[Fact]
		public void DistinguishedName_Basic_DottedValue_Exception()
		{
			string distinguishedNameString = "CN=#04024869";

			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			ArgumentException exception = Assert.Throws<ArgumentException>(act);
			Assert.Equal("Character '#' at the position 3 is not valid in attribute value.", exception.Message);
		}
	}
}