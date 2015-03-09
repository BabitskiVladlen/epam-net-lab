#region using
using AutoMapper;
using DAL.Entities;
using RpR.Models; 
#endregion

namespace RpR.Mappers
{
    public static class DefaultMappers
    {
        #region GetUser
        public static User GetUser(UserModel user, User exUser)
        {
            Mapper.CreateMap<UserModel, User>();
            User newUser = Mapper.Map<UserModel, User>(user);
            if (exUser == null)
                newUser.Role = 2;
            else
            {
                newUser.UserID = exUser.UserID;
                newUser.Role = exUser.Role;
                newUser.Image = exUser.Image;
                newUser.CompressedImage = exUser.CompressedImage;
                newUser.ImageMimeType = exUser.ImageMimeType;
                newUser.NewMessages = exUser.NewMessages;
                newUser.NewFriends = exUser.NewFriends;
            }
            return newUser;
        }
        #endregion

        #region GetUserModel
        public static UserModel GetUserModel(User user)
        {
            Mapper.CreateMap<User, UserModel>();
            return Mapper.Map<User, UserModel>(user);
        }
        #endregion
    }
}