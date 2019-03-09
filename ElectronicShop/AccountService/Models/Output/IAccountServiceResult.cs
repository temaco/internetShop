using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountService.Models.Output
{
    public enum OperationCode
    {
        Success,
        Error,
        NotFound,
        FoundCopy,
        ConfirmedError
    }


    public interface IAccountServiceResult
    {
        int ActualID { get; set; }
        OperationCode OperationCode { get; set; }
    }
}