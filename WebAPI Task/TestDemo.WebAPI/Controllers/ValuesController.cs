using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Data;
using TestDemo.Business;
using TestDemo.Data.Entities;

// This values controller is using (Action Result return without await and Aysnc) 
//+ (Basic Entity Framework Connection) + try-catch(Nlog Dll log concpets) + Dependency Injection
namespace TestDemo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Authorize]  //Uncomment When you want to use JWT(Json Web Token) Aunentication
    public class ValuesController : ControllerBase
    {

        private readonly ILoggerManager logger;
        public ValuesController(ILoggerManager _logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public ActionResult Get()  //<IEnumerable<Employee>>
        {

            try
            {
                logger.LogInfo(" The Get() Method of Values Controller is called");
                using (var Context = new AppDataContext())
                {
                    var emplist = Context.Employee.ToList();
                    logger.LogInfo($"The Count of Employee is {emplist.Count} employees");
                    return Ok(emplist);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                logger.LogInfo(" The Get(id) Method of Values Controller is called");
                using (var context = new AppDataContext())
                {
                    var empresult = context.Employee.Find(id);
                    return Ok(empresult);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        // POST api/values

        [HttpPost]
        public ActionResult Post([FromBody] Employee EmpModel)
        {
            try
            {
                logger.LogInfo(" The Post(frombody,employee model) Method of Values Controller is called");
                using (var context = new AppDataContext())
                {
                    context.Employee.Add(EmpModel);
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Employee EmpModel)
        {
            try
            {
                logger.LogInfo(" The Put(id,Employee) Method of Values Controller is called");
                using (var context = new AppDataContext())
                {
                    var newuserupdate = context.Employee.Find(id);
                    newuserupdate.EmpName = EmpModel.EmpName;
                    newuserupdate.EmpSalary = EmpModel.EmpSalary;
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // DELETE api/values/5  //StudentList.RemoveAt(id);
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                logger.LogInfo(" The Delete(id) Method of Values Controller is called");
                using (var context = new AppDataContext())
                {
                    var deleteRecord = context.Employee.Find(id);
                    context.Employee.Remove(deleteRecord);
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Something Went Wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
