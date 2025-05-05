using AutoMapper;
using SACS.Data.Models;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.Profiles;

public class SummaryProfile : Profile
{
    public SummaryProfile()
    {
        CreateMap<CreateEmployeeAndSummaryViewModel, Summary>();
        CreateMap<Summary, CreateEmployeeAndSummaryViewModel>();
    }
}