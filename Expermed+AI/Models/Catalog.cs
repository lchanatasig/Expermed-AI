using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Catalog
{
    public int CatalogId { get; set; }

    public string CatalogName { get; set; } = null!;

    public string CatalogCategory { get; set; } = null!;

    public int CategoryStatus { get; set; }
}
