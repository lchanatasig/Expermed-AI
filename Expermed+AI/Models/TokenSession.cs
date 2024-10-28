using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class TokenSession
{
    public int TokensessionId { get; set; }

    public DateTime TokensessionExpirationdate { get; set; }

    public int? TokensessionUserid { get; set; }

    public virtual User? TokensessionUser { get; set; }
}
