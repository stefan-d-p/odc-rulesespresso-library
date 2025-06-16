# RulesEspresso - A simple rules engine for OutSystems Developer Cloud

This external logic library provides a simple rules engine for OutSystems Developer Cloud, allowing you to define and evaluate rules in a straightforward manner.

It contains a single action `EvaluateRules` that takes a list of rules, a JSON document with data and an optional JSON schema,
it verifies the data document against the schema, if provided, evaluates the rules against the context, and returns the results.
A rule also is able to extend the data context with additional properties.

Parameters

- `ruleDefinitions`: A list of rules to evaluate.
- `dataObject`: A JSON document containing the data to evaluate against the rules.
- `jsonSchema`: An optional JSON schema to validate the data document against.

A rule definition has the following structure:

- `RuleName`: The name of the rule.
- `EvaluationExpression`: A C# expression to evaluate the rule. An evaluation expression must return a boolean value.
- `ResultExpression`: (optional) A C# expression to evaluate a result if the rule is true.
- `ResultPropertyName`: (mandatory if ResultExpression is set) The name of the property to set in the result if the rule is true.

Output

- `IsValid`: A boolean indicating if ALL provided rules have passed the evaluation.
- `Document`: A JSON document containing the results of the evaluation. Including additional properties set during rules evaluation.
- `RuleResults`: A list of rule results, each containing:
  - `RuleName`: The name of the rule.
  - `IsValid`: A boolean indicating if the rule passed the evaluation.






