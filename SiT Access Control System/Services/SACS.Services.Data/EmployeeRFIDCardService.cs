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

    public class EmployeeRFIDCardService : IEmployeeRFIDCardService
    {
        private readonly IDeletableEntityRepository<EmployeeRFIDCard> employeeRFIDCardRepository;

        public EmployeeRFIDCardService(IDeletableEntityRepository<EmployeeRFIDCard> employeeRFIDCardRepository)
        {
            this.employeeRFIDCardRepository = employeeRFIDCardRepository;
        }

        public async Task AddEmployeeAndRFIDCardServiceAsync(EmployeeRFIDCard employeeRFIDCard)
        {
            await this.employeeRFIDCardRepository.AddAsync(employeeRFIDCard);
            await this.employeeRFIDCardRepository.SaveChangesAsync();
        }

        public List<EmployeeRFIDCard> All()
        {
            return this.employeeRFIDCardRepository.All().ToList();
        }
    }
}
