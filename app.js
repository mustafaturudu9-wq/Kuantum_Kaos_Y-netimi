// Node.js ortamında kullanıcı girişi için readline modülü gereklidir.
const readline = require('readline');

// --- 1. Özel Hata Yönetimi (Custom Exception) ---
class KuantumCokusuException extends Error {
    constructor(nesneId) {
        super(`Kuantum Çöküşü! Patlayan Nesne ID: ${nesneId}`);
        this.name = "KuantumCokusuException";
        this.nesneId = nesneId;
    }
}

// --- 2. Arayüz (Interface Segregation) Taklidi (Class) ---
// Bu sınıf, IKritik nesnelerin sahip olması gereken davranışı tanımlar.
class IKritik {
    AcilDurumSogutmasi() {
        throw new Error("AcilDurumSogutmasi metodu alt sınıfta uygulanmalıdır.");
    }
}

// --- 3. Temel Yapı (Abstract Class & Encapsulation) ---
class KuantumNesnesi {
    // Private alanlar (ECMAScript Private Class Fields)
    #stabilite; 
    #sicaklik; // YENİ ÖZELLİK: Sıcaklık

    constructor(tehlikeSeviyesi, baslangicStabilite) {
        this.id = Math.random().toString(36).substring(2, 10); // Rastgele ID
        this.tehlikeSeviyesi = tehlikeSeviyesi;
        this.#stabilite = 0.0;
        
        // YENİ ÖZELLİK: Rastgele Sıcaklık (20 ile 81 arasında tam sayı)
        this.#sicaklik = Math.floor(Math.random() * (81 - 20 + 1)) + 20; 
        
        // Kapsülleme: Setter ile ilk atama
        this.stabilite = baslangicStabilite;
        
        // Abstract class taklidi
        if (new.target === KuantumNesnesi) {
            throw new TypeError("KuantumNesnesi soyut bir sınıftır ve doğrudan örneklenemez.");
        }
    }
    
    // Stabilite için Kapsülleme (Getter)
    get stabilite() {
        return this.#stabilite;
    }

    // Stabilite için Kapsülleme (Setter: 0-100 kontrolü)
    set stabilite(yeniStabilite) {
        if (yeniStabilite > 100) {
            this.#stabilite = 100.0;
        } else if (yeniStabilite < 0) {
            this.#stabilite = 0.0;
        } else {
            this.#stabilite = yeniStabilite;
        }
    }

    // Sıcaklık için Getter
    get sicaklik() {
        return this.#sicaklik;
    }

    // Soyut Metot taklidi
    AnalizEt() {
        throw new Error("AnalizEt metodu alt sınıflarca uygulanmalıdır.");
    }

    // Ortak Metot (Polimorfizm)
    DurumBilgisi() {
        // Sıcaklık bilgisi eklendi:
        return `ID: ${this.id} | Stabilite: ${this.#stabilite.toFixed(2)}% | Tehlike: ${this.tehlikeSeviyesi}/10 | Sıcaklık: ${this.#sicaklik}°C | Tip: ${this.constructor.name}`;
    }

    // Stabilitenin 0'ın altına düşüp düşmediğini kontrol eden yardımcı metot
    _cokusuKontrolEt() {
        if (this.#stabilite <= 0) {
            throw new KuantumCokusuException(this.id);
        }
    }
}

// --- 4. Nesne Çeşitleri (Inheritance & Polymorphism) 

class VeriPaketi extends KuantumNesnesi {
    constructor() {
        super(1, Math.random() * 20.0 + 80.0);
    }

    AnalizEt() {
        this.stabilite -= 5.0;
        console.log(" Veri içeriği okundu. Stabilite -5.");
        this._cokusuKontrolEt();
    }
}

class KaranlikMadde extends KuantumNesnesi {
    constructor() {
        super(7, Math.random() * 30.0 + 50.0);
    }

    AnalizEt() {
        this.stabilite -= 15.0;
        console.log("⚠️ Karanlık Madde analizi yapıldı. Stabilite -15.");
        this._cokusuKontrolEt();
    }
    
    // IKritik Arayüz Metodu
    AcilDurumSogutmasi() {
        this.stabilite += 50.0;
        console.log("❄️ Karanlık Maddeye Acil Soğutma Uygulandı. Stabilite +50.");
    }
}

class AntiMadde extends KuantumNesnesi {
    constructor() {
        super(10, Math.random() * 30.0 + 30.0);
    }

    AnalizEt() {
        this.stabilite -= 25.0;
        console.log("🚨 Evrenin dokusu titriyor... Anti Madde analizi yapıldı. Stabilite -25.");
        this._cokusuKontrolEt();
    }
    
    // IKritik Arayüz Metodu
    AcilDurumSogutmasi() {
        this.stabilite += 50.0;
        console.log("❄️ Anti Maddeye KRİTİK Soğutma Uygulandı. Stabilite +50.");
    }
}

// --- 5. Oynanış Döngüsü (MAIN LOOP) ---
class KuantumAmbarı {
    constructor() {
        this.envanter = [];
        this.envanter.push(new VeriPaketi());
        this.envanter.push(new KaranlikMadde());
    }

    menuGoster() {
        return `
KUANTUM AMBARI KONTROL PANELİ
1. Yeni Nesne Ekle (Rastgele Veri/Karanlık Madde/Anti Madde üretir)
2. Tüm Envanteri Listele (Durum Raporu)
3. Nesneyi Analiz Et (ID isteyerek)
4. Acil Durum Soğutması Yap (Sadece IKritik olanlar için!)
5. Çıkış
Seçiminiz: `;
    }

    yeniNesneEkle() {
        const nesneTurleri = [VeriPaketi, KaranlikMadde, AntiMadde];
        const SecilenTur = nesneTurleri[Math.floor(Math.random() * nesneTurleri.length)];
        const yeniNesne = new SecilenTur();
        this.envanter.push(yeniNesne);
        console.log(` Yeni ${SecilenTur.name} (ID: ${yeniNesne.id}) depoya eklendi. Sıcaklık: ${yeniNesne.sicaklik}°C`);
    }

    envanteriListele() {
        if (this.envanter.length === 0) {
            console.log("Envanterde hiç nesne yok.");
            return;
        }
        console.log("\n--- ENVANTER DURUM RAPORU ---");
        this.envanter.forEach(nesne => {
            console.log(nesne.DurumBilgisi());
        });
        console.log("-----------------------------");
    }

    nesneBul(id) {
        return this.envanter.find(nesne => nesne.id === id);
    }

    async nesneAnalizEt(rl) {
        const id = await this.soruSor(rl, "Analiz edilecek nesnenin ID'sini girin: ");
        const nesne = this.nesneBul(id);

        if (!nesne) {
            console.log(`ID'ye sahip bir nesne bulunamadı: ${id}`);
            return;
        }

        nesne.AnalizEt();
        console.log("Analiz Sonrası Durum: " + nesne.DurumBilgisi());
    }

    async acilDurumSogutmasiYap(rl) {
        const id = await this.soruSor(rl, "Soğutma yapılacak nesnenin ID'sini girin: ");
        const nesne = this.nesneBul(id);

        if (!nesne) {
            console.log(`ID'ye sahip bir nesne bulunamadı: ${id}`);
            return;
        }

        // Type Checking: Nesnenin Kritik olup olmadığı kontrolü (instanceof)
        if (nesne instanceof KaranlikMadde || nesne instanceof AntiMadde) {
            nesne.AcilDurumSogutmasi();
            console.log("Soğutma Sonrası Durum: " + nesne.DurumBilgisi());
        } else {
            console.log(" Bu nesne soğutulamaz! (Yalnızca Karanlık Madde ve Anti Madde soğutulabilir)");
        }
    }
    
    // Kullanıcı girişini yönetmek için yardımcı fonksiyon
    soruSor(rl, soru) {
        return new Promise(resolve => rl.question(soru, resolve));
    }

    async calistir() {
        console.log("KUANTUM KAOS YÖNETİMİNE HOŞ GELDİNİZ, ŞİMDİ VARDİYA SİZDE!");
        
        const rl = readline.createInterface({
            input: process.stdin,
            output: process.stdout
        });

        while (true) {
            try {
                const secim = parseInt(await this.soruSor(rl, this.menuGoster()));
                console.log("---");

                switch (secim) {
                    case 1:
                        this.yeniNesneEkle();
                        break;
                    case 2:
                        this.envanteriListele();
                        break;
                    case 3:
                        await this.nesneAnalizEt(rl);
                        break;
                    case 4:
                        await this.acilDurumSogutmasiYap(rl);
                        break;
                    case 5:
                        console.log("Gün sonu. Çıkış yapılıyor. İyi çalışmalar!");
                        rl.close();
                        return;
                    default:
                        console.log("Hatalı seçim. Lütfen 1 ile 5 arasında bir sayı girin.");
                        break;
                }
                console.log("---\n");

            } catch (e) {
                if (e instanceof KuantumCokusuException) {
                    console.error(`\n\n${e.message}`);
                    console.error("████████████████████████████████");
                    console.error("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                    console.error("████████████████████████████████");
                    rl.close();
                    return;
                } else if (e.message === "KuantumNesnesi soyut bir sınıftır ve doğrudan örneklenemez.") {
                    console.error("HATA: " + e.message);
                } else {
                    console.error("Beklenmeyen bir hata oluştu:", e.message);
                }
            }
        }
    }
}

// Uygulamayı başlat
new KuantumAmbarı().calistir();