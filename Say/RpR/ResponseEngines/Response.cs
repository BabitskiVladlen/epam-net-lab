#region using
using Newtonsoft.Json;
using RpR.JsonEntities;
using RpR.ResponseEngines.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions; 
#endregion

namespace RpR.ResponseEngines
{
    public class Response : IResponse
    {
        #region Fields&Props
        private readonly string _baseDirectory;
        public string Title { get; set; }

        // Item2 = id for validation-list
        public Tuple<IEnumerable<string>, string> Errors { get; set; } 
        #endregion

        #region .ctor
        public Response()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        } 
        #endregion

        #region GetContent
        public string GetContent(string content, string innerContent, string id = "main", bool isJson = false)
        {
            if (innerContent != null)
            {
                if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                    throw new ArgumentNullException("ID is null");

                #region IsJson
                if (isJson)
                {
                    CoreJsonEntity pageInfo = JsonResult(innerContent);
                    if (pageInfo == null)
                        throw new ArgumentNullException("It cannot represent inner content as CoreJsonEntity",
                            (Exception)null);
                    if (!(pageInfo.Title == null)) Title = pageInfo.Title;
                    innerContent = pageInfo.Content;
                }
                #endregion

                #region WriteInnerContent
                Tuple<int, int> replacement = GetById(content, id);
                if (replacement == null)
                    throw new ArgumentException("It cannot write inner content", (Exception)null);
                content = WriteInnerText(content, innerContent, replacement);
                #endregion
            }

            #region Errors
            if (Errors != null && Errors.Item1 != null &&
                !String.IsNullOrEmpty(Errors.Item2) && !String.IsNullOrWhiteSpace(Errors.Item2))
            {
                string errorList = GetHtmlErrorList(Errors.Item1);
                Tuple<int, int>  errorPlace = GetById(content, Errors.Item2);
                if (errorPlace != null)
                    content = WriteInnerText(content, errorList, errorPlace);
            }
            #endregion

            return content;
        } 
        #endregion

        #region GetByRoutes
        public string GetByRoutes(string target, string innerTarget, string id = "main", bool isJson = false)
        {
            try
            {
                string content = GetByRoute(target);
                string innerContent = null;
                if (innerTarget != null) GetByRoute(innerTarget);
                return GetContent(content, innerContent, id, isJson);
            }
            catch (ArgumentException exc)
            { throw exc; }
        } 
        #endregion

        #region RouteToContent
        public string RouteToContent(string content, string innerTarget, string id = "main", bool isJson = false)
        {
            try
            {
                string innerContent = null;
                if (innerTarget != null) innerContent = GetByRoute(innerTarget);
                return GetContent(content, innerContent, id, isJson);
            }
            catch (ArgumentException exc)
            { throw exc; }
        }
        #endregion

        #region ContentToRoute
        public string ContentToRoute(string target, string innerContent, string id = "main", bool isJson = false)
        {
            try
            {
                string content = GetByRoute(target);
                return GetContent(content, innerContent, id, isJson);
            }
            catch (ArgumentException exc)
            { throw exc; }
        }
        #endregion

        #region JsonResult
        private CoreJsonEntity JsonResult(string text)
        {
            int i = text.IndexOf("{");
            if (i == -1) return null;
            text = text.Substring(i);
            CoreJsonEntity pageInfo = JsonConvert.DeserializeObject<CoreJsonEntity>(text);
            if (pageInfo == null) return null;
            return pageInfo;
        }
        #endregion

        #region GetById
        private Tuple<int, int> GetById(string text, string id)
        {
            text.ToLower(CultureInfo.InvariantCulture);
            id.ToLower(CultureInfo.InvariantCulture);

            #region <tag id=id>
            Match match;
            try
            {
                match = Regex.Match(text, @"id\s*=\s*""?\s*" + id + @"\s*""?");
            }
            catch { return null; }
            if (!match.Success) return null;
            int i = match.Index;
            #endregion

            #region <tagName>
            StringBuilder tag = new StringBuilder();
            while (i >= 0)
            {
                if (text[i] == '<')
                {
                    ++i;
                    while ((i < text.Length) && Char.IsLetter(text[i]))
                    {
                        tag.Append(text[i]);
                        ++i;
                    }
                    break;
                }
                --i;
            }
            string tagName = tag.ToString();
            if (String.IsNullOrEmpty(tagName) || (i == -1) || (i == text.Length)) return null;
            #endregion

            #region ...>Item1
            i = text.IndexOf('>', i);
            if (i == -1) return null;
            ++i; 
            #endregion

            #region </tag>
            int count = 1, j = i;
            while (true)
            {
                j = text.IndexOf(tagName, j);
                if (j == -1) return null;
                if (text[j - 1] == '/') --count;
                else ++count;
                if (count == 0) break;
                ++j;
            }
            #endregion

            #region <[==Item2]/...
            for (; i <= j; --j)
                if (text[j] == '<')
                    break;
            if (i > j) return null; 
            #endregion

            return new Tuple<int, int>(i, j);
        } 
        #endregion

        #region SetTitle
        private string SetTilte(string html, string title)
        {
            html.ToLower(CultureInfo.InvariantCulture);
            title.ToLower(CultureInfo.InvariantCulture);
            int i = html.IndexOf("<title");
            if (i == -1) return html;
            i = html.IndexOf(">", i);
            if (i == -1) return html;
            ++i;
            int j = html.IndexOf("</title");
            if (j == -1) return html;
            return WriteInnerText(html, title, new Tuple<int, int>(i, j));
        } 
        #endregion

        #region GetErrorList
        private string GetHtmlErrorList(IEnumerable<string> errors)
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

        #region GetByRoute
        private string GetByRoute(string target)
        {
            try
            {
                using (StreamReader stream = new StreamReader(_baseDirectory + "Content/" + target))
                {
                    return stream.ReadToEnd();
                }
            }
            catch
            {
                throw new ArgumentException("Probably target " + target + " doesn't exist");
            }
        } 
        #endregion
    } 
}