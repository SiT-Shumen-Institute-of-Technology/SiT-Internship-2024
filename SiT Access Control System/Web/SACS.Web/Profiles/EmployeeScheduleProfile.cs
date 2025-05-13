using AutoMapper;
using SACS.Data.Models;
using SACS.Web.ViewModels;

namespace SACS.Web.Profiles;

public class EmployeeScheduleProfile : Profile
{
    public EmployeeScheduleProfile()
    {
        CreateMap<ScheduleViewModel, EmployeeSchedule>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.EmployeeId))
        .ForMember(dest => dest.User, opt => opt.Ignore())
        .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<EmployeeSchedule, ScheduleViewModel>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.UserId));
    }
}