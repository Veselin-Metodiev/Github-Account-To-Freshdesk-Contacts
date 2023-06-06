using System.Net.Http.Headers;

namespace Github_Data_To_Freshdesk_Contacts.Controllers;

using System.Text;

internal static class ApiController
{
	public static async Task<string> GetGithubAccountInfo(string username)
	{
		HttpClient client = new();
		client.DefaultRequestHeaders.Add("User-Agent", username);

		HttpResponseMessage response = await client.GetAsync($"https://api.github.com/users/{username}");
		ValidateRespone(response);

		string body = await response.Content.ReadAsStringAsync();
		return body;
	}

	public static async Task CreateContact(string domain, string account)
	{
		HttpClient client = ConfigureHttpClientForFreshdesk();

		HttpContent content = new StringContent(account);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		HttpResponseMessage respone =
			await client.PostAsync($"https://{domain}.freshdesk.com/api/v2/contacts", content);
		ValidateRespone(respone);
	}

	public static async Task UpdateContact(string domain, string account, long id)
	{
		HttpClient client = ConfigureHttpClientForFreshdesk();

		HttpContent content = new StringContent(account);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		HttpResponseMessage respone =
			await client.PatchAsync($"https://{domain}.freshdesk.com/api/v2/contacts/{id}", content);
		ValidateRespone(respone);
	}

	public static async Task<string> GetFreshdeskUser(string username, string domain)
	{
		HttpClient client = ConfigureHttpClientForFreshdesk();

		HttpResponseMessage response =
			await client.GetAsync($"https://{domain}.freshdesk.com/api/v2/contacts/autocomplete?term={username}");
		ValidateRespone(response);

		string body = await response.Content.ReadAsStringAsync();
		return body;
	}

	private static HttpClient ConfigureHttpClientForFreshdesk()
	{
		HttpClient client = new();

		string apiKey = "EjyTYiMhzb5B0xwGIsTh:X";
		apiKey = Convert.ToBase64String(Encoding.Default.GetBytes(apiKey));

		client.DefaultRequestHeaders.Add("Authorization", "Basic " + apiKey);

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
