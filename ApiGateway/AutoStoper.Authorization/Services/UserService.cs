﻿using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using ApiGateway.Core.User;
using AutoStoper.Authorization.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoper.Authorization.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _appSettings;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IConfiguration appSettings, IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings;
            _unitOfWork = unitOfWork;
        }

        public LoginResponse Authenticate(LoginRequest model)
        {
            var user = _unitOfWork.Korisnici.Get(p => p.Username == model.Username && p.Password == model.Password).FirstOrDefault();

            if (user == null) 
                return null;

            var token = generateJwtToken(user);
            return new LoginResponse(user, token);
        }

        public IEnumerable<ApiGateway.Core.User.User> GetAll()
        {
            return _unitOfWork.Korisnici.Get();
        }

        public ApiGateway.Core.User.User GetById(int id)
        {
            return _unitOfWork.Korisnici.GetByID(id);
        }

        public LoginResponse Register(RegisterRequest model)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes(@"Files\Images\avatar.png");

            var userToRegister = new User { 
                Email = model.Email,
                Username = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                DateRegistered = DateTime.Now,
                Image = imgdata
            };

            _unitOfWork.Korisnici.Insert(userToRegister);
            _unitOfWork.Commit();

            return this.Authenticate(new LoginRequest { Username = userToRegister.Username, Password = userToRegister.Password });
        }

        public User Update(User user)
        {
            _unitOfWork.Korisnici.Update(user);
            _unitOfWork.Commit();
            return _unitOfWork.Korisnici.GetByID(user.Id);
        }

        private string generateJwtToken(ApiGateway.Core.User.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings["JwtOptions:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
