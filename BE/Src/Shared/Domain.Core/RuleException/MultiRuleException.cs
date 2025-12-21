namespace Domain.Core.RuleException
{
    public class MultiRuleException : Exception
    {
        public IEnumerable<Exception> Errors { get; }

        public MultiRuleException(IEnumerable<Exception> errors)
        {
            Errors = errors;
        }
    }
}