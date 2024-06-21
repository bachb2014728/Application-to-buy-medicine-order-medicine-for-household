
using backend.Error;
using backend.Jwt;
using backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Helper
{
    public static class GlobalVariables
    {
        public static string Token { get; set; } = string.Empty;
    }

}