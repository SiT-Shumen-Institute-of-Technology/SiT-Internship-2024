namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;

    public class DayService : IDayService
    {
        private readonly IDeletableEntityRepository<Day> dayRepository;

        public DayService(IDeletableEntityRepository<Day> dayRepository)
        {
            this.dayRepository = dayRepository;
        }

        public async Task AddAsync(Day day)
        {
            await this.dayRepository.AddAsync(day);
            await this.dayRepository.SaveChangesAsync();
        }

        public async Task RemoveByIdAsync(string id)
        {
            var choosenDay = this.dayRepository.All().FirstOrDefault(x => x.Id == id);
            this.dayRepository.Delete(choosenDay);
            await this.dayRepository.SaveChangesAsync();
        }
    }
}
