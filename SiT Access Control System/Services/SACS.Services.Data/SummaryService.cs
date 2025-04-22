namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;

    public class SummaryService : ISummaryService
    {
        private readonly IDeletableEntityRepository<DailySummary> summaryRepository;

        public SummaryService(IDeletableEntityRepository<DailySummary> summaryRepository)
        {
            this.summaryRepository = summaryRepository;
        }

        public async Task CreateSummaryAsync(DailySummary summary)
        {
            await this.summaryRepository.AddAsync(summary);
            await this.summaryRepository.SaveChangesAsync();
        }

        public List<DailySummary> GetAllSummaries()
        {
            return this.summaryRepository.All().ToList();
        }
    }
}
