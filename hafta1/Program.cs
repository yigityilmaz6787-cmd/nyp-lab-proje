using System;
using System.Collections.Generic;

namespace AkilliEvYonetimi
{
    public enum CihazTuru
    {
        Lamba = 1,
        Klima = 2,
        GuvenlikSistemi = 3
    }

    public class Cihaz
    {
        public int Kimlik { get; private set; }
        public string Ad { get; private set; }
        public CihazTuru Tur { get; private set; }
        public bool AcikMi { get; private set; }

        public Cihaz(int kimlik, string ad, CihazTuru tur)
        {
            Kimlik = kimlik;
            Ad = ad;
            Tur = tur;
            AcikMi = false;
        }

        public void Ac()
        {
            if (AcikMi)
            {
                Console.WriteLine($"{Ad} zaten açık.");
                return;
            }

            AcikMi = true;
            Console.WriteLine($"{Ad} açıldı.");
        }

        public void Kapat()
        {
            if (!AcikMi)
            {
                Console.WriteLine($"{Ad} zaten kapalı.");
                return;
            }

            AcikMi = false;
            Console.WriteLine($"{Ad} kapatıldı.");
        }

        public void BilgileriGoster()
        {
            string durum = AcikMi ? "Açık" : "Kapalı";
            Console.WriteLine($"Kimlik: {Kimlik} | Ad: {Ad} | Tür: {Tur} | Durum: {durum}");
        }
    }

    public interface ICihazYonetici
    {
        void CihazlariListele();
        bool CihazEkle(Cihaz yeniCihaz);
        void CihazAc(int kimlik);
        void CihazKapat(int kimlik);
    }

    public class CihazYonetici : ICihazYonetici
    {
        private readonly List<Cihaz> _cihazlar;

        public CihazYonetici(List<Cihaz> cihazlar)
        {
            _cihazlar = cihazlar;
        }

        private Cihaz CihazBul(int kimlik)
        {
            foreach (Cihaz cihaz in _cihazlar)
            {
                if (cihaz.Kimlik == kimlik)
                {
                    return cihaz;
                }
            }
            return null;
        }

        public void CihazlariListele()
        {
            if (_cihazlar.Count == 0)
            {
                Console.WriteLine("Sistemde kayıtlı cihaz bulunmamaktadır.");
                return;
            }

            Console.WriteLine("--- KAYITLI CİHAZLAR ---");
            foreach (Cihaz cihaz in _cihazlar)
            {
                cihaz.BilgileriGoster();
            }
        }

        public bool CihazEkle(Cihaz yeniCihaz)
        {
            if (CihazBul(yeniCihaz.Kimlik) != null)
            {
                Console.WriteLine("Hata: Bu kimliğe sahip bir cihaz zaten mevcut!");
                return false;
            }

            _cihazlar.Add(yeniCihaz);
            Console.WriteLine("Yeni cihaz başarıyla eklendi.");
            return true;
        }

        public void CihazAc(int kimlik)
        {
            Cihaz cihaz = CihazBul(kimlik);
            if (cihaz == null)
            {
                Console.WriteLine("Hata: Belirtilen kimliğe sahip bir cihaz bulunamadı!");
                return;
            }

            cihaz.Ac();
        }

        public void CihazKapat(int kimlik)
        {
            Cihaz cihaz = CihazBul(kimlik);
            if (cihaz == null)
            {
                Console.WriteLine("Hata: Belirtilen kimliğe sahip bir cihaz bulunamadı!");
                return;
            }

            cihaz.Kapat();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Cihaz> cihazlar = new List<Cihaz>
            {
                new Cihaz(1, "Salon Lambası", CihazTuru.Lamba),
                new Cihaz(2, "Yatak Odası Kliması", CihazTuru.Klima),
                new Cihaz(3, "Giriş Kapısı Alarmı", CihazTuru.GuvenlikSistemi)
            };

            ICihazYonetici yonetici = new CihazYonetici(cihazlar);

            while (true)
            {
                Console.WriteLine("\n--- AKILLI EV CİHAZ YÖNETİMİ ---");
                Console.WriteLine("1. Cihazları Listele");
                Console.WriteLine("2. Yeni Cihaz Ekle");
                Console.WriteLine("3. Cihaz Aç");
                Console.WriteLine("4. Cihaz Kapat");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        yonetici.CihazlariListele();
                        break;

                    case "2":
                        Console.Write("Cihaz Kimliği (Sayı): ");
                        if (!int.TryParse(Console.ReadLine(), out int yeniKimlik))
                        {
                            Console.WriteLine("Hata: Kimlik sayısal bir değer olmalıdır.");
                            break;
                        }

                        Console.Write("Cihaz Adı: ");
                        string ad = Console.ReadLine();

                        Console.WriteLine("Cihaz Türü Seçiniz: (1: Lamba, 2: Klima, 3: GuvenlikSistemi)");
                        Console.Write("Tür: ");
                        if (!int.TryParse(Console.ReadLine(), out int turSecim) || !Enum.IsDefined(typeof(CihazTuru), turSecim))
                        {
                            Console.WriteLine("Hata: Geçersiz cihaz türü seçtiniz.");
                            break;
                        }

                        Cihaz yeniCihaz = new Cihaz(yeniKimlik, ad, (CihazTuru)turSecim);
                        yonetici.CihazEkle(yeniCihaz);
                        break;

                    case "3":
                        Console.Write("Açmak istediğiniz cihazın kimliği: ");
                        if (!int.TryParse(Console.ReadLine(), out int acilacakKimlik))
                        {
                            Console.WriteLine("Hata: Geçersiz kimlik formatı.");
                            break;
                        }

                        yonetici.CihazAc(acilacakKimlik);
                        break;

                    case "4":
                        Console.Write("Kapatmak istediğiniz cihazın kimliği: ");
                        if (!int.TryParse(Console.ReadLine(), out int kapanacakKimlik))
                        {
                            Console.WriteLine("Hata: Geçersiz kimlik formatı.");
                            break;
                        }

                        yonetici.CihazKapat(kapanacakKimlik);
                        break;

                    case "0":
                        Console.WriteLine("Programdan çıkılıyor...");
                        return;

                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                        break;
                }
            }
        }
    }
}