﻿namespace Expermed_AI.Models
{
    public class UserViewModel
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

        public byte[]? UserProfilephoto { get; set; }

        public string? UserPrfilephoto64 { get; set; }

        public string? UserSenecytcode { get; set; }

        public string? UserXkeytaxo { get; set; }

        public string? UserXpasstaxo { get; set; }

        public int? UserSequentialBilling { get; set; }

        public string UserLogin { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public int UserStatus { get; set; }

        public int? UserProfileid { get; set; }

        public int? UserEstablishmentid { get; set; }

        public int? UserSpecialtyid { get; set; }

        public int? UserCountryid { get; set; }

        public string? UserDescription { get; set; }

        public int? UserVatpercentageid { get; set; }


        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int AppointmentInterval { get; set; }

        public string? WorksDays { get; set; }


        public string ProfileName { get; set; } = null!;
        public string EstablishmentName { get; set; } = null!;
        public string SpecialityName { get; set; } = null!;
        public string CountryName { get; set; } = null!;

        public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipAssistantUsers { get; set; } = new List<AssistantDoctorRelationship>();

        public virtual ICollection<AssistantDoctorRelationship> AssistantDoctorRelationshipDoctorUsers { get; set; } = new List<AssistantDoctorRelationship>();

    

        public virtual Country? UserCountry { get; set; }

        public virtual Establishment? UserEstablishment { get; set; }

        public virtual Profile? UserProfile { get; set; }

        public virtual Specialty? UserSpecialty { get; set; }

        public virtual VatBilling? UserVatpercentage { get; set; }
    }
}
