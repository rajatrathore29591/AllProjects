using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IEmployeeService : IService
    {
        List<Employee> GetAllEmployee();
        Employee GetEmployeeByEmployeeId(Guid EmployeeId);
        string AddEmployee(EmployeeDataModel EmployeeDataModel);
        string UpdateEmployee(EmployeeDataModel EmployeeDataModel);
        bool UpdateDocumentId(Guid EmployeeId, Guid DocumentId);
        bool DeleteEmployee(Guid EmployeeId);
        Employee GetEmployeeByEmail(string Email);
        bool EditEmployeePassword(Guid EmployeeId, string NewPassword);
       
    }
}
