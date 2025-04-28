using SACS.Web.ViewModels;
using SACS.Data.Models;

namespace SACS.Web.Profiles
{
    using AutoMapper;

    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            this.CreateMap<CreateEmployeeAndSummaryViewModel, Employee>();
            this.CreateMap<Employee, CreateEmployeeAndSummaryViewModel>();
        }
    }
}
