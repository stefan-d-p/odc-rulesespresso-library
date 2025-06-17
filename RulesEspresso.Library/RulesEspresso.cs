using System.Text.Json;
using System.Text.Json.Nodes;
using DynamicExpresso;
using DynamicExpresso.Exceptions;
using Without.Systems.RulesEspresso.Exceptions;
using Without.Systems.RulesEspresso.Structures;
using NJsonSchema;

namespace Without.Systems.RulesEspresso;

public class RulesEspresso : IRulesEspresso
{
    public ValidationResult EvaluateRules(
        string dataObject,
        List<RuleDefinition> ruleDefinitions,
        string? jsonSchema = null)
    {
        var dataNode = ParseJson(dataObject);
        ValidateJsonSchema(jsonSchema, dataNode);

        var parameters = ExtractParameters(dataNode);
        var ruleResults = new List<RuleEvaluationResult>();
        var isValid = EvaluateRuleDefinitions(ruleDefinitions, parameters, ruleResults, dataNode);

        return new ValidationResult
        {
            IsValid = isValid,
            RuleResults = ruleResults,
            Document = dataNode.ToString()
        };
    }

    private JsonNode ParseJson(string dataObject) =>
        JsonNode.Parse(dataObject) ?? throw new ArgumentException("JSON data object is null", nameof(dataObject));

    private void ValidateJsonSchema(string? jsonSchema, JsonNode dataNode)
    {
        if (string.IsNullOrWhiteSpace(jsonSchema)) return;
        var schema = JsonSchema.FromJsonAsync(jsonSchema).GetAwaiter().GetResult();
        var errors = schema.Validate(dataNode.ToJsonString());
        if (errors.Count > 0) throw new Exception("JSON schema validation failed");
    }

    private List<Parameter> ExtractParameters(JsonNode dataNode)
    {
        var parameters = new List<Parameter>();
        if (dataNode is JsonObject obj)
        {
            foreach (var prop in obj)
            {
                var (val, type) = ResolvePropertyType(prop.Value);
                parameters.Add(new Parameter(prop.Key, type, val));
            }
        }

        return parameters;
    }

    private (object? value, Type type) ResolvePropertyType(JsonNode? node)
    {
        if (node is not JsonValue val) return (null, typeof(object));
        if (val.TryGetValue<int>(out var i)) return (i, typeof(int));
        if (val.TryGetValue<long>(out var l)) return (l, typeof(long));
        if (val.TryGetValue<decimal>(out var d)) return (d, typeof(decimal));
        if (val.TryGetValue<double>(out var db)) return (db, typeof(double));
        if (val.TryGetValue<DateTime>(out var dt)) return (dt, typeof(DateTime));
        if (val.TryGetValue<string>(out var s)) return (s, typeof(string));
        if (val.TryGetValue<bool>(out var b)) return (b, typeof(bool));
        if (val.TryGetValue<Guid>(out var g)) return (g, typeof(Guid));
        if (node.GetValue<JsonElement>().ValueKind == JsonValueKind.Null) return (null, typeof(object));
        throw new Exception($"Unsupported type: {val.GetType().Name}");
    }

    private JsonValue? CreateTypedJsonValue(object? value)
    {
        if (value == null) return null;
        var (_, type) = ResolvePropertyType(JsonValue.Create(value));
        return JsonValue.Create(Convert.ChangeType(value, type));
    }

    private bool EvaluateRuleDefinitions(
        List<RuleDefinition> ruleDefinitions,
        List<Parameter> parameters,
        List<RuleEvaluationResult> ruleResults,
        JsonNode dataNode)
    {
        var interpreter = new Interpreter();
        foreach (var p in parameters)
            interpreter.SetVariable(p.Name, p.Value, p.Type);

        var allPassed = true;
        foreach (var rule in ruleDefinitions)
        {
            try
            {
                var passed = interpreter.Eval<bool>(rule.EvaluationExpression);
                ruleResults.Add(new RuleEvaluationResult { RuleName = rule.RuleName, IsValid = passed });

                if (passed && !string.IsNullOrWhiteSpace(rule.ResultExpression) &&
                    !string.IsNullOrWhiteSpace(rule.ResultPropertyName))
                {
                    var result = interpreter.Eval(rule.ResultExpression);
                    dataNode[rule.ResultPropertyName] = CreateTypedJsonValue(result);
                    var (val, type) = ResolvePropertyType(dataNode[rule.ResultPropertyName]);
                    interpreter.SetVariable(rule.ResultPropertyName, val, type);
                }

                if (!passed) allPassed = false;
            }
            catch (UnknownIdentifierException ex)
            {
                throw new RuleEvaluationException(
                    $"Rule '{rule.RuleName}' refers to an undefined identifier: '{ex.Identifier}'",
                    rule.RuleName, rule.EvaluationExpression, ex);
            }
            catch (Exception ex)
            {
                throw new RuleEvaluationException(
                    $"Error during evaluation of rule '{rule.RuleName}': {ex.Message}",
                    rule.RuleName, rule.EvaluationExpression, ex);
            }
        }

        return allPassed;
    }
}