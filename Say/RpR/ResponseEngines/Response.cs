#region using
using Newtonsoft.Json;
using RpR.JsonEntities;
using RpR.ResponseEngines.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        // placement-errors
        public Tuple<string, IEnumerable<string>> Errors { get; set; } 
        #endregion

        #region .ctors
        public Response()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        } 
        #endregion

        #region BindContent
        public void BindContent(StringBuilder content, Dictionary<string, string> placeValue)
        {
            if ((content == null) || String.IsNullOrEmpty(content.ToString()) ||
                String.IsNullOrWhiteSpace(content.ToString()))
                throw new ArgumentNullException("Content is null or empty", (Exception)null);
            if (placeValue == null)
                throw new ArgumentNullException("Collection is null", (Exception)null);

            Match match;
            foreach (var e in placeValue)
            {
                match = Regex.Match(content.ToString(), @"\[\s*" + e.Key + @"\s*/\]");
                if (match.Success)
                    content.Replace(match.Value, e.Value);
            }
        }
        #endregion

        #region GetContent
        public string GetContent(string content, string innerContent, string placement = "main", bool isJson = false)
        {
            if (String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                    throw new ArgumentNullException("Placement is null");

            StringBuilder response = new StringBuilder().Append(content);
            Dictionary<string, string> placeValue = new Dictionary<string, string>();

            #region InnerContent
		    if (innerContent != null)
            {
                if (String.IsNullOrEmpty(placement) || String.IsNullOrWhiteSpace(placement))
                    throw new ArgumentNullException("Placement is null");

                #region IsJson
                if (isJson)
                {
                    CoreJsonEntity pageInfo = JsonResult(innerContent);
                    if (pageInfo == null)
                        throw new ArgumentNullException("It cannot represent inner content as CoreJsonEntity",
                            (Exception)null);
                    if (pageInfo.Title != null) Title = pageInfo.Title;
                    innerContent = pageInfo.Content;
                }
                #endregion

                placeValue.Add(placement, innerContent);
            } 
	        #endregion

            // SetTitle
            if (Title != null) SetTitle(response, Title);

            // SetErrors
            if ((Errors != null) && (Errors.Item2 != null) && (Errors.Item2.Count() > 0) &&
                !String.IsNullOrEmpty(Errors.Item1) && !String.IsNullOrWhiteSpace(Errors.Item1))
                placeValue.Add(Errors.Item1, GetHtmlErrorList(Errors.Item2));

            BindContent(response, placeValue);
            return response.ToString();
        } 
        #endregion

        #region GetByRoutes
        public string GetByRoutes(string target, string innerTarget, string placement = "main", bool isJson = false)
        {
            try
            {
                string content = GetByRoute(target), innerContent = null;
                if (innerTarget != null) innerContent = GetByRoute(innerTarget);
                    content = GetContent(content, innerContent, placement, isJson);
                return content.ToString();
            }
            catch (ArgumentException exc)
            { throw exc; }
        } 
        #endregion

        #region RouteToContent
        public string RouteToContent(string content, string innerTarget, string placement = "main", bool isJson = false)
        {
            try
            {
                string innerContent = null;
                if (innerTarget != null) innerContent = GetByRoute(innerTarget);
                return GetContent(content, innerContent, placement, isJson);
            }
            catch (ArgumentException exc)
            { throw exc; }
        }
        #endregion

        #region ContentToRoute
        public string ContentToRoute(string target, string innerContent, string placement = "main", bool isJson = false)
        {
            try
            {
                return GetContent(GetByRoute(target), innerContent, placement, isJson);
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

        #region SetTitle
        private void SetTitle(StringBuilder html, string title)
        {
            html.ToString().ToLower(CultureInfo.InvariantCulture);
            title.ToLower(CultureInfo.InvariantCulture);
            int i = html.ToString().IndexOf("<title");
            if (i == -1) return;
            int j = html.ToString(). IndexOf("</title");
            if (j == -1) return;
            html.Remove(i, j - i);
            html.Insert(i, "<title>" + title);
        } 
        #endregion

        #region GetErrorList
        private string GetHtmlErrorList(IEnumerable<string> errors)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<ul>");
            foreach (var e in errors)
            {
                result.Append("<li>" + e + "</li>");
            }
            result.Append("</ul>");
            return result.ToString();;
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