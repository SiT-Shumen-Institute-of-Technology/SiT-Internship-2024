using System.Collections.Generic;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IRFIDCardService
{
    List<RFIDCard> All();

    Task AddAsync(RFIDCard rfidCard);
}