using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Consultation
{
    public int ConsultationId { get; set; }

    public DateTime? ConsultationCreationdate { get; set; }

    public int? ConsultationUsercreate { get; set; }

    public int ConsultationPatient { get; set; }

    public int? ConsultationSpeciality { get; set; }

    public string ConsultationHistoryclinic { get; set; } = null!;

    public int? ConsultationSequential { get; set; }

    public string? ConsultationReason { get; set; }

    public string? ConsultationDisease { get; set; }

    public string? ConsultationRelativename { get; set; }

    public string? ConsultationWarningsings { get; set; }

    public string? ConsultationNonpharmacologycal { get; set; }

    public int? ConsultationReasontype { get; set; }

    public string? ConsultationReasonphone { get; set; }

    public string ConsultationTemperature { get; set; } = null!;

    public string ConsultationRespiratoryrate { get; set; } = null!;

    public string ConsultationBloodpressuresd { get; set; } = null!;

    public string ConsultationPulse { get; set; } = null!;

    public string ConsultationWeight { get; set; } = null!;

    public string ConsultationSize { get; set; } = null!;

    public string? ConsultationTreatmentplan { get; set; }

    public string? ConsultationObservation { get; set; }

    public string? ConsultationPersonalbackground { get; set; }

    public int? ConsultationDisabilitydays { get; set; }

    public int? ConsultationType { get; set; }

    public string? ConsultationEvolutionnotes { get; set; }

    public int ConsultationStatus { get; set; }

    public virtual ICollection<AllergiesConsultation> AllergiesConsultations { get; set; } = new List<AllergiesConsultation>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Patient ConsultationPatientNavigation { get; set; } = null!;

    public virtual Specialty? ConsultationSpecialityNavigation { get; set; }

    public virtual User? ConsultationUsercreateNavigation { get; set; }

    public virtual ICollection<DiagnosisConsultation> DiagnosisConsultations { get; set; } = new List<DiagnosisConsultation>();

    public virtual ICollection<FamiliaryBackground> FamiliaryBackgrounds { get; set; } = new List<FamiliaryBackground>();

    public virtual ICollection<ImagesConsultation> ImagesConsultations { get; set; } = new List<ImagesConsultation>();

    public virtual ICollection<LaboratoryConsultation> LaboratoryConsultations { get; set; } = new List<LaboratoryConsultation>();

    public virtual ICollection<MedicationsConsultation> MedicationsConsultations { get; set; } = new List<MedicationsConsultation>();

    public virtual ICollection<OrgansSystem> OrgansSystems { get; set; } = new List<OrgansSystem>();

    public virtual ICollection<PhysicalExamination> PhysicalExaminations { get; set; } = new List<PhysicalExamination>();

    public virtual ICollection<SurgerisConsultation> SurgerisConsultations { get; set; } = new List<SurgerisConsultation>();
}
