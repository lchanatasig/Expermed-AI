using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Establishment
{
    public int EstablishmentId { get; set; }

    public DateTime? EstablishmentCreationdate { get; set; }

    public DateTime? EstablishmentUpdatedate { get; set; }

    public string EstablishmentName { get; set; } = null!;

    public string EstablishmentAddress { get; set; } = null!;

    public string? EstablishmentEmissionpoint { get; set; }

    public string? EstablishmentPointofsale { get; set; }

    public string? EstablishmentLocality { get; set; }

    public int EstablishmentStatus { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
