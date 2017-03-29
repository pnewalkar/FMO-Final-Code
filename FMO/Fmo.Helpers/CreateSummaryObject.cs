using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

namespace Fmo.Helpers
{
    public class CreateSummaryObject
    {
        dynamic expando = new ExpandoObject();

        public object SummariseProperties(Object o,string fields)
        {
            char[] delimiterChars = { ',' };

            //add the ID of the object to the summary (We always want the ID in the summary)
            fields = "Id," + fields;

            //parse the fields string
            string[] parsedFields = fields.Split(delimiterChars);

            foreach (string fieldName in parsedFields)
            {             
                //use reflection to access the properties
                Type t = o.GetType();
                PropertyInfo prop = t.GetProperty(fieldName);
                object value = prop.GetValue(o, null);

                //create a summary object
                AddProperty(expando, fieldName, value);
            }
            return expando;
        }

        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
