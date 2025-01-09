﻿using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class OrgansSystem
{
    public int OrganssystemsId { get; set; }

    public int? OrganssystemsConsultationsid { get; set; }

    public bool? OrganssystemsRespiratory { get; set; }

    public string? OrganssystemsRespiratoryObs { get; set; }

    public bool? OrganssystemsCardiovascular { get; set; }

    public string? OrganssystemsCardiovascularObs { get; set; }

    public bool? OrganssystemsDigestive { get; set; }

    public string? OrganssystemsDigestiveObs { get; set; }

    public bool? OrganssystemsGenital { get; set; }

    public string? OrganssystemsGenitalObs { get; set; }

    public bool? OrganssystemsUrinary { get; set; }

    public string? OrganssystemsUrinaryObs { get; set; }

    public bool? OrganssystemsSkeletalM { get; set; }

    public string? OrganssystemsSkeletalMObs { get; set; }

    public bool? OrganssystemsEndocrine { get; set; }

    public string? OrganssystemsEndocrineObs { get; set; }

    public bool? OrganssystemsLymphatic { get; set; }

    public string? OrganssystemsLymphaticObs { get; set; }

    public bool? OrganssystemsNervous { get; set; }

    public string? OrganssystemsNervousObs { get; set; }

    public virtual Consultation? OrganssystemsConsultations { get; set; }
}