using Application.Interfaces;

namespace Application.Services
{
    public class MathService : IMathService
    {
        public int GetSecondLargest(IEnumerable<int> numbers, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            return GetSecondLargestWithLoop(numbers, cancellationToken);
        }

        private static int GetSecondLargestWithLoop(IEnumerable<int> numbers, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(numbers);
            int? largest = null;
            int? secondLargest = null;
            foreach (var number in numbers)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (largest == null || number > largest)
                {
                    secondLargest = largest;
                    largest = number;
                }
                else if (number != largest && (secondLargest == null || number > secondLargest))
                {
                    secondLargest = number;
                }
            }
            if (secondLargest == null)
                throw new ArgumentException("At least two distinct numbers required", nameof(numbers));
            return secondLargest.Value;
        }
    }
}