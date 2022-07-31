using DistinguishedName.Helpers;
using DistinguishedName.RelativeDistinguishedNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistinguishedName
{
	/// <summary>
	/// Class for creating Distinguished Name object from it's string representation.
	/// </summary>
	public sealed class DnReader
	{
		private Dn Dn = new Dn();
		private MultiValued? MultiValued = null;

		private bool IsMultiValued = false;

		private bool IsType = true;
		private bool IsTypeFirstChar = true;
		private bool IsTypePotentiallyEnd = false;
		private bool IsTypeDottedDecimal = false;
		private bool IsTypePreviousDotChar = false;
		private bool IsValueBeginning = false;
		private bool IsValueEscaped = false;
		private bool WasValueEscaped = false;
		private bool IsValueCharacterEscaped = false;
		private int ValueCurrentSpaceCharCount = 0;
		private char? ValuePreviousHexChar = null;
		private bool IsValueHexPairBeingProcessed = false;
		private int ValueHexByteCountExpected = 0;
		private IList<byte> ValueEscapedBytes = new List<byte>();

		private StringBuilder TypeName = new StringBuilder();
		private StringBuilder Value = new StringBuilder();

		private int CurrentCharacterPosition = 0;

		/// <summary>
		/// Parses Distinguished Name string into <see cref="DistinguishedName.Dn">Distinguished Name</see> object.
		/// </summary>
		/// <param name="dnString">Input Distinguished Name string.</param>
		/// <returns><see cref="DistinguishedName.Dn">Distinguished Name</see> object.</returns>
		public Dn Parse(ReadOnlySpan<char> dnString)
		{
			Reset();

			foreach (char character in dnString)
			{
				EnsureAsciiCharacter(character);

				if (IsType)
				{
					ParseTypeCharacter(character);
				}
				else
				{
					if (IsTypeDottedDecimal)
					{
						ParseDottedValueCharacter(character);
					}
					else
					{
						ParseStringValueCharacter(character);
					}
				}

				CurrentCharacterPosition++;
			}

			EndOfStringParsing();

			return Dn;
		}

		private void EndOfStringParsing()
		{
			if (CurrentCharacterPosition > 0)
			{
				EnsureValueIsBeingParsed();

				ParseStringValueCharacter(SpecialCharacters.NewLine);
			}
		}

		private void ParseTypeCharacter(char character)
		{
			if (character == SpecialCharacters.SpaceChar)
			{
				if (!IsTypeFirstChar)
				{
					IsTypePotentiallyEnd = true;
				}

				return;
			}

			if (IsTypeFirstChar)
			{
				IsTypeDottedDecimal = char.IsDigit(character);
			}
			else if (character == SpecialCharacters.EqualChar)
			{
				SetTypePropertiesAfterEquals();

				return;
			}

			if (!SpecialCharacters.IsAcceptableForType(character, IsTypeFirstChar, IsTypeDottedDecimal, IsTypePreviousDotChar))
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} is not valid in attribute type.");
			}
			else if (IsTypePotentiallyEnd)
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} is not valid in attribute type after the SPACE character(s).");
			}

			IsTypePreviousDotChar = character == SpecialCharacters.DotChar;

			TypeName.Append(character);

			IsTypeFirstChar = false;
		}

		private void ParseStringValueCharacter(char character)
		{
			if (IsValueBeginning && ShouldSkipAfterParsingStringValueBegining(character))
			{
				return;
			}

			if (!IsValueCharacterEscaped && character == SpecialCharacters.BackslashChar)
			{
				EnsureNoValuesAfterDoubleQuotes();

				IsValueCharacterEscaped = true;

				return;
			}

			EnsureNotEndOfLine(character);

			if (IsValueHexPairBeingProcessed)
			{
				ProcessValueHexPair(character);

				return;
			}
			else if (IsValueCharacterEscaped && HexStringHelper.IsAcceptableForHexValue(character))
			{
				IsValueHexPairBeingProcessed = true;
				ValuePreviousHexChar = character;

				return;
			}

			if (IsValueEscaped && !IsValueCharacterEscaped && character == SpecialCharacters.DoubleQuotesChar)
			{
				IsValueEscaped = false;
				WasValueEscaped = true;

				return;
			}

			if (!IsValueEscaped && !IsValueCharacterEscaped)
			{
				if (ShouldSkipAfterEndCheck(character))
				{
					return;
				}

				EnsureNoValuesAfterDoubleQuotes();
			}

			EnsureCharacterAllowedForStringValue(character);

			AppendValueCharacter(character);

			IsValueCharacterEscaped = false;
		}

		private void ProcessValueHexPair(char character)
		{
			EnsureCharacterAllowedForHexProcessing(character);

			if (!ValuePreviousHexChar.HasValue && ValueEscapedBytes.Any())
			{
				ValuePreviousHexChar = character;

				return;
			}

			byte escapedByte = GetBytes(new char[] { ValuePreviousHexChar!.Value, character }).First();

			if (!ValueEscapedBytes.Any())
			{
				ValueHexByteCountExpected = Utf8Helper.GetNumberOfBytesByFirstByte(escapedByte);
				EnsureValueHexByteCountExpectedNotZero(character);
			}

			ValueEscapedBytes.Add(escapedByte);

			if (ValueHexByteCountExpected == ValueEscapedBytes.Count)
			{
				Span<char> characters = Utf8Helper.ToReadableChars(ValueEscapedBytes.ToArray());

				AppendValueCharacter(characters);

				IsValueHexPairBeingProcessed = false;
				ValueEscapedBytes.Clear();
				ValueHexByteCountExpected = 0;
			}

			IsValueCharacterEscaped = false;
			ValuePreviousHexChar = null;
		}

		private void ParseDottedValueCharacter(char character)
		{
			if (IsValueBeginning)
			{
				ParseDottedValueBegining(character);

				return;
			}

			if (ShouldSkipAfterEndCheck(character))
			{
				return;
			}

			EnsureCharacterAllowedForDottedValue(character);

			AppendValueCharacter(character);
		}

		private void ResetProperties()
		{
			IsMultiValued = false;
			MultiValued = null;

			ResetPropertiesForMultiValued();
		}

		private void ResetPropertiesForMultiValued()
		{
			IsType = true;
			IsTypeFirstChar = true;
			IsTypePotentiallyEnd = false;
			IsTypeDottedDecimal = false;
			IsTypePreviousDotChar = false;
			IsValueBeginning = false;
			IsValueEscaped = false;
			WasValueEscaped = false;
			IsValueCharacterEscaped = false;
			
			ValueCurrentSpaceCharCount = 0;

			TypeName = new StringBuilder();
			Value = new StringBuilder();
		}

		private void SetValuePropertiesWhenFirstDoubleQuotesHit()
		{
			IsValueEscaped = true;
			IsValueBeginning = false;
		}

		private void SetTypePropertiesAfterEquals()
		{
			IsType = false;
			IsValueBeginning = true;
		}

		private void EnsureNoValuesAfterDoubleQuotes()
		{
			if (WasValueEscaped)
			{
				throw new ArgumentException($"Characters other than Space are not allowed after ending double quotes. Character position: {CurrentCharacterPosition}.");
			}
		}

		private void AppendValueCharacter(char character)
		{
			if (ValueCurrentSpaceCharCount > 0)
			{
				Value.Append(SpecialCharacters.SpaceChar, ValueCurrentSpaceCharCount);
				ValueCurrentSpaceCharCount = 0;
			}

			Value.Append(character);
		}

		private void AppendValueCharacter(ReadOnlySpan<char> characters)
		{
			foreach (char character in characters)
			{
				AppendValueCharacter(character);
			}
		}

		private void Reset()
		{
			Dn = new Dn();
			MultiValued = null;

			IsMultiValued = false;

			IsType = true;
			IsTypeFirstChar = true;
			IsTypePotentiallyEnd = false;
			IsTypeDottedDecimal = false;
			IsTypePreviousDotChar = false;
			IsValueBeginning = false;
			IsValueEscaped = false;
			IsValueCharacterEscaped = false;
			ValueCurrentSpaceCharCount = 0;
			ValuePreviousHexChar = null;
			IsValueHexPairBeingProcessed = false;
			ValueHexByteCountExpected = 0;
			ValueEscapedBytes = new List<byte>();

			TypeName = new StringBuilder();
			Value = new StringBuilder();

			CurrentCharacterPosition = 0;
		}

		private bool IsCharacterAllowedForDottedDecimal(char character)
		{
			return
				HexStringHelper.IsAcceptableForHexValue(character) && ValueCurrentSpaceCharCount == 0
				|| character == SpecialCharacters.SpaceChar;
		}

		private SingleValued CreateSingleValuedRdn()
		{
			if (IsTypeDottedDecimal)
			{
				byte[] encodedBytes = GetBytes(Value.ToString()).ToArray();

				Span<byte> decodedBytes = BerHelper.Decode(encodedBytes);

				Span<char> characters = Utf8Helper.ToReadableChars(decodedBytes);

				return RdnFactory.CreateFromDottedNumberString(TypeName.ToString(), new string(characters));
			}

			return RdnFactory.Create(TypeName.ToString(), Value.ToString());
		}

		private IEnumerable<byte> GetBytes(ReadOnlySpan<char> stringValue)
		{
			if (stringValue.Length % 2 == 1)
			{
				throw new ArgumentException($"Conversion for value ending at character position {CurrentCharacterPosition} failed. The number of characters is not an even number.");
			}

			return HexStringHelper.ConvertToBytes(stringValue);
		}

		private bool ShouldSkipAfterParsingStringValueBegining(char character)
		{
			if (character == SpecialCharacters.SpaceChar)
			{
				return true;
			}
			else if (character == SpecialCharacters.DoubleQuotesChar)
			{
				SetValuePropertiesWhenFirstDoubleQuotesHit();

				return true;
			}

			IsValueBeginning = false;

			return false;
		}

		private void ParseDottedValueBegining(char character)
		{
			if (character == SpecialCharacters.SpaceChar)
			{
				return;
			}

			EnsureCharacterIsNotNumberSign(character);

			IsValueBeginning = false;
		}

		private bool ShouldSkipAfterEndCheck(char character)
		{
			if (character == SpecialCharacters.SpaceChar)
			{
				ValueCurrentSpaceCharCount++;

				return true;
			}
			else if (character == SpecialCharacters.CommaChar || character == SpecialCharacters.SemicolonChar || character == SpecialCharacters.NewLine)
			{
				SingleValued singleValuedRdn = CreateSingleValuedRdn();

				if (IsMultiValued && MultiValued != null)
				{
					MultiValued.SingleValuedRdns.Add(singleValuedRdn);
					Dn.Rdns.Add(MultiValued);
				}
				else
				{
					Dn.Rdns.Add(singleValuedRdn);
				}

				ResetProperties();

				return true;
			}
			else if (character == SpecialCharacters.PlusChar)
			{
				if (MultiValued == null)
				{
					MultiValued = new MultiValued();
					IsMultiValued = true;
				}

				SingleValued singleValuedRdn = CreateSingleValuedRdn();

				MultiValued.SingleValuedRdns.Add(singleValuedRdn);

				ResetPropertiesForMultiValued();

				return true;
			}

			return false;
		}

		private void EnsureAsciiCharacter(char character)
		{
			if (!AsciiHelper.IsAsciiCharacter(character))
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} is not ASCII character.");
			}
		}

		private void EnsureValueIsBeingParsed()
		{
			if (IsType)
			{
				throw new ArgumentException("The last relative distuinguished name is incomplete.");
			}
		}

		private void EnsureNotEndOfLine(char character)
		{
			if ((IsValueHexPairBeingProcessed || IsValueEscaped || IsValueCharacterEscaped)
					&& character == SpecialCharacters.NewLine)
			{
				throw new ArgumentException($"The end of line was not expected at the position {CurrentCharacterPosition}.");
			}
		}

		private void EnsureCharacterIsNotNumberSign(char character)
		{
			if (character != SpecialCharacters.NumberSignChar)
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} was not expected for attribute value (expected '{SpecialCharacters.NumberSignChar}').");
			}
		}

		private void EnsureCharacterAllowedForStringValue(char character)
		{
			if (!SpecialCharacters.IsAcceptableForValue(character, IsValueEscaped || IsValueCharacterEscaped))
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} is not valid in attribute value.");
			}
		}

		private void EnsureCharacterAllowedForDottedValue(char character)
		{
			if (!IsCharacterAllowedForDottedDecimal(character))
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} was not expected for attribute value (expected hex character).");
			}
		}

		private void EnsureCharacterAllowedForHexProcessing(char character)
		{
			if (!IsValueCharacterEscaped && character != SpecialCharacters.BackslashChar
								|| IsValueCharacterEscaped && !HexStringHelper.IsAcceptableForHexValue(character))
			{
				throw new ArgumentException($"Character '{character}' at the position {CurrentCharacterPosition} was not expected for attribute value.");
			}
		}

		private void EnsureValueHexByteCountExpectedNotZero(char character)
		{
			if (ValueHexByteCountExpected == 0)
			{
				throw new ArgumentException($"Escaped value '{ValuePreviousHexChar!.Value + character}' at the position {CurrentCharacterPosition - 1} is not valid in attribute value.");
			}
		}
	}
}
