using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MDA.Infrastructure
{  
    public class PublischContract : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isContractRequest = context.HttpContext.Request.Headers["RequestContract"].Equals("true");
            if (!isContractRequest) return;

            var mainParameter = context.ActionDescriptor.Parameters[0];
            if (mainParameter == null) return;

            var jsonString = TypeContract.TypeToJson(mainParameter.ParameterType);
            context.Result = new ContentResult()
            {
                ContentType = "application/json",
                Content = jsonString
            };
        }
    }

    public class TypeContract
    {
        public static string TypeToJson(Type type)
        {
            return "[" + string.Join(",", type.GetProperties().Select(p => new EntityModel(p).ToString())) + "]";
        }

        public class EntityModel
        {

            public EntityModel(PropertyInfo property)
            {
                var relationWith = property.GetAttributeProp<ForeignKeyPlusAttribute>("Name", null);
                var relationConstrain = property.GetAttributeProp<ForeignKeyPlusAttribute>("Constrain", null);
                var relationTarget = (Type)property.GetAttributeProp<ForeignKeyPlusAttribute>("Target", null);

                Key = property.Name;
                Label = property.GetAttributeProp<DisplayAttribute>("Name", property.Name).ToString();
                var maxLength = (property.GetCustomAttribute<MaxLengthAttribute>() != null)
                    ? property.GetCustomAttribute<MaxLengthAttribute>().Length
                    : -1;
                Type = GetJsonType(property.PropertyType, relationWith != null, maxLength);

                var numAttr = property.GetAttribute<NumberAttribute>();
                Props = new Properties()
                {
                    Required = (property.GetAttribute<RequiredAttribute>() != null),
                    Disabled = (property.GetAttribute<ReadOnlyAttribute>() != null),
                    Min = numAttr?.Min.ToString(),
                    Max = numAttr?.Max.ToString(),
                    Step = numAttr?.Step.ToString(CultureInfo.InvariantCulture)
                };

                if (relationWith != null && relationConstrain == null)
                    Relation = new Relation() { With = relationWith.ToString() };

                if (relationWith != null && relationConstrain != null)
                    Relation = new Relation() { With = relationWith.ToString(), Constrain = relationConstrain.ToString() };

                if (relationTarget != null)
                    Relation.TargetProperties = relationTarget.GetProperties().Select(p => p.Name).ToArray();

                if (property.PropertyType.IsEnum)
                {
                    var enumValues = Enum.GetValues(property.PropertyType);
                    var enumNames = Enum.GetNames(property.PropertyType);

                    Options = new List<Option>();
                    for (var j = 0; j < enumValues.Length; j++)
                        Options.Add(new Option()
                        {
                            Key = property.PropertyType.Name + j,
                            Label = enumNames[j],
                            Value = enumValues.GetValue(j).ToString()
                        });
                }
            }

            public string Key { get; set; }

            public string Label { get; set; }

            public string Type { get; set; }

            public Properties Props { get; set; }

            public Relation Relation { get; set; }

            public List<Option> Options { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }

            public string GetJsonType(Type type, bool isForeignKeyn, int maxLength)
            {
                if (isForeignKeyn)
                    return "foreignkey";

                if (type.IsEnum)
                    return "select";

                if (maxLength > 1000)
                    return "textarea";

                switch (type.ToString())
                {
                    case "System.Integer":
                    case "System.Decimal":
                        return "number";
                    case "System.Boolean":
                        return "checkbox";
                    case "System.DateTime":
                        return "date";
                    default:
                        return "text";
                }
            }
        }

        public class Properties
        {
            public bool Required { get; set; }

            public bool Disabled { get; set; }

            public string Min { get; set; }

            public string Max { get; set; }

            public string Step { get; set; }

            public string Pattern { get; set; }
        }

        public class Relation
        {
            public string With { get; set; }

            public string Constrain { get; set; }

            public string[] TargetProperties { get; set; }
        }

        public class Option
        {
            public string Key { get; set; }
            public string Label { get; set; }
            public string Value { get; set; }
        }

        public class ForeignKeyPlusAttribute : Attribute
        {
            public ForeignKeyPlusAttribute(string name)
            {
                Name = name;
            }

            public ForeignKeyPlusAttribute(string name, object constrain)
            {
                Name = name;
                Constrain = constrain;
            }

            public ForeignKeyPlusAttribute(string name, object constrain, Type target)
            {
                Name = name;
                Constrain = constrain;
                Target = target;
            }

            public string Name { get; set; }

            public Type Target { get; set; }

            public object Constrain { get; set; }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class NumberAttribute : Attribute
        {
            public int Min;
            public int Max;
            public double Step;

            public NumberAttribute(int min, int max, double step)
            {
                Min = min;
                Max = max;
                Step = step;
            }
        }
    }
}