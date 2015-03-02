using DAL.Contexts;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Validators;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        #region Users
        public IQueryable<User> Users
        {
            get { return _context.Users; }
        } 
        #endregion

        #region GetUserByID
        public User GetUserByID(int userID)
        {
            return _context.Users.Find(userID);
        } 
        #endregion

        #region SaveUser
        public void SaveUser(User user)
        {
            User exUser;
            try { Validator.UserValidator(user, out exUser); }
            catch (ArgumentException exc)
            { throw exc; }

            if (user.UserID == 0)
                _context.Users.Add(user);
            else if (exUser != null)
            {
                exUser.Username = user.Username;
                exUser.FirstName = user.FirstName;
                exUser.Surname = user.Surname;
                exUser.Password = user.Password;
                exUser.Email = user.Email;
                exUser.Role = user.Role;
                exUser.IsDeleted = user.IsDeleted;
                exUser.Image = user.Image;
                exUser.CompressedImage = user.CompressedImage;
                exUser.ImageMimeType = exUser.ImageMimeType;
            }
            _context.SaveChanges();
        } 
        #endregion

        #region DeleteUser
        public void DeleteUser(int userID)
        {
            User user = GetUserByID(userID);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        } 
        #endregion
    }
}
