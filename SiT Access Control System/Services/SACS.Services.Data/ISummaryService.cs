﻿namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public interface ISummaryService
    {
        public List<Summary> GetAllSummaries();
    }
}
