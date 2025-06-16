namespace Without.Systems.RulesEspresso.Exceptions;

public class RuleEvaluationException : Exception
{
    public string RuleName { get; }
    public string RuleExpression { get; }

    public RuleEvaluationException(string message, string ruleName, string ruleExpression) : base(message)
    {
        RuleName = ruleName;
        RuleExpression = ruleExpression;
    }

    public RuleEvaluationException(string message, string ruleName, string ruleExpression, Exception innerException) :
        base(message, innerException)
    {
        RuleName = ruleName;
        RuleExpression = ruleExpression;
    }
    
}