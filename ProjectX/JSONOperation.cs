using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProjectX
{
    class JSONOperation
    {
        public static TreeViewItem Json2Tree(JToken root, string rootName = "")
        {
            var parent = new TreeViewItem() { Header = rootName };

            foreach (JToken obj in root)
                foreach (KeyValuePair<string, JToken> token in (JObject)obj)
                    switch (token.Value.Type)
                    {
                        case JTokenType.Array:
                            var jArray = token.Value as JArray;

                            if (jArray?.Any() ?? false)
                                parent.Items.Add(Json2Tree(token.Value as JArray, token.Key));
                            else
                                parent.Items.Add($"\x22{token.Key}\x22 : [ ]"); // Empty array   
                            break;

                        case JTokenType.Object:
                            parent.Items.Add(Json2Tree((JObject)token.Value, token.Key));
                            break;

                        default:
                            parent.Items.Add(GetChild(token));
                            break;
                    }

            return parent;
        }

        private static TreeViewItem GetChild(KeyValuePair<string, JToken> token)
        {
            var value = token.Value.ToString();
            var outputValue = string.IsNullOrEmpty(value) ? "null" : value;
            return new TreeViewItem() { Header = $" \x22{token.Key}\x22 : \x22{outputValue}\x22" };
        }
    }
}
