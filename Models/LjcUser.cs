using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcUser
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
