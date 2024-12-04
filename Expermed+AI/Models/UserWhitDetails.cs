namespace Expermed_AI.Models
{
    public class UserWithDetails
    {
        // Propiedades de la tabla Users
        public int UsersId { get; set; }
        public string UserDocumentNumber { get; set; }
        public string UserNames { get; set; }
        public string UserSurnames { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public DateTime UserCreationDate { get; set; }
        public DateTime? UserModificationDate { get; set; }
        public string UserAddress { get; set; }
        public string UserDigitalSignature { get; set; }
        public string UserProfilePhoto { get; set; }
        public string UserProfilePhoto64 { get; set; }
        public string UserSenecytCode { get; set; }
        public string UserXKeyTaxo { get; set; }
        public string UserXPassTaxo { get; set; }
        public string UserSequentialBilling { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public string UserStatus { get; set; }
        public int UserProfileId { get; set; }
        public int UserEstablishmentId { get; set; }
        public int UserVatPercentageId { get; set; }
        public int UserSpecialtyId { get; set; }
        public int UserCountryId { get; set; }
        public string UserDescription { get; set; }

        // Propiedades de las tablas relacionadas
        public string ProfileName { get; set; }
        public string EstablishmentName { get; set; }
        public string SpecialtyName { get; set; }
        public string CountryName { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int AppointmentInterval { get; set; }
        public string WorksDays { get; set; }
    }

}
