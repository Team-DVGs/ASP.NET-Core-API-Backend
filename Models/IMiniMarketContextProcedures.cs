﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Do_an_mon_hoc.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Do_an_mon_hoc.Models
{
    public partial interface IMiniMarketContextProcedures
    {
        Task<int> uspAddUserAsync(string pEmail, string pPassword, string pFullName, OutputParameter<string> responseMessage, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}