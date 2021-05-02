using FakeItEasy;
using LogProxy.API.Controllers;
using LogProxy.API.Entities;
using LogProxy.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LogProxy.API.UnitTests
{
    public class MessageControllerTests
    {
        [Fact]
        public async Task GetAllMessages_Returns_CorrectNumberOfMessages()
        {
            const int count = 3;
            var fakeMessages = A.CollectionOfDummy<Message>(count).AsEnumerable();
            var mockService = A.Fake<IMessageService>();
            A.CallTo(() => mockService.GetAllAsync()).Returns(Task.FromResult(fakeMessages));
            var controller = new MessageController(mockService);

            var actionResult = await controller.Get();

            var result = actionResult.Result as OkObjectResult;
            var returnMessages = result.Value as IEnumerable<Message>;
            Assert.Equal(count, returnMessages.Count());
        }

        [Fact]
        public async Task GetAllMessages_ReturnsNotFound_WithZeroMessages()
        {
            const int count = 0;
            var fakeMessages = A.CollectionOfDummy<Message>(count).AsEnumerable();
            var mockService = A.Fake<IMessageService>();
            A.CallTo(() => mockService.GetAllAsync()).Returns(Task.FromResult(fakeMessages));
            var controller = new MessageController(mockService);

            var actionResult = await controller.Get();

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetMessageWithId_Returns_CorrectMessage()
        {
            const string recordId = "xxx";
            var message = new Message() { Id = "1", Title = "Title", Text = "Text", ReceivedAt = DateTime.Now };
            var mockService = A.Fake<IMessageService>();
            A.CallTo(() => mockService.GetByIdAsync(recordId)).Returns(message);
            var controller = new MessageController(mockService);

            var actionResult = await controller.Get(recordId);
            var result = actionResult.Result as OkObjectResult;
            var resultValue = result.Value as Message;

            Assert.Equal(message.Id, resultValue.Id);
            Assert.Equal(message.Title, resultValue.Title);
            Assert.Equal(message.Text, resultValue.Text);
            Assert.Equal(message.ReceivedAt, resultValue.ReceivedAt);
        }

        [Fact]
        public async Task GetMessageWithId_ReturnsNotFound_IfMessageNotExists()
        {
            const string recordId = "xxx";
            Message message = null;
            var mockService = A.Fake<IMessageService>();
            A.CallTo(() => mockService.GetByIdAsync(recordId)).Returns(message);
            var controller = new MessageController(mockService);

            var actionResult = await controller.Get(recordId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostMessage_ReturnsBadRequest_IfModelStaeIsNotValid()
        {
            var mockService = A.Fake<IMessageService>();
            var controller = new MessageController(mockService);
            var message = new Message();
            controller.ModelState.AddModelError("error", "some error");

            var actionResult = await controller.Post(message);

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostMessage_ReturnsCreatedAtActionResult_IfModelStateIsValid()
        {
            var message = new Message() { Title = "Title", Text = "Text" };
            var mockService = A.Fake<IMessageService>();
            var controller = new MessageController(mockService);

            var actionResult = await controller.Post(message);

            Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostMessage_ReturnsCreatedItem_IfModelStateIsValid()
        {
            var message = new Message() { Title = "Title", Text = "Text" };
            var mockService = A.Fake<IMessageService>();
            var controller = new MessageController(mockService);

            var actionResult = await controller.Post(message);
            var result = actionResult.Result as CreatedAtActionResult;
            var item = result.Value as Message;

            Assert.IsType<Message>(item);
            Assert.Equal(message.Title, item.Title);
        }
    }
}