using AutoMapper;
using SACS.Data.Models;
using SACS.Web.ViewModels;

namespace SACS.Web.Profiles;

public class EmployeeScheduleProfile : Profile
{
    public EmployeeScheduleProfile()
    {
        CreateMap<ScheduleViewModel, EmployeeSchedule>();
        CreateMap<EmployeeSchedule, ScheduleViewModel>();
    }
}