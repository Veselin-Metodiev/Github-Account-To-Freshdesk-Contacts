using Github_Account_To_Freshdesk_Contacts;
using Github_Account_To_Freshdesk_Contacts.DbModels;
using Microsoft.Extensions.Configuration;

ConfigurationBuilder builder  = new ();

builder.AddUserSecrets<Program>();

GithubAccountToFreshdeskContactContext context = new(builder);

Engine engine = new (context);
await engine.Run();