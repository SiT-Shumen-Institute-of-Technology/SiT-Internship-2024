using AutoMapper;
using SACS.Data.Models;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeAndSummaryViewModel, Employee>();
        CreateMap<Employee, CreateEmployeeAndSummaryViewModel>();
    }
}