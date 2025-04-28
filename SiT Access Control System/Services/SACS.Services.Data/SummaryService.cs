namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data.Interfaces;

    public class SummaryService : ISummaryService
    {
        private readonly IRepository<Summary> summaryRepository;

        public SummaryService(IRepository<Summary> summaryRepository)
        {
            this.summaryRepository = summaryRepository;
        }

        public async Task CreateSummaryAsync(Summary summary)
        {
            await this.summaryRepository.AddAsync(summary);
            await this.summaryRepository.SaveChangesAsync();
        }

        public List<Summary> GetAllSummaries()
        {
            return this.summaryRepository.All().ToList();
        }
    }
}
