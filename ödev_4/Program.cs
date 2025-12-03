using System;
using System.Collections.Generic;

namespace ödev_4
{
    internal class Program
    {
        // Tüm kuantum nesnelerini saklayan envanter listesi
        static List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();

        static void Main(string[] args)
        {
            try
            {
                // Sonsuz döngü: Kullanıcı çıkış yapana kadar menü gösterilir
                while (true)
                {
                    Console.WriteLine("\n=== KUANTUM AMBARI KONTROL PANELİ ===");
                    Console.WriteLine("1 - Yeni Nesne Ekle");       // Rastgele türde kuantum nesnesi oluşturur
                    Console.WriteLine("2 - Envanteri Listele");     // Mevcut tüm nesneleri listeler
                    Console.WriteLine("3 - Nesne Analiz Et");       // ID'ye göre analiz işlemi yapar
                    Console.WriteLine("4 - Acil Durum Soğutması");  // Sadece IKritik nesnelerinde soğutma yapar
                    Console.WriteLine("5 - Çıkış");                 // Programdan çıkar

                    Console.Write("Seçiminiz: ");
                    string secim = Console.ReadLine();

                    // Kullanıcı seçimlerine göre ilgili fonksiyonlar çağrılır
                    switch (secim)
                    {
                        case "1": YeniNesneEkle(); break;   // Envantere rastgele nesne ekleme
                        case "2": Listele(); break;         // Tüm envanteri yazdırma
                        case "3": NesneAnalizEt(); break;    // ID'ye göre analiz
                        case "4": SogutmaYap(); break;        // Kritik nesnelerde soğutma işlemi
                        case "5": return;                     // Programdan çıkış
                        default: Console.WriteLine("Geçersiz seçim!"); break;
                    }
                }
            }
            catch (KuantumCokusuException ex)
            {
                // Eğer analiz sırasında stabilite sıfır altına düşerse çalışır
                Console.WriteLine("\n*** SİSTEM ÇÖKTÜ! ***");
                Console.WriteLine(ex.Message);
            }
        }

        // Rastgele nesne oluşturup envantere ekleyen fonksiyon
        static void YeniNesneEkle()
        {
            Random r = new Random();
            int tip = r.Next(0, 3); // 0 → VeriPaketi / 1 → KaranlıkMadde / 2 → AntiMadde rastgele
            string id = Guid.NewGuid().ToString().Substring(0, 8); // Benzersiz ID üretimi
            double stabilite = r.Next(30, 101);  // Stabilite 30–100 arası
            int Sıcaklık = r.Next(20, 81);       // (Kullanılmıyor ama üretildi)

            // VeriPaketi oluşturma
            if (tip == 0)
            {
                envanter.Add(new VeriPaketi(id, stabilite));
                Console.WriteLine($"VeriPaketi oluşturuldu. ID: {id}");
            }
            // Karanlık madde oluşturma
            else if (tip == 1)
            {
                int tehlike = r.Next(4, 9); // Tehlike seviyesi 4-8
                envanter.Add(new KaranlikMadde(id, stabilite, tehlike));
                Console.WriteLine($"Karanlık Madde oluşturuldu. ID: {id}");
            }
            // AntiMadde oluşturma
            else
            {
                int tehlike = r.Next(7, 11); // Tehlike seviyesi 7-10
                envanter.Add(new AntiMadde(id, stabilite, tehlike));
                Console.WriteLine($"AntiMadde oluşturuldu. ID: {id}");
            }
        }

        // Envanteri ekrana yazdırır
        static void Listele()
        {
            Console.WriteLine("\n--- ENVANTER ---");

            // Envanter boşsa uyarı ver
            if (envanter.Count == 0)
            {
                Console.WriteLine("Envanter boş!");
                return;
            }

            // Her nesnenin DurumBilgisi fonksiyonu çağrılır
            foreach (var nesne in envanter)
                Console.WriteLine(nesne.DurumBilgisi()); // Nesne tipi + stabilite + ID + tehlike
        }

        // Kullanıcının girdiği ID'ye göre analiz işlemi yapar
        static void NesneAnalizEt()
        {
            Console.Write("Analiz edilecek ID: ");
            string id = Console.ReadLine();

            // ID'yi küçük harf yaparak eşleşme kontrolü
            var nesne = envanter.Find(n => n.ID.ToLower() == id.ToLower());

            // Nesne bulunamadıysa
            if (nesne == null)
            {
                Console.WriteLine("Nesne bulunamadı!");
                return;
            }

            // Nesnenin kendi override edilmiş analiz metodu çalışır
            nesne.AnalizEt();
        }

        // Sadece IKritik arayüzüne sahip nesnelerde soğutma çalıştırır
        static void SogutmaYap()
        {
            Console.Write("Soğutulacak ID: ");
            string id = Console.ReadLine();

            // ID ile nesne arama
            var nesne = envanter.Find(n => n.ID.ToLower() == id.ToLower());

            // Bulunamadıysa uyarı
            if (nesne == null)
            {
                Console.WriteLine("Nesne bulunamadı!");
                return;
            }

            // Nesne IKritik ise → soğutma yapılabilir
            if (nesne is IKritik kritik)
            {
                kritik.AcilDurumSogutmasi(); // Arayüzde tanımlanan metot çağrılır
            }
            else
            {
                Console.WriteLine("Bu nesne soğutulamaz!"); // VeriPaketi gibi soğutma desteklemeyenler
            }
        }
    }
}
