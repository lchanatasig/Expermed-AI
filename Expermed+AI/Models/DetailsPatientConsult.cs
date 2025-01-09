namespace Expermed_AI.Models
{
    public class DetailsPatientConsult
    {

        public int PatientId { get; set; }
        public DateTime PatientCreationdate { get; set; }
        public DateTime PatientModificationdate { get; set; }
        public int PatientCreationuser { get; set; }
        public int PatientModificationuser { get; set; }
        public int PatientDocumenttype { get; set; }
        public string PatientDocumentnumber { get; set; }
        public string PatientFirstname { get; set; }
        public string PatientMiddlename { get; set; }
        public string PatientFirstsurname { get; set; }
        public string PatientSecondlastname { get; set; }
        public int? PatientGender { get; set; }
        public string PatientGenderName { get; set; } // Nuevo campo
        public DateOnly? PatientBirthdate { get; set; }
        public int PatientAge { get; set; }
        public int? PatientBloodtype { get; set; }
        public string PatientBloodtypeName { get; set; } // Nuevo campo
        public string PatientDonor { get; set; }
        public int? PatientMaritalstatus { get; set; }
        public string PatientMaritalstatusName { get; set; } // Nuevo campo
        public int? PatientVocationalTraining { get; set; }
        public string PatientVocationalTrainingName { get; set; } // Nuevo campo
        public string PatientLandlinePhone { get; set; }
        public string PatientCellularPhone { get; set; }
        public string PatientEmail { get; set; }
        public int? PatientNationality { get; set; }
        public string PatientNationalityName { get; set; } // Nuevo campo
        public int? PatientProvince { get; set; }
        public string PatientProvinceName { get; set; } // Nuevo campo
        public string PatientAddress { get; set; }
        public string PatientOcupation { get; set; }
        public string PatientCompany { get; set; }
        public int? PatientHealthInsurance { get; set; }
        public string PatientHealthInsuranceName { get; set; } // Nuevo campo
        public string PatientCode { get; set; }
        public int PatientStatus { get; set; }
    }
}
