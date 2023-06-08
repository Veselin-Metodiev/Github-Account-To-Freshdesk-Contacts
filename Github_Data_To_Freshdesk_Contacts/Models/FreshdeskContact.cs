namespace Github_Account_To_Freshdesk_Contacts.Models;

using System.Text.Json.Serialization;

public class FreshdeskContact
{
	[JsonPropertyName("address")]
	public string? Address { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("email")]
	public string? Email { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;

	[JsonPropertyName("twitter_id")]
	public string? TwitterId { get; set; }
}