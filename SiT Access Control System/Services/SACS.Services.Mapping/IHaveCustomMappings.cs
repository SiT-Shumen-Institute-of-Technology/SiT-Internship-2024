using AutoMapper;

namespace SACS.Services.Mapping;

public interface IHaveCustomMappings
{
    void CreateMappings(IProfileExpression configuration);
}