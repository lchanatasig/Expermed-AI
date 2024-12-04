namespace Expermed_AI.Models
{
    public class NewUserViewModel
    {
        public List<Profile> Profiles { get; set; }
        public List<Specialty> Specialties { get; set; }
        public List<Establishment> Establishments { get; set; }
        public List<User> Users { get; set; }
        public List<Country> Countries { get; set; }
        public List<VatBilling> VatBillings { get; set; }

        public User User { get; set; }  // For user details
        public UserViewModel Uservm { get; set; }  // For user details

    }
}
