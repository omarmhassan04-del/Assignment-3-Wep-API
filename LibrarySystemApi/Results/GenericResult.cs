namespace LibrarySystemApi.Results
{
    public class GenericResult<T> : Result
    {
        public T Value { get; set; }

        public GenericResult(bool isSuccess, T value, string error) : base(isSuccess, error)
        {
            Value = value;
        }

        public static GenericResult<T> Success(T value)
            => new GenericResult<T>(true, value, string.Empty);

        public static new GenericResult<T> Failure(string error = null)
            => new GenericResult<T>(false, default, error);
    }
}
