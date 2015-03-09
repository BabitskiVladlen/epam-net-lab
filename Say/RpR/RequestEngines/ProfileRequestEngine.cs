#region using
using BLL.Infrastructure;
using BLL.Security;
using BLL.Security.Infrastructure;
using BLL.Security.PasswordEngines;
using DAL.Entities;
using Ninject;
using RpR.Mappers;
using RpR.Models;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion


namespace RpR.RequestEngines
{
    public class ProfileRequestEngine : RequestEngine
    {
        #region Fields&Props
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        #endregion

        #region .ctors
        public ProfileRequestEngine()
            : this(DependencyResolution.Kernel.Get<IUserService>(),
            DependencyResolution.Kernel.Get<IRoleService>())
        { }

        public ProfileRequestEngine(IUserService userService, IRoleService roleService)
        {
            _userService = userService ?? DependencyResolution.Kernel.Get<IUserService>();
            _roleService = roleService ?? DependencyResolution.Kernel.Get<IRoleService>();
        }
        #endregion

        #region Get
        public ResponseEngine Get()
        {
            if (!IsAuthenticated) return new NotFoundResponseEngine(this);
            if (HttpContext != null)
                HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            User user = _userService.GetUserByID(User.Identity.Name);
            return new RegistrationResponseEngine(this) { Model = DefaultMappers.GetUserModel(user),
                Messages_friends = new Tuple<int,int>(user.NewMessages, user.NewFriends) };
        } 
        #endregion

        #region Post
        public ResponseEngine Post(UserModel model, HttpPostedFileBase image)
        {
            if (!IsAuthenticated) return new NotFoundResponseEngine(this);

            IRegistration registration = new DefaultRegistration(_userService);
            IPasswordEngine passwordEngine = new MD5PasswordEngine();
            User exUser = _userService.GetUserByID(User.Identity.Name);
            List<string> info = new List<string>();
            
            if (String.IsNullOrEmpty(model.OldPassword) || String.IsNullOrWhiteSpace(model.OldPassword) ||
                !passwordEngine.Verify(model.OldPassword, exUser.Password))
                info.Add("Wrong password");
            else
            {
                User newUser = DefaultMappers.GetUser(model, exUser);
                if (String.IsNullOrEmpty(model.Password))
                {
                    newUser.Password = model.OldPassword;
                    model.PasswordAgain = model.OldPassword;
                }
                if (registration.TryAddUser(newUser, model.PasswordAgain, info))
                    info.Add("Saved");
            }

            model.OldPassword = null;
            model.Password = null;
            model.PasswordAgain = null;

            LayoutResponseEngine responseEngine = new RegistrationResponseEngine(this);
            responseEngine.Model = model;
            responseEngine.Messages_friends = new Tuple<int, int>(exUser.NewMessages, exUser.NewFriends);
            responseEngine.Info.AddRange(info.Distinct(EqualityComparer<string>.Default));
            return responseEngine;
        }
        #endregion
    }
}