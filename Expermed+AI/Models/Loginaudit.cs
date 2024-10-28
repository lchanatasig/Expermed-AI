using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Loginaudit
{
    public int LoginauditId { get; set; }

    public DateTime? LoginauditDate { get; set; }

    public bool LoginauditSuccess { get; set; }

    public string? LoginauditAddresip { get; set; }

    public string? LoginauditMessage { get; set; }

    public int? LoginauditUserid { get; set; }

    public virtual User? LoginauditUser { get; set; }
}
