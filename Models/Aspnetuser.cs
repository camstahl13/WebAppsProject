﻿using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class Aspnetuser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<Aspnetuserclaim> Aspnetuserclaims { get; } = new List<Aspnetuserclaim>();

    public virtual ICollection<Aspnetuserlogin> Aspnetuserlogins { get; } = new List<Aspnetuserlogin>();

    public virtual ICollection<Aspnetusertoken> Aspnetusertokens { get; } = new List<Aspnetusertoken>();

    public virtual ICollection<Aspnetrole> Roles { get; } = new List<Aspnetrole>();
}
