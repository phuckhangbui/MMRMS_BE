﻿namespace DTOs.Authentication
{
    public class LoginAccountDto
    {
        public int? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? DateCreate { get; set; }
        public string? Status { get; set; }
        public int? RoleId { get; set; }
        public string? RefreshToken { get; set; }
        public string? Token { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? AvatarImg { get; set; }
    }
}
