KOPYALA – YAPIŞTIR HAZIR README.md
# 🚀 Kuantum Nesne Yönetim Sistemi

Bu proje; VeriPaketi, KaranlikMadde ve AntiMadde gibi kuantum tabanlı nesneleri yöneten bir OOP simülasyon sistemidir.  
Nesnelerin stabilite, sıcaklık ve tehlike seviyeleri bulunur ve analiz/soğutma işlemleri fiziksel davranışları simüle eder.

---

## ⚙️ Özellikler

### 🔹 OOP Prensipleri
- Kalıtım
- Kapsülleme
- Polimorfizm
- Soyutlama
- Arayüz kullanımı

### 🔹 Dinamik Nesne Üretimi
Her nesne rastgele:
- ID
- Stabilite (30–100)
- Sıcaklık (20–80)
- Tehlike Seviyesi (türe göre)

ile oluşur.

### 🔹 Analiz Mekanizması
| Nesne Türü      | Stabilite Azalışı |
|----------------|-------------------|
| VeriPaketi     | -5                |
| Karanlık Madde | -15               |
| AntiMadde      | -25               |

### 🔹 Acil Durum Soğutması
| Nesne Türü      | Stabilite Artışı | Sıcaklık Azalışı |
|----------------|------------------|------------------|
| Karanlık Madde | +50              | -15°C            |
| AntiMadde      | +50              | -25°C            |

### 🔹 Kuantum Çöküşü
Stabilite **0** olursa:



KUANTUM ÇÖKÜŞÜ! Patlayan nesne ID: XXXXX


---

## 🧭 Ana Menü



1 - Yeni Nesne Ekle
2 - Envanteri Listele
3 - Nesne Analiz Et
4 - Acil Durum Soğutması
5 - Çıkış


---

## ▶ Çalıştırma

### JavaScript (Node.js)
```bash
node kuantum_ambari.js

Python
python kuantum_ambari.py

C#

Visual Studio → F5

Java
javac *.java
java Main

🧪 Örnek Çıktı
Yeni nesne üretildi: KaranlikMadde (ID: ab12cd)
Stabilite: 70
Tehlike: 6
Sıcaklık: 55°C

Analiz ediliyor...
Karanlık madde analiz edildi.

Acil durum soğutması uygulanıyor...
Yeni sıcaklık: 40°C

📄 Lisans

Bu proje eğitim amaçlıdır.
