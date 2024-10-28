﻿using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class User
{
    public int UsersId { get; set; }

    public string UserDocumentNumber { get; set; } = null!;

    public string UserNames { get; set; } = null!;

    public string UserSurnames { get; set; } = null!;

    public string UserPhone { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public DateTime? UserCreationdate { get; set; }

    public DateTime? UserModificationdate { get; set; }

    public string UserAddress { get; set; } = null!;

    public byte[]? UserDigitalsignature { get; set; }

    public byte[]? UserQrcode { get; set; }

    public byte[]? UserProfilephoto { get; set; }

    public string? UserPrfilephoto64 { get; set; }

    public string? UserSenecytcode { get; set; }

    public string? UserXkeytaxo { get; set; }

    public string? UserXpasstaxo { get; set; }

    public int? UserSequentialBilling { get; set; }

    public string UserLogin { get; set; } = null!;

    public byte[] UserPassword { get; set; } = null!;

    public int UserStatus { get; set; }

    public int? UserProfileid { get; set; }

    public int? UserEstablishmentid { get; set; }

    public int? UserSpecialtyid { get; set; }

    public int? UserCountryid { get; set; }

    public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipAssistantUsers { get; set; } = new List<AssistantDoctorRelationship>();

    public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipDoctorUsers { get; set; } = new List<AssistantDoctorRelationship>();

    public virtual ICollection<Loginaudit> Loginaudits { get; set; } = new List<Loginaudit>();

    public virtual ICollection<TokenSession> TokenSessions { get; set; } = new List<TokenSession>();

    public virtual Country? UserCountry { get; set; }

    public virtual Establishment? UserEstablishment { get; set; }

    public virtual Profile? UserProfile { get; set; }

    public virtual Specialty? UserSpecialty { get; set; }
}
