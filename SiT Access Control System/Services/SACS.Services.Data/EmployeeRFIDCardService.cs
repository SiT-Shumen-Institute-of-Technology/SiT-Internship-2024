using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

public class EmployeeRFIDCardService : IEmployeeRFIDCardService
{
    private readonly IDeletableEntityRepository<EmployeeRFIDCard> employeeRFIDCardRepository;

    public EmployeeRFIDCardService(IDeletableEntityRepository<EmployeeRFIDCard> employeeRFIDCardRepository)
    {
        this.employeeRFIDCardRepository = employeeRFIDCardRepository;
    }

    public async Task AddEmployeeAndRFIDCardServiceAsync(EmployeeRFIDCard employeeRFIDCard)
    {
        await employeeRFIDCardRepository.AddAsync(employeeRFIDCard);
        await employeeRFIDCardRepository.SaveChangesAsync();
    }

    public List<EmployeeRFIDCard> All()
    {
        return employeeRFIDCardRepository.All().ToList();
    }
}