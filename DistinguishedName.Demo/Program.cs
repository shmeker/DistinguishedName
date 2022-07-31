// See https://aka.ms/new-console-template for more information
using DistinguishedName;
using DistinguishedName.RelativeDistinguishedNames;

ParseExample();

WriteSeparator();

CreateExample();

WriteSeparator();

EqualityExample();

WriteSeparator();

void WriteSeparator()
{
	Console.WriteLine(new string('-', 20));
}

void ParseExample()
{
	string dnExample = "OU=Sales+CN=J. Smith,1.3.6.1.4.1.1466.0=#04024869,O=Test,C=GB";
	Console.WriteLine("Parsing DN value:");
	Console.WriteLine(dnExample);

	DnReader dnReader = new();
	Dn dn = dnReader.Parse(dnExample);

	Console.WriteLine("Parsed objects:");
	foreach (SingleValued rdn in dn.Rdns.SelectMany(s => s.SingleValuedRdns))
	{
		Console.WriteLine($"Type: {AddPadding(rdn.RdnType.ToString(), 24)} | Name: {AddPadding(rdn.NameString, 20)} | Value: {AddPadding(rdn.Value, 20)} | IsDotted: {AddPadding(rdn.IsDottedType.ToString(), 5)}");
	}
}

void CreateExample()
{
	IEnumerable<Rdn> rdns = new List<Rdn>()
	{
		new UserId("12345"),
		new LocalityName("Some city"),
		new StreetAddress("Street of \"beast\" 1"),
		new CountryName("US"),
		new OrganizationName("Finance #1")
	};

	Console.WriteLine("Creating DN string from:");
	foreach (SingleValued rdn in rdns)
	{
		Console.WriteLine($"Type: {AddPadding(rdn.RdnType.ToString(), 24)} | Name: {AddPadding(rdn.NameString, 20)} | Value: {AddPadding(rdn.Value, 20)} | IsDotted: {AddPadding(rdn.IsDottedType.ToString(), 5)}");
	}

	Dn dn = new();
	dn.Rdns.AddRange(rdns);

	Console.WriteLine("Created DN string:");
	Console.WriteLine(dn.GetString());
}

void EqualityExample()
{
	string dnString1 = "CN=Jeff Smith,OU=Finance \\> 3,DC=Fabrikam,DC=COM";
	string dnString2 = " CN= \"Jeff Smith\" , OU=\"Finance > 3\", DC=Fabrikam,0.9.2342.19200300.100.1.25=#0403434F4D ";

	Console.WriteLine("Are the following DN strings equal?");
	Console.WriteLine(dnString1);
	Console.WriteLine(dnString2);

	DnReader dnReader = new();
	Dn dn1 = dnReader.Parse(dnString1);
	Dn dn2 = dnReader.Parse(dnString2);

	Console.WriteLine($"Result: {dn1.IsEqual(dn2)}.");
}

string AddPadding(string value, int totalWidth)
{
	return value.PadRight(totalWidth, ' ');
}
