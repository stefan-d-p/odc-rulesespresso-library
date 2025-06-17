using System.Runtime.InteropServices.JavaScript;
using Without.Systems.RulesEspresso.Structures;

namespace Without.Systems.RulesEspresso.Test;

public class Tests
{
    private static readonly IRulesEspresso _actions = new RulesEspresso();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Rule_Eval()
    {
        string data = File.ReadAllText("Samples/data.json");
        string schema = File.ReadAllText("Samples/schema.json");
        var rules = new List<RuleDefinition>
        {
            new RuleDefinition()
            {
                RuleName = "Rule1",
                EvaluationExpression = "age > 35"
            },
            new RuleDefinition
            {
                RuleName = "Rule2",
                EvaluationExpression = "name != null",
                ResultExpression = "\"Hello\"",
                ResultPropertyName = "greeting"
            },
            new RuleDefinition
            {
                RuleName = "Rule3",
                EvaluationExpression = "birthdate < DateTime.Now",
                ResultExpression = "birthdate.AddMonths(1)",
                ResultPropertyName = "birthdateNew"
            },
            new RuleDefinition
            {
                RuleName = "Rule4",
                EvaluationExpression = "birthdate > DateTime.Now"
            },
            new RuleDefinition
            {
                RuleName = "Rule5",
                EvaluationExpression = "birthdate < birthdateNew"
            }
        };
        
        var result = _actions.EvaluateRules(data, rules, schema);
    }
}