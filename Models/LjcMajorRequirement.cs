using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcMajorRequirement
{
    public int MajorId { get; set; }

    public int CatalogYear { get; set; }

    public string CourseId { get; set; } = null!;

    public string Category { get; set; } = null!;
}
