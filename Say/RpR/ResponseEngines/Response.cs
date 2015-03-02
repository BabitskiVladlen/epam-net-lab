using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RpR.ResponseEngines
{
    public class Response : IResponse
    {
        private readonly string _baseDirectory;

        public Tuple<IEnumerable<string>, string> Errors { get; set; }
        // Item2 = id for validation-list

        #region .ctor
        public Response()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        } 
        #endregion

        #region Get
        public string Get(string target, string innerTarget, string id = "main", bool isJSON = false)
        {
            string text;
            using (StreamReader stream = new StreamReader(_baseDirectory + "Content/" + target))
            {
                text = stream.ReadToEnd();
            }

            #region Errors
            if (Errors != null && Errors.Item1 != null &&
                !String.IsNullOrEmpty(Errors.Item2) && !String.IsNullOrWhiteSpace(Errors.Item2))
            {
                string errorList = GetErrorList(Errors.Item1);
                Tuple<int, int> place = GetById(text, Errors.Item2);
                if (place != null)
                    text = WriteInnerText(text, errorList, place);
            } 
            #endregion

            if (innerTarget == null)
                return text;
            else
            {
                if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                    throw new ArgumentNullException("ID is null");

                #region InsertInnerText

                #region ReadInnerText
                string innerText;
                using (StreamReader stream = new StreamReader(_baseDirectory +
                    "Content/" + innerTarget))
                {
                    innerText = stream.ReadToEnd();
                }
                #endregion

                #region IsJson
                if (isJSON)
                    innerText = JsonResult(innerText);
                if (innerText == null) return text;
                #endregion

                #region WriteInnerText
                Tuple<int, int> t = GetById(text, id);
                if (t == null) return text;
                return WriteInnerText(text, innerText, t);
                #endregion

                #endregion
            }
        } 
        #endregion

        #region JsonResult
        private string JsonResult(string text)
        {
            int i = text.IndexOf("{");
            if (i == -1) return null;
            text = text.Substring(i);
            object pageInfo = null; // = JsonConvert.DeserializeObject<...>(text);
            if (pageInfo == null) return null;
            StringBuilder response = new StringBuilder();
            // response.Append(...
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

        #region GetErrorList
        private string GetErrorList(IEnumerable<string> errors)
        {
            StringBuilder strB = new StringBuilder();
            strB.Append("<ul>");
            foreach (var e in errors)
            {
                strB.Append("<li>" + e + "</li>");
            }
            strB.Append("</ul>");
            return strB.ToString();
        } 
        #endregion

        #region WriteInnerText
		private string WriteInnerText(string text, string innerText, Tuple<int, int> placement)
        {
            StringBuilder response = new StringBuilder();
            response.Append(text.Substring(0, placement.Item1));
            response.Append(innerText);
            response.Append(text.Substring(placement.Item2));
            return response.ToString();
        } 
	    #endregion
    } 
}