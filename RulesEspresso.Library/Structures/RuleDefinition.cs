using OutSystems.ExternalLibraries.SDK;

namespace Without.Systems.RulesEspresso.Structures;

[OSStructure(Description = "Rule definition structure.")]
public struct RuleDefinition
{
    [OSStructureField(Description = "The name of the rule.",
        DataType = OSDataType.Text,
        IsMandatory = true)]
    public string RuleName;
    
    [OSStructureField(Description = "Evaluation expression for the rule.",
        DataType = OSDataType.Text,
        IsMandatory = true)]
    public string EvaluationExpression;
    
    [OSStructureField(Description = "Optional result expression for the rule.",
        DataType = OSDataType.Text,
        IsMandatory = false)]
    public string? ResultExpression;
    
    [OSStructureField(Description = "Optional property name where the result of the expression is stored.",
        DataType = OSDataType.Text,
        IsMandatory = false)]
    public string? ResultPropertyName;
}