using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace crudInMacos
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // we want json  not those weird string ? Ohh Microsoft ?
        [HttpGet]
        public ActionResult Get()
        {
            PersonRepository personRepository = new();

            List<PersonModel> data = personRepository.Read();

            return Ok(new { data });
        }
        // what if we need post ? we don't some idiot playing with mvc / / / / ?
        [HttpPost]
        public ActionResult Post()
        {
            var status = false;
            // wait // anybody tell this before ? 
            var mode = Request.Form["mode"];

            var name = Request.Form["name"];
            var age = Request.Form["age"];
            var personId = Request.Form["personId"];

            PersonRepository personRepository = new();

            List<PersonModel> data = new();

            var code = "";
            // but we think something missing .. what ya ? 
            switch (mode)
            {
                case "create":
                    // for ide like php storm maybe they will angry ? wher's my catch ?
                    try
                    {
                        personRepository.Create(name, Convert.ToInt32(age));
                        // do remember if like this is will return CREATE SUCCESS ReturnCode.CREATE_SUCCESS.ToString();

                        code = ((int)ReturnCode.CREATE_SUCCESS).ToString();
                        status = true;

                    }
                    catch (Exception ex)
                    {
                        code = ex.Message;

                    }
                    break;
                case "read":
                    try
                    {
                        data = personRepository.Read();
                        code = ((int)ReturnCode.CREATE_SUCCESS).ToString();
                        status = true;

                    }
                    catch (Exception ex)
                    {
                        code = ex.Message;

                    }

                    break;
                case "update":
                    try
                    {
                        personRepository.Update(name, Convert.ToInt32(age), Convert.ToInt32(personId));
                        code = ((int)ReturnCode.UPDATE_SUCCESS).ToString();
                        status = true;

                    }
                    catch (Exception ex)
                    {
                        code = ex.Message;

                    }
                    break;
                case "delete":
                    try
                    {
                        personRepository.Delete(Convert.ToInt32(personId));

                        code = ((int)ReturnCode.DELETE_SUCCESS).ToString();
                        status = true;

                    }
                    catch (Exception ex)
                    {
                        code = ex.Message;

                    }
                    break;
                default:
                    // do remmeber hacker always exist anywhere ?
                    code = ((int)ReturnCode.ACCESS_DENIED_NO_MODE).ToString();
                    break;
            }

            return Ok(new { status, code, data });
        }

    }
}
