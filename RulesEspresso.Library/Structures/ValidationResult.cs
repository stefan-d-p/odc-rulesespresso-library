using OutSystems.ExternalLibraries.SDK;

namespace Without.Systems.RulesEspresso.Structures;

[OSStructure(Description = "Represents the result of a rules evaluation.")]
public struct ValidationResult
{
    [OSStructureField(Description = "True if all rules passed, false otherwise.",
        DataType = OSDataType.Boolean,
        IsMandatory = true)]
    public bool IsValid;
    
    [OSStructureField(Description = "A list of rule evaluation results.",
        DataType = OSDataType.InferredFromDotNetType,
        IsMandatory = true)]
    public List<RuleEvaluationResult> RuleResults;
    
    [OSStructureField(Description = "JSON Document after rule evaluation.",
        DataType = OSDataType.Text,
        IsMandatory = true)]
    public string Document;
}