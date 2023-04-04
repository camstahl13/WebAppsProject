using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcPlan
{
    public int PlanId { get; set; }

    public string Planname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int CatalogYear { get; set; }

    public bool Default { get; set; }
}
