using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcMinorRequirement
{
    public int MinorId { get; set; }

    public int CatalogYear { get; set; }

    public string CourseId { get; set; } = null!;
}
