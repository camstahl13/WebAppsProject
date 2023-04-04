using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcPlannedCourse
{
    public int PlanId { get; set; }

    public string CourseId { get; set; } = null!;

    public int Year { get; set; }

    public string Term { get; set; } = null!;
}
