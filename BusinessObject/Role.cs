﻿namespace BusinessObject
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; } = new List<Account>();
    }
}
