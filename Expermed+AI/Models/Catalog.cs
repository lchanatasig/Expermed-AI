using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Catalog
{
    public int CatalogId { get; set; }

    public string CatalogName { get; set; } = null!;

    public string CatalogCategory { get; set; } = null!;

    public int CategoryStatus { get; set; }

    public virtual ICollection<Patient> PatientPatientBloodtypeNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientDocumenttypeNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientGenderNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientHealthInsuranceNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientMaritalstatusNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientVocationalTrainingNavigations { get; set; } = new List<Patient>();
}
