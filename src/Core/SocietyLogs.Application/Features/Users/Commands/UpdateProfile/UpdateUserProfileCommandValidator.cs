using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        // Sabit değerleri (Magic Numbers) burada tanımlamak temiz kod kuralıdır.
        private const int MaxNameLength = 50;
        private const int MaxBioLength = 500; // Biyografi sınırı
        private const int MaxProfileImageSize = 2 * 1024 * 1024; // 2 MB
        private const int MaxBannerImageSize = 5 * 1024 * 1024;  // 5 MB

        public UpdateUserProfileCommandValidator()
        {
            // 1. TEMEL METİN KONTROLLERİ
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim alanı boş bırakılamaz.")
                .MaximumLength(MaxNameLength).WithMessage($"İsim en fazla {MaxNameLength} karakter olabilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyisim alanı boş bırakılamaz.")
                .MaximumLength(MaxNameLength).WithMessage($"Soyisim en fazla {MaxNameLength} karakter olabilir.");

            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Ünvan en fazla 100 karakter olabilir.");

            RuleFor(x => x.Bio)
                .MaximumLength(MaxBioLength).WithMessage($"Biyografi alanı {MaxBioLength} karakteri geçemez.");

            // 2. TARİH VE ENUM KONTROLLERİ
            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow).WithMessage("Doğum tarihi gelecekte olamaz.")
                .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Geçerli bir doğum tarihi giriniz."); // Mantıksız tarihleri engelle

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Lütfen geçerli bir cinsiyet seçiniz.");

            // 3. DOSYA (RESİM) KONTROLLERİ - KRİTİK BÖLÜM 🚨

            // Profil Resmi Kontrolü
            RuleFor(x => x.ProfileFile)
                // Dosya zorunlu değil (null olabilir), ama varsa kurallara uymalı
                .Must(file => file == null || file.Length <= MaxProfileImageSize)
                .WithMessage("Profil resmi 2MB boyutundan büyük olamaz.")

                .Must(file => file == null || file.ContentType.StartsWith("image/"))
                .WithMessage("Profil resmi için sadece resim dosyaları (JPG, PNG vb.) yüklenebilir.");

            // Banner Resmi Kontrolü (Boyut sınırı farklı olabilir)
            RuleFor(x => x.BannerFile)
                .Must(file => file == null || file.Length <= MaxBannerImageSize)
                .WithMessage("Kapak fotoğrafı 5MB boyutundan büyük olamaz.")

                .Must(file => file == null || file.ContentType.StartsWith("image/"))
                .WithMessage("Kapak fotoğrafı sadece resim formatında olabilir.");
        }
    }
}
