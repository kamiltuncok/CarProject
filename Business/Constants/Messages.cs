using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string CarAdded = "Araba Eklendi";
        public static string CarDeleted = "Araba Silindi";
        public static string CarUpdated = "Araba Güncellendi";
        public static string CarNameInvalid = "Araba İsmi 2 Karakterden Büyük Olmalı ve Günlük Fiyat 0'dan Büyük olmalıdır";
        internal static string CarsListed = "Arabalar Listelendi";
        internal static string MaintenanceTime = "Sistem Bakımdadır";
        public static string CustomerAdded = "Müşteri Eklendi";
        public static string CustomerDeleted = "Müşteri Silindi";
        public static string CustomerUpdated = "Müşteri Güncellendi";
        internal static string CustomerListed = "Müşteri Listelendi";
        public static string ColorAdded = "Renk Eklendi";
        public static string ColorDeleted = "Renk Silindi";
        public static string ColorUpdated = "Renk Güncellendi";
        internal static string ColorsListed = "Renkler Listelendi";
        internal static string CarImageDeleted = "Araba Resimleri Silindi";
        internal static string CarImageAdded = "Araba Resimleri Eklendi";
        internal static string CarImageUpdated = "Araba Resimleri Güncellendi";
        internal static string CarImageListed = "Araba Resimleri Listelendi";
        public static string BrandAdded = "Marka Eklendi";
        public static string BrandDeleted = "Marka Silindi";
        public static string BrandUpdated = "Marka Güncellendi";
        internal static string BrandListed = "Markalar Listelendi";
        public static string RentalAdded = "Kiralama Eklendi";
        public static string RentalNotAdded = "Kiralama Eklenemedi";
        public static string RentalDeleted = "Kiralama Silindi";
        public static string RetalUpdated = "Kiralama Güncellendi";
        internal static string RentalListed = "Kiralamalar Listelendi";
        public static string AuthorizationDenied = "Yetkiniz Yok";
        internal static string UserNotFound = "Kullanıcı Bulunamadı";
        internal static string UserRegistered = "Kullanıcı Kayıt Oldu";
        internal static string PasswordError = "Parola Hatası";
        internal static string SuccessfulLogin = "Başarılı Giriş";
        internal static string UserAlreadyExists = "Bu Kullanıcı Zaten Mevcut";
        internal static string AccessTokenCreated = "Erişim Belirteci Oluşturuldu";
        internal static string CarImageLimitExceeded = "Araba Resim Ekleme Limiti Aşıldı";
        internal static readonly string carDetailsListed = "Araba Detayları Listelendi";
        internal static readonly string rentalDetailsListed = "Kiralama Detayları Listelendi";
    }

}
