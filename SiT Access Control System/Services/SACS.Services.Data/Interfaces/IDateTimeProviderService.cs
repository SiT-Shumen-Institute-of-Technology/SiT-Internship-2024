using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Services.Data.Interfaces
{
    public interface IDateTimeProviderService
    {
        public DateTime Today { get; }
    }
}
