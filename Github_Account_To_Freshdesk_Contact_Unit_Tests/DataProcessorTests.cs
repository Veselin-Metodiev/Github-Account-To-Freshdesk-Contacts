namespace Github_Account_To_Freshdesk_Contact.Tests;

using Github_Account_To_Freshdesk_Contacts.DbModels;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.Json;
using Github_Account_To_Freshdesk_Contacts.Dtos;
using System;
using Github_Account_To_Freshdesk_Contacts;
using Github_Account_To_Freshdesk_Contacts.Models;
using AutoMapper;

public static class DataProcessorTests
{
	private static Engine engine = new(new GithubAccountToFreshdeskContactContext(new ConfigurationBuilder()));

	[Fact]
	public static void CanCallDeserializeToGithubAccount()
	{
		string accountAsJson = File.ReadAllText(@"../../../MockData/GithubAccount.txt");

		GithubAccount expected = new()
		{
			Name = "Veselin Metodiev",
			Login = "Veselin-Metodiev",
			Location = "Sofia, Bulgaria",
			CreatedAt = DateTime.ParseExact("2022-07-11T10:32:46Z", "yyyy-MM-ddThh:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(),
			Email = null,
			Bio = null,
			TwitterUsername = null
		};

		GithubAccount actual = DataProcessor.DeserializeToGithubAccount(accountAsJson);

		Assert.Equivalent(expected, actual);
	}



	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public static void CannotCallDeserializeToGithubAccountWithEmptyJsonAccount(string value)
	{
		Assert.Throws<JsonException>(() => DataProcessor.DeserializeToGithubAccount(value));
	}

	[Fact]
	public static void CannotCallDeserializeToGithubAccountWithNullJsonAccount()
	{
		Assert.Throws<ArgumentNullException>(() => DataProcessor.DeserializeToGithubAccount(null));
	}

	[Fact]
	public static void CanCallDeserializeToGithubAccountEmail()
	{
		string accountAsJson = File.ReadAllText(@"../../../MockData/GithubAccountEmail.txt");

		GithubAccountEmail[] expected =
		{
			new()
			{
				Email = "vesselin.metodiev@gmail.com",
				Primary = true
			},
			new()
			{
				Email = "109075090+Veselin-Metodiev@users.noreply.github.com",
				Primary = false
			}
		};

		GithubAccountEmail[] actual = DataProcessor.DeserializeToGithubAccountEmails(accountAsJson);

		Assert.Equivalent(expected, actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public static void CannotCallDeserializeToGithubAccountEmailWithEmptyJsonAccount(string value)
	{
		Assert.Throws<JsonException>(() => DataProcessor.DeserializeToGithubAccountEmails(value));
	}

	[Fact]
	public static void CannotCallDeserializeToGithubAccountEmailWithNullJsonAccount()
	{
		Assert.Throws<ArgumentNullException>(() => DataProcessor.DeserializeToGithubAccountEmails(null));
	}

	[Fact]
	public static void CanCallDeserializeToFreshdeskContactDto()
	{
		FreshdeskContactDto expected = new()
		{
			Id = 103093976356
		};

		string freshdeskContactAsJson = File.ReadAllText(@"../../../MockData/FreshdeskContactDto.txt");

		FreshdeskContactDto actual = DataProcessor.DeserializeToFreshdeskContactDto(freshdeskContactAsJson);

		Assert.Equivalent(expected, actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public static void CannotCallDeserializeToFreshdeskContactDtoWithEmptyFreshdeskContactAsJson(string value)
	{
		Assert.Throws<JsonException>(() => DataProcessor.DeserializeToFreshdeskContactDto(value));
	}

	[Fact]
	public static void CannotCallDeserializeToFreshdeskContactDtoWithNullFreshdeskContactAsJson()
	{
		Assert.Throws<ArgumentNullException>(() => DataProcessor.DeserializeToFreshdeskContactDto(null));
	}

	[Fact]
	public static void CanCallConvertGithubAccountToFreshdeskContact()
	{
		GithubAccount account = new()
		{
			Name = "Veselin Metodiev",
			Login = "Veselin-Metodiev",
			Location = "Sofia, Bulgaria",
			CreatedAt = DateTime.ParseExact("2022-07-11T10:32:46Z", "yyyy-MM-ddThh:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(),
			Email = null,
			Bio = null,
			TwitterUsername = null
		};
		IMapper mapper = engine.CreateMapper();

		FreshdeskContact actual = DataProcessor.ConvertGithubAccountToFreshdeskContact(account, mapper);

		FreshdeskContact expected = new()
		{
			Address = account.Location,
			Name = account.Name,
			Description = account.Bio,
			Email = null,
			TwitterId = null
		};

		Assert.Equivalent(expected, actual);
	}

	[Fact]
	public static void ConvertGithubAccountToFreshdeskContactPerformsMapping()
	{
		GithubAccount account = new()
		{
			Name = "Veselin Metodiev",
			Login = "Veselin-Metodiev",
			Location = "Sofia, Bulgaria",
			CreatedAt = DateTime.ParseExact("2022-07-11T10:32:46Z", "yyyy-MM-ddThh:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(),
			Email = null,
			Bio = null,
			TwitterUsername = null
		};
		IMapper mapper = engine.CreateMapper();

		FreshdeskContact actual = DataProcessor.ConvertGithubAccountToFreshdeskContact(account, mapper);

		FreshdeskContact expected = new()
		{
			Address = account.Location,
			Name = account.Name,
			Description = account.Bio,
			Email = null,
			TwitterId = null
		};

		Assert.Equivalent(actual, expected);
	}

	[Fact]
	public static void CanCallSerializeToFreshdeskContact()
	{
		FreshdeskContact contact = new()
		{
			Address = "Sofia, Bulgaria",
			Name = "Veselin Metodiev",
			Description = null,
			Email = null,
			TwitterId = null
		};

		string actual = DataProcessor.SerializeToFreshdeskContact(contact);

		string expected = File.ReadAllText(@"../../../MockData/FreshdeskContact.txt");

		Assert.Equivalent(actual, expected);
	}
}