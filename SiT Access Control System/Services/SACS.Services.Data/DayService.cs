using System.Linq;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

public class DayService : IDayService
{
    private readonly IDeletableEntityRepository<Day> dayRepository;

    public DayService(IDeletableEntityRepository<Day> dayRepository)
    {
        this.dayRepository = dayRepository;
    }

    public void Add(Day day)
    {
        dayRepository.AddAsync(day);
        dayRepository.SaveChangesAsync();
    }

    public void RemoveById(string id)
    {
        var choosenDay = dayRepository.All().FirstOrDefault(x => x.Id == id);
        dayRepository.Delete(choosenDay);
        dayRepository.SaveChangesAsync();
    }
}