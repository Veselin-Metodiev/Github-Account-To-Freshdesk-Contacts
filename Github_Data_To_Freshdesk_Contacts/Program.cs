using System.Text.Json;
using AutoMapper;
using Github_Data_To_Freshdesk_Contacts;
using Github_Data_To_Freshdesk_Contacts.Controllers;
using Github_Data_To_Freshdesk_Contacts.Dtos;
using Github_Data_To_Freshdesk_Contacts.Models;

Console.Write("Enter a Github username: ");
string username = ReadAndValidateInput(Console.ReadLine());

IMapper mapper = CreateMapper();

string accountAsJson = await ApiController.GetGithubAccountInfo(username);
GithubAccount account = DeserializeToGithubAccount(accountAsJson);
FreshdeskContact contact = ConvertToFreshdeskContact(account);

Console.WriteLine("Enter an email: ");
string email = ReadAndValidateInput(Console.ReadLine());
contact.Email = email;

string contactAsJson = SerializeToFreshdeskContact(contact);

Console.Write("Enter a Freshdesk subdomain: ");
string subdomain = ReadAndValidateInput(Console.ReadLine());

string freshdeskContactAsJson = await ApiController.GetFreshdeskUser(contact.Name, subdomain);

if (IsRegistered(freshdeskContactAsJson))
{
	FreshdeskContactDto freshdeskContact = DeserializeToFreaFreshdeskContactDto(freshdeskContactAsJson);
	await ApiController.UpdateContact(subdomain, contactAsJson, freshdeskContact.Id);

	Console.WriteLine("Successfully updated a new contact");
	return;
}

await ApiController.CreateContact(subdomain, contactAsJson);

Console.WriteLine("Successfully created a new contact");

static GithubAccount DeserializeToGithubAccount(string accountAsJson) =>
	JsonSerializer.Deserialize<GithubAccount>(accountAsJson)!;

static FreshdeskContactDto DeserializeToFreaFreshdeskContactDto(string freshdeskContactAsJson) =>
	JsonSerializer.Deserialize<FreshdeskContactDto[]>(freshdeskContactAsJson)![0];

FreshdeskContact ConvertToFreshdeskContact(GithubAccount account) =>
	mapper.Map<FreshdeskContact>(account);

static string SerializeToFreshdeskContact(FreshdeskContact contact) =>
	JsonSerializer.Serialize(contact);

static IMapper CreateMapper() =>
	new Mapper(new MapperConfiguration(cfg =>
		cfg.AddProfile<AutoMapperProfile>()));

static string ReadAndValidateInput(string? input)
{
	while (string.IsNullOrWhiteSpace(input))
	{
		Console.Write("Please enter a valid text: ");
		input = Console.ReadLine();
	}

	return input;
}

static bool IsRegistered(string contact) =>
	contact != "[]";