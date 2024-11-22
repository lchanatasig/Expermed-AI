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

    public virtual DbSet<AssistantDoctorRelationship> AssistantDoctorRelationships { get; set; }

    public virtual DbSet<Catalog> Catalogs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Establishment> Establishments { get; set; }

    public virtual DbSet<Loginaudit> Loginaudits { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<TokenSession> TokenSessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ExpermedBDAI;User Id=sa;Password=1717;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.UserDigitalsignature).HasColumnName("user_digitalsignature");
            entity.Property(e => e.UserDocumentNumber)
                .HasMaxLength(13)
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
            entity.Property(e => e.UserQrcode).HasColumnName("user_qrcode");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}