import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.Scanner;
import java.util.UUID;

// D. Özel Hata Yönetimi (Default Access)
class KuantumCokusuException extends Exception {
    public KuantumCokusuException(String nesneId) {
        super("Kuantum Çöküşü! Patlayan Nesne ID: " + nesneId);
    }
}

// B. Arayüz (Default Access)
interface IKritik {
    void AcilDurumSogutmasi(); 
}

// A. Temel Yapı (Abstract Class - Default Access)
abstract class KuantumNesnesi {
    private final String id; 
    private double stabilite; 
    private final int tehlikeSeviyesi; 
    private final int sicaklik; 

    private static final Random random = new Random();

    public KuantumNesnesi(int tehlikeSeviyesi, double baslangicStabilite) {
        this.id = UUID.randomUUID().toString().substring(0, 8); 
        this.tehlikeSeviyesi = tehlikeSeviyesi;
        
        this.sicaklik = random.nextInt(62) + 20; 

        setStabilite(baslangicStabilite);
    }

    public double getStabilite() {
        return stabilite;
    }

    public void setStabilite(double yeniStabilite) {
        if (yeniStabilite > 100) {
            this.stabilite = 100.0;
        } else if (yeniStabilite < 0) {
            this.stabilite = 0.0; 
        } else {
            this.stabilite = yeniStabilite;
        }
    }

    public String getId() {
        return id;
    }

    public int getSicaklik() {
        return sicaklik;
    }

    public abstract void AnalizEt() throws KuantumCokusuException;

    public String DurumBilgisi() {
        return String.format("ID: %s | Stabilite: %.2f%% | Tehlike: %d/10 | Sıcaklık: %d°C | Tip: %s",
                id, stabilite, tehlikeSeviyesi, sicaklik, this.getClass().getSimpleName());
    }

    protected void CokusuKontrolEt() throws KuantumCokusuException {
        if (stabilite <= 0) {
            throw new KuantumCokusuException(id);
        }
    }
}

// C. Nesne Çeşitleri: VeriPaketi (Default Access)
class VeriPaketi extends KuantumNesnesi {
    public VeriPaketi() {
        super(1, 80.0 + Math.random() * 20.0);
    }

    @Override
    public void AnalizEt() throws KuantumCokusuException {
        setStabilite(getStabilite() - 5.0); 
        System.out.println("✅ Veri içeriği okundu."); 
        CokusuKontrolEt();
    }
}

// C. Nesne Çeşitleri: KaranlikMadde (Default Access)
class KaranlikMadde extends KuantumNesnesi implements IKritik {
    public KaranlikMadde() {
        super(7, 50.0 + Math.random() * 30.0);
    }

    @Override
    public void AnalizEt() throws KuantumCokusuException {
        setStabilite(getStabilite() - 15.0); 
        System.out.println("⚠️ Karanlık Madde analizi yapıldı. Stabilite -15.");
        CokusuKontrolEt();
    }

    @Override
    public void AcilDurumSogutmasi() { 
        setStabilite(getStabilite() + 50.0); 
        System.out.println("❄️ Karanlık Maddeye Acil Soğutma Uygulandı. Stabilite +50.");
    }
}

// C. Nesne Çeşitleri: AntiMadde (Default Access)
class AntiMadde extends KuantumNesnesi implements IKritik {
    public AntiMadde() {
        super(10, 30.0 + Math.random() * 30.0);
    }

    @Override
    public void AnalizEt() throws KuantumCokusuException {
        setStabilite(getStabilite() - 25.0); 
        System.out.println("🚨 Evrenin dokusu titriyor... Anti Madde analizi yapıldı. Stabilite -25."); 
        CokusuKontrolEt();
    }

    @Override
    public void AcilDurumSogutmasi() { 
        setStabilite(getStabilite() + 50.0); 
        System.out.println("❄️ Anti Maddeye KRİTİK Soğutma Uygulandı. Stabilite +50.");
    }
}


// 3. Oynanış Döngüsü (MAIN LOOP) - Default Access
class Main {
    private static final List<KuantumNesnesi> envanter = new ArrayList<>();
    private static final Random random = new Random();
    private static final Scanner scanner = new Scanner(System.in);

    // main metodu public kalmalıdır
    public static void main(String[] args) {
        System.out.println("KUANTUM KAOS YÖNETİMİNE HOŞ GELDİNİZ, ŞİMDİ VARDİYA SİZDE!");
        
        envanter.add(new VeriPaketi());
        envanter.add(new KaranlikMadde());

        while (true) {
            try {
                menuGoster(); 
                String input = scanner.nextLine();
                int secim = 0;
                try {
                    secim = Integer.parseInt(input);
                } catch (NumberFormatException e) {
                    secim = 0; 
                }
                
                System.out.println("---");
                
                switch (secim) {
                    case 1:
                        yeniNesneEkle(); 
                        break;
                    case 2:
                        envanteriListele(); 
                        break;
                    case 3:
                        nesneAnalizEt(); 
                        break;
                    case 4:
                        acilDurumSogutmasiYap(); 
                        break;
                    case 5:
                        System.out.println("Gün sonu. Çıkış yapılıyor. İyi çalışmalar!"); 
                        scanner.close(); // Scanner'ı kapatmayı deniyoruz
                        return;
                    default:
                        System.out.println("Hatalı seçim. Lütfen 1 ile 5 arasında bir sayı girin.");
                        break;
                }
                
                System.out.println("---\n");

            } catch (KuantumCokusuException e) {
                System.err.println("\n\n" + e.getMessage());
                System.err.println("████████████████████████████████");
                System.err.println("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR..."); 
                System.err.println("████████████████████████████████");
                break; 
            } catch (Exception e) {
                System.out.println("Geçersiz giriş yaptınız veya beklenmeyen bir hata oluştu: " + e.getMessage());
            }
        }
    }

    private static void menuGoster() {
        System.out.println("\nKUANTUM AMBARI KONTROL PANELİ");
        System.out.println("1. Yeni Nesne Ekle (Rastgele Veri/Karanlık Madde/Anti Madde üretir)"); 
        System.out.println("2. Tüm Envanteri Listele (Durum Raporu)"); 
        System.out.println("3. Nesneyi Analiz Et (ID isteyerek)"); 
        System.out.println("4. Acil Durum Soğutması Yap (Sadece IKritik olanlar için!)"); 
        System.out.println("5. Çıkış"); 
        System.out.print("Seçiminiz: ");
    }

    private static void yeniNesneEkle() {
        int tur = random.nextInt(3);
        KuantumNesnesi yeniNesne;
        
        if (tur == 0) {
            yeniNesne = new VeriPaketi();
            System.out.println("✅ Yeni Veri Paketi depoya eklendi.");
        } else if (tur == 1) {
            yeniNesne = new KaranlikMadde();
            System.out.println("⚠️ Yeni Karanlık Madde depoya eklendi.");
        } else {
            yeniNesne = new AntiMadde();
            System.out.println("🚨 Yeni Anti Madde depoya eklendi. Dikkat!");
        }
        envanter.add(yeniNesne);
        System.out.printf("Yeni Nesne ID: %s, Sıcaklık: %d°C\n", yeniNesne.getId(), yeniNesne.getSicaklik());
    }

    private static void envanteriListele() {
        if (envanter.isEmpty()) {
            System.out.println("Envanterde hiç nesne yok.");
            return;
        }
        System.out.println("--- ENVANTER DURUM RAPORU ---");
        for (KuantumNesnesi nesne : envanter) {
            System.out.println(nesne.DurumBilgisi());
        }
    }

    private static KuantumNesnesi nesneBul(String id) {
        for (KuantumNesnesi nesne : envanter) {
            if (nesne.getId().equals(id)) {
                return nesne;
            }
        }
        return null;
    }

    private static void nesneAnalizEt() throws KuantumCokusuException {
        System.out.print("Analiz edilecek nesnenin ID'sini girin: ");
        String id = scanner.nextLine().trim();
        KuantumNesnesi nesne = nesneBul(id);

        if (nesne == null) {
            System.out.println("ID'ye sahip bir nesne bulunamadı: " + id);
            return;
        }

        nesne.AnalizEt();
        System.out.println("Analiz Sonrası Durum: " + nesne.DurumBilgisi());
    }

    private static void acilDurumSogutmasiYap() {
        System.out.print("Soğutma yapılacak nesnenin ID'sini girin: ");
        String id = scanner.nextLine().trim();
        KuantumNesnesi nesne = nesneBul(id);

        if (nesne == null) {
            System.out.println("ID'ye sahip bir nesne bulunamadı: " + id);
            return;
        }

        if (nesne instanceof IKritik kritikNesne) {
            kritikNesne.AcilDurumSogutmasi();
            System.out.println("Soğutma Sonrası Durum: " + nesne.DurumBilgisi());
        } else {
            System.out.println("⛔ Bu nesne soğutulamaz! (Yalnızca IKritik nesneler soğutulabilir)");
        }
    }
}