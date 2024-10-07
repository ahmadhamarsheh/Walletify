using AutoMapper;
using Walletify.Models.Entities;
using Walletify.ViewModel.Dashboard;
namespace Walletify.ViewModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Transaction, DashboardTransactionViewModel>().ReverseMap();
            //CreateMap<AccountViewModel, Account>().ReverseMap();


        }

    }
}
