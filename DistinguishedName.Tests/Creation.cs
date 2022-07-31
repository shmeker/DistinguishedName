using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistinguishedName.Tests
{
	public class Creation
	{
		[Fact]
		public void DistinguishedName_Creation_Success()
		{
			UserId user = new("12345");
			LocalityName localityName = new("Some city");
			StreetAddress streetAddress = new("Street of \"beast\" 1");
			CountryName countryName = new("US", true);
			OrganizationName organizationName = new("Finance #1");

			Dn dn = new();
			dn.Rdns.AddRange(new List<Rdn>()
			{
				user,
				localityName,
				streetAddress,
				countryName,
				organizationName
			});

			string dnString = dn.GetString();

			DnReader dnReader = new();
			Dn distinguishedName = dnReader.Parse(dnString);

			Assert.NotNull(dnString);
			Assert.NotNull(distinguishedName);
			Assert.False(distinguishedName.IsEmpty);
			Assert.Equal(5, distinguishedName.Rdns.Count);
			Assert.IsType<UserId>(distinguishedName.Rdns[0]);
			Assert.IsType<LocalityName>(distinguishedName.Rdns[1]);
			Assert.IsType<StreetAddress>(distinguishedName.Rdns[2]);
			Assert.IsType<CountryName>(distinguishedName.Rdns[3]);
			Assert.IsType<OrganizationName>(distinguishedName.Rdns[4]);
			Assert.Equal("UID=12345", distinguishedName.Rdns[0].GetString());
			Assert.Equal("L=Some city", distinguishedName.Rdns[1].GetString());
			Assert.Equal(@"STREET=Street of \""beast\"" 1", distinguishedName.Rdns[2].GetString());
			Assert.Equal("2.5.4.6=#04025553", distinguishedName.Rdns[3].GetString());
			Assert.Equal(@"O=Finance \#1", distinguishedName.Rdns[4].GetString());
			Assert.Equal(@"UID=12345,L=Some city,STREET=Street of \""beast\"" 1,2.5.4.6=#04025553,O=Finance \#1", distinguishedName.GetString());
		}
	}
}
