using System.Collections.Generic;
using System.Data;
using Dapper;
using Moq;
using ProjectMarket.Server.Data.Model.VO;
using ProjectMarket.Server.Infra.Repository;
using Xunit;

public class AdvertisementStatusRepositoryTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly AdvertisementStatusRepository _repository;

    public AdvertisementStatusRepositoryTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockDbConnection = new Mock<IDbConnection>();

        _mockUnitOfWork.Setup(uow => uow.Connection).Returns(_mockDbConnection.Object);

        _repository = new AdvertisementStatusRepository(_mockUnitOfWork.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnAllAdvertisementStatuses()
    {
        // Arrange
        var expectedStatuses = new List<AdvertisementStatusVO>
        {
            new AdvertisementStatusVO { AdvertisementStatusName = "Active" },
            new AdvertisementStatusVO { AdvertisementStatusName = "Inactive" }
        };

        _mockDbConnection
            .Setup(conn => conn.Query<AdvertisementStatusVO>("SELECT AdvertisementStatusName FROM AdvertisementStatus", null, null, true, null, null))
            .Returns(expectedStatuses);

        // Act
        var result = _repository.GetAll();

        // Assert
        Assert.Equal(expectedStatuses, result);
    }

    [Fact]
    public void GetByAdvertisementStatusName_ShouldReturnStatus_WhenStatusExists()
    {
        // Arrange
        var statusName = "Active";
        var expectedStatus = new AdvertisementStatusVO { AdvertisementStatusName = statusName };

        _mockDbConnection
            .Setup(conn => conn.QueryFirstOrDefault<AdvertisementStatusVO>(
                "SELECT AdvertisementStatusName FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName",
                It.Is<object>(param => ((dynamic)param).AdvertisementStatusName == statusName), null, null, null))
            .Returns(expectedStatus);

        // Act
        var result = _repository.GetByAdvertisementStatusName(statusName);

        // Assert
        Assert.Equal(expectedStatus, result);
    }

    [Fact]
    public void Insert_ShouldCallExecuteWithCorrectQuery()
    {
        // Arrange
        var status = new AdvertisementStatusVO { AdvertisementStatusName = "NewStatus" };
        var query = "INSERT INTO AdvertisementStatus (AdvertisementStatusName) VALUES (@AdvertisementStatusName)";

        // Act
        _repository.Insert(status);

        // Assert
        _mockDbConnection.Verify(conn => conn.Execute(query, status, null, null, null), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallExecuteWithCorrectQuery()
    {
        // Arrange
        var status = new AdvertisementStatusVO { AdvertisementStatusName = "UpdatedStatus" };
        var query = "UPDATE AdvertisementStatus SET AdvertisementStatusName = @AdvertisementStatusName WHERE AdvertisementStatusName = @AdvertisementStatusName";

        // Act
        _repository.Update(status);

        // Assert
        _mockDbConnection.Verify(conn => conn.Execute(query, status, null, null, null), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallExecuteWithCorrectQuery()
    {
        // Arrange
        var status = new AdvertisementStatusVO { AdvertisementStatusName = "DeleteStatus" };
        var query = "DELETE CASCADE FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";

        // Act
        _repository.Delete(status);

        // Assert
        _mockDbConnection.Verify(conn => conn.Execute(query, status, null, null, null), Times.Once);
    }
}
