🚇 MetroBakımTakip

Bir tıkla arıza verilerini yönet, bakım geçmişini görüntüle ve geleceğe dair öngörüler al!

✨ Öne Çıkan Özellikler

Kayıt Yönetin 📝

Hızlı arıza kaydı oluşturma, silme, filtreleme

Gerçek zamanlı tablo araması ve tarih aralığıyla filtre

Risk Skoru Hesaplama ⚠️

Son 7 gün içindeki arıza sayısını otomatik hesaplar

Her istasyon için anında RiskScore gösterir

Veri Dışa Aktarım 📤

Profesyonel PDF raporları (iText) oluşturun

CSV çıktısıyla tüm veriyi zahmetsizce dışa aktarın

Yedekleme 💾

metro_backup.db adımında yedeğinizi alın

Akıllı Tahmin 🤖

Tek tuşla faults_train.csv oluşturarak eğitim verisi hazırlayın

ML.NET (FastTree) ile model eğitimi: faultModel.zip

One-Hot Encoding ve hızlı eğitim (milisaniyeler!)

Seçilen istasyon/tarih/saat için arıza olasılığını % formatında görün

🚀 Başlarken

Depoyu Klonlayın:



git clone https://github.com//MetroBakimTakip.git
cd MetroBakimTakip

2. **Visual Studio’yu Açın** ve `MetroBakimTakip.sln` dosyasını yükleyin.
3. **NuGet Paketlerini Yükleyin:**
   - `Microsoft.ML`, `Microsoft.ML.FastTree`
   - `System.Data.SQLite` veya `Microsoft.Data.SQLite`
   - `iText7` (`iText.Kernel`, `iText.Layout`)
4. **Veritabanını Konumlandırın:**
   - `metro.db` dosyasını proje köküne veya `bin\Debug` klasörüne kopyalayın.
5. **Projeyi Derleyip Çalıştırın** (F5).

---

## 🎯 Kullanım Akışı

1. **Arıza Kayıtları**: Formu doldurun, kayıt ekleyin veya silebilirsiniz.
2. **Risk Skorunu Görüntüle**: Tabloya bakın, her satırda son 7 gün skoru hazır.
3. **Eğitim Verisi Oluştur**: "Eğitim Verisi" butonuna tıklayın ➡️ `faults_train.csv` oluşturulur.
4. **Modeli Eğit**: "Train" tuşu ile FastTree modelini eğitin; `faultModel.zip` hemen kaydedilir.
5. **Tahmin (Predict)**: İstasyon, tarih ve saati seçin; saniyeler içinde arıza ihtimalini `%` olarak görün!
6. **PDF & CSV Dışa Aktarım**: Raporlarınızı canınızın istediği gibi paylaşın.
7. **Yedekleme**: Veritabanınızı yedekleyin, geri yüklemesi kolay olsun.

---

## 💡 Neden MetroBakımTakip?

- **Hızlı ve Pratik:** Windows Forms ile anında erişim
- **Veriye Dayalı Karar:** ML destekli tahminlerle planlama
- **Esnek ve Özelleştirilebilir:** Kendi veri kaynaklarınıza kolay entegre edin

---

## 🤝 Katkıda Bulunma

Bu projeyi parlatmak için pull request’ler ve issue’lara açılan kapımız **her zaman açık**!

---

## 📜 Lisans

MIT Lisansı ile korunur. Detaylar `LICENSE` dosyasında.
