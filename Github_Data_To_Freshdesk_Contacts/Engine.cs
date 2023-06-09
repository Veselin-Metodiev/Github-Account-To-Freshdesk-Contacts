namespace Github_Account_To_Freshdesk_Contacts;

using DbModels;
using Dtos;
using Models;
using AutoMapper;
using Controllers;


public class Engine
{
	private GithubAccountToFreshdeskContactContext context = null!;

	public GithubAccountToFreshdeskContactContext Context
	{
		get => context;
		set => context = value;
	}

	public Engine(GithubAccountToFreshdeskContactContext context)
	{
		Context = context;
	}

	public async Task Run()
	{
		Console.Write("Enter a Github username: ");
		string username = ReadAndValidateInput(Console.ReadLine());

		IMapper mapper = CreateMapper();

		string accountAsJson = await ApiController
			.GetGithubAccountInfo(username, ApiController.CreateHttpClientForGithub(username));

		GithubAccount account = DataProcessor.DeserializeToGithubAccount(accountAsJson);

		GithubAccountDb accountDb = DataProcessor.ConvertGithubAccountToGithubAccountDb(account, mapper);

		if (IsRegisteredDbEntity(accountDb.Login))
		{
			DbController.UpdateGithubAccount(accountDb, Context);
		}
		else
		{
			DbController.AddGithubAccount(accountDb, Context);
		}

		string accountEmailsAsJson = await ApiController
			.GetGithubAccountEmails(ApiController.CreateHttpClientForGithubEmail(username));

		GithubAccountEmail[] accountEmails = DataProcessor.DeserializeToGithubAccountEmails(accountEmailsAsJson);

		string primaryEmail = GetPrimaryEmail(accountEmails);

		account.Email = primaryEmail;

		FreshdeskContact contact = DataProcessor.ConvertGithubAccountToFreshdeskContact(account, mapper);

		string contactAsJson = DataProcessor.SerializeToFreshdeskContact(contact);

		Console.Write("Enter a Freshdesk subdomain: ");
		string subdomain = ReadAndValidateInput(Console.ReadLine());

		string freshdeskContactAsJson = await ApiController.GetFreshdeskContact(contact.Name, subdomain);

		if (IsRegisteredContact(freshdeskContactAsJson))
		{
			FreshdeskContactDto freshdeskContact = 
				DataProcessor.DeserializeToFreshdeskContactDto(freshdeskContactAsJson);
			await ApiController.UpdateContact(
				subdomain, contactAsJson, freshdeskContact.Id, ApiController.CreateHttpClientForFreshdesk());

			Console.WriteLine("Successfully updated a contact");
			return;
		}

		await ApiController.CreateContact(subdomain, contactAsJson, ApiController.CreateHttpClientForFreshdesk());

		Console.WriteLine("Successfully created a new contact");
	}

	public IMapper CreateMapper() =>
		new Mapper(new MapperConfiguration(cfg =>
			cfg.AddProfile<AutoMapperProfile>()));

	public bool IsRegisteredDbEntity(string login) =>
		Context.GithubAccounts.Any(a => a.Login == login);

	public string GetPrimaryEmail(GithubAccountEmail[] emails) =>
		emails.First(e => e.Primary).Email;

	public string ReadAndValidateInput(string? input)
	{
		while (string.IsNullOrWhiteSpace(input))
		{
			Console.Write("Please enter a valid text: ");
			input = Console.ReadLine();
		}

		return input;
	}

	public bool IsRegisteredContact(string contact) =>
		contact != "[]";
}