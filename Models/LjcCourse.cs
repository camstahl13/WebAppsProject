using System;
using System.Collections.Generic;

namespace ljcProject5.Models;

public partial class LjcCourse
{
    public string CourseId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Credits { get; set; }
}
