using OutSystems.ExternalLibraries.SDK;
using Without.Systems.RulesEspresso.Structures;

namespace Without.Systems.RulesEspresso
{
    [OSInterface(
        Name = "RulesEspresso",
        IconResourceName = "Without.Systems.RulesEspresso.Resources.Rules.png",
        Description = "Simple rules engine for evaluating expression based rules against data objects.")]
    public interface IRulesEspresso
    {
        [OSAction(
            Description = "Evaluates one or more rules against a data object and returns the validation result.",
            IconResourceName = "Without.Systems.RulesEspresso.Resources.Rules.png",
            ReturnName = "ValidationResult",
            ReturnType = OSDataType.InferredFromDotNetType)]
        ValidationResult EvaluateRules(
            [OSParameter(
                Description = "The data object to validate against the rules",
                DataType = OSDataType.Text)]
            string dataObject,
            [OSParameter(
                Description = "The rules to evaluate",
                DataType = OSDataType.InferredFromDotNetType)]
            List<RuleDefinition> ruleDefinitions,
            [OSParameter(
                Description = "Optional JSON schema to validate the data object against",
                DataType = OSDataType.Text)]
            string? jsonSchema = null);
    }
}