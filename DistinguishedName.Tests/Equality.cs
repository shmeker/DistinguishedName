namespace DistinguishedName.Tests
{
	public class Equality
	{
		[Fact]
		public void DistinguishedName_Equality_Success()
		{
			string distinguishedNameString = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM";
			string distinguishedNameStringToCompare = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.True(areEqual);
		}

		[Fact]
		public void DistinguishedName_Equality_MultiValue_Success()
		{
			string distinguishedNameString = "OU=Sales+CN=J. Smith,O=Widget Inc.,C=US";
			string distinguishedNameStringToCompare = "OU=Sales+CN=J. Smith,O=Widget Inc.,C=US";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.True(areEqual);
		}

		[Fact]
		public void DistinguishedName_Equality_CustomRdn_Success()
		{
			string distinguishedNameString = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM,Test=Value";
			string distinguishedNameStringToCompare = "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM,Test=Value";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.True(areEqual);
		}

		[Theory]
		[InlineData("CN= Jeff Smith ,OU  =Sales,  DC=Fabrikam   ,DC =COM", " CN=  Jeff Smith,OU  = Sales,DC= Fabrikam   ,DC =  COM")]
		[InlineData("  CN=Jeff Smith,   OU =  Sales ,DC   =Fabrikam  , DC =  COM ", " CN= Jeff Smith , OU=Sales, DC = Fabrikam ,DC =COM   ")]
		[InlineData(" CN =  Jeff Smith , OU =Sales, DC=Fabrikam  , DC =COM  ", "CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM")]
		public void DistinguishedName_Equality_Spaces_Success(string distinguishedNameString, string distinguishedNameStringToCompare)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.True(areEqual);
		}

		[Theory]
		[InlineData("CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM", "CN=Jeff Smith,DC=Fabrikam,DC=COM,OU=Sales")]
		[InlineData("OU=Sales,DC=Fabrikam,DC=COM,CN=Jeff Smith", "CN=Jeff Smith,DC=Fabrikam,OU=Sales,DC=COM")]
		[InlineData("DC=Fabrikam,DC=COM,CN=Jeff Smith,OU=Sales", "DC=COM,CN=Jeff Smith,DC=Fabrikam,OU=Sales")]
		public void DistinguishedName_Equality_NoOrderChecking_Success(string distinguishedNameString, string distinguishedNameStringToCompare)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare);

			Assert.True(areEqual);
		}

		[Theory]
		[InlineData("CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM", "CN=Jeff Smith,DC=Fabrikam,DC=COM,OU=Sales")]
		[InlineData("OU=Sales,DC=Fabrikam,DC=COM,CN=Jeff Smith", "CN=Jeff Smith,DC=Fabrikam,OU=Sales,DC=COM")]
		[InlineData("DC=Fabrikam,DC=COM,CN=Jeff Smith,OU=Sales", "DC=COM,CN=Jeff Smith,DC=Fabrikam,OU=Sales")]
		public void DistinguishedName_Equality_OrderChecking_Failure(string distinguishedNameString, string distinguishedNameStringToCompare)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.False(areEqual);
		}

		[Theory]
		[InlineData("CN=Jeff Smith,OU=Sales,DC=Fabrikam,DC=COM", "CN=Jeff Smith,DC=Fabrikam,DC=COM")]
		[InlineData("OU=Sales,DC=Fabrikam,DC=COM,CN=Jeff Smith", "CN=Jeff Smith,DC=Fabrikam,OU=Sales,DC=COM,DN=Test")]
		[InlineData("DC=Fabrikam,DC=COM,CN=Jeff Smith+OU=Sales", "DC=COM,CN=Jeff Smith,DC=Fabrikam,OU=Sales")]
		public void DistinguishedName_Equality_DifferentRdns_Failure(string distinguishedNameString, string distinguishedNameStringToCompare)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);
			Dn distinguishedNameToCompare = dnReader.Parse(distinguishedNameStringToCompare);

			bool areEqual = distinguishedName.IsEqual(distinguishedNameToCompare, true);

			Assert.False(areEqual);
		}
	}
}
