using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface IDayService
{
    void RemoveById(string id);

    void Add(Day day);
}