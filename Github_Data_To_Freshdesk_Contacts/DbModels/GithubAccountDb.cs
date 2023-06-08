namespace Github_Account_To_Freshdesk_Contacts.DbModels;

public class GithubAccountDb
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }
}
