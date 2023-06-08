using Github_Account_To_Freshdesk_Contacts.DbModels;

namespace Github_Account_To_Freshdesk_Contacts.Controllers;

public static class DbController
{
	public static void AddGithubAccount(GithubAccountDb account, GithubAccountToFreshdeskContactContext context)
	{
		context.GithubAccounts.Add(account);
		context.SaveChanges();
	}

	public static void UpdateGithubAccount(GithubAccountDb account, GithubAccountToFreshdeskContactContext context)
	{
		GithubAccountDb oldAccount = context.GithubAccounts.First(a => a.Login == account.Login);
		context.GithubAccounts.Remove(oldAccount);
		context.GithubAccounts.Add(account);
		context.SaveChanges();
	}
}