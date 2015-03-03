using AutoMapper;
using DAL.Entities;
using RpR.Models;

namespace RpR.Mappers
{
    public static class DefaultMapper
    {
        #region GetUser
        public static User GetUser(UserModel user)
        {
            Mapper.CreateMap<UserModel, User>();
            return Mapper.Map<UserModel, User>(user);
        } 
        #endregion
    }
}