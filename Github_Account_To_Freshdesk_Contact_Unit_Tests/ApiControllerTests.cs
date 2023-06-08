namespace Github_Account_To_Freshdesk_Contact.Tests;

using System.Threading.Tasks;
using Github_Account_To_Freshdesk_Contacts.Controllers;
using System.Net;
using RichardSzalay.MockHttp;

public static class ApiControllerTests
{
	[Fact]
	public static async Task CanCallGetGithubAccountInfo()
	{
		string actual = await ApiController.GetGithubAccountInfo("Veselin-Metodiev");

		string expected = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	[InlineData(null)]
	public static async Task CannotCallGetGithubAccountInfoWhenGivenInvalidUsername(string username)
	{
		await Assert.ThrowsAsync<FormatException>(() => ApiController.GetGithubAccountInfo(username));
	}

	[Fact]
	public static async Task CanCallCreateContact()
	{
		MockHttpMessageHandler mockHandler = new();
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");

		mockHandler.When("https://blank.freshdesk.com/api/v2/contacts")
			.WithContent(accountAsJson)
			.Respond(HttpStatusCode.Created);


		HttpClient client = mockHandler.ToHttpClient();

		HttpResponseMessage response = await ApiController.CreateContact("blank", accountAsJson, client);

		Assert.Equivalent(response.StatusCode, HttpStatusCode.Created);
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	[InlineData(null)]
	public static async Task CannotCallCreateContactWithInvalidDomain(string subdomain)
	{
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");
		await Assert.ThrowsAsync<UriFormatException>(() => 
			ApiController.CreateContact(subdomain, accountAsJson, ApiController.CreateHttpClientForFreshdesk()));
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	public static async Task CannotCallCreateContactWithInvalidAccount(string account)
	{
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");
		await Assert.ThrowsAsync<HttpRequestException>(() => 
			ApiController.CreateContact("blank", account, ApiController.CreateHttpClientForFreshdesk()));
	}

	[Fact]
	public static async Task CanCallUpdateContact()
	{
		MockHttpMessageHandler mockHandler = new();
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");

		mockHandler.When("https://blank.freshdesk.com/api/v2/contacts/103093976356")
			.WithContent(accountAsJson)
			.Respond(HttpStatusCode.OK);


		HttpClient client = mockHandler.ToHttpClient();

		HttpResponseMessage response = await ApiController.UpdateContact("blank", accountAsJson, 103093976356, client);

		Assert.Equivalent(response.StatusCode, HttpStatusCode.OK);
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	public static async Task CannotCallUpdateContactWithInvalidAccount(string account)
	{
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");
		await Assert.ThrowsAsync<HttpRequestException>(() => 
			ApiController.UpdateContact("blank", account, 103093976356, ApiController.CreateHttpClientForFreshdesk()));
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	[InlineData(null)]
	public static async Task CannotCallUpdateContactWithInvalidDomain(string subdomain)
	{
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");
		await Assert.ThrowsAsync<UriFormatException>(() => 
			ApiController.UpdateContact(subdomain, accountAsJson, 103093976356, ApiController.CreateHttpClientForFreshdesk()));
	}

	[Fact]
	public static async Task CanCallGetFreshdeskContact()
	{
		string actual = await ApiController.GetFreshdeskContact("Veselin Metodiev", "blank");

		string expected = await File.ReadAllTextAsync(@"../../../MockData/FreshDeskContactDto.txt");

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("    ")]
	[InlineData(null)]
	public static async Task CannotCalGetFreshdeskContactWithInvalidDomain(string subdomain)
	{
		string accountAsJson = await File.ReadAllTextAsync(@"../../../MockData/GithubAccount.txt");
		await Assert.ThrowsAsync<UriFormatException>(() =>
			ApiController.GetFreshdeskContact("Veselin Metodiev", subdomain));
	}
}
