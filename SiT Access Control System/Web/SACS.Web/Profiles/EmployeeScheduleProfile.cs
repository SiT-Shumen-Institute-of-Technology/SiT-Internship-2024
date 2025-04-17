namespace SACS.Web.Profiles
{
    using AutoMapper;
    using SACS.Data.Models;
    using SACS.Web.ViewModels;

    public class EmployeeScheduleProfile : Profile
    {
        public EmployeeScheduleProfile()
        {
            this.CreateMap<ScheduleViewModel, EmployeeSchedule>();
            this.CreateMap<EmployeeSchedule, ScheduleViewModel>();
        }
    }
}
