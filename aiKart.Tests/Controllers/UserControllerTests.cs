using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using aiKart.Controllers;
using aiKart.Interfaces;
using aiKart.Models;
using aiKart.Dtos.UserDtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace aiKart.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<IDeckService> mockDeckService;
        private readonly Mock<IMapper> mockMapper;
        private readonly UserController userController;

        public UserControllerTests()
        {
            mockUserService = new Mock<IUserService>();
            mockDeckService = new Mock<IDeckService>();
            mockMapper = new Mock<IMapper>();
            userController = new UserController(mockUserService.Object, mockDeckService.Object, mockMapper.Object);
        }

        [Fact]
        public void GetUsers_ReturnsOk()
        {
            var userList = new List<User> { new User { Id = 1, Name = "User1" } };
            var userDtoList = new List<UserDto> { new UserDto("User1") };

            mockUserService.Setup(s => s.GetUsers()).Returns(userList);
            mockMapper.Setup(m => m.Map<List<UserDto>>(userList)).Returns(userDtoList);

            var result = userController.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userDtoList, okResult.Value as List<UserDto>);
        }

        [Fact]
        public void GetUser_ReturnsOk()
        {
            var user = new User { Id = 1, Name = "User1" };
            var userDto = new UserDto("User1");

            mockUserService.Setup(s => s.UserExists(1)).Returns(true);
            mockUserService.Setup(s => s.GetUser(1)).Returns(user);
            mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            var result = userController.GetUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userDto, okResult.Value as UserDto);
        }


        [Fact]
        public void AddUser_Success()
        {
            var userDto = new UserDto("NewUser");
            var user = new User { Name = "NewUser" };
            var userResponseDto = new UserResponseDto(1, "NewUser");

            mockUserService.Setup(s => s.GetUsers()).Returns(new List<User>());
            mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
            mockUserService.Setup(s => s.AddUser(user)).Returns(true);
            mockMapper.Setup(m => m.Map<UserResponseDto>(user)).Returns(userResponseDto);

            var result = userController.AddUser(userDto);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(userResponseDto, createdAtResult.Value as UserResponseDto);
        }



        [Fact]
        public void AddUser_UserExists()
        {
            var userDto = new UserDto("ExistingUser");
            var existingUser = new User { Id = 1, Name = "ExistingUser" };
            var existingUserDto = new UserResponseDto(1, "ExistingUser");

            mockUserService.Setup(s => s.GetUsers()).Returns(new List<User> { existingUser });
            mockMapper.Setup(m => m.Map<UserResponseDto>(existingUser)).Returns(existingUserDto);

            var result = userController.AddUser(userDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(existingUserDto, okResult.Value as UserResponseDto);
        }



        [Fact]
        public void AddUser_Failure()
        {
            var userDto = new UserDto("NewUser");
            var user = new User { Name = "NewUser" };

            mockUserService.Setup(s => s.GetUsers()).Returns(new List<User>());
            mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);
            mockUserService.Setup(s => s.AddUser(user)).Returns(false);

            var result = userController.AddUser(userDto);

            Assert.IsType<ObjectResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }



    }
}
