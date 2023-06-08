namespace Github_Account_To_Freshdesk_Contacts;

using DbModels;
using AutoMapper;
using Models;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<GithubAccount, FreshdeskContact>()
			.ForMember(m => m.Address, opt => opt
				.MapFrom(s => s.Location))
			.ForMember(m => m.Description, opt => opt
				.MapFrom(s => s.Bio))
			.ForMember(m => m.TwitterId, opt => opt
				.MapFrom(s => s.TwitterUsername));

		CreateMap<GithubAccount, GithubAccountDb>()
			.ForMember(m => m.CreationDate, opt => opt
				.MapFrom(s => s.CreatedAt));
	}
}