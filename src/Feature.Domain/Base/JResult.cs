namespace Feature.Domain.Base;

public class JResults
{
    /// <summary>
    /// 결과 메세지 목록
    /// </summary>
    public IEnumerable<string> Messages { get; set; }

    /// <summary>
    /// 정합성 체크 결과
    /// </summary>
    public Dictionary<string, string> ValidateResults { get; set; }
        = new Dictionary<string, string>();

    public JResults()
    {
    }

    /// <summary>
    /// 요청 성공 여부, false라면 ValidateResults에 내역이 있음. empty는  true로 처리함.
    /// </summary>
    public bool Succeeded { get; set; }

    public static JResults Fail()
    {
        return new JResults { Succeeded = false };
    }

    public static JResults Fail(string message)
    {
        return new JResults { Succeeded = false, Messages = [message] };
    }

    public static JResults Fail(IEnumerable<string> messages)
    {
        return new JResults { Succeeded = false, Messages = messages };
    }

    public static JResults Fail(Dictionary<string, string> validate)
    {
        return new JResults() { Succeeded = false, ValidateResults = validate };
    }

    public static Task<JResults> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Task<JResults> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<JResults> FailAsync(IEnumerable<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public static Task<JResults> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }

    public static JResults Success()
    {
        return new JResults { Succeeded = true, Messages = ["Success."] };
    }

    public static JResults Success(string message)
    {
        return new JResults { Succeeded = true, Messages = ["Success.", message] };
    }

    public static Task<JResults> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<JResults> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class JResults<T> : JResults
{
    public JResults()
    {
    }

    /// <summary>
    /// 결과 데이터
    /// </summary>
    public T Data { get; set; }

    public new static JResults<T> Fail()
    {
        return new JResults<T> { Succeeded = false };
    }

    public new static JResults<T> Fail(string message)
    {
        return new JResults<T> { Succeeded = false, Messages = [message] };
    }

    public static JResults<T> Fail(List<string> messages)
    {
        return new JResults<T> { Succeeded = false, Messages = messages };
    }

    public new static JResults<T> Fail(Dictionary<string, string> errors)
    {
        return new JResults<T>() { Succeeded = false, ValidateResults = errors };
    }

    public static JResults<T> Fail(T item)
    {
        return new JResults<T>() { Succeeded = false, Data = item};
    }

    public new static Task<JResults<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static Task<JResults<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<JResults<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public new static Task<JResults<T>> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }
    
    public static Task<JResults<T>> FailAsync(T data)
    {
        return Task.FromResult(Fail(data));
    }

    public new static JResults<T> Success()
    {
        return new JResults<T> { Succeeded = true, Messages = ["Success."] };
    }

    public new static JResults<T> Success(string message)
    {
        return new JResults<T> { Succeeded = true, Messages = ["Success.", message] };
    }

    public static JResults<T> Success(T data)
    {
        return new JResults<T> { Succeeded = true, Data = data, Messages = ["Success."] };
    }

    public static JResults<T> Success(T data, string message)
    {
        return new JResults<T> { Succeeded = true, Data = data, Messages = ["Success.", message] };
    }

    public static JResults<T> Success(T data, List<string> messages)
    {
        messages.Insert(0, "Search Success.");
        return new JResults<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<JResults<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<JResults<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<JResults<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}