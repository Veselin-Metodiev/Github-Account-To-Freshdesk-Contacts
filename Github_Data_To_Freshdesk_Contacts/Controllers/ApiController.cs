using Github_Account_To_Freshdesk_Contacts.Models;

namespace Github_Account_To_Freshdesk_Contacts.Controllers;

using System.Net.Http.Headers;
using System.Text;

public static class ApiController
{
	public static async Task<string> GetGithubAccountInfo(string username, HttpClient client)
	{
		HttpResponseMessage response = await client.GetAsync($"https://api.github.com/users/{username}");
		ValidateRespone(response);

		string body = await response.Content.ReadAsStringAsync();
		return body;
	}

	public static async Task<string> GetGithubAccountEmail(HttpClient client)
	{
		HttpResponseMessage response = await client.GetAsync("https://api.github.com/user/emails");
		ValidateRespone(response);

		string body = await response.Content.ReadAsStringAsync();
		return body;
	}

	public static async Task<HttpResponseMessage> CreateContact(string subdomain, string account, HttpClient client)
	{
		HttpContent content = new StringContent(account);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		HttpResponseMessage response =
			await client.PostAsync($"https://{subdomain}.freshdesk.com/api/v2/contacts", content);
		ValidateRespone(response);
		return response;
	}

	public static async Task<HttpResponseMessage> UpdateContact(string subdomain, string account, long id, HttpClient client)
	{
		HttpContent content = new StringContent(account);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		HttpResponseMessage respone =
			await client.PatchAsync($"https://{subdomain}.freshdesk.com/api/v2/contacts/{id}", content);
		ValidateRespone(respone);
		return respone;
	}

	public static async Task<string> GetFreshdeskContact(string username, string domain)
	{
		HttpClient client = CreateHttpClientForFreshdesk();

		HttpResponseMessage response =
			await client.GetAsync($"https://{domain}.freshdesk.com/api/v2/contacts/autocomplete?term={username}");
		ValidateRespone(response);

		string body = await response.Content.ReadAsStringAsync();
		return body;
	}

	public static HttpClient CreateHttpClientForFreshdesk()
	{
		HttpClient client = new();

		string apiKey = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN")!;
		string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(apiKey + ":X"));

		client.DefaultRequestHeaders.Add("Authorization", "Basic " + authInfo);

		return client;
	}

	public static HttpClient CreateHttpClientForGithub(string username)
	{
		HttpClient client = new();
		client.DefaultRequestHeaders.Add("User-Agent", username);

		return client;
	}

	public static HttpClient CreateHttpClientForGithubEmail(string username)
	{
		string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN")!;

		HttpClient client = new();
		client.DefaultRequestHeaders.Add("User-Agent", username);
		client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

		return client;
	}

	private static void ValidateRespone(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException(response.ReasonPhrase);
		}
	}
}
