using API.Controllers;
using Common;
using Common.Enum;
using DTOs.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Exceptions;
using Service.Interface;
using Xunit;
using Assert = Xunit.Assert;

namespace Test.Controllers
{
    public class ContentControllerTests
    {
        private readonly Mock<IContentService> _contentServiceMock;
        private ContentController _contentController;

        public ContentControllerTests()
        {
            _contentServiceMock = new Mock<IContentService>();
            _contentController = new ContentController(_contentServiceMock.Object);
        }

        [Fact]
        public async Task GetContentsTest_ReturnsOkResult()
        {
            // Arrange
            var mockContents = new List<ContentDto>
            {
                new ContentDto
                {
                    ContentId = 1,
                    Title = "Sample Title",
                    ImageUrl = "http://example.com/image1.jpg",
                    Summary = "This is a summary",
                    ContentBody = "This is the content body",
                    DateCreate = DateTime.Now,
                    Status = ContentStatusEnum.Active.ToString(),
                },
                new ContentDto
                {
                    ContentId = 2,
                    Title = "Another Title",
                    ImageUrl = "http://example.com/image2.jpg",
                    Summary = "Another summary",
                    ContentBody = "This is another content body",
                    DateCreate = DateTime.Now,
                    Status = ContentStatusEnum.Active.ToString(),
                }
            };

            _contentServiceMock.Setup(service => service.GetContents())
                               .ReturnsAsync(mockContents);

            // Act
            var result = await _contentController.GetContents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ContentDto>>(okResult.Value);
            Assert.Equal(mockContents.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetContentDetailById_ReturnsOkResult()
        {
            // Arrange
            var contentId = 1;
            var mockContent = new ContentDto
            {
                ContentId = contentId,
                Title = "Sample Title",
                ImageUrl = "http://example.com/image1.jpg",
                Summary = "This is a summary",
                ContentBody = "This is the content body",
                DateCreate = DateTime.Now,
                Status = ContentStatusEnum.Active.ToString()
            };

            _contentServiceMock.Setup(service => service.GetContentDetailById(contentId))
                               .ReturnsAsync(mockContent);

            // Act
            var result = await _contentController.GetContentDetailById(contentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ContentDto>(okResult.Value);
            Assert.Equal(mockContent.ContentId, returnValue.ContentId);
            Assert.Equal(mockContent.Title, returnValue.Title);
        }

        [Fact]
        public async Task GetContentDetailById_ReturnsNotFoundResult()
        {
            // Arrange
            var contentId = 1;

            _contentServiceMock.Setup(service => service.GetContentDetailById(contentId))
                               .ThrowsAsync(new ServiceException(MessageConstant.Content.ContentNotFound));

            // Act
            var result = await _contentController.GetContentDetailById(contentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(MessageConstant.Content.ContentNotFound, notFoundResult.Value);
        }

        [Fact]
        public async Task CreateContent_ReturnsCreatedResult_WhenCreateIsSuccessful()
        {
            // Arrange
            var mockContentRequest = new ContentCreateRequestDto
            {
                Title = "New Content",
                Summary = "This is a summary",
                ContentBody = "This is the content body",
                ImageUrl = Mock.Of<IFormFile>()
            };

            //_contentServiceMock.Setup(service => service.CreateContent(mockContentRequest))
            //                   .Returns(await Task.CompletedTask);

            // Act
            var result = await _contentController.CreateContent(mockContentRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(mockContentRequest, createdResult.Value);
        }

        [Fact]
        public async Task UpdateContent_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var contentId = 1;
            var mockContentUpdateRequest = new ContentUpdateRequestDto
            {
                Title = "Updated Title",
                Summary = "Updated Summary",
                ContentBody = "Updated Content Body"
            };

            //_contentServiceMock.Setup(service => service.UpdateContent(contentId, mockContentUpdateRequest))
            //                   .Returns(Task.CompletedTask);

            // Act
            var result = await _contentController.UpdateContent(contentId, mockContentUpdateRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteContent_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var contentId = 1;

            _contentServiceMock.Setup(service => service.DeleteContent(contentId))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _contentController.DeleteContent(contentId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}