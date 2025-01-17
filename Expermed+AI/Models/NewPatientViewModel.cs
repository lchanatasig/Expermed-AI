﻿namespace Expermed_AI.Models
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
        public List<Catalog> AllergiesTypes { get; set; }
        public List<Catalog> SurgeriesTypes { get; set; }
        public List<Catalog> FamilyMember { get; set; }
        public List<Country> Countries { get; set; }
        public List<Catalog> Parents { get; set; }
        public List<Province> Provinces { get; set; }

        public Patient Patient { get; set; }  // For user details

        public DetailsPatientConsult DetailsPatient { get; set; }




        public virtual Catalog? PatientBloodtypeNavigation { get; set; }

        public virtual User? PatientCreationuserNavigation { get; set; }

        public virtual Catalog? PatientDocumenttypeNavigation { get; set; }

        public virtual Catalog? PatientGenderNavigation { get; set; }

        public virtual Catalog? PatientHealthInsuranceNavigation { get; set; }

        public virtual Catalog? PatientMaritalstatusNavigation { get; set; }

        public virtual User? PatientModificationuserNavigation { get; set; }

        public virtual Country? PatientNationalityNavigation { get; set; }

        public virtual Province? PatientProvinceNavigation { get; set; }

        public virtual Catalog? PatientVocationalTrainingNavigation { get; set; }

    }
}
