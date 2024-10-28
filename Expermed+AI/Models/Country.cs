using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public string? CountryIso { get; set; }

    public string CountryCode { get; set; } = null!;

    public string? CountryNationality { get; set; }

    public int CountryStatus { get; set; }

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
