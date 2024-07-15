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
    public partial class MiniMarketContext
    {
        private IMiniMarketContextProcedures _procedures;

        public virtual IMiniMarketContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new MiniMarketContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IMiniMarketContextProcedures GetProcedures()
        {
            return Procedures;
        }

        protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
        {
        }
    }

    public partial class MiniMarketContextProcedures : IMiniMarketContextProcedures
    {
        private readonly MiniMarketContext _context;

        public MiniMarketContextProcedures(MiniMarketContext context)
        {
            _context = context;
        }

        public virtual async Task<int> uspAddUserAsync(string pEmail, string pPassword, string pFullName, OutputParameter<string> responseMessage, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterresponseMessage = new SqlParameter
            {
                ParameterName = "responseMessage",
                Size = 500,
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = responseMessage?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "pEmail",
                    Size = 508,
                    Value = pEmail ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "pPassword",
                    Size = 50,
                    Value = pPassword ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "pFullName",
                    Size = 100,
                    Value = pFullName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterresponseMessage,
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[uspAddUser] @pEmail, @pPassword, @pFullName, @responseMessage OUTPUT", sqlParameters, cancellationToken);

            responseMessage.SetValue(parameterresponseMessage.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
