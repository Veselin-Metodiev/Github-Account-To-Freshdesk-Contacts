# GitHub Account to Freshdesk Contact

This C# application uses GitHub API v3 and Freshdesk API v2 to perform the following tasks:
- Retrieve a GitHub account using the GitHub API
- Create or Update a Freshdesk contact using the Freshdesk API
- Save the login, name, and creation date of the GitHub account to an MS SQL Database

## Prerequisites

Before running the application, make sure you have the following:

- Visual Studio
- .Net 7
- Github personal access token with permision to read user email address: [Create a personal access token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token).
- Freshdesk account, domain and API key: [Generate an API key](https://developers.freshdesk.com/api/#authentication).
- MS SQL Server.

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/Veselin-Metodiev/Github-Account-To-Freshdesk-Contacts.git
   
2. Open the project in Visual Studio
3. Set up environmental variables
   - Freshdesk API Key: Create an environmental variable named FRESHDESK_TOKEN and set its value to your Freshdesk API key.
   - GitHub API Token: Create an environmental variable named GITHUB_TOKEN and set its value to your GitHub API token.
4. Run the provided SQL script to create the database and tables.
5. Configure the connection string using user secrets:
   - Right-click on the project in Visual Studio and select "Manage User Secrets."
   - Add the following JSON configuration:
   <br/>
   
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server={Your_Server_Name};Database=GithubAccountToFreshdeskContact;Integrated Security=True;TrustServerCertificate=True;"
     }
   }
   ```
   - Replace "Your_Server_Name" with your actual MS SQL Server Name. Feel free to edit the connection string to your needs.
6. Run the application.

## Usage

1. When prompted, enter the GitHub username for the account you want to retrieve information for given you have the token.
2. The application will prompt you again to enter your freshdesk subdomain.
3. The application will save the login, name and creation date in your database.
4. Finally, the application will create or update a contact depending if you have a contact with the same name.
