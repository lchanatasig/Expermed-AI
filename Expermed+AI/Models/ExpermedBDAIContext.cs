﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Expermed_AI.Models;

public partial class ExpermedBDAIContext : DbContext
{
    public ExpermedBDAIContext()
    {
    }

    public ExpermedBDAIContext(DbContextOptions<ExpermedBDAIContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllergiesConsultation> AllergiesConsultations { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AssistantDoctorRelationship> AssistantDoctorRelationships { get; set; }

    public virtual DbSet<Catalog> Catalogs { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<DiagnosisConsultation> DiagnosisConsultations { get; set; }

    public virtual DbSet<Establishment> Establishments { get; set; }

    public virtual DbSet<FamiliaryBackground> FamiliaryBackgrounds { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImagesConsultation> ImagesConsultations { get; set; }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<LaboratoryConsultation> LaboratoryConsultations { get; set; }

    public virtual DbSet<Loginaudit> Loginaudits { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<MedicationsConsultation> MedicationsConsultations { get; set; }

    public virtual DbSet<OrgansSystem> OrgansSystems { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PhysicalExamination> PhysicalExaminations { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<SurgerisConsultation> SurgerisConsultations { get; set; }

    public virtual DbSet<TokenSession> TokenSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSchedule> UserSchedules { get; set; }

    public virtual DbSet<VatBilling> VatBillings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ExpermedBDAI;User Id=sa;Password=1717;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllergiesConsultation>(entity =>
        {
            entity.HasKey(e => e.AllergiesId).HasName("PK__allergie__1079FBD04888648F");

            entity.ToTable("allergies_consultation");

            entity.Property(e => e.AllergiesId).HasColumnName("allergies_id");
            entity.Property(e => e.AllergiesCatalogid).HasColumnName("allergies_catalogid");
            entity.Property(e => e.AllergiesConsultationid).HasColumnName("allergies_consultationid");
            entity.Property(e => e.AllergiesCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("allergies_creationdate");
            entity.Property(e => e.AllergiesObservation).HasColumnName("allergies_observation");
            entity.Property(e => e.AllergiesStatus)
                .HasDefaultValue(1)
                .HasColumnName("allergies_status");

            entity.HasOne(d => d.AllergiesCatalog).WithMany(p => p.AllergiesConsultations)
                .HasForeignKey(d => d.AllergiesCatalogid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_allergies_catalog");

            entity.HasOne(d => d.AllergiesConsultationNavigation).WithMany(p => p.AllergiesConsultations)
                .HasForeignKey(d => d.AllergiesConsultationid)
                .HasConstraintName("FK_allergies_consultation");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__A50828FCF29C0F1B");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentConsultationid).HasColumnName("appointment_consultationid");
            entity.Property(e => e.AppointmentCreatedate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("appointment_createdate");
            entity.Property(e => e.AppointmentCreateuser).HasColumnName("appointment_createuser");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointment_date");
            entity.Property(e => e.AppointmentHour).HasColumnName("appointment_hour");
            entity.Property(e => e.AppointmentModifydate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("appointment_modifydate");
            entity.Property(e => e.AppointmentModifyuser).HasColumnName("appointment_modifyuser");
            entity.Property(e => e.AppointmentPatientid).HasColumnName("appointment_patientid");
            entity.Property(e => e.AppointmentStatus)
                .HasDefaultValue(1)
                .HasColumnName("appointment_status");

            entity.HasOne(d => d.AppointmentConsultation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.AppointmentConsultationid)
                .HasConstraintName("FK_appointment_consultation");

            entity.HasOne(d => d.AppointmentCreateuserNavigation).WithMany(p => p.AppointmentAppointmentCreateuserNavigations)
                .HasForeignKey(d => d.AppointmentCreateuser)
                .HasConstraintName("FK_appointment_createuser");

            entity.HasOne(d => d.AppointmentModifyuserNavigation).WithMany(p => p.AppointmentAppointmentModifyuserNavigations)
                .HasForeignKey(d => d.AppointmentModifyuser)
                .HasConstraintName("FK_appointment_modifyuser");

            entity.HasOne(d => d.AppointmentPatient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.AppointmentPatientid)
                .HasConstraintName("FK_appointment_patientid");
        });

        modelBuilder.Entity<AssistantDoctorRelationship>(entity =>
        {
            entity.HasKey(e => e.AssistandoctorId).HasName("PK__assistan__017A7BA9D633C781");

            entity.ToTable("assistant_doctor_relationship");

            entity.Property(e => e.AssistandoctorId).HasColumnName("assistandoctor_id");
            entity.Property(e => e.AssistandoctorDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("assistandoctor_date");
            entity.Property(e => e.AssistantUserid).HasColumnName("assistant_userid");
            entity.Property(e => e.DoctorUserid).HasColumnName("doctor_userid");
            entity.Property(e => e.RelationshipStatus)
                .HasDefaultValue(1)
                .HasColumnName("relationship_status");

            entity.HasOne(d => d.AssistantUser).WithMany(p => p.AssistantDoctorRelationshipAssistantUsers)
                .HasForeignKey(d => d.AssistantUserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assistant_User");

            entity.HasOne(d => d.DoctorUser).WithMany(p => p.AssistantDoctorRelationshipDoctorUsers)
                .HasForeignKey(d => d.DoctorUserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctor_User");
        });

        modelBuilder.Entity<Catalog>(entity =>
        {
            entity.HasKey(e => e.CatalogId).HasName("PK__catalogs__9871D9509FC52A1F");

            entity.ToTable("catalogs");

            entity.Property(e => e.CatalogId).HasColumnName("catalog_id");
            entity.Property(e => e.CatalogCategory)
                .HasMaxLength(255)
                .HasColumnName("catalog_category");
            entity.Property(e => e.CatalogName)
                .HasMaxLength(255)
                .HasColumnName("catalog_name");
            entity.Property(e => e.CategoryStatus)
                .HasDefaultValue(1)
                .HasColumnName("category_status");
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("PK__Consulta__650FE0FB0CBDA109");

            entity.ToTable("Consultation");

            entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
            entity.Property(e => e.ConsultationBloodpressuresd)
                .HasMaxLength(10)
                .HasColumnName("consultation_bloodpressuresd");
            entity.Property(e => e.ConsultationCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("consultation_creationdate");
            entity.Property(e => e.ConsultationDisabilitydays).HasColumnName("consultation_disabilitydays");
            entity.Property(e => e.ConsultationDisease).HasColumnName("consultation_disease");
            entity.Property(e => e.ConsultationEvolutionnotes).HasColumnName("consultation_evolutionnotes");
            entity.Property(e => e.ConsultationHistoryclinic)
                .HasMaxLength(255)
                .HasColumnName("consultation_historyclinic");
            entity.Property(e => e.ConsultationNonpharmacologycal).HasColumnName("consultation_nonpharmacologycal");
            entity.Property(e => e.ConsultationObservation).HasColumnName("consultation_observation");
            entity.Property(e => e.ConsultationPatient).HasColumnName("consultation_patient");
            entity.Property(e => e.ConsultationPersonalbackground).HasColumnName("consultation_personalbackground");
            entity.Property(e => e.ConsultationPulse)
                .HasMaxLength(3)
                .HasColumnName("consultation_pulse");
            entity.Property(e => e.ConsultationReason)
                .HasMaxLength(255)
                .HasColumnName("consultation_reason");
            entity.Property(e => e.ConsultationReasonphone)
                .HasMaxLength(50)
                .HasColumnName("consultation_reasonphone");
            entity.Property(e => e.ConsultationReasontype).HasColumnName("consultation_reasontype");
            entity.Property(e => e.ConsultationRelativename).HasColumnName("consultation_relativename");
            entity.Property(e => e.ConsultationRespiratoryrate)
                .HasMaxLength(10)
                .HasColumnName("consultation_respiratoryrate");
            entity.Property(e => e.ConsultationSequential).HasColumnName("consultation_sequential");
            entity.Property(e => e.ConsultationSize)
                .HasMaxLength(3)
                .HasColumnName("consultation_size");
            entity.Property(e => e.ConsultationSpeciality).HasColumnName("consultation_speciality");
            entity.Property(e => e.ConsultationStatus)
                .HasDefaultValue(1)
                .HasColumnName("consultation_status");
            entity.Property(e => e.ConsultationTemperature)
                .HasMaxLength(10)
                .HasColumnName("consultation_temperature");
            entity.Property(e => e.ConsultationTreatmentplan).HasColumnName("consultation_treatmentplan");
            entity.Property(e => e.ConsultationType).HasColumnName("consultation_type");
            entity.Property(e => e.ConsultationUsercreate).HasColumnName("consultation_usercreate");
            entity.Property(e => e.ConsultationWarningsings).HasColumnName("consultation_warningsings");
            entity.Property(e => e.ConsultationWeight)
                .HasMaxLength(3)
                .HasColumnName("consultation_weight");

            entity.HasOne(d => d.ConsultationPatientNavigation).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.ConsultationPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_consultation_patient");

            entity.HasOne(d => d.ConsultationSpecialityNavigation).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.ConsultationSpeciality)
                .HasConstraintName("FK_consultation_speciality");

            entity.HasOne(d => d.ConsultationUsercreateNavigation).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.ConsultationUsercreate)
                .HasConstraintName("FK_consultation_usercreate");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__countrie__7E8CD0556D2BF661");

            entity.ToTable("countries");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(6)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryIso)
                .HasMaxLength(6)
                .HasColumnName("country_iso");
            entity.Property(e => e.CountryName)
                .HasMaxLength(250)
                .HasColumnName("country_name");
            entity.Property(e => e.CountryNationality)
                .HasMaxLength(250)
                .HasColumnName("country_nationality");
            entity.Property(e => e.CountryStatus)
                .HasDefaultValue(1)
                .HasColumnName("country_status");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__diagnosi__D49E32B40F861FAA");

            entity.ToTable("diagnosis");

            entity.Property(e => e.DiagnosisId).HasColumnName("diagnosis_id");
            entity.Property(e => e.DiagnosisCategory)
                .HasMaxLength(255)
                .HasColumnName("diagnosis_category");
            entity.Property(e => e.DiagnosisCie10)
                .HasMaxLength(20)
                .HasColumnName("diagnosis_cie10");
            entity.Property(e => e.DiagnosisDescription)
                .HasMaxLength(255)
                .HasColumnName("diagnosis_description");
            entity.Property(e => e.DiagnosisName)
                .HasMaxLength(255)
                .HasColumnName("diagnosis_name");
            entity.Property(e => e.DiagnosisStatus)
                .HasDefaultValue(1)
                .HasColumnName("diagnosis_status");
        });

        modelBuilder.Entity<DiagnosisConsultation>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__diagnosi__D49E32B4844906F5");

            entity.ToTable("diagnosis_consultation");

            entity.Property(e => e.DiagnosisId).HasColumnName("diagnosis_id");
            entity.Property(e => e.DiagnosisConsultationid).HasColumnName("diagnosis_consultationid");
            entity.Property(e => e.DiagnosisDefinitive).HasColumnName("diagnosis_definitive");
            entity.Property(e => e.DiagnosisDiagnosisid).HasColumnName("diagnosis_diagnosisid");
            entity.Property(e => e.DiagnosisObservation)
                .HasMaxLength(255)
                .HasColumnName("diagnosis_observation");
            entity.Property(e => e.DiagnosisPresumptive).HasColumnName("diagnosis_presumptive");
            entity.Property(e => e.DiagnosisSequential).HasColumnName("diagnosis_sequential");
            entity.Property(e => e.DiagnosisStatus)
                .HasDefaultValue(1)
                .HasColumnName("diagnosis_status");

            entity.HasOne(d => d.DiagnosisConsultationNavigation).WithMany(p => p.DiagnosisConsultations)
                .HasForeignKey(d => d.DiagnosisConsultationid)
                .HasConstraintName("FK_diagnosis_consultation");

            entity.HasOne(d => d.DiagnosisDiagnosis).WithMany(p => p.DiagnosisConsultations)
                .HasForeignKey(d => d.DiagnosisDiagnosisid)
                .HasConstraintName("FK_diagnosis_diagnosis");
        });

        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.HasKey(e => e.EstablishmentId).HasName("PK__establis__95F24A08F95ADDE6");

            entity.ToTable("establishments");

            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.EstablishmentAddress)
                .HasMaxLength(500)
                .HasColumnName("establishment_address");
            entity.Property(e => e.EstablishmentCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("establishment_creationdate");
            entity.Property(e => e.EstablishmentEmissionpoint)
                .HasMaxLength(4)
                .HasColumnName("establishment_emissionpoint");
            entity.Property(e => e.EstablishmentLocality)
                .HasMaxLength(250)
                .HasColumnName("establishment_locality");
            entity.Property(e => e.EstablishmentName)
                .HasMaxLength(250)
                .HasColumnName("establishment_name");
            entity.Property(e => e.EstablishmentPointofsale)
                .HasMaxLength(4)
                .HasColumnName("establishment_pointofsale");
            entity.Property(e => e.EstablishmentStatus)
                .HasDefaultValue(1)
                .HasColumnName("establishment_status");
            entity.Property(e => e.EstablishmentUpdatedate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("establishment_updatedate");
        });

        modelBuilder.Entity<FamiliaryBackground>(entity =>
        {
            entity.HasKey(e => e.FamiliarybackgroundId).HasName("PK__familiar__64008C9F5F6F9706");

            entity.ToTable("familiary_background");

            entity.Property(e => e.FamiliarybackgroundId).HasColumnName("familiarybackground_id");
            entity.Property(e => e.FamiliarybackgroundCancer).HasColumnName("familiarybackground_cancer");
            entity.Property(e => e.FamiliarybackgroundCancerObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_cancer_obs");
            entity.Property(e => e.FamiliarybackgroundConsultationid).HasColumnName("familiarybackground_consultationid");
            entity.Property(e => e.FamiliarybackgroundDiabetes).HasColumnName("familiarybackground_diabetes");
            entity.Property(e => e.FamiliarybackgroundDiabetesObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_diabetes_obs");
            entity.Property(e => e.FamiliarybackgroundDxcardiovascular).HasColumnName("familiarybackground_dxcardiovascular");
            entity.Property(e => e.FamiliarybackgroundDxcardiovascularObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_dxcardiovascular_obs");
            entity.Property(e => e.FamiliarybackgroundDxinfectious).HasColumnName("familiarybackground_dxinfectious");
            entity.Property(e => e.FamiliarybackgroundDxinfectiousObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_dxinfectious_obs");
            entity.Property(e => e.FamiliarybackgroundDxmental).HasColumnName("familiarybackground_dxmental");
            entity.Property(e => e.FamiliarybackgroundDxmentalObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_dxmental_obs");
            entity.Property(e => e.FamiliarybackgroundHeartdisease).HasColumnName("familiarybackground_heartdisease");
            entity.Property(e => e.FamiliarybackgroundHeartdiseaseObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_heartdisease_obs");
            entity.Property(e => e.FamiliarybackgroundHypertension).HasColumnName("familiarybackground_hypertension");
            entity.Property(e => e.FamiliarybackgroundHypertensionObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_hypertension_obs");
            entity.Property(e => e.FamiliarybackgroundMalformation).HasColumnName("familiarybackground_malformation");
            entity.Property(e => e.FamiliarybackgroundMalformationObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_malformation_obs");
            entity.Property(e => e.FamiliarybackgroundOther).HasColumnName("familiarybackground_other");
            entity.Property(e => e.FamiliarybackgroundOtherObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_other_obs");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogCancer).HasColumnName("familiarybackground_relatshcatalog_cancer");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogDiabetes).HasColumnName("familiarybackground_relatshcatalog_diabetes");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogDxcardiovascular).HasColumnName("familiarybackground_relatshcatalog_dxcardiovascular");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogDxinfectious).HasColumnName("familiarybackground_relatshcatalog_dxinfectious");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogDxmental).HasColumnName("familiarybackground_relatshcatalog_dxmental");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogHeartdisease).HasColumnName("familiarybackground_relatshcatalog_heartdisease");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogHypertension).HasColumnName("familiarybackground_relatshcatalog_hypertension");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogMalformation).HasColumnName("familiarybackground_relatshcatalog_malformation");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogOther).HasColumnName("familiarybackground_relatshcatalog_other");
            entity.Property(e => e.FamiliarybackgroundRelatshcatalogTuberculosis).HasColumnName("familiarybackground_relatshcatalog_tuberculosis");
            entity.Property(e => e.FamiliarybackgroundTuberculosis).HasColumnName("familiarybackground_tuberculosis");
            entity.Property(e => e.FamiliarybackgroundTuberculosisObs)
                .HasMaxLength(255)
                .HasColumnName("familiarybackground_tuberculosis_obs");

            entity.HasOne(d => d.FamiliarybackgroundConsultation).WithMany(p => p.FamiliaryBackgrounds)
                .HasForeignKey(d => d.FamiliarybackgroundConsultationid)
                .HasConstraintName("FK_familiarybackground_consultation");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogCancerNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogCancerNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogCancer)
                .HasConstraintName("FK_familiarybackgroundcancer_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogDiabetesNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDiabetesNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogDiabetes)
                .HasConstraintName("FK_familiarybackgrounddiabetes_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogDxcardiovascularNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxcardiovascularNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogDxcardiovascular)
                .HasConstraintName("FK_familiarybackgroundcardiovascular_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogDxinfectiousNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxinfectiousNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogDxinfectious)
                .HasConstraintName("FK_familiarybackgroundinfectious_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogDxmentalNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogDxmentalNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogDxmental)
                .HasConstraintName("FK_familiarybackgroundmental_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogHeartdiseaseNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogHeartdiseaseNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogHeartdisease)
                .HasConstraintName("FK_familiarybackgroundheart_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogHypertensionNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogHypertensionNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogHypertension)
                .HasConstraintName("FK_familiarybackgroundhypertension_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogMalformationNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogMalformationNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogMalformation)
                .HasConstraintName("FK_familiarybackgroundmalformation_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogOtherNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogOtherNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogOther)
                .HasConstraintName("FK_familiarybackgroundother_catalog");

            entity.HasOne(d => d.FamiliarybackgroundRelatshcatalogTuberculosisNavigation).WithMany(p => p.FamiliaryBackgroundFamiliarybackgroundRelatshcatalogTuberculosisNavigations)
                .HasForeignKey(d => d.FamiliarybackgroundRelatshcatalogTuberculosis)
                .HasConstraintName("FK_familiarybackgroundtuberculosis_catalog");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImagesId).HasName("PK__images__FA2651F764857548");

            entity.ToTable("images");

            entity.Property(e => e.ImagesId).HasColumnName("images_id");
            entity.Property(e => e.ImagesCategory)
                .HasMaxLength(255)
                .HasColumnName("images_category");
            entity.Property(e => e.ImagesCie10)
                .HasMaxLength(20)
                .HasColumnName("images_cie10");
            entity.Property(e => e.ImagesDescription)
                .HasMaxLength(255)
                .HasColumnName("images_description");
            entity.Property(e => e.ImagesName)
                .HasMaxLength(255)
                .HasColumnName("images_name");
            entity.Property(e => e.ImagesStatus)
                .HasDefaultValue(1)
                .HasColumnName("images_status");
        });

        modelBuilder.Entity<ImagesConsultation>(entity =>
        {
            entity.HasKey(e => e.ImagesId).HasName("PK__images_c__FA2651F727019D81");

            entity.ToTable("images_consultation");

            entity.Property(e => e.ImagesId).HasColumnName("images_id");
            entity.Property(e => e.ImagesAmount)
                .HasMaxLength(255)
                .HasColumnName("images_amount");
            entity.Property(e => e.ImagesConsultationsid).HasColumnName("images_consultationsid");
            entity.Property(e => e.ImagesImagesid).HasColumnName("images_imagesid");
            entity.Property(e => e.ImagesObservation)
                .HasMaxLength(255)
                .HasColumnName("images_observation");
            entity.Property(e => e.ImagesSequential).HasColumnName("images_sequential");
            entity.Property(e => e.ImagesStatus)
                .HasDefaultValue(1)
                .HasColumnName("images_status");

            entity.HasOne(d => d.ImagesConsultations).WithMany(p => p.ImagesConsultations)
                .HasForeignKey(d => d.ImagesConsultationsid)
                .HasConstraintName("FK_images_consultation");

            entity.HasOne(d => d.ImagesImages).WithMany(p => p.ImagesConsultations)
                .HasForeignKey(d => d.ImagesImagesid)
                .HasConstraintName("FK_images_images");
        });

        modelBuilder.Entity<Laboratory>(entity =>
        {
            entity.HasKey(e => e.LaboratoriesId).HasName("PK__laborato__949BB039678F1C75");

            entity.ToTable("laboratories");

            entity.Property(e => e.LaboratoriesId).HasColumnName("laboratories_id");
            entity.Property(e => e.LaboratoriesCategory)
                .HasMaxLength(255)
                .HasColumnName("laboratories_category");
            entity.Property(e => e.LaboratoriesCie10)
                .HasMaxLength(20)
                .HasColumnName("laboratories_cie10");
            entity.Property(e => e.LaboratoriesDescription)
                .HasMaxLength(255)
                .HasColumnName("laboratories_description");
            entity.Property(e => e.LaboratoriesName)
                .HasMaxLength(255)
                .HasColumnName("laboratories_name");
            entity.Property(e => e.LaboratoriesStatus)
                .HasDefaultValue(1)
                .HasColumnName("laboratories_status");
        });

        modelBuilder.Entity<LaboratoryConsultation>(entity =>
        {
            entity.HasKey(e => e.LaboratoryId).HasName("PK__laborato__3A591C74B79585C5");

            entity.ToTable("laboratory_consultation");

            entity.Property(e => e.LaboratoryId).HasColumnName("laboratory_id");
            entity.Property(e => e.LaboatoryStatus)
                .HasDefaultValue(1)
                .HasColumnName("laboatory_status");
            entity.Property(e => e.LaboratoryAmount)
                .HasMaxLength(255)
                .HasColumnName("laboratory_amount");
            entity.Property(e => e.LaboratoryConsultationid).HasColumnName("laboratory_consultationid");
            entity.Property(e => e.LaboratoryLaboratoryid).HasColumnName("laboratory_laboratoryid");
            entity.Property(e => e.LaboratoryObservation)
                .HasMaxLength(255)
                .HasColumnName("laboratory_observation");
            entity.Property(e => e.LaboratorySequential).HasColumnName("laboratory_sequential");

            entity.HasOne(d => d.LaboratoryLaboratory).WithMany(p => p.LaboratoryConsultations)
                .HasForeignKey(d => d.LaboratoryLaboratoryid)
                .HasConstraintName("FK_laboratory_consultation");

            entity.HasOne(d => d.LaboratoryLaboratoryNavigation).WithMany(p => p.LaboratoryConsultations)
                .HasForeignKey(d => d.LaboratoryLaboratoryid)
                .HasConstraintName("FK_laboratory_laboratory");
        });

        modelBuilder.Entity<Loginaudit>(entity =>
        {
            entity.HasKey(e => e.LoginauditId).HasName("PK__loginaud__C1371996B878F93A");

            entity.ToTable("loginaudit");

            entity.Property(e => e.LoginauditId).HasColumnName("loginaudit_id");
            entity.Property(e => e.LoginauditAddresip)
                .HasMaxLength(255)
                .HasColumnName("loginaudit_addresip");
            entity.Property(e => e.LoginauditDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("loginaudit_date");
            entity.Property(e => e.LoginauditMessage)
                .HasMaxLength(255)
                .HasColumnName("loginaudit_message");
            entity.Property(e => e.LoginauditSuccess).HasColumnName("loginaudit_success");
            entity.Property(e => e.LoginauditUserid).HasColumnName("loginaudit_userid");

            entity.HasOne(d => d.LoginauditUser).WithMany(p => p.Loginaudits)
                .HasForeignKey(d => d.LoginauditUserid)
                .HasConstraintName("FK_LoginAudit_User_Id");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.MedicationsId).HasName("PK__medicati__CF638DC5FEDD3852");

            entity.ToTable("medications");

            entity.Property(e => e.MedicationsId).HasColumnName("medications_id");
            entity.Property(e => e.MedicationsCategory)
                .HasMaxLength(255)
                .HasColumnName("medications_category");
            entity.Property(e => e.MedicationsCie10)
                .HasMaxLength(50)
                .HasColumnName("medications_cie10");
            entity.Property(e => e.MedicationsConcentration)
                .HasMaxLength(255)
                .HasColumnName("medications_concentration");
            entity.Property(e => e.MedicationsDescription)
                .HasMaxLength(255)
                .HasColumnName("medications_description");
            entity.Property(e => e.MedicationsDistinctive)
                .HasMaxLength(255)
                .HasColumnName("medications_distinctive");
            entity.Property(e => e.MedicationsName)
                .HasMaxLength(255)
                .HasColumnName("medications_name");
            entity.Property(e => e.MedicationsStatus)
                .HasDefaultValue(1)
                .HasColumnName("medications_status");
        });

        modelBuilder.Entity<MedicationsConsultation>(entity =>
        {
            entity.HasKey(e => e.MedicationsId).HasName("PK__medicati__CF638DC5C7DF967E");

            entity.ToTable("medications_consultation");

            entity.Property(e => e.MedicationsId).HasColumnName("medications_id");
            entity.Property(e => e.MedicationsAmount)
                .HasMaxLength(255)
                .HasColumnName("medications_amount");
            entity.Property(e => e.MedicationsConsultationid).HasColumnName("medications_consultationid");
            entity.Property(e => e.MedicationsMedicationsid).HasColumnName("medications_medicationsid");
            entity.Property(e => e.MedicationsObservation)
                .HasMaxLength(255)
                .HasColumnName("medications_observation");
            entity.Property(e => e.MedicationsSequential).HasColumnName("medications_sequential");
            entity.Property(e => e.MedicationsStatus)
                .HasDefaultValue(1)
                .HasColumnName("medications_status");

            entity.HasOne(d => d.MedicationsConsultationNavigation).WithMany(p => p.MedicationsConsultations)
                .HasForeignKey(d => d.MedicationsConsultationid)
                .HasConstraintName("FK_medications_consultation");

            entity.HasOne(d => d.MedicationsMedications).WithMany(p => p.MedicationsConsultations)
                .HasForeignKey(d => d.MedicationsMedicationsid)
                .HasConstraintName("FK_medications_medications");
        });

        modelBuilder.Entity<OrgansSystem>(entity =>
        {
            entity.HasKey(e => e.OrganssystemsId).HasName("PK__organs_s__FEA205F7AD8B95FB");

            entity.ToTable("organs_systems");

            entity.Property(e => e.OrganssystemsId).HasColumnName("organssystems_id");
            entity.Property(e => e.OrganssystemsCardiovascular).HasColumnName("organssystems_cardiovascular");
            entity.Property(e => e.OrganssystemsCardiovascularObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_cardiovascular_obs");
            entity.Property(e => e.OrganssystemsConsultationsid).HasColumnName("organssystems_consultationsid");
            entity.Property(e => e.OrganssystemsDigestive).HasColumnName("organssystems_digestive");
            entity.Property(e => e.OrganssystemsDigestiveObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_digestive_obs");
            entity.Property(e => e.OrganssystemsEndocrine).HasColumnName("organssystems_endocrine");
            entity.Property(e => e.OrganssystemsEndocrineObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_endocrine_obs");
            entity.Property(e => e.OrganssystemsGenital).HasColumnName("organssystems_genital");
            entity.Property(e => e.OrganssystemsGenitalObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_genital_obs");
            entity.Property(e => e.OrganssystemsLymphatic).HasColumnName("organssystems_lymphatic");
            entity.Property(e => e.OrganssystemsLymphaticObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_lymphatic_obs");
            entity.Property(e => e.OrganssystemsNervous).HasColumnName("organssystems_nervous");
            entity.Property(e => e.OrganssystemsNervousObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_nervous_obs");
            entity.Property(e => e.OrganssystemsRespiratory).HasColumnName("organssystems_respiratory");
            entity.Property(e => e.OrganssystemsRespiratoryObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_respiratory_obs");
            entity.Property(e => e.OrganssystemsSkeletalM).HasColumnName("organssystems_skeletal_m");
            entity.Property(e => e.OrganssystemsSkeletalMObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_skeletal_m_obs");
            entity.Property(e => e.OrganssystemsUrinary).HasColumnName("organssystems_urinary");
            entity.Property(e => e.OrganssystemsUrinaryObs)
                .HasMaxLength(255)
                .HasColumnName("organssystems_urinary_obs");

            entity.HasOne(d => d.OrganssystemsConsultations).WithMany(p => p.OrgansSystems)
                .HasForeignKey(d => d.OrganssystemsConsultationsid)
                .HasConstraintName("FK_organssystems_consultation");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__patient__4D5CE476C6539F4A");

            entity.ToTable("patient");

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.PatientAddress).HasColumnName("patient_address");
            entity.Property(e => e.PatientAge).HasColumnName("patient_age");
            entity.Property(e => e.PatientBirthdate).HasColumnName("patient_birthdate");
            entity.Property(e => e.PatientBloodtype).HasColumnName("patient_bloodtype");
            entity.Property(e => e.PatientCellularPhone)
                .HasMaxLength(255)
                .HasColumnName("patient_cellular_phone");
            entity.Property(e => e.PatientCode)
                .HasMaxLength(5)
                .HasColumnName("patient_code");
            entity.Property(e => e.PatientCompany)
                .HasMaxLength(255)
                .HasColumnName("patient_company");
            entity.Property(e => e.PatientCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("patient_creationdate");
            entity.Property(e => e.PatientCreationuser).HasColumnName("patient_creationuser");
            entity.Property(e => e.PatientDocumentnumber)
                .HasMaxLength(10)
                .HasColumnName("patient_documentnumber");
            entity.Property(e => e.PatientDocumenttype).HasColumnName("patient_documenttype");
            entity.Property(e => e.PatientDonor)
                .HasMaxLength(50)
                .HasColumnName("patient_donor");
            entity.Property(e => e.PatientEmail)
                .HasMaxLength(255)
                .HasColumnName("patient_email");
            entity.Property(e => e.PatientFirstname)
                .HasMaxLength(255)
                .HasColumnName("patient_firstname");
            entity.Property(e => e.PatientFirstsurname)
                .HasMaxLength(255)
                .HasColumnName("patient_firstsurname");
            entity.Property(e => e.PatientGender).HasColumnName("patient_gender");
            entity.Property(e => e.PatientHealthInsurance).HasColumnName("patient_health_insurance");
            entity.Property(e => e.PatientLandlinePhone)
                .HasMaxLength(255)
                .HasColumnName("patient_landline_phone");
            entity.Property(e => e.PatientMaritalstatus).HasColumnName("patient_maritalstatus");
            entity.Property(e => e.PatientMiddlename)
                .HasMaxLength(255)
                .HasColumnName("patient_middlename");
            entity.Property(e => e.PatientModificationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("patient_modificationdate");
            entity.Property(e => e.PatientModificationuser).HasColumnName("patient_modificationuser");
            entity.Property(e => e.PatientNationality).HasColumnName("patient_nationality");
            entity.Property(e => e.PatientOcupation)
                .HasMaxLength(255)
                .HasColumnName("patient_ocupation");
            entity.Property(e => e.PatientProvince).HasColumnName("patient_province");
            entity.Property(e => e.PatientSecondlastname)
                .HasMaxLength(255)
                .HasColumnName("patient_secondlastname");
            entity.Property(e => e.PatientStatus)
                .HasDefaultValue(1)
                .HasColumnName("patient_status");
            entity.Property(e => e.PatientVocationalTraining).HasColumnName("patient_vocational_training");

            entity.HasOne(d => d.PatientBloodtypeNavigation).WithMany(p => p.PatientPatientBloodtypeNavigations)
                .HasForeignKey(d => d.PatientBloodtype)
                .HasConstraintName("FK_patient_bloodtype");

            entity.HasOne(d => d.PatientCreationuserNavigation).WithMany(p => p.PatientPatientCreationuserNavigations)
                .HasForeignKey(d => d.PatientCreationuser)
                .HasConstraintName("FK_patient_creationuser");

            entity.HasOne(d => d.PatientDocumenttypeNavigation).WithMany(p => p.PatientPatientDocumenttypeNavigations)
                .HasForeignKey(d => d.PatientDocumenttype)
                .HasConstraintName("FK_patient_documenttype");

            entity.HasOne(d => d.PatientGenderNavigation).WithMany(p => p.PatientPatientGenderNavigations)
                .HasForeignKey(d => d.PatientGender)
                .HasConstraintName("FK_patient_gender");

            entity.HasOne(d => d.PatientHealthInsuranceNavigation).WithMany(p => p.PatientPatientHealthInsuranceNavigations)
                .HasForeignKey(d => d.PatientHealthInsurance)
                .HasConstraintName("FK_patient_health_insurance");

            entity.HasOne(d => d.PatientMaritalstatusNavigation).WithMany(p => p.PatientPatientMaritalstatusNavigations)
                .HasForeignKey(d => d.PatientMaritalstatus)
                .HasConstraintName("FK_patient_maritalstatus");

            entity.HasOne(d => d.PatientModificationuserNavigation).WithMany(p => p.PatientPatientModificationuserNavigations)
                .HasForeignKey(d => d.PatientModificationuser)
                .HasConstraintName("FK_patient_modificationuser");

            entity.HasOne(d => d.PatientNationalityNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.PatientNationality)
                .HasConstraintName("FK_patient_nationality");

            entity.HasOne(d => d.PatientProvinceNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.PatientProvince)
                .HasConstraintName("FK_patient_province");

            entity.HasOne(d => d.PatientVocationalTrainingNavigation).WithMany(p => p.PatientPatientVocationalTrainingNavigations)
                .HasForeignKey(d => d.PatientVocationalTraining)
                .HasConstraintName("FK_patient_vocational_training");
        });

        modelBuilder.Entity<PhysicalExamination>(entity =>
        {
            entity.HasKey(e => e.PhysicalexaminationId).HasName("PK__physical__3846C7A77EF97FAF");

            entity.ToTable("physical_examination");

            entity.Property(e => e.PhysicalexaminationId).HasColumnName("physicalexamination_id");
            entity.Property(e => e.PhysicalexaminationAbdomen).HasColumnName("physicalexamination_abdomen");
            entity.Property(e => e.PhysicalexaminationAbdomenObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_abdomen_obs");
            entity.Property(e => e.PhysicalexaminationChest).HasColumnName("physicalexamination_chest");
            entity.Property(e => e.PhysicalexaminationChestObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_chest_OBS");
            entity.Property(e => e.PhysicalexaminationConsultationid).HasColumnName("physicalexamination_consultationid");
            entity.Property(e => e.PhysicalexaminationHead).HasColumnName("physicalexamination_head");
            entity.Property(e => e.PhysicalexaminationHeadObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_head_obs");
            entity.Property(e => e.PhysicalexaminationLimbs).HasColumnName("physicalexamination_limbs");
            entity.Property(e => e.PhysicalexaminationLimbsObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_limbs_obs");
            entity.Property(e => e.PhysicalexaminationNeck).HasColumnName("physicalexamination_neck");
            entity.Property(e => e.PhysicalexaminationNeckObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_neck_obs");
            entity.Property(e => e.PhysicalexaminationPelvis).HasColumnName("physicalexamination_pelvis");
            entity.Property(e => e.PhysicalexaminationPelvisObs)
                .HasMaxLength(255)
                .HasColumnName("physicalexamination_pelvis_obs");

            entity.HasOne(d => d.PhysicalexaminationConsultation).WithMany(p => p.PhysicalExaminations)
                .HasForeignKey(d => d.PhysicalexaminationConsultationid)
                .HasConstraintName("FK_physicalexamination_consultation");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__profiles__AEBB701FD79C4EEC");

            entity.ToTable("profiles");

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.ProfileCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("profile_creationdate");
            entity.Property(e => e.ProfileDescription).HasColumnName("profile_description");
            entity.Property(e => e.ProfileName)
                .HasMaxLength(255)
                .HasColumnName("profile_name");
            entity.Property(e => e.ProfileState)
                .HasDefaultValue(1)
                .HasColumnName("profile_state");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__province__08DCB60FB7C4BF60");

            entity.ToTable("provinces");

            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.ProvinceCode)
                .HasMaxLength(4)
                .HasColumnName("province_code");
            entity.Property(e => e.ProvinceCountryid).HasColumnName("province_countryid");
            entity.Property(e => e.ProvinceDemonym)
                .HasMaxLength(255)
                .HasColumnName("province_demonym");
            entity.Property(e => e.ProvinceIso)
                .HasMaxLength(5)
                .HasColumnName("province_iso");
            entity.Property(e => e.ProvinceName)
                .HasMaxLength(255)
                .HasColumnName("province_name");
            entity.Property(e => e.ProvinceStatus)
                .HasDefaultValue(1)
                .HasColumnName("province_status");
            entity.Property(e => e.ProvinvePrefix)
                .HasMaxLength(100)
                .HasColumnName("provinve_prefix");

            entity.HasOne(d => d.ProvinceCountry).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.ProvinceCountryid)
                .HasConstraintName("FK_Province_Country_Id");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialityId).HasName("PK__specialt__E82ED620A9E4313A");

            entity.ToTable("specialties");

            entity.HasIndex(e => e.SpecialityName, "UQ__specialt__C773CF25B029E902").IsUnique();

            entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");
            entity.Property(e => e.SpecialityCategory)
                .HasMaxLength(500)
                .HasColumnName("speciality_category");
            entity.Property(e => e.SpecialityDescription)
                .HasMaxLength(500)
                .HasColumnName("speciality_description");
            entity.Property(e => e.SpecialityName)
                .HasMaxLength(250)
                .HasColumnName("speciality_name");
            entity.Property(e => e.SpecialityStatus)
                .HasDefaultValue(1)
                .HasColumnName("speciality_status");
        });

        modelBuilder.Entity<SurgerisConsultation>(entity =>
        {
            entity.HasKey(e => e.SurgeriesId).HasName("PK__surgeris__0D3E1F77C4F98A42");

            entity.ToTable("surgeris_consultation");

            entity.Property(e => e.SurgeriesId).HasColumnName("surgeries_id");
            entity.Property(e => e.SurgeriesCatalogid).HasColumnName("surgeries_catalogid");
            entity.Property(e => e.SurgeriesConsultationid).HasColumnName("surgeries_consultationid");
            entity.Property(e => e.SurgeriesCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("surgeries_creationdate");
            entity.Property(e => e.SurgeriesObservation).HasColumnName("surgeries_observation");
            entity.Property(e => e.SurgeriesStatus)
                .HasDefaultValue(1)
                .HasColumnName("surgeries_status");

            entity.HasOne(d => d.SurgeriesCatalog).WithMany(p => p.SurgerisConsultations)
                .HasForeignKey(d => d.SurgeriesCatalogid)
                .HasConstraintName("FK_surgeries_catalog");

            entity.HasOne(d => d.SurgeriesConsultation).WithMany(p => p.SurgerisConsultations)
                .HasForeignKey(d => d.SurgeriesConsultationid)
                .HasConstraintName("FK_surgeries_consultation");
        });

        modelBuilder.Entity<TokenSession>(entity =>
        {
            entity.HasKey(e => e.TokensessionId).HasName("PK__token_se__40BA28947DD442FA");

            entity.ToTable("token_session");

            entity.Property(e => e.TokensessionId).HasColumnName("tokensession_id");
            entity.Property(e => e.TokensessionExpirationdate)
                .HasColumnType("datetime")
                .HasColumnName("tokensession_expirationdate");
            entity.Property(e => e.TokensessionUserid).HasColumnName("tokensession_userid");

            entity.HasOne(d => d.TokensessionUser).WithMany(p => p.TokenSessions)
                .HasForeignKey(d => d.TokensessionUserid)
                .HasConstraintName("FK_TokenSession_User_Id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsersId).HasName("PK__users__EAA7D14B57FDDEA5");

            entity.ToTable("users");

            entity.HasIndex(e => e.UserDocumentNumber, "UQ__users__5B58ED0B385B680A").IsUnique();

            entity.HasIndex(e => e.UserLogin, "UQ__users__9EA1B5AF49705C20").IsUnique();

            entity.HasIndex(e => e.UserEmail, "UQ__users__B0FBA212728ED35C").IsUnique();

            entity.Property(e => e.UsersId).HasColumnName("users_id");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(255)
                .HasColumnName("user_address");
            entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");
            entity.Property(e => e.UserCreationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("user_creationdate");
            entity.Property(e => e.UserDescription).HasColumnName("user_description");
            entity.Property(e => e.UserDigitalsignature).HasColumnName("user_digitalsignature");
            entity.Property(e => e.UserDocumentNumber)
                .HasMaxLength(255)
                .HasColumnName("user_document_number");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(250)
                .HasColumnName("user_email");
            entity.Property(e => e.UserEstablishmentid).HasColumnName("user_establishmentid");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(255)
                .HasColumnName("user_login");
            entity.Property(e => e.UserModificationdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("user_modificationdate");
            entity.Property(e => e.UserNames)
                .HasMaxLength(250)
                .HasColumnName("user_names");
            entity.Property(e => e.UserPassword).HasColumnName("user_password");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(250)
                .HasColumnName("user_phone");
            entity.Property(e => e.UserPrfilephoto64).HasColumnName("user_prfilephoto64");
            entity.Property(e => e.UserProfileid).HasColumnName("user_profileid");
            entity.Property(e => e.UserProfilephoto).HasColumnName("user_profilephoto");
            entity.Property(e => e.UserSenecytcode)
                .HasMaxLength(250)
                .HasColumnName("user_senecytcode");
            entity.Property(e => e.UserSequentialBilling).HasColumnName("user_sequential_billing");
            entity.Property(e => e.UserSpecialtyid).HasColumnName("user_specialtyid");
            entity.Property(e => e.UserStatus)
                .HasDefaultValue(1)
                .HasColumnName("user_status");
            entity.Property(e => e.UserSurnames)
                .HasMaxLength(250)
                .HasColumnName("user_surnames");
            entity.Property(e => e.UserVatpercentageid).HasColumnName("user_vatpercentageid");
            entity.Property(e => e.UserXkeytaxo).HasColumnName("user_xkeytaxo");
            entity.Property(e => e.UserXpasstaxo).HasColumnName("user_xpasstaxo");

            entity.HasOne(d => d.UserCountry).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserCountryid)
                .HasConstraintName("FK_User_Country_Id");

            entity.HasOne(d => d.UserEstablishment).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserEstablishmentid)
                .HasConstraintName("FK_User_Establishment_Id");

            entity.HasOne(d => d.UserProfile).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserProfileid)
                .HasConstraintName("FK_User_Profile_Id");

            entity.HasOne(d => d.UserSpecialty).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserSpecialtyid)
                .HasConstraintName("FK_User_Speciality_Id");

            entity.HasOne(d => d.UserVatpercentage).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserVatpercentageid)
                .HasConstraintName("FK_user_vatpercentage");
        });

        modelBuilder.Entity<UserSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__user_sch__C46A8A6F704E3D53");

            entity.ToTable("user_schedules");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.AppointmentInterval).HasColumnName("appointment_interval");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UsersId).HasColumnName("users_id");
            entity.Property(e => e.WorksDays)
                .HasMaxLength(255)
                .HasColumnName("works_days");

            entity.HasOne(d => d.Users).WithMany(p => p.UserSchedules)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_schedules_users");
        });

        modelBuilder.Entity<VatBilling>(entity =>
        {
            entity.HasKey(e => e.VatbillingId).HasName("PK__vat_bill__B6D1E35E191A19E5");

            entity.ToTable("vat_billing");

            entity.Property(e => e.VatbillingId).HasColumnName("vatbilling_id");
            entity.Property(e => e.VatbillingCode)
                .HasMaxLength(4)
                .HasColumnName("vatbilling_code");
            entity.Property(e => e.VatbillingPercentage)
                .HasMaxLength(255)
                .HasColumnName("vatbilling_percentage");
            entity.Property(e => e.VatbillingRate)
                .HasMaxLength(5)
                .HasColumnName("vatbilling_rate");
            entity.Property(e => e.VatbillingStatus)
                .HasDefaultValue(1)
                .HasColumnName("vatbilling_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
