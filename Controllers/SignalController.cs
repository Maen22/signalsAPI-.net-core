using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Authentication;
using Server.Entities;
using Server.Helpers;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private ISignalService _signalService;
        private IMapper _mapper;
        private string userRole;

        public SignalController(
            ISignalService signalService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _signalService = signalService;
            _mapper = mapper;
            userRole = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("signal/create")]
        public IActionResult createSignal([FromBody] SignalModel model)
        {
            var signal = _mapper.Map<Signal>(model);
            try
            {
                // create signal
                _signalService.create(signal);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("signal/{id}")]
        public IActionResult activateSignal(int id)
        {
            _signalService.activateSignal(id);
            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("signal/{id}")]
        public IActionResult deactivateSignal(int id)
        {
            _signalService.deactivateSignal(id);
            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("signal/confirm")]
        public IActionResult toggleConfirm()
        {
            AppSettings.AutoConfirm = !AppSettings.AutoConfirm;
            return Ok();
        }

        [HttpGet("signals")]
        public IActionResult GetAll()
        {
            var signals = _signalService.GetAll();
            if (userRole == "Admin") // All signals for the admin
            {
                var model = _mapper.Map<IList<SignalModel>>(signals);
                return Ok(model);
            } else // Only active signals for normal user
            {
                signals = signals.ToList<Signal>();
                var signalQuery =
                    from Signal signal in signals
                    where signal.Status == Status.Active
                    select signal;
                var model = _mapper.Map<IList<SignalModel>>(signalQuery);
                return Ok(model);
            }
            
        }
    }
}