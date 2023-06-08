namespace Github_Account_To_Freshdesk_Contacts;

using Dtos;
using Models;

using System.Text.Json;
using AutoMapper;

public static class DataProcessor
{
	public static GithubAccount DeserializeToGithubAccount(string accountAsJson) =>
		JsonSerializer.Deserialize<GithubAccount>(accountAsJson)!;
	

	public static FreshdeskContactDto DeserializeToFreshdeskContactDto(string freshdeskContactAsJson) =>
	JsonSerializer.Deserialize<FreshdeskContactDto[]>(freshdeskContactAsJson)![0];
	

	public static FreshdeskContact ConvertGithubAccountToFreshdeskContact(GithubAccount account, IMapper mapper) =>
		mapper.Map<FreshdeskContact>(account);
	

	public static string SerializeToFreshdeskContact(FreshdeskContact contact) =>
		JsonSerializer.Serialize(contact);
}