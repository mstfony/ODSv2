
using Business.Handlers.SensorValues.Commands;
using Business.Handlers.SensorValues.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    /// <summary>
    /// SensorValues If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SensorValuesController : BaseApiController
    {
        ///<summary>
        ///List SensorValues
        ///</summary>
        ///<remarks>SensorValues</remarks>
        ///<return>List SensorValues</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SensorValue>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetSensorValuesQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>SensorValues</remarks>
        ///<return>SensorValues List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new GetSensorValueQuery { Id = id });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add SensorValue.
        /// </summary>
        /// <param name="createSensorValue"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSensorValueCommand createSensorValue)
        {
            var result = await Mediator.Send(createSensorValue);
            if (result.Success)
            {
              
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        public class SensorValueSign : Hub
        {
            public override Task OnConnectedAsync()
            {
                return Clients.Client(Context.ConnectionId).SendAsync("GetConnectionID", Context.ConnectionId);
            }
           
            public Task PushValues(List<SensorValue> sensorValue)
            {
                
                return Clients.All.SendAsync("GetValues", sensorValue);
            }
        }

        /// <summary>
        /// Update SensorValue.
        /// </summary>
        /// <param name="updateSensorValue"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSensorValueCommand updateSensorValue)
        {
            var result = await Mediator.Send(updateSensorValue);
            if (result.Success)
            {
               
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete SensorValue.
        /// </summary>
        /// <param name="deleteSensorValue"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteSensorValueCommand deleteSensorValue)
        {
            var result = await Mediator.Send(deleteSensorValue);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
