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
        #region Fields&Props
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService; 
        #endregion

        #region .ctors
        public UserService(IUserRepository userRepository = null, IRoleService roleService = null)
        {
            _userRepository = userRepository ?? DependencyResolution.Kernel.Get<IUserRepository>();
            _roleService = roleService ?? new RoleService();
        } 
        #endregion

        #region Users
        public IEnumerable<User> Users
        {
            get { return _userRepository.Users.AsEnumerable<User>(); }
        } 
        #endregion

        #region GetUserByID(int)
        public User GetUserByID(int userID)
        {
            return _userRepository.GetUserByID(userID);
        } 
        #endregion

        #region GetUserByID(string)
        public User GetUserByID(string userID)
        {
            int i;
            User user = null; ;
            if (Int32.TryParse(userID, out i))
                user = GetUserByID(i);
            return user; ;
        }
        #endregion

        #region GetUser(selector)
        public User GetUser(IEnumerable<string> username_email)
        {
            if ((username_email == null) || (username_email.Count() < 2))
                throw new ArgumentException("Invalid selector", (Exception)null);

            string[] selector = username_email.ToArray<string>();
            return Users.FirstOrDefault(u =>
                (String.Equals(u.Username, selector[0], StringComparison.InvariantCultureIgnoreCase) ||
                (String.Equals(u.Email, selector[1], StringComparison.InvariantCultureIgnoreCase))));
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
            if (user == null) return;
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
            if (user == null) return;
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
                    memory.Flush();
                    imgProc.CompressImage(img, mimeType, 100, memory);
                    originalImage = memory.ToArray();
                }
            }
            
            user.Image = originalImage;
            user.CompressedImage = compressedImage;
            user.ImageMimeType = mimeType;
            SaveUser(user);
        } 
        #endregion
    }
}
