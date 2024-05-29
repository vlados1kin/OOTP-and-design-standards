using System;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PluginBase;

namespace TransformatorPlugin;

public class TransformatorPlugin: ITransformator
    {
        public string ToXml(string json){
        JArray jsonArray;
            try
            {
                jsonArray = JArray.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Ошибка при парсинге JSON: " + ex.Message);
                return null;
            }
            XElement root = new XElement("root");
            void AddJsonToXml(JToken token, XElement element)
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                        foreach (JProperty property in token.Children<JProperty>())
                        {
                            XElement childElement = new XElement(property.Name);
                            element.Add(childElement);
                            AddJsonToXml(property.Value, childElement);
                        }
                        break;
                    case JTokenType.Array:
                        foreach (JToken arrayItem in token.Children())
                        {
                            XElement childElement = new XElement("item");
                            element.Add(childElement);
                            AddJsonToXml(arrayItem, childElement);
                        }
                        break;
                    case JTokenType.Integer:
                        element.Add(new XText(token.Value<int>().ToString()));
                        break;
                    case JTokenType.Float:
                        element.Add(new XText(token.Value<float>().ToString()));
                        break;
                    case JTokenType.String:
                        element.Add(new XText((string)token));
                        break;
                    case JTokenType.Boolean:
                        element.Add(new XText(token.Value<bool>().ToString()));
                        break;
                    case JTokenType.Null:
                        element.Add(new XText(""));
                        break;
                    default:
                        throw new InvalidOperationException("Unknown token type: " + token.Type);
                }
            }
            foreach (JObject jsonObject in jsonArray.Children<JObject>())
            {
                XElement itemElement = new XElement("item");
                root.Add(itemElement);
                AddJsonToXml(jsonObject, itemElement);
            }
            return root.ToString();
        }


        public string FromXml(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            JToken ConvertElementToJToken(XElement element)
            {
                if (element.HasElements)
                {
                    JObject obj = new JObject();
                    foreach (var child in element.Elements())
                    {
                        if (obj[child.Name.LocalName] != null)
                        {
                            if (obj[child.Name.LocalName] is not JArray)
                            {
                                var existing = obj[child.Name.LocalName];
                                obj[child.Name.LocalName] = new JArray { existing };
                            }

                            ((JArray)obj[child.Name.LocalName]).Add(ConvertElementToJToken(child));
                        }
                        else
                        {
                            obj[child.Name.LocalName] = ConvertElementToJToken(child);
                        }
                    }
                    return obj;
                }
                else
                {
                    return new JValue(element.Value);
                }
            }
            JArray jsonArray = new JArray();
            foreach (var child in doc.Root.Elements())
            {
                jsonArray.Add(ConvertElementToJToken(child));
            }
            
            return jsonArray.ToString(Newtonsoft.Json.Formatting.None);
        }
    }