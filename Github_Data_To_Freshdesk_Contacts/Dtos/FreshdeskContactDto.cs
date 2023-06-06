using System.Text.Json.Serialization;

namespace Github_Data_To_Freshdesk_Contacts.Dtos
{
	internal class FreshdeskContactDto
	{
		[JsonPropertyName("id")]
		public long Id { get; set; }
	}
}
