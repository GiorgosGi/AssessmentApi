using Application.Services;

namespace Tests
{
    public class MathServiceTests
    {
        [Fact]
        public void GetSecondLargest_ReturnsCorrectValue()
        {
            var service = new MathService();
            var result = service.GetSecondLargest(new int[] { 1, 2, 3, 4 });
            Assert.Equal(3, result);
        }
    }
}
