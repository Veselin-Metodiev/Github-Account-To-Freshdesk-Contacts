using Github_Data_To_Freshdesk_Contacts.Models;

namespace Github_Data_To_Freshdesk_Contacts;

using AutoMapper;

internal class AutoMapperProfile : Profile
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
	}
}