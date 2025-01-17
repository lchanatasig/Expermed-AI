﻿using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class User
{
    public int UsersId { get; set; }

    public string? UserDocumentNumber { get; set; }

    public string UserNames { get; set; } = null!;

    public string UserSurnames { get; set; } = null!;

    public string UserPhone { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public DateTime? UserCreationdate { get; set; }

    public DateTime? UserModificationdate { get; set; }

    public string UserAddress { get; set; } = null!;

    public byte[]? UserDigitalsignature { get; set; }

    public byte[]? UserProfilephoto { get; set; }

    public string? UserPrfilephoto64 { get; set; }

    public string? UserSenecytcode { get; set; }

    public string? UserXkeytaxo { get; set; }

    public string? UserXpasstaxo { get; set; }

    public int? UserSequentialBilling { get; set; }

    public string UserLogin { get; set; } = null!;

    public string? UserPassword { get; set; }

    public int UserStatus { get; set; }

    public int? UserProfileid { get; set; }

    public int? UserEstablishmentid { get; set; }

    public int? UserSpecialtyid { get; set; }

    public int? UserCountryid { get; set; }

    public string? UserDescription { get; set; }

    public int? UserVatpercentageid { get; set; }

    public virtual ICollection<Appointment> AppointmentAppointmentCreateuserNavigations { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentAppointmentModifyuserNavigations { get; set; } = new List<Appointment>();

    public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipAssistantUsers { get; set; } = new List<AssistantDoctorRelationship>();

    public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipDoctorUsers { get; set; } = new List<AssistantDoctorRelationship>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Loginaudit> Loginaudits { get; set; } = new List<Loginaudit>();

    public virtual ICollection<Patient> PatientPatientCreationuserNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<Patient> PatientPatientModificationuserNavigations { get; set; } = new List<Patient>();

    public virtual ICollection<TokenSession> TokenSessions { get; set; } = new List<TokenSession>();

    public virtual Country? UserCountry { get; set; }

    public virtual Establishment? UserEstablishment { get; set; }

    public virtual Profile? UserProfile { get; set; }

    public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();

    public virtual Specialty? UserSpecialty { get; set; }

    public virtual VatBilling? UserVatpercentage { get; set; }
}
