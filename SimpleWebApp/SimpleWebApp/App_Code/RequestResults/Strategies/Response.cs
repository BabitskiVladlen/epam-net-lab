using Newtonsoft.Json;
using SimpleWebApp.JsonEntities;
using SimpleWebApp.RequestResults.Strategies.Interfasces;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SimpleWebApp.RequestResults.Strategies
{
    public class Response : IResponse
    {
        private readonly string _baseDirectory;
        public Response()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public string Get(Routes target, Routes? innerTarget = null, string id = "main")
        {
            try
            {
                string text;
                using (StreamReader stream = new StreamReader(_baseDirectory + "Content/" + Route.Get(target)))
                {
                    text = stream.ReadToEnd();
                }

                if (innerTarget == null)
                {
                    #region IsJson
                    if (text.IndexOf("#json", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        int i = 0;
                        i = text.IndexOf("{");
                        if (i == -1) return null;
                        text = text.Substring(i);
                    } 
                    #endregion
                    return text;
                }
                else
                {
                    if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                        throw new ArgumentNullException("ID is null");

                    #region InsertInnerText

                    #region ReadInnerText
                    string innerText;
                    using (StreamReader stream2 = new StreamReader(_baseDirectory +
                        "Content/" + Route.Get((Routes)innerTarget)))
                    {
                        innerText = stream2.ReadToEnd();
                    }
                    #endregion

                    #region IsJson
                    if (innerText.IndexOf("#json", StringComparison.OrdinalIgnoreCase) != -1)
                        innerText = JsonResult(innerText);
                    if (innerText == null) return text;
                    #endregion

                    #region WriteInnerText
                    Tuple<int, int> t = GetById(text, id);
                    if (t == null) return text;
                    StringBuilder response = new StringBuilder();
                    response.Append(text.Substring(0, t.Item1));
                    response.Append(innerText);
                    response.Append(text.Substring(t.Item2));
                    return response.ToString();
                    #endregion

                    #endregion
                }
            }
            catch (IOException exc)
            {
                LogMessage.Add(exc, Level.Error);
                return null;
            }
        }

        #region JsonResult
        private string JsonResult(string text)
        {
            int i = text.IndexOf("{");
            if (i == -1) return null;
            text = text.Substring(i);
            PersonalInfo info = JsonConvert.DeserializeObject<PersonalInfo>(text);
            if (info == null) return null;
            StringBuilder response = new StringBuilder();
            try
            {
                response.Append("<ul><li id='infoList-item1'>Name: " + info.Name + "</li>");
                response.Append("<li id='infoList-item2'>Surname: " +
                    info.Surname.Surname1 + " " + info.Surname.Surname2 + "</li>");
                response.Append("<li id='infoList-item3'>Phones: ");
                for (i = 0; i < info.Phones.Length; ++i)
                {
                    response.Append(info.Phones[i]);
                    if (i != (info.Phones.Length - 1)) response.Append(", ");
                }
            }
            catch (NullReferenceException exc)
            {
                LogMessage.Add(exc, Level.Error);
                return null;
            }
            response.Append("</li></ul>");
            return response.ToString();
        }
        #endregion

        #region GetById
        // <tag id=id>Item1...Item2</tag>
        private Tuple<int, int> GetById(string text, string id)
        {
            Regex.Replace(text, @"\s+", String.Empty);

            // <tag id=id>
            int i = text.IndexOf("id=" + id, StringComparison.OrdinalIgnoreCase);
            // <... id="id"...>
            if (i == -1) i = text.IndexOf("id=\"" + id + "\"", StringComparison.OrdinalIgnoreCase);
            if (i == -1) return null;

            #region <tag?>
            string tag = String.Empty;
            while (i >= 0)
            {
                if (text[i] == '<')
                {
                    while (!Char.IsLetter(text[i]))
                        ++i;
                    while ((i < text.Length) && Char.IsLetter(text[i]))
                    {
                        tag += text[i];
                        ++i;
                    }
                    break;
                }
                --i;
            }
            if (String.IsNullOrEmpty(tag) || (i == -1) || (i == text.Length)) return null;
            #endregion

            // ...>Item1
            i = text.IndexOf('>', i);
            if (i == -1) return null;
            ++i;

            #region </tag>
            int count = 1, j = i;
            while (true)
            {
                j = text.IndexOf(tag, j, StringComparison.OrdinalIgnoreCase);
                if (j == -1) return null;
                if (text[j - 1] == '/') --count;
                else ++count;
                if (count == 0) break;
                ++j;
            }
            #endregion

            // Item2</...
            for (; i < j; --j)
                if (text[j] == '<')
                    break;
            if (i == j) return null;

            return new Tuple<int, int>(i, j);
        } 
        #endregion
    } 
}