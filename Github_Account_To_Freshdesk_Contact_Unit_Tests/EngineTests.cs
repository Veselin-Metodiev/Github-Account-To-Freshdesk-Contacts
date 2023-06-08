namespace Github_Account_To_Freshdesk_Contact.Tests;

using System;
using Github_Account_To_Freshdesk_Contacts;
using Xunit;

public class EngineTests
{
	private Engine engine;

	public EngineTests()
	{
		engine = new Engine();
	}

	[Fact]
	public void CanCallReadAndValidateInput()
	{
		string input = "Name";

		string result = engine.ReadAndValidateInput(input);

		Assert.Equal(input, result);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	[InlineData(null)]
	public void CannotCallReadAndValidateInputWithInvalidInput(string value)
	{
		string expected = "Name";

		StringWriter writerOut = new();
		StringReader writerIn = new("Name");
		Console.SetOut(writerOut);
		Console.SetIn(writerIn);

		string result = engine.ReadAndValidateInput(value);

		Assert.Equal(expected, result);
	}

	[Fact]
	public void IsRegisteredReturnsTrue()
	{
		bool result = engine.IsRegistered("Name");
		Assert.True(result);
	}

	[Fact]
	public void IsRegisteredReturnsFalse()
	{
		Assert.False(engine.IsRegistered("[]"));
	}
}
