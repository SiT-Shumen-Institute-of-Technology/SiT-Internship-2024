namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public interface IEmployeeRFIDCardService
    {
        Task AddEmployeeAndRFIDCardServiceAsync(EmployeeRFIDCard employeeRFIDCard);

        List<EmployeeRFIDCard> All();
    }
}
