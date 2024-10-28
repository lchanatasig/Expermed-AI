using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public string ProfileName { get; set; } = null!;

    public string? ProfileDescription { get; set; }

    public DateTime? ProfileCreationdate { get; set; }

    public int ProfileState { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
