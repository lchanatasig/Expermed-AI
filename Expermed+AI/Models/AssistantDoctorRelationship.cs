using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class AssistantDoctorRelationship
{
    public int AssistandoctorId { get; set; }

    public DateTime? AssistandoctorDate { get; set; }

    public int RelationshipStatus { get; set; }

    public int AssistantUserid { get; set; }

    public int DoctorUserid { get; set; }

    public virtual User AssistantUser { get; set; } = null!;

    public virtual User DoctorUser { get; set; } = null!;
}
