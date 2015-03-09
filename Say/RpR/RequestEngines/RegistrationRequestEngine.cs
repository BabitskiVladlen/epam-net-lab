#region using
using BLL.Infrastructure;
using BLL.Security;
using BLL.Security.Infrastructure;
using DAL.Entities;
using Ninject;
using RpR.Mappers;
using RpR.Models;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines;
using RpR.ResponseEngines.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion


namespace RpR.RequestEngines
{
    public class RegistrationRequestEngine : RequestEngine
    {
        #region Fields&Props
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        #endregion

        #region .ctors
        public RegistrationRequestEngine()
            : this(DependencyResolution.Kernel.Get<IUserService>(),
            DependencyResolution.Kernel.Get<IRoleService>())
        { }

        public RegistrationRequestEngine(IUserService userService, IRoleService roleService)
        {
            _userService = userService ?? DependencyResolution.Kernel.Get<IUserService>();
            _roleService = roleService ?? DependencyResolution.Kernel.Get<IRoleService>();
        }
        #endregion

        #region Get
        public ResponseEngine Get()
        {
            if (IsAuthenticated)
                return new NotFoundResponseEngine(this);
            return new RegistrationResponseEngine(this) { Model = new UserModel() };
        } 
        #endregion

        #region Post
        public ResponseEngine Post(UserModel model, HttpPostedFileBase image)
        {
            if (IsAuthenticated)
                return new NotFoundResponseEngine(this);

            IRegistration registration = new DefaultRegistration(_userService);
            List<string> info = new List<string>();
            User  newUser = DefaultMappers.GetUser(model, null);

            if (registration.TryAddUser(newUser, model.PasswordAgain, info))
                info.Add("Welcome to Say! You can upload your profile picture now");

            LayoutResponseEngine responseEngine = new RegistrationResponseEngine(this);
            model.Password = null;
            model.PasswordAgain = null;
            responseEngine.Model = model;
            responseEngine.Info.AddRange(info.Distinct(EqualityComparer<string>.Default));
            return responseEngine;
        }
        #endregion
    }
}