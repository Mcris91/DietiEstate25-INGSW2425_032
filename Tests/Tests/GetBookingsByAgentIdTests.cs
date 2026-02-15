using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Responses;
using Moq;
using Xunit;

namespace DietiEstate.Tests.Tests;

public class GetBookingsByAgentIdTests
{
    // DTO necessari per la compilazione
        public class BookingFilterDto { public virtual string ToQueryString() => ""; }
        public class BookingResponseDto { }
        public class PagedResponseDto<T> { }

    
        public class BookingService
        {
            // Rendiamo GetAsync virtuale per poterlo "spiare" con Moq
            public virtual async Task<PagedResponseDto<BookingResponseDto>> GetAsync<T>(string uri) 
                => new PagedResponseDto<BookingResponseDto>();
            
            public async Task<PagedResponseDto<BookingResponseDto>> GetBookingsByAgentIdAsync(
                BookingFilterDto filterDto, int? pageNumber, int? pageSize)
            {
                var queryString = filterDto.ToQueryString();
                if (pageNumber.HasValue)
                {
                    queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
                    queryString += $"pageNumber={pageNumber.Value}";
                }

                if (pageSize.HasValue)
                {
                    queryString += string.IsNullOrEmpty(queryString) ? "?" : "&";
                    queryString += $"pageSize={pageSize.Value}";
                }

                var uri = $"GetByAgentId/{queryString}";
                return await GetAsync<PagedResponseDto<BookingResponseDto>>(uri);
            }
        }

        [Fact]
        public async Task GetBookings_Path_Empty_ShouldNotAddParams()
        {
            // ARRANGE
            // Usiamo 'CallBase = true' per eseguire il codice REALE di GetBookingsByAgentIdAsync
            // ma intercettare (mockare) solo GetAsync
            var mockService = new Mock<BookingService>() { CallBase = true };
            var filter = new BookingFilterDto(); 

            // ACT
            await mockService.Object.GetBookingsByAgentIdAsync(filter, null, null);

            // ASSERT
            // Ora questo passerà perché il codice dell'immagine è stato eseguito!
            mockService.Verify(s => s.GetAsync<PagedResponseDto<BookingResponseDto>>("GetByAgentId/"), Times.Once);
        }
        
        [Fact]
        public async Task Path2_OnlyPageNumber_ShouldStartWithQuestionMark()
        {
            var mockService = new Mock<BookingService>() { CallBase = true };
            var filter = new BookingFilterDto(); 

            await mockService.Object.GetBookingsByAgentIdAsync(filter, 1, null);

            // Verifica che il ternario abbia scelto "?" perché queryString era vuota
            mockService.Verify(s => s.GetAsync<PagedResponseDto<BookingResponseDto>>(
                "GetByAgentId/?pageNumber=1"), Times.Once);
        }
        
        [Fact]
        public async Task Path3_FilterAndPageSize_ShouldUseAmpersand()
        {
            // ARRANGE
            var mockService = new Mock<BookingService>() { CallBase = true };
    
            // Mockiamo il filtro per assicurarci che restituisca una stringa NON vuota
            var mockFilter = new Mock<BookingFilterDto>();
            mockFilter.Setup(f => f.ToQueryString()).Returns("status=Active"); 

            // ACT
            await mockService.Object.GetBookingsByAgentIdAsync(mockFilter.Object, null, 10);

            // ASSERT
            // Se ToQueryString dà "status=Active", il codice deve aggiungere "&pageSize=10"
            mockService.Verify(s => s.GetAsync<PagedResponseDto<BookingResponseDto>>(
                "GetByAgentId/status=Active&pageSize=10"), Times.Once);
        }

        [Fact]
        public async Task Path4_AllParameters_ShouldConcatenateEverything()
        {
            // ARRANGE
            var mockService = new Mock<BookingService>() { CallBase = true };
    
            var mockFilter = new Mock<BookingFilterDto>();
            mockFilter.Setup(f => f.ToQueryString()).Returns("status=Pending");

            // ACT
            await mockService.Object.GetBookingsByAgentIdAsync(mockFilter.Object, 5, 20);

            // ASSERT
            string expectedUri = "GetByAgentId/status=Pending&pageNumber=5&pageSize=20";
            mockService.Verify(s => s.GetAsync<PagedResponseDto<BookingResponseDto>>(expectedUri), Times.Once);
        }
}
