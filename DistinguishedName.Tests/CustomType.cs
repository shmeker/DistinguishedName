namespace DistinguishedName.Tests
{
	public class CustomType
	{
		[Fact]
		public void DistinguishedName_CustomType_Success()
		{
			string distinguishedNameString = "CN=Jeff Smith,Department=Sales,DC=Fabrikam,DC=COM";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(4, distinguishedName.Rdns.Count);
			Assert.IsType<CommonName>(distinguishedName.Rdns[0]);
			Assert.IsType<CustomRdn>(distinguishedName.Rdns[1]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[2]);
			Assert.IsType<DomainComponent>(distinguishedName.Rdns[3]);
			Assert.Equal("CN=Jeff Smith", distinguishedName.Rdns[0].GetString());
			Assert.Equal("Department=Sales", distinguishedName.Rdns[1].GetString());
			Assert.Equal("DC=Fabrikam", distinguishedName.Rdns[2].GetString());
			Assert.Equal("DC=COM", distinguishedName.Rdns[3].GetString());
			Assert.Equal("CN=Jeff Smith,Department=Sales,DC=Fabrikam,DC=COM", distinguishedName.GetString());
		}

		[Theory]
		[InlineData("CN=Jeff Smith,Dept3=Sales,DC=Fabrikam,DC=COM")]
		[InlineData("first-Name=Jeff,last-Name=Smith,OU=Sales,DC=Fabrikam")]
		[InlineData("CN=Jeff Smith,OU=Sales,DC1=Fabrikam,DC2=COM")]
		[InlineData("CN=Jeff Smith,Org14-1=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_CustomType_Allowed_Success(string distinguishedNameString)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(4, distinguishedName.Rdns.Count);
			Assert.Contains(distinguishedName.Rdns, rdn => rdn.GetType() == typeof(CustomRdn));
		}

		[Theory]
		[InlineData("CN=Jeff Smith,1Dept=Sales,DC=Fabrikam,DC=COM")]
		[InlineData("first.Name=Jeff,last.Name=Smith,OU=Sales,DC=Fabrikam")]
		[InlineData("CN=Jeff Smith,OU=Sales,DC*1=Fabrikam,DC*2=COM")]
		[InlineData("CN=Jeff Smith,Org14_1=Sales,DC=Fabrikam,DC=COM")]
		[InlineData("CN=Jeff Smith,Organizational Unit=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_CustomType_Failure(string distinguishedNameString)
		{
			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			Assert.Throws<ArgumentException>(act);
		}
	}
}
