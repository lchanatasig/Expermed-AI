using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class FamiliaryBackground
{
    public int FamiliarybackgroundId { get; set; }

    public int? FamiliarybackgroundConsultationid { get; set; }

    public bool? FamiliarybackgroundHeartdisease { get; set; }

    public string? FamiliarybackgroundHeartdiseaseObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogHeartdisease { get; set; }

    public bool? FamiliarybackgroundDiabetes { get; set; }

    public string? FamiliarybackgroundDiabetesObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogDiabetes { get; set; }

    public bool? FamiliarybackgroundDxcardiovascular { get; set; }

    public string? FamiliarybackgroundDxcardiovascularObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogDxcardiovascular { get; set; }

    public bool? FamiliarybackgroundHypertension { get; set; }

    public string? FamiliarybackgroundHypertensionObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogHypertension { get; set; }

    public bool? FamiliarybackgroundCancer { get; set; }

    public string? FamiliarybackgroundCancerObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogCancer { get; set; }

    public bool? FamiliarybackgroundTuberculosis { get; set; }

    public string? FamiliarybackgroundTuberculosisObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogTuberculosis { get; set; }

    public bool? FamiliarybackgroundDxmental { get; set; }

    public string? FamiliarybackgroundDxmentalObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogDxmental { get; set; }

    public bool? FamiliarybackgroundDxinfectious { get; set; }

    public string? FamiliarybackgroundDxinfectiousObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogDxinfectious { get; set; }

    public bool? FamiliarybackgroundMalformation { get; set; }

    public string? FamiliarybackgroundMalformationObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogMalformation { get; set; }

    public bool? FamiliarybackgroundOther { get; set; }

    public string? FamiliarybackgroundOtherObs { get; set; }

    public int? FamiliarybackgroundRelatshcatalogOther { get; set; }

    public virtual Consultation? FamiliarybackgroundConsultation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogCancerNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogDiabetesNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogDxcardiovascularNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogDxinfectiousNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogDxmentalNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogHeartdiseaseNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogHypertensionNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogMalformationNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogOtherNavigation { get; set; }

    public virtual Catalog? FamiliarybackgroundRelatshcatalogTuberculosisNavigation { get; set; }
}
