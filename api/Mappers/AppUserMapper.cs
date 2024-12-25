using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;

namespace api.Mappers
{
    public static class AppUserMapper
    {
        public static UserSignInDto toUserSignInDto(this AppUser appUser, string token)
        {
            return new UserSignInDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Token = token
            };
        }
    }
}