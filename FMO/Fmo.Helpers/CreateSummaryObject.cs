using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

namespace Fmo.Helpers
{
    public class CreateSummaryObject<T> where T : class
    {
        dynamic expando = new ExpandoObject();

        /// <summary>
        /// To summarize the properties of Single object
        /// </summary>
        /// <param name="source">The object to be summarized</param>
        /// <param name="fields">The fields of the source object</param>
        /// <returns>The summarized Expando Object</returns>
        public object SummariseProperties(T source, string fields)
        {
            char[] delimiterChars = { ',' };
            string[] parsedFields = fields.Split(delimiterChars);

            foreach (string fieldName in parsedFields)
            {
                Type type = source.GetType();
                PropertyInfo prop = type.GetProperty(fieldName);
                object value = prop.GetValue(source, null);

                AddProperty(expando, fieldName, value);
            }
            return expando;
        }

        /// <summary>
        /// To summarize the properties of list of objects
        /// </summary>
        /// <param name="source">The list to be summarized</param>
        /// <param name="fields">The fields of the source list of objects</param>
        /// <returns>The summarized list of Expando Object</returns>
        public List<object> SummarisePropertiesForList(List<T> source, string fields)
        {
            List<object> summerizedList = new List<object>();
            char[] delimiterChars = { ',' };
            string[] parsedFields = fields.Split(delimiterChars);

            foreach (object objSource in source)
            {
                expando = new ExpandoObject();
                foreach (string fieldName in parsedFields)
                {
                    Type type = objSource.GetType();
                    PropertyInfo prop = type.GetProperty(fieldName);
                    object value = prop.GetValue(objSource, null);

                    AddProperty(expando, fieldName, value);
                }
                summerizedList.Add(expando);
            }
            return summerizedList;
        }

        /// <summary>
        /// Add a property to the expando object
        /// </summary>
        /// <param name="expando">The expando object</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyValue">The property value</param>
        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
