using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Events.Handlers;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;

namespace OnlineShop.Domain.Tests
{
    public class AccountTests
    {
        [Fact]
        public async Task Account_registred_event_notifies_the_user_by_email()
        {
            var account = new Account(Guid.Empty, "John", "Hello", "hgg@mail.ru", new[] { Role.Customer });

            var emailSenderMock = new Mock<IEmailSender>();
            var handler = new UserRegistrationNotificationByEmailHandler(emailSenderMock.Object);
            var @event = new AccountRegistredEvent(account);

            await handler.Handle(@event, default);
            handler.Should().BeAssignableTo<INotificationHandler<AccountRegistredEvent>>();
            emailSenderMock
                .Verify(it => it.SendEmailAsync(account.Email, It.IsAny<string>(), It.IsAny<string>(), default));
        }

        [Theory]
        [InlineData("Hello123", "Hello123", null)]
        [InlineData("Hello123", null, "dog@mail.ru")]
        [InlineData(null, "Hello123", "dog@mail.ru")]
        [InlineData(null, null, null)]
        public async Task Account_with_null_fileds_throw_exception
            (string login, string password, string email)
        {
            //Arrange
            var hasherMock = new Mock<IApplicationPasswordHasher>();
            var mediatorMock = new Mock<IMediator>();
            var cart = new Cart(Guid.NewGuid());
            var uowMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService
               (uowMock.Object, hasherMock.Object, mediatorMock.Object);
            //Act Assert
            await FluentActions.Invoking(async () =>
            {
                await accountService.Register
                    (login, password, email, new[] { Role.Customer }, default);
            })
                .Should()
                .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Account_is_registred()
        {
            //Arrange
            var faker = new Faker();
            var login = "Login";
            var email = faker.Internet.Email();
            var password = faker.Internet.Password();
            var hasherMock = new Mock<IApplicationPasswordHasher>();
            var mediatorMock = new Mock<IMediator>();
            var cart = new Cart(Guid.NewGuid());
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup
                (it => it.AccountRepozitory
                .FindAccountByLogin(login, default))
                .ReturnsAsync((Account?)null);
            uowMock.Setup(it => it.CartRepozitory.Add(cart, default));
            var accountService = new AccountService
               (uowMock.Object, hasherMock.Object, mediatorMock.Object);
            hasherMock.Setup(it => it.HashPassword(password)).Returns(password).Verifiable();

            //Act
            await accountService.Register
                (login, password, email, new[] { Role.Customer }, default);
            //Assert
            uowMock.Verify(it => it.AccountRepozitory.Add(It.IsAny<Account>(), default));
            uowMock.Verify(it => it.CartRepozitory.Add(It.IsAny<Cart>(), default));
        }

        [Fact]
        public async Task Account_with_the_same_login_is_existed()
        {
            //Arrange
            var faker = new Faker();
            var password = faker.Internet.Password();
            var email = faker.Internet.Email();
            var login = "Login";
            var hasherMock = new Mock<IApplicationPasswordHasher>();
            var mediatorMock = new Mock<IMediator>();
            var existedAccount = new Account(Guid.Empty, login, password, email, new[] { Role.Customer });
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(it => it.AccountRepozitory
            .FindAccountByLogin(login, default))
                .ReturnsAsync(existedAccount);
            var accountService = new AccountService
                (uowMock.Object, hasherMock.Object, mediatorMock.Object);

            //Act Assert
            await FluentActions.Invoking(async () =>
            {
                await accountService.Register
                    (login, password, email, new[] { Role.Customer }, default);
            })
                .Should()
                .ThrowAsync<EmailAlreadyExistsException>();
        }
    }
}
