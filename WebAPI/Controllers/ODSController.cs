﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ODSController : ControllerBase
    {
        // GET: api/<ODSController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ODSController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ODSController>
        [HttpPost]
        public void Post([FromBody] ODS ods)
        {

        }

        // PUT api/<ODSController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ODSController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class ODS
    {
        public int id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<double> temp { get; set; }
        public Nullable<double> humanity { get; set; }
        public Nullable<double> ldr { get; set; }
        public string explain { get; set; }
        public Nullable<double> uzaklik { get; set; }
        public Nullable<double> su { get; set; }
    }
}