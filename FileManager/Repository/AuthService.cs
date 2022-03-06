﻿using FileManager.DTOs;
using FileManager.IRepository;
using FileManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Repository
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> BuildToken( string email)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(email);
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) 
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var token = new JwtSecurityToken(
                
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);

            return token;
        }

        private List<Claim> GetClaims(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateToken(string key, string issuer, string token)
        {
            //var user = await _user.findByNameAsync(userDTO.Email)
            return true;
        }
    }
}
