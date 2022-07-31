using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistinguishedName.Tests
{
	public class MultiValue
	{
		[Fact]
		public void DistinguishedName_MultiValue_Success()
		{
			string distinguishedNameString = "OU=Sales+CN=J. Smith,O=Widget Inc.,C=US";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(3, distinguishedName.Rdns.Count);
			Assert.IsType<MultiValued>(distinguishedName.Rdns[0]);
			Assert.Equal(2, ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns.Count);
			Assert.IsType<OrganizationalUnitName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0]);
			Assert.IsType<CommonName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1]);
			Assert.Equal("OU=Sales", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0].GetString());
			Assert.Equal("CN=J. Smith", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith", distinguishedName.Rdns[0].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith,O=Widget Inc.,C=US", distinguishedName.GetString());
		}

		[Fact]
		public void DistinguishedName_MultiValue_Ber_Success()
		{
			string distinguishedNameString = "OU=Sales+CN=J. Smith+1.3.6.1.4.1.1466.0=#04024869,O=Widget Inc.,C=US";

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(3, distinguishedName.Rdns.Count);
			Assert.IsType<MultiValued>(distinguishedName.Rdns[0]);
			Assert.Equal(3, ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns.Count);
			Assert.IsType<OrganizationalUnitName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0]);
			Assert.IsType<CommonName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1]);
			Assert.IsType<CustomRdn>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[2]);
			Assert.Equal("OU=Sales", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0].GetString());
			Assert.Equal("CN=J. Smith", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1].GetString());
			Assert.Equal("1.3.6.1.4.1.1466.0=#04024869", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[2].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith+1.3.6.1.4.1.1466.0=#04024869", distinguishedName.Rdns[0].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith+1.3.6.1.4.1.1466.0=#04024869,O=Widget Inc.,C=US", distinguishedName.GetString());
		}

		[Theory]
		[InlineData("OU=Sales + CN=J. Smith , O= Widget Inc. , C=US")]
		[InlineData(" OU =Sales+  CN=J. Smith  , O =  Widget Inc. ,C=US  ")]
		[InlineData("   OU=  Sales  +CN=J. Smith , O =  Widget Inc. ,C= US  ")]
		[InlineData(" OU =Sales  + CN  =J. Smith , O=  Widget Inc. , C=US  ")]
		[InlineData(" OU=  Sales+ CN  = J. Smith, O =Widget Inc. , C =  US  ")]
		public void DistinguishedName_MultiValue_Spaces_Success(string distinguishedNameString)
		{
			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(distinguishedNameString);

			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(3, distinguishedName.Rdns.Count);
			Assert.IsType<MultiValued>(distinguishedName.Rdns[0]);
			Assert.Equal(2, ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns.Count);
			Assert.IsType<OrganizationalUnitName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0]);
			Assert.IsType<CommonName>(((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1]);
			Assert.Equal("OU=Sales", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[0].GetString());
			Assert.Equal("CN=J. Smith", ((MultiValued)distinguishedName.Rdns[0]).SingleValuedRdns[1].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith", distinguishedName.Rdns[0].GetString());
			Assert.Equal("OU=Sales+CN=J. Smith,O=Widget Inc.,C=US", distinguishedName.GetString());
		}

		[Fact]
		public void DistinguishedName_MultiValue_Uncompleted_Exception()
		{
			string distinguishedNameString = "OU=Sales+";

			DnReader dnReader = new();

			void act() => dnReader.Parse(distinguishedNameString);

			ArgumentException exception = Assert.Throws<ArgumentException>(act);
			Assert.Equal("The last relative distuinguished name is incomplete.", exception.Message);
		}
	}
}
