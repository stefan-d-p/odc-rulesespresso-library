using OutSystems.ExternalLibraries.SDK;

namespace Without.Systems.RulesEspresso.Structures;

[OSStructure(Description = "Represents the result of a single rule evaluation.")]
public struct RuleEvaluationResult
{
    [OSStructureField(
        Description = "The name of the rule that was evaluated.",
        DataType = OSDataType.Text,
        IsMandatory = true)]
    public string RuleName;
    [OSStructureField(
        Description = "The result of the rule evaluation. True if the rule passed, false otherwise.",
        DataType = OSDataType.Boolean,
        IsMandatory = true)]
    public bool IsValid;
}