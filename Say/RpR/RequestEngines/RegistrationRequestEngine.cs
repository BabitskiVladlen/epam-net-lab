#region using
using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using BLL.Tools;
using DAL.Entities;
using Ninject;
using RpR.Mappers;
using RpR.Models;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web; 
#endregion


namespace RpR.RequestEngines
{
    public class RegistrationRequestEngine : RequestEngine
    {
        public void Post(UserModel model, HttpPostedFileBase image)
        {
            IUserService userSevice = DependencyResolution.Kernel.Get<IUserService>();
            IRegistration registration = DependencyResolution.Kernel.Get<IRegistration>();
            List<string> errors;

            #region AddNewUser
            User user = DefaultMapper.GetUser(model);
            user.Role = 2;
            bool ok = registration.AddNewUser(user, out errors);
            if (errors == null) errors = new List<string>();
            if ((model.Password != null) &&
                String.Equals(model.Password, model.PasswordAgain, StringComparison.InvariantCulture))
            { errors.Add("Different passwords"); ok = false; } 
            #endregion

            #region ForErrors
            if (!ok)
            {
                new RegistrationResponseEngine().GetResponse(model,
                    new Tuple<IEnumerable<string>, string>
                        (errors.Distinct(EqualityComparer<string>.Default), "validation"));
            } 
            #endregion

            #region SaveImage
            if (image != null)
            {
                ImageProcessor imgProc = new ImageProcessor();
                int quality = 20;
                byte[] bytesOrig = new byte[image.ContentLength],
                    bytesCompr = new byte[image.ContentLength];
                string mimeType = image.ContentType;
                using (Stream input = image.InputStream)
                {
                    try
                    {
                        Image img = Image.FromStream(input);
                        using (MemoryStream memory = new MemoryStream())
                        {
                            imgProc.CompressImage(img, mimeType, quality, memory);
                            bytesCompr = memory.ToArray();
                        }
                    }
                    catch
                    {
                        input.Read(bytesCompr, 0, image.ContentLength);
                    }
                    input.Read(bytesOrig, 0, image.ContentLength);
                }
                user.CompressedImage = bytesCompr;
                user.Image = bytesOrig;
                user.ImageMimeType = mimeType;
                userSevice.SaveUser(user);
            }  
            #endregion
        }
    }
}