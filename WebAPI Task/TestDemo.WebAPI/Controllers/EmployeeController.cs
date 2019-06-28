using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using TestDemo.Data;
using TestDemo.Business;
using Microsoft.AspNetCore.Authorization;


// This Employee controller is using (Action Result return with await and Aysnc) 
//  + (Business Layer for Entity Framework) + Gloal Exception Filter using (Nlog Dll log concpets) + Depedency Injection
namespace TestDemo.WebAPI.Controllers
{

    [ApiController]
    // [Authorize] // Uncomment When you want to use JWT(Json Web Token) Aunentication
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly AppDataContext _context;
        private readonly ILoggerManager _logger;

        public EmployeeController(IRepository<Employee> repository, AppDataContext context, ILoggerManager logger)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
        }

        //[OverrideAuthorization]
        [Route("api/[controller]/Employees")]
        [HttpGet]

        public async Task<object> GetEmployees()  //<IEnumerable<Employee>>
        {
            _logger.LogInfo("User hit GetEmployees() action method in Employee Controller");

            var user = await _repository.GetAllAsync();

            _logger.LogInfo($"Employee Count is {user.Count}");

            if (user.Count > 0)
                return Ok(user);
            else
                return NotFound(user);
        }


        [Route("api/[controller]/GetEmployeeBy/{id}")]
        [HttpGet]

        public async Task<object> GetEmployeeBy(int id)
        {
            _logger.LogInfo("User hit GetEmployeeBy() action method in Employee Controller");

            var user = await _repository.GetAsync(id);

            _logger.LogInfo($" id for GetEmployeeBy action method is {id}");

            if (user != null)
                return Ok(user);
            else
                return NotFound(user);
        }


        [Route("api/[controller]/EmployeeDetailsInsertion")]
        [HttpPost]

        public async Task<object> EmployeeDetailsInsertion([FromBody] Employee Empmodel)
        {
            _logger.LogInfo("User hit EmployeeDetailsInsertion() action method in Employee Controller");

            await _repository.AddAsync(Empmodel);

            await _repository.SaveAsync();

            _logger.LogInfo($"Employee Data has been inserted succesfully, EmployeeId is {Empmodel.EmpId} ");
            return Ok();
        }


        [Route("api/[controller]/EmployeeDetailsUpdate/{id}")]
        [HttpPut]

        public async Task<object> EmployeeDetailsUpdate(int id, [FromBody] Employee EmpModel)
        {
            _logger.LogInfo("User hit EmployeeDetailsUpdate() action method in Employee Controller");

            var finduser = await _repository.GetAsync(id);

            _logger.LogInfo($"Id for which record is updating is {id} coming values from Reuqest body is EmpName: {EmpModel.EmpName} and EmpSalary;{EmpModel.EmpSalary} ");

            finduser.EmpName = EmpModel.EmpName;  // Updating these 2 columns only
            finduser.EmpSalary = EmpModel.EmpSalary;

            await _repository.SaveAsync();
            return Ok();
        }

        [Route("api/[controller]/EmployeeDeletion/{id}")]
        [HttpDelete]

        public async Task<object> EmployeeDeletion(int id)
        {
            _logger.LogInfo("User hit EmployeeDeletion() action method in Employee Controller");

            await _repository.DeleteAsync(id);

            _logger.LogInfo($"id for which employee record has been deleted is {id}");


            return Ok();

        }
    }
}