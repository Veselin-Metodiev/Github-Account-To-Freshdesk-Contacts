namespace Github_Account_To_Freshdesk_Contacts.Dtos;

using System.Text.Json.Serialization;

public class FreshdeskContactDto
{
	[JsonPropertyName("id")]
	public long Id { get; set; }
}
