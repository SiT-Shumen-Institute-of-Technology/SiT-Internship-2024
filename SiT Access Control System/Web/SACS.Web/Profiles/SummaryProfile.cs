using AutoMapper;
using SACS.Data.Models;
using SACS.Web.ViewModels;
namespace SACS.Web.Profiles
{
    public class SummaryProfile : Profile
    {
        public SummaryProfile() 
        {
            this.CreateMap<CreateEmployeeAndSummaryViewModel, Summary>();
            this.CreateMap<Summary, CreateEmployeeAndSummaryViewModel>();

        }
    }
}
