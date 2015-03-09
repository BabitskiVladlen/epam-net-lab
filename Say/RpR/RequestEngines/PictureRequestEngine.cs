#region using
using BLL.Infrastructure;
using DAL.Entities;
using Ninject;
using RpR.Mappers;
using RpR.Models;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Web; 
#endregion

namespace RpR.RequestEngines
{
    public class PictureRequestEngine : RequestEngine
    {
        #region Fields&Props
        private readonly IUserService _userService;
        #endregion

        #region .ctors
        public PictureRequestEngine()
            : this(DependencyResolution.Kernel.Get<IUserService>())
        { }

        public PictureRequestEngine(IUserService userService)
        {
            _userService = userService ?? DependencyResolution.Kernel.Get<IUserService>();
        }
        #endregion

        #region Get
        public ResponseEngine Get(string email = null)
        {
            byte[] file = null;
            string contentType = String.Empty;

            #region IsAuthenticated
            if (IsAuthenticated)
            {
                if (email == null)
                {
                    User user = _userService.GetUserByID(User.Identity.Name);
                    file = user.CompressedImage;
                    contentType = user.ImageMimeType;            
                }
                else
                {
                    User user = _userService.Users
                        .FirstOrDefault(u => String.Equals(u.Email, email, StringComparison.InvariantCultureIgnoreCase));
                    if (user != null)
                    {
                        file = user.CompressedImage;
                        contentType = user.ImageMimeType;
                    }
                }
            } 
            #endregion

            #region DefaultImage
            if (file == null)
            {
                LogMessage.Add("File is null", Level.Error);
                string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                file = File.ReadAllBytes(_baseDirectory + "Content/images/default.png");
                contentType = "image/png";
            } 
            #endregion

            return new FileResponseEngine(file, contentType, this);
        }
        #endregion

        #region Post
        public ResponseEngine Post(HttpPostedFileBase image)
        {
            RedirectResponseEngine responseEngine;

            #region NonAuth
            if (!IsAuthenticated)
            {
                responseEngine = new RedirectResponseEngine("registration.rpr", this);
                responseEngine.Stash["info"] = "Sign up first";
                responseEngine.Stash["model"] = new UserModel();
                return responseEngine;
            }
            #endregion

            responseEngine = new RedirectResponseEngine("profile.rpr", this);
            User user = _userService.GetUserByID(User.Identity.Name); ;
            UserModel userModel = DefaultMappers.GetUserModel(user);
            responseEngine.Stash["model"] = userModel;

            #region NullImage
            if (image == null || image.ContentLength == 0)
            {
                responseEngine.Stash["info"] = "Upload image first";
                return responseEngine;
            }
            #endregion

            #region SaveImage
            try
            {
                _userService.SaveImage(user.UserID, image.InputStream, image.ContentType);
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                responseEngine.Stash["info"] = "It cannot save an image";
                return responseEngine;
            }
            #endregion

            responseEngine.Stash["info"] = "Saved";
            return responseEngine;
        } 
        #endregion
    }
}