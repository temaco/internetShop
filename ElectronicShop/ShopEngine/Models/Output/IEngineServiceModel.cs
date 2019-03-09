using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Output
{
    public enum OperationCode
    {
        Success,
        Error
    }
    public interface IEngineServiceModel
    {
        int ActualID { get; set; }
        OperationCode OperationCode { get; set; }
    }
}