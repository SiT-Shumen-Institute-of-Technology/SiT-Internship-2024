using SACS.Data.Models;
using SACS.Web.ViewModels;

namespace SACS.Web.Profiles
{
    using AutoMapper;

    public class EmployeeScheduleProfile : Profile
    {
        public EmployeeScheduleProfile()
        {
            this.CreateMap<ScheduleViewModel, EmployeeSchedule>();
            this.CreateMap<EmployeeSchedule, ScheduleViewModel>();
        }
    }
}
