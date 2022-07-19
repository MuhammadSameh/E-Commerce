using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<User>>> GetUsers()
        {
            return Ok(await _repo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            return Ok(await _repo.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] RegisterDto registerDto)
        {

            User user = _mapper.Map<User>(registerDto);

            //## Assigned [Password] in RegisterDto to [passwordHash] in User directlry XXX
            //          in mapper class

            //## need to be nullable in db
            //## ↓↓             ↓↓              ↓↓
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 1;
            //## ↑↑             ↑↑              ↑↑

            await _repo.Add(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        [HttpPost("password/{id}")]
        public async Task<ActionResult<User>> UpdateUserPassword(string id, [FromBody] UserPasswordDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }
            else if (userDto is null)
            {
                return NotFound();
            }


            User user = await _repo.GetById(id);
            user.PasswordHash = userDto.Password;

            _repo.Update(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);

        }



        [HttpPost("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            User user = await _repo.GetById(id);

            if (user is null)
                return NotFound();
            if (user.Id != id)
                return NotFound();


            _repo.Delete(user);

            return NoContent();
        }
    }
}
