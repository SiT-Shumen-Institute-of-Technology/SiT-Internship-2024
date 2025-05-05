using System.Collections.Generic;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IEmployeeRFIDCardService
{
    Task AddEmployeeAndRFIDCardServiceAsync(EmployeeRFIDCard employeeRFIDCard);

    List<EmployeeRFIDCard> All();
}