#region using
using BLL.Infrastructure;
using BLL.Tools;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq; 
#endregion

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;

        #region .ctors
        public UserService()
            : this(DependencyResolution.Kernel.Get<IUserRepository>(), new RoleService())
        { }

        public UserService(IUserRepository userRepository)
            : this(userRepository, new RoleService())
        { }

        public UserService(IUserRepository userRepository, RoleService roleService)
        {
            if (userRepository == null)
                throw new ArgumentNullException("Repository is null", (Exception)null);
            if (roleService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);

            _userRepository = userRepository;
            _roleService = roleService;
        } 
        #endregion

        #region Users
        public IEnumerable<User> Users
        {
            get { return _userRepository.Users.AsEnumerable<User>(); }
        } 
        #endregion

        #region GetUserByID
        public User GetUserByID(int userID)
        {
            return _userRepository.GetUserByID(userID);
        } 
        #endregion

        #region IsExist
        public bool IsExist(string username)
        {
            User user = Users.FirstOrDefault(u => u.Username == username);
            return user != null;
        } 
        #endregion

        #region SaveUser
        public void SaveUser(User user)
        {
            try
            {
                _userRepository.SaveUser(user);
            }
            catch (ArgumentException exc) { throw exc; }
        } 
        #endregion

        #region DeleteUser
        public void DeleteUser(int userID)
        {
            _userRepository.DeleteUser(userID);
        } 
        #endregion

        #region DisableUser
        public void DisableUser(int userID)
        {
            User user = GetUserByID(userID);
            user.IsDeleted = true;
            SaveUser(user);
        } 
        #endregion

        #region EnableUser
        public void EnableUser(int userID)
        {
            User user = GetUserByID(userID);
            user.IsDeleted = false;
            SaveUser(user);
        }
        #endregion

        #region IsInRole
        public bool IsInRole(int userID, string role)
        {
            User user = GetUserByID(userID);
            if (user == null) return false;
            int roleID = user.Role;
            RoleEntity roleEntity = _roleService.GetRoleByID(roleID);
            return String.Equals(roleEntity.Role, role, StringComparison.InvariantCultureIgnoreCase);
        } 
        #endregion

        #region SaveImage
        public void SaveImage(int userID, Stream inputStream, string contentType)
        {
            User user = GetUserByID(userID);
            if (inputStream == null)
                throw new ArgumentNullException("Stream is null", (Exception)null);
            if (!inputStream.CanRead)
                throw new ArgumentException("It cannot read from a stream", (Exception)null);
            if (String.IsNullOrEmpty(contentType) || String.IsNullOrWhiteSpace(contentType))
                throw new ArgumentNullException("Content type is null or empty");

            ImageProcessor imgProc = new ImageProcessor();
            int quality = 20;
            byte[] originalImage = new byte[inputStream.Length],
                compressedImage = new byte[inputStream.Length];
            string mimeType = contentType;

            using (inputStream)
            {
                Image img = Image.FromStream(inputStream);
                using (MemoryStream memory = new MemoryStream())
                {
                    imgProc.CompressImage(img, mimeType, quality, memory);
                    compressedImage = memory.ToArray();
                }
                inputStream.Read(originalImage, 0, (int)inputStream.Length);
            }

            user.Image = originalImage;
            user.CompressedImage = compressedImage;
            user.ImageMimeType = mimeType;
            SaveUser(user);
        } 
        #endregion
    }
}
