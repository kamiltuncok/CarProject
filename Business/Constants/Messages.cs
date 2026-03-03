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
        public static string CarNameInvalid = "Araba Ýsmi 2 Karakterden Büyük Olmalý ve Günlük Fiyat 0'dan Büyük olmalýdýr";
        internal static string CarsListed = "Arabalar Listelendi";
        internal static string MaintenanceTime = "Sistem Bakýmdadýr";
        public static string CustomerAdded = "Müþteri Eklendi";
        public static string CustomerDeleted = "Müþteri Silindi";
        public static string CustomerUpdated = "Müþteri Güncellendi";
        internal static string CustomerListed = "Müþteri Listelendi";
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
        internal static string UserNotFound = "Kullanýcý Bulunamadý";
        internal static string UserRegistered = "Kullanýcý Kayýt Oldu";
        internal static string PasswordError = "Parola Hatasý";
        internal static string SuccessfulLogin = "Baþarýlý Giriþ";
        internal static string UserAlreadyExists = "Bu Kullanýcý Zaten Mevcut";
        internal static string AccessTokenCreated = "Giriþ Baþarýlý";
        internal static string CarImageLimitExceeded = "Araba Resim Ekleme Limiti Aþýldý";
        internal static readonly string carDetailsListed = "Araba Detaylarý Listelendi";
        internal static readonly string rentalDetailsListed = "Kiralama Detaylarý Listelendi";

        public static string UserPasswordUpdated = "Kullanýcý Þifresi Güncellendi";
        public static string UserDeleted = "Kullanýcý Silindi";
        public static string UserUpdated = "Kullanýcý Güncellendi";
        public static string FuelAdded = "Yakýt Eklendi";
        public static string FuelUpdated = "Yakýt Güncellendi";
        public static string FuelDeleted = "Yakýt Silindi";
        public static string FuelListed = "Yakýtlar Listelendi";

        public static string GearAdded = "Vites Eklendi";
        public static string GearUpdated = "Vites Güncellendi";
        public static string GearDeleted = "Vites Silindi";
        public static string GearListed = "Vitesler Listelendi";

        public static string SegmentAdded = "Segment Eklendi";
        public static string SegmentUpdated = "Segment Güncellendi";
        public static string SegmentDeleted = "Segment Silindi";
        public static string SegmentListed = "Segmentler Listelendi";

        public static string LocationAdded = "Lokasyon Eklendi";
        public static string LocationUpdated = "Lokasyon Güncellendi";
        public static string LocationDeleted = "Lokasyon Silindi";
        public static string LocationListed = "Lokasyonlar Listelendi";

        public static string LocationCityAdded = "Þehir Eklendi";
        public static string LocationCityUpdated = "Þehir Güncellendi";
        public static string LocationCityDeleted = "Þehir Silindi";
        public static string LocationCityListed = "Þehirler Listelendi";


    }

}

