import uuid
import random
from abc import ABC, abstractmethod

#--- 1. Özel Hata Yönetimi (Custom Exception) ---
class KuantumCokusuException(Exception):
    """Bir nesnenin stabilitesi 0'ın altına düştüğünde fırlatılır."""
    def __init__(self, nesne_id: str):
        super().__init__(f"Kuantum Çöküşü! Patlayan Nesne ID: {nesne_id}")
        self.nesne_id = nesne_id

# --- 2. Arayüz (Interface Segregation) Taklidi (Mixin Class) ---
class IKritik:
    """Sadece tehlikeli nesneler için uygulanan arayüz/Mixin."""
    def acil_durum_sogutmasi(self):
        """Bu metot, uygulayan somut siniflarda tanimlanmalidir."""
        if self.stabilite < 100:
            # Max 100 olacak şekilde +50 artar
            self.stabilite = min(100.0, self.stabilite + 50.0) 
            print("❄️ Acil Soğutma Uygulandi. Stabilite +50.")
        else:
            print("Nesnenin stabilitesi zaten maksimum (100). Soğutma gereksiz.")

# --- 3. Temel Yapı (Abstract Class & Encapsulation) ---
class KuantumNesnesi(ABC):
    """Tüm kuantum nesnelerinin temel soyut sinifi."""

    def __init__(self, tehlike_seviyesi: int, baslangic_stabilite: float):
        self._id = str(uuid.uuid4())[:8] # Kapsülleme (private alan _id)
        self._stabilite = 0.0 # Kapsülleme (private alan _stabilite)
        self._tehlike_seviyesi = tehlike_seviyesi
        
        # YENİ ÖZELLİK: Rastgele Sıcaklık (20 ile 81 arasında)
        self._sicaklik = random.randint(20, 81) 
        
        self.stabilite = baslangic_stabilite # Setter ile ilk atama

    # ID için getter
    @property
    def id(self):
        return self._id
    
    # Stabilite için getter
    @property
    def stabilite(self):
        return self._stabilite

    # Stabilite için Kapsülleme (Setter: 0-100 kontrolü)
    @stabilite.setter
    def stabilite(self, yeni_stabilite: float):
        if yeni_stabilite > 100:
            self._stabilite = 100.0
        elif yeni_stabilite < 0:
            self._stabilite = 0.0
        else:
            self._stabilite = yeni_stabilite
            
    # Tehlike Seviyesi için getter
    @property
    def tehlike_seviyesi(self):
        return self._tehlike_seviyesi
    
    # Sıcaklık için getter
    @property
    def sicaklik(self):
        return self._sicaklik

    @abstractmethod
    def analiz_et(self):
        """Soyut metot. Alt siniflar bunu dolduracak."""
        pass

    def durum_bilgisi(self) -> str:
        """Nesnenin ID'sini ve o anki stabilitesini string olarak döndürür."""
        # Sıcaklık bilgisi eklendi:
        return f"ID: {self.id} | Stabilite: {self.stabilite:.2f}% | Tehlike: {self.tehlike_seviyesi}/10 | Sıcaklık: {self.sicaklik}°C | Tip: {self.__class__.__name__}"
    
    def _cokusu_kontrol_et(self):
        """Stabilitenin 0'in altina düşüp düşmediğini kontrol eder."""
        if self.stabilite <= 0:
            raise KuantumCokusuException(self.id)

# --- 4. Nesne Çeşitleri (Inheritance & Polymorphism) ---
class VeriPaketi(KuantumNesnesi):
    """Siradan, güvenli veri. IKritik değildir."""
    def __init__(self):
        super().__init__(tehlike_seviyesi=1, baslangic_stabilite=random.uniform(80.0, 100.0))

    def analiz_et(self):
        self.stabilite -= 5.0
        print("✅ Veri içeriği okundu. Stabilite -5.")
        self._cokusu_kontrol_et()

class KaranlikMadde(KuantumNesnesi, IKritik):
    """Tehlikelidir! IKritik arayüzünü uygular."""
    def __init__(self):
        super().__init__(tehlike_seviyesi=7, baslangic_stabilite=random.uniform(50.0, 80.0))

    def analiz_et(self):
        self.stabilite -= 15.0
        print("⚠️ Karanlık Madde analizi yapıldı. Stabilite -15.")
        self._cokusu_kontrol_et()

class AntiMadde(KuantumNesnesi, IKritik):
    """Çok Tehlikelidir! IKritik arayüzünü uygular."""
    def __init__(self):
        super().__init__(tehlike_seviyesi=10, baslangic_stabilite=random.uniform(30.0, 60.0))

    def analiz_et(self):
        self.stabilite -= 25.0
        print("🚨 Evrenin dokusu titriyor... Anti Madde analizi yapıldı. Stabilite -25.")
        self._cokusu_kontrol_et()

# --- 5. Oynanış Döngüsü (MAIN LOOP) ---
class KuantumAmbarı:
    def __init__(self):
        self.envanter: list[KuantumNesnesi] = []
        
        # Örnek başlangıç nesneleri
        self.envanter.append(VeriPaketi())
        self.envanter.append(KaranlikMadde())

    def menu_goster(self):
        print("\nKUANTUM AMBARI KONTROL PANELİ")
        print("1. Yeni Nesne Ekle (Rastgele)")
        print("2. Tüm Envanteri Listele (Durum Raporu)")
        print("3. Nesneyi Analiz Et (ID isteyerek)")
        print("4. Acil Durum Soğutması Yap (Sadece IKritik olanlar için!)")
        print("5. Çıkış")
        secim = input("Seçiminiz: ")
        return secim

    def yeni_nesne_ekle(self):
        tur = random.choice([VeriPaketi, KaranlikMadde, AntiMadde])
        yeni_nesne = tur()
        self.envanter.append(yeni_nesne)
        print(f"✅ Yeni {tur.__name__} (ID: {yeni_nesne.id}) depoya eklendi. Sıcaklık: {yeni_nesne.sicaklik}°C")

    def envanteri_listele(self):
        if not self.envanter:
            print("Envanterde hiç nesne yok.")
            return
        print("\n--- ENVANTER DURUM RAPORU ---")
        for nesne in self.envanter:
            print(nesne.durum_bilgisi())
        print("-----------------------------")

    def nesne_bul(self, id: str) -> KuantumNesnesi | None:
        return next((n for n in self.envanter if n.id == id), None)

    def nesne_analiz_et(self):
        id = input("Analiz edilecek nesnenin ID'sini girin: ").strip()
        nesne = self.nesne_bul(id)

        if nesne is None:
            print(f"ID'ye sahip bir nesne bulunamadı: {id}")
            return

        nesne.analiz_et()
        print(f"Analiz Sonrası Durum: {nesne.durum_bilgisi()}")

    def acil_durum_sogutmasi_yap(self):
        id = input("Soğutma yapılacak nesnenin ID'sini girin: ").strip()
        nesne = self.nesne_bul(id)

        if nesne is None:
            print(f"ID'ye sahip bir nesne bulunamadı: {id}")
            return
        
        if isinstance(nesne, IKritik):
            nesne.acil_durum_sogutmasi()
            print(f"Soğutma Sonrası Durum: {nesne.durum_bilgisi()}")
        else:
            print("⛔ Bu nesne soğutulamaz! (Yalnızca IKritik nesneler soğutulabilir)")

    def calistir(self):
        print("KUANTUM KAOS YÖNETİMİNE HOŞ GELDİNİZ, ŞİMDİ VARDİYA SİZDE!")

        while True:
            try:
                secim = self.menu_goster()
                print("---")
                
                if secim == '1':
                    self.yeni_nesne_ekle()
                elif secim == '2':
                    self.envanteri_listele()
                elif secim == '3':
                    self.nesne_analiz_et()
                elif secim == '4':
                    self.acil_durum_sogutmasi_yap()
                elif secim == '5':
                    print("Gün sonu. Çıkış yapılıyor. İyi çalışmalar!")
                    break
                else:
                    print("Hatalı seçim. Lütfen 1 ile 5 arasında bir sayı girin.")
                
                print("---\n")

            except KuantumCokusuException as e:
                print(f"\n\n{e}")
                print("████████████████████████████████")
                print("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...")
                print("████████████████████████████████")
                break
            except Exception:
                print("Geçersiz giriş veya beklenmeyen bir genel hata oluştu.")

if __name__ == "__main__":
    ambar = KuantumAmbarı()
    ambar.calistir()