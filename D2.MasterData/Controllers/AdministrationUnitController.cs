using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using D2.MasterData.Controllers.Validators;
using D2.MasterData.Models;
using D2.MasterData.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D2.MasterData.Controllers
{
    [Route("[controller]")]
    public class AdministrationUnitController : Controller
    {
        private AdministrationUnitRepository _repository;
        private ModelValidator<AdministrationUnit> _validator;

        public AdministrationUnitController(
            AdministrationUnitRepository repository,
            ModelValidator<AdministrationUnit> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpPost]
        public IActionResult Post([FromBody]AdministrationUnit value)
        {
            var validation = _validator.Validate(value, "Post");

            if (validation.IsValid == false) return StatusCode(StatusCodes.Status422UnprocessableEntity); 
            
            _repository.Save(value);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
