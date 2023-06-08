namespace Github_Account_To_Freshdesk_Contacts.Models;

using System.Text.Json.Serialization;

public class GithubAccount
{
	[JsonPropertyName("login")]
	public string Login { get; set; } = null!;

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;

	[JsonPropertyName("location")]
	public string? Location { get; set; }

	[JsonPropertyName("email")]
	public string? Email { get; set; }

	[JsonPropertyName("bio")]
	public string? Bio { get; set; }

	[JsonPropertyName("twitter_username")]
	public string? TwitterUsername { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }
}