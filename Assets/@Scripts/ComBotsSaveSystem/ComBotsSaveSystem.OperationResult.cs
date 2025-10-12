public static partial class ComBotsSaveSystem
{
    public class OperationResult
    {
        public ResultType ResultType { get; }
        public string Message { get; }

        public OperationResult(ResultType resultType, string message = "")
        {
            ResultType = resultType;
            Message = message;
        }
    }
}
