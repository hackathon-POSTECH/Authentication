using AUTHENTICATION.API.Controllers;
using AUTHENTICATION.API.request;
using AUTHENTICATION.APPLICATION.Authentication.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace APITEST
{
    public class AuthenticationControllerTest
    {
        private readonly IMediator _mediatorMock;

        public AuthenticationControllerTest()
        {
            _mediatorMock = Substitute.For<IMediator>();
        }


        [Fact]
        public async void Should_Return_Created_When_Create_New_User()
        {
            var mockResult = new CreateUserResponse();
            var request = new CreateUserRequest();

            _mediatorMock.Send(request.ToCommand(), Arg.Any<CancellationToken>())
               .Returns(mockResult);

            var controller = new AuthenticationController(_mediatorMock);

            var result = await controller.RegisterAsync(request);

            Assert.IsType<CreatedResult>(result);

        }
    }
}