using System.Text.Json.Serialization;

namespace Github_Account_To_Freshdesk_Contacts.Models;

public class GithubAccountEmail
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = null!;

	[JsonPropertyName("primary")]
	public bool Primary { get; set; }
}