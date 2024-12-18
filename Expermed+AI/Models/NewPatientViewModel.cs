namespace Expermed_AI.Models
{
    public class NewPatientViewModel
    {
        public List<Catalog> GenderTypes { get; set; }
        public List<Catalog> BloodTypes { get; set; }
        public List<Catalog> DocumentTypes { get; set; }
        public List<Catalog> CivilTypes { get; set; }
        public List<Catalog> ProfessionalTrainingTypes { get; set; }
        public List<Catalog> SureHealthTypes { get; set; }
        public List<Catalog> RelationshipTypes { get; set; }
        public List<Catalog> FamiliarTypes { get; set; }
        public List<Catalog> AllergiesTypes { get; set; }
        public List<Catalog> SurgeriesTypes { get; set; }
        public List<Country> Countries { get; set; }
        public List<Province> Provinces { get; set; }

    }
}
