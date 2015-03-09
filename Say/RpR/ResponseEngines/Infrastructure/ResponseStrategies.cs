#region using
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class ResponseStrategies : IResponseStrategies
    {
        #region Fields&Props
        private readonly string _baseDirectory;
        #endregion

        #region .ctors
        public ResponseStrategies()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        } 
        #endregion

        #region BindContent
        public string BindContent(string content, Dictionary<string, string> placesValues)
        {
            if ((content == null) || String.IsNullOrEmpty(content.ToString()) ||
                String.IsNullOrWhiteSpace(content.ToString()))
                throw new ArgumentNullException("Content is null or empty", (Exception)null);
            if (placesValues == null)
                throw new ArgumentNullException("Collection is null", (Exception)null);
            if (placesValues.Count == 0) return content;

            StringBuilder response = new StringBuilder().Append(content);
            Match match;
            foreach (var e in placesValues)
            {
                match = Regex.Match(content.ToString(), @"\[\s*" + e.Key + @"\s*/\]", RegexOptions.IgnoreCase);
                if (match.Success)
                    response.Replace(match.Value, e.Value);
            }
            return response.ToString();
        }
        #endregion

        #region GetContent
        public string GetContent(string content, string innerContent, string placement = "main")
        {
            if (String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                    throw new ArgumentNullException("Content is null or empty");

            #region InnerContent
            if (!String.IsNullOrEmpty(innerContent) || !String.IsNullOrWhiteSpace(innerContent))
            {
                if (String.IsNullOrEmpty(placement) || String.IsNullOrWhiteSpace(placement))
                    throw new ArgumentNullException("Placement is null");

                Dictionary<string, string> placeValue = new Dictionary<string, string>();
                placeValue.Add(placement, innerContent);
                content = BindContent(content, placeValue);
            } 
	        #endregion

            return content;
        } 
        #endregion

        #region GetByRoutes
        public string GetByRoutes(string target, string innerTarget, string placement = "main")
        {
            try
            {
                string content = GetByRoute(target), innerContent = null;
                if (innerTarget != null) innerContent = GetByRoute(innerTarget);
                    content = GetContent(content, innerContent, placement);
                return content;
            }
            catch (ArgumentException exc)
            { throw exc; }
        } 
        #endregion

        #region RouteToContent
        public string RouteToContent(string content, string innerTarget, string placement = "main")
        {
            try
            {
                string innerContent = null;
                if (innerTarget != null) innerContent = GetByRoute(innerTarget);
                return GetContent(content, innerContent, placement);
            }
            catch (ArgumentException exc)
            { throw exc; }
        }
        #endregion

        #region ContentToRoute
        public string ContentToRoute(string target, string innerContent, string placement = "main")
        {
            try
            {
                return GetContent(GetByRoute(target), innerContent, placement);
            }
            catch (ArgumentException exc)
            { throw exc; }
        }
        #endregion

        #region SetTitle
        public string SetTitle(string content, string title)
        {
            if (String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("Content is null or empty");
            if (String.IsNullOrEmpty(title) || String.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("Title is null or empty");

            StringBuilder response = new StringBuilder().Append(content);
            content.ToString().ToLower(CultureInfo.InvariantCulture);
            title.ToLower(CultureInfo.InvariantCulture);
            int i = content.ToString().IndexOf("<title");
            if (i == -1) return content;
            int j = content.ToString().IndexOf("</title");
            if (j == -1) return content;
            response.Remove(i, j - i);
            response.Insert(i, "<title>" + title);
            return response.ToString();
        }
        #endregion

        #region SetInfo
        public string SetInfo(string content, IEnumerable<string> errors, string placement)
        {
            if (String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("Content is null or empty");
            if (String.IsNullOrEmpty(placement) || String.IsNullOrWhiteSpace(placement))
                throw new ArgumentNullException("Placement is null or empty");

            Dictionary<string, string> placeValue = new Dictionary<string, string>();
            placeValue.Add(placement, GetHtmlErrorList(errors));
            return BindContent(content, placeValue);
        }
        #endregion

        #region JsonResult
        public string JsonResult(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        #endregion

        #region GetErrorList
        private string GetHtmlErrorList(IEnumerable<string> errors)
        {
            if ((errors == null) || (errors.Count() == 0)) return String.Empty;
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