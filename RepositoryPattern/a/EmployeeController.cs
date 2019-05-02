using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.a
{
    public class EmployeeController : Controller
    {
        private UnitOfWork<EmployeeDBContext> unitOfWork =
            new UnitOfWork<EmployeeDBContext>();
        private GenericRepository<Employee> repository;
        private IEmployeeRepository employeeRepository;

        public EmployeeController()
        {
            repository = new GenericRepository<Employee>(unitOfWork);
            employeeRepository = new EmployeeRepository(unitOfWork);
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee model)
        {
            try
            {
                unitOfWork.CreateTransaction();
                if (ModelState.IsValid)
                {
                    repository.Insert(model);
                    unitOfWork.Save();
                    return RedirectToAction("Index", "Employee");
                }
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditEmployee(int EmployeeId)
        {
            Employee model = repository.GetById(EmployeeId);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee model)
        {
            if(ModelState.IsValid)
            {
                repository.Update(model);
                unitOfWork.Save();
                return RedirectToAction("Index", "Employee");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteEmployee(int EmployeeId)
        {
            Employee model = repository.GetById(EmployeeId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int EmployeeID)
        {
            Employee model = repository.GetById(EmployeeID);
            repository.Delete(model);
            unitOfWork.Save();
            return RedirectToAction("Index", "Employee");
        }
    }
}
