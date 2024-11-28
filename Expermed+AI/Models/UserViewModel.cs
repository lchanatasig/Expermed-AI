namespace Expermed_AI.Models
{
    public class UserViewModel
    {
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
        public int UserStatus { get; set; }
        public int UserProfileId { get; set; }
        public int UserEstablishmentId { get; set; }
        public int UserSpecialtyId { get; set; }
        public int UserCountryId { get; set; }
        public string UserDescription { get; set; }

        // Propiedad para el nombre del establecimiento
        public string EstablishmentName { get; set; }
    }
}
