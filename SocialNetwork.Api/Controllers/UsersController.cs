using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.DataTransferObjects.Post;
using SocialNetwork.Api.DataTransferObjects.User;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> userRepository;

    public UsersController(IRepository<User> userRepository)
    {
        this.userRepository = userRepository;
    }

    //https://localhost:50670/users?username=a
    [HttpGet(Name = "GetUsers")]
    public ActionResult<IEnumerable<UserListDto>> GetUsers([FromQuery] string? username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return Ok(userRepository.Get()
                .Select(x => new UserListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username
                }));
        }
        var users = userRepository.Filter(x => x.Username.StartsWith(username));
        return Ok(users.Select(x => new UserListDto
        {
            Id = x.Id,
            Name = x.Name,
            Username = x.Username
        }));
    }

    //https://localhost:50670/users/{id}
    [HttpGet("{id}")]
    public ActionResult<UserDetailDto> GetUserById(int id)
    {
        var user = userRepository.GetById(id);
        if (user is null)
        {
            return BadRequest("No existe el usuario");
        }
        return Ok(new UserDetailDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Posts = user.Posts.Select(x => new PostDetailDto
            {
                Id = x.Id,
                Content = x.Content,
                UserId = x.UserId
            }).ToList()
        });
    }

    [HttpPost]
    public ActionResult<UserDetailDto> CreateUser([FromBody] UserCreateDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Username = userDto.Username
        };
        var newUser = userRepository.Add(user);
        return Ok(new UserDetailDto
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Username = newUser.Username
        });
    }

    [HttpPut("{id}")]
    public ActionResult<UserDetailDto> UpdateUser(int id, [FromBody] UserCreateDto user)
    {
        var updatedUser = userRepository.Update(new User
        {
            Id = id,
            Name = user.Name,
            Username = user.Username
        });

        return Ok(new UserDetailDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Username = updatedUser.Username
        });
    }
}

