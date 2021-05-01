using Newtonsoft.Json;
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

        private static bool IsNumber(string input)
        {
            foreach (char item in input)
            {
                if (char.IsNumber(item))
                    continue;
                else
                    return false;
            }
            return true;
        }
        public List<Root> partialEntries { get; set; }
        public static TreeViewItem Json2Tree(JToken root, string rootName = "")
        {
            var parent = new TreeViewItem() { Header = rootName };

            foreach (JToken obj in root)
            {
                if (obj.Type==JTokenType.Property)
                {
                    var values = obj.ToString().Split(':');
                    if (values[0].Contains("partialEntries"))
                    {
                        

                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>("{"+obj.ToString()+"}");
                        foreach (var item in myDeserializedClass.partialEntries)
                        {
                            var p = new TreeViewItem() { Header = "partialentry" };
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"id"}\x22 : \x22{item.id}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"transactionType"}\x22 : \x22{item.transactionType}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"adding"}\x22 : \x22{item.adding}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"tradeDate"}\x22 : \x22{UnixToDateTime(item.tradeDate.ToString())}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"shares"}\x22 : \x22{item.shares}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"price"}\x22 : \x22{item.price}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"sharesRemaining"}\x22 : \x22{item.sharesRemaining}\x22" });
                            p.Items.Add(new TreeViewItem { Header = $" \x22{"short"}\x22 : \x22{item.@short}\x22" });
                            parent.Items.Add(p);

                        }

                        continue;
                    }

                    values[1] = values[1].TrimStart();
                    if (values[1].Count()==13 && IsNumber(values[1]))
                    {
                        parent.Items.Add(new TreeViewItem { Header = $" \x22{values[0]}\x22 : \x22{UnixToDateTime(values[1])}\x22" });
                        continue;
                    }
                    parent.Items.Add(new TreeViewItem { Header = $" \x22{obj}\x22 " });
                    continue;
                }
              


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
            }
    return parent;
}

private static TreeViewItem GetChild(KeyValuePair<string, JToken> token)
{
        var value = token.Value.ToString();
        var outputValue = string.IsNullOrEmpty(value) ? "null" : value;

           


            if (outputValue.Count() == 13 && IsNumber(outputValue))
                outputValue = UnixToDateTime(outputValue);
            
            return new TreeViewItem() { Header = $" \x22{token.Key}\x22 : \x22{outputValue}\x22" };
}

        public static string UnixToDateTime(string Timestamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(Convert.ToInt64(Timestamp)).ToLocalTime();
            return dtDateTime.ToString();
        }
}
}
