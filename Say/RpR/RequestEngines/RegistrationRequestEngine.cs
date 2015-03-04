#region using
using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using DAL.Entities;
using Ninject;
using RpR.Mappers;
using RpR.Models;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
#endregion


namespace RpR.RequestEngines
{
    public class RegistrationRequestEngine : RequestEngine
    {
        public void Get(UserModel model, HttpPostedFileBase image)
        {
            IUserService userSevice = DependencyResolution.Kernel.Get<IUserService>();
            IRegistration registration = DependencyResolution.Kernel.Get<IRegistration>();

            #region AddNewUser
            User user = DefaultMapper.GetUser(model);
            user.Role = 2;
            bool ok = registration.TryAddUser(user, model.PasswordAgain, Errors);
            model.Password = null;
            model.PasswordAgain = null;
            #endregion

            #region SaveImage
            if (ok && (image != null))
            {
                try
                {
                    int userID =
                        userSevice.Users.FirstOrDefault(u => (u.Username == user.Username)).UserID;
                    userSevice.SaveImage(userID, image.InputStream, image.ContentType);
                }
                catch (Exception exc)
                {
                    LogMessage.Add(exc, Level.Error);
                    Errors.Add("It cannot save an image");
                }
            }
            #endregion

            new RegistrationResponseEngine().GetResponse(model,
                new Tuple<string, IEnumerable<string>>
                    ("validation", Errors.Distinct(EqualityComparer<string>.Default)));
        }
    }
}