using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Catalog
{
    public int CatalogId { get; set; }

    public string CatalogName { get; set; } = null!;

    public string CatalogCategory { get; set; } = null!;

    public int CategoryStatus { get; set; }

    public virtual ICollection<AllergiesConsultation> AllergiesConsultations { get; set; } = new List<AllergiesConsultation>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogCancerNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDiabetesNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxcardiovascularNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxinfectiousNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxmentalNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogHeartdiseaseNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogHypertensionNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogMalformationNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogOtherNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgroundFamiliarybackgroundRelatshcatalogTuberculosisNavigations { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<Patient> PatientPatientBloodtypeNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientDocumenttypeNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientGenderNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientHealthInsuranceNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientMaritalstatusNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientVocationalTrainingNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<SurgerisConsultation> SurgerisConsultations { get; set; } = new List<SurgerisConsultation>();
}
