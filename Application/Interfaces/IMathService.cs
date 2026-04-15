namespace Application.Interfaces
{
    public interface IMathService
    {
        int GetSecondLargest(IEnumerable<int> numbers, CancellationToken cancellationToken = default);
    }
}