#region using
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Generic;
#endregion

namespace RpR.ResponseEngines
{
    public abstract class LayoutResponseEngine : ContentResponseEngine
    {
        #region Fields&Props
        public Tuple<int, int> Messages_friends { get; set; }
        public string Layout { get; set; }
        #endregion

        #region .ctors
        public LayoutResponseEngine(RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        {
            PrepareLayout();
        } 
        #endregion

        #region PrepareLayout
        public void PrepareLayout()
        {
            string index = ResponseStrategies.GetByRoutes("../index.rpr", null);
            index = ResponseStrategies.SetTitle(index, "Say");
            Dictionary<string, string> placesValues = new Dictionary<string, string>();
            if (IsAuthenticated)
                placesValues.Add("menu", GetMenu());
            else
                placesValues.Add("menu", GetNonAuthMenu());
            Layout = ResponseStrategies.BindContent(index, placesValues);
        } 
        #endregion

        #region GetNonAuthMenu
        public string GetNonAuthMenu()
        {
            return ResponseStrategies.GetByRoutes("partial/non_auth_menu.rpr", (string)null);
        } 
        #endregion

        #region GetMenu
        public string GetMenu()
        {
            Dictionary<string, string> placesValues = new Dictionary<string, string>();

            int newMessages = ((Messages_friends != null) &&
                (Messages_friends.Item1 != 0)) ? Messages_friends.Item1 : 0;
            if (newMessages > 0)
                placesValues.Add("messages", "(" + newMessages + ")");
            else placesValues.Add("messages", String.Empty);

            int newFriends = ((Messages_friends != null) &&
                (Messages_friends.Item2 != 0)) ? Messages_friends.Item2 : 0;
            if (newFriends > 0)
                placesValues.Add("friends", "(" + newFriends + ")");
            else placesValues.Add("friends", String.Empty);

            string menu = ResponseStrategies.GetByRoutes("partial/menu.rpr", (string)null);
            return ResponseStrategies.BindContent(menu, placesValues);
        } 
        #endregion
    }
}