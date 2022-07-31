
# Welcome to the Distinguished Name library

This library was made to help you with parsing, creating and checking for equality various Distinguished Name strings by the [RFC2253 v2](https://www.ietf.org/rfc/rfc2253.txt) format.

## Current functionality

At this moment, there are following functionalities supported:
- **Parsing of DN strings** - for a given Distinguished Name string the library will try to parse it into the Distinguished Name object, with the list of Relative Distinguished Name child objects (if they exist in string).
- **Creation of DN string** - for created Distinguished Name object (with the list of Relative Distinguished Name child objects) the library created the Distinguished Name string.
- **Check of Equality** - you can check if two Distinguished Name objects are equal - this is being done by checking if both objects contain Relative Distinguished Name child objects with the equal types and names (and their order if set).

## Some features

Here are some features of the library:
- Defined well known attribute types:
	- Common name
	- Country name
	- Domain component
	- Locality name
	- Organizational unit name
	- Organization name
	- State or province name
	- Street address
	- User ID
- Added Custom attribute type
- Supported dotted-decimal attribute type with BER encoded value
