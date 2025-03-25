﻿namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Models;

    public interface IRFIDCardService
    {
        List<RFIDCard> All();

        void Add(RFIDCard rfidCard);
    }
}
