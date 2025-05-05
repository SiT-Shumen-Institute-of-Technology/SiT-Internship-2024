using System;
using System.Threading.Tasks;

namespace SACS.Data.Common;

public interface IDbQueryRunner : IDisposable
{
    Task RunQueryAsync(string query, params object[] parameters);
}