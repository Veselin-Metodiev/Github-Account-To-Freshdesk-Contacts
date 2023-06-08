namespace Github_Account_To_Freshdesk_Contacts;

using Dtos;
using Models;

using AutoMapper;
using Controllers;


public class Engine
{
	public async Task Run()
	{
		Console.Write("Enter a Github username: ");
		string username = ReadAndValidateInput(Console.ReadLine());

		IMapper mapper = CreateMapper();

		string accountAsJson = await ApiController.GetGithubAccountInfo(username);
		GithubAccount account = DataProcessor.DeserializeToGithubAccount(accountAsJson);
		FreshdeskContact contact = DataProcessor.ConvertGithubAccountToFreshdeskContact(account, mapper);

		Console.Write("Enter an email: ");
		string email = ReadAndValidateInput(Console.ReadLine());
		contact.Email = email;

		string contactAsJson = DataProcessor.SerializeToFreshdeskContact(contact);

		Console.Write("Enter a Freshdesk subdomain: ");
		string subdomain = ReadAndValidateInput(Console.ReadLine());

		string freshdeskContactAsJson = await ApiController.GetFreshdeskContact(contact.Name, subdomain);

		if (IsRegistered(freshdeskContactAsJson))
		{
			FreshdeskContactDto freshdeskContact = 
				DataProcessor.DeserializeToFreshdeskContactDto(freshdeskContactAsJson);
			await ApiController.UpdateContact(
				subdomain, contactAsJson, freshdeskContact.Id, ApiController.CreateHttpClientForFreshdesk());

			Console.WriteLine("Successfully updated a new contact");
			return;
		}

		await ApiController.CreateContact(subdomain, contactAsJson, ApiController.CreateHttpClientForFreshdesk());

		Console.WriteLine("Successfully created a new contact");
	}

	public IMapper CreateMapper() =>
		new Mapper(new MapperConfiguration(cfg =>
			cfg.AddProfile<AutoMapperProfile>()));

	public string ReadAndValidateInput(string? input)
	{
		while (string.IsNullOrWhiteSpace(input))
		{
			Console.Write("Please enter a valid text: ");
			input = Console.ReadLine();
		}

		return input;
	}

	public bool IsRegistered(string contact) =>
		contact != "[]";
}