using Business.ViewModel.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public class ConditionEvaluator
    {
        public (bool success, Dictionary<string, string> extractedValues) Evaluate(object conditionsJsonObject, object context)
        {
            // Convert conditionsJsonObject to string if it is a JValue
            string conditionsJson = conditionsJsonObject is Newtonsoft.Json.Linq.JValue jValue
                ? jValue.ToString()
                : conditionsJsonObject?.ToString();

            // Return true if no conditions are provided
            if (string.IsNullOrEmpty(conditionsJson))
                return (true, new Dictionary<string, string>());

            // Preprocess the JSON to make it valid
            if (!IsValidJson(conditionsJson))
            {
                conditionsJson = PreprocessJson(conditionsJson);
            }

            // Deserialize conditions JSON into a list of Condition objects
            List<Condition> conditions;
            if (conditionsJson.TrimStart().StartsWith("{"))
            {
                // Single object: wrap it in a list
                var singleCondition = JsonConvert.DeserializeObject<Condition>(conditionsJson);
                conditions = new List<Condition> { singleCondition };
            }
            else
            {
                // Array of conditions
                conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);
            }

            // Dictionary to store specific values
            var extractedValues = new Dictionary<string, string>();

            // Iterate over each condition and evaluate
            foreach (var condition in conditions)
            {
                // Get the property from the context that matches the condition field
                var property = context.GetType().GetProperty(condition.Field);

                if (property == null)
                    throw new Exception($"Field '{condition.Field}' not found in context.");

                // Get the value of the field from the context object
                var fieldValue = property.GetValue(context);

                // Store the extracted values
                extractedValues[condition.Field] = condition.Value.ToString();

                // Evaluate the condition; if any condition fails, return false
                if (!EvaluateCondition(fieldValue, condition.Operator, condition.Value))
                    return (false, extractedValues);
            }

            // All conditions met
            return (true, extractedValues);
        }

        private string PreprocessJson(string json)
        {
            // Handle malformed input JSON
            json = json.Trim();

            // Add quotes around keys and values if missing
            if (!json.StartsWith("{") || !json.EndsWith("}"))
                throw new ArgumentException("Invalid JSON format.");

            json = System.Text.RegularExpressions.Regex.Replace(json, @"(\w+)\s*=", "\"$1\":");
            json = System.Text.RegularExpressions.Regex.Replace(json, @"=\s*([^,}\s]+)", ":\"$1\"");
            json = json.Replace(",\"", ",").Replace("}\"", "}");

            return json;
        }

        private bool IsValidJson(string json)
        {
            try
            {
                json = json.Trim();
                if ((json.StartsWith("{") && json.EndsWith("}")) || (json.StartsWith("[") && json.EndsWith("]")))
                {
                    Newtonsoft.Json.Linq.JToken.Parse(json); // Validate JSON structure
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool EvaluateCondition(object fieldValue, string @operator, object conditionValue)
        {
            switch (@operator)
            {
                case "=":
                    return fieldValue?.ToString() == conditionValue?.ToString();
                case "!=":
                    return fieldValue?.ToString() != conditionValue?.ToString();
                case ">":
                    return Convert.ToDecimal(fieldValue) > Convert.ToDecimal(conditionValue);
                case "<":
                    return Convert.ToDecimal(fieldValue) < Convert.ToDecimal(conditionValue);
                case ">=":
                    return Convert.ToDecimal(fieldValue) >= Convert.ToDecimal(conditionValue);
                case "<=":
                    return Convert.ToDecimal(fieldValue) <= Convert.ToDecimal(conditionValue);
                default:
                    throw new NotSupportedException($"Operator '{@operator}' is not supported.");
            }
        }


    }
}
