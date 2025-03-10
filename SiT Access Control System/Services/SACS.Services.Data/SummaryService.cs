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
        private readonly IDeletableEntityRepository<Summary> summaryRepository;

        public SummaryService(IDeletableEntityRepository<Summary> summaryRepository)
        {
            this.summaryRepository = summaryRepository;
        }

        public void CreateSummary(Summary summary)
        {
            this.summaryRepository.AddAsync(summary);
            this.summaryRepository.SaveChangesAsync();
        }

        public List<Summary> GetAllSummaries()
        {
            return this.summaryRepository.All().ToList();
        }
    }
}
