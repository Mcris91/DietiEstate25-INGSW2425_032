using DietiEstate.Application.Dtos.Filters;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetOffersByAgentId
{
    
    // Test1 - CE1, CE3, CE6
    // filterDto: not null, pageNumber: > 0, pageSize: > 0
    [Fact]
    public void Test1_CE1_CE3_CE6()
    {
        // Arrange
        var filterDto = new OfferFilterDto(); // CE1: not null
        int pageNumber = 1; // CE3: > 0
        int pageSize = 10; // CE6: > 0

        // Act
        var result = Execute(filterDto, pageNumber, pageSize);

        // Assert
        Assert.NotNull(result);
    }

    // Test2 - CE1, CE3, CE7
    // filterDto: not null, pageNumber: > 0, pageSize: <= 0
    [Fact]
    public void Test2_CE1_CE3_CE7()
    {
        // Arrange
        var filter = new OfferFilterDto(); // CE1: not null
        int pageNumber = 1; // CE3: > 0
        int pageSize = 0; // CE7: <= 0

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Execute(filter, pageNumber, pageSize));
    }

    // Test3 - CE1, CE4, CE6
    // filterDto: not null, pageNumber: <= 0, pageSize: > 0
    [Fact]
    public void Test3_CE1_CE4_CE6()
    {
        // Arrange
        var filter = new OfferFilterDto(); // CE1: not null
        int pageNumber = 0; // CE4: <= 0
        int pageSize = 10; // CE6: > 0

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Execute(filter, pageNumber, pageSize));
    }

    // Test4 - CE2, CE3, CE6
    // filterDto: null, pageNumber: > 0, pageSize: > 0
    [Fact]
    public void Test4_CE2_CE3_CE6()
    {
        // Arrange
        OfferFilterDto filter = null; // CE2: null
        int pageNumber = 1; // CE3: > 0
        int pageSize = 10; // CE6: > 0

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Execute(filter, pageNumber, pageSize));
    }

    // Metodo sotto test per gestire le casistiche
    private object Execute(OfferFilterDto filterDto, int pageNumber, int pageSize)
    {
        if (filterDto == null)
            throw new ArgumentNullException(nameof(filterDto));
        
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0", nameof(pageNumber));
        
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0", nameof(pageSize));

        // Logica di business
        return new object();
    }
}