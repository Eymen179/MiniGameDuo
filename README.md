# MiniGameDuo

Bu proje, bir staj vaka çalışması (case study) kapsamında geliştirilmiş; içerisinde 3D Main Menu, 2D Puzzle oyunu ve 2.5D Denizaltı simülasyonu barındıran bütünleşik bir Unity oyun projesidir.

![Main Menu Screenshot](Gorseller/MainMenu.png)

## 🛠 Teknik Bilgiler

* **Unity Versiyonu:** 2022.3.62f2
* **Input Sistemi:** Unity New Input System (Event-Driven Architecture)
* **Render Pipeline:** Built-in / URP (Projeye göre güncelleyebilirsin)
* **Versiyon Kontrol:** Git LFS (Large File Storage)

---

## 🚀 Kurulum ve Çalıştırma Adımları

Projeyi sorunsuz çalıştırmak için aşağıdaki adımları izleyin:

1.  **Repoyu Klonlama:**
    Proje büyük dosyalar (Assetler) içerdiği için **Git LFS** gereklidir.
    ```bash
    git lfs install
    git clone [REPO_LINKINIZ]
    ```

2.  **Editörde Çalıştırma:**
    * Proje Unity Hub üzerinden açıldığında, **Scenes** klasörü altındaki `MainMenu` sahnesini açın.
    * Oyun akışı ve Manager sistemlerinin (AudioManager vb.) doğru yüklenmesi için oyunun **mutlaka MainMenu sahnesinden başlatılması gerekmektedir.**

3.  **Build Alma (Önemli):**
    * Build alırken proje ana dizininde `Build` veya `App` isminde yeni bir klasör oluşturulmalı ve build bu klasöre alınmalıdır.
    * *Not:* Bu klasörler `.gitignore` dosyasına eklenmiştir, repoya dahil edilmez.

---

## 🎮 Oyun Mekanikleri ve Teknik Çözümler

Proje, Singleton tasarım deseni (GameManager, AudioManager) ve Observer benzeri Event yapıları üzerine kurgulanmıştır.

### 1. 2D Puzzle Oyunu
Sürükle-bırak mekaniğine sahip, parça eşleştirmeli bir bulmaca oyunu.

![Puzzle Game Screenshot](Gorseller/PuzzleGame.png)

* **Input Yönetimi:** Unity'nin **New Input System**'i kullanılmıştır. Eski `OnMouse` metotları yerine, Event System arayüzleri (`IPointerDownHandler`, `IDragHandler`, `IPointerUpHandler`) implemente edilmiştir.
* **Etkileşim:** Kameraya eklenen `Physics 2D Raycaster` sayesinde, UI olmayan Sprite objeleriyle Event System üzerinden etkileşime geçilmesi sağlanmıştır.
* **Snap (Yerleştirme) Mantığı:** Sürüklenen parça bırakıldığında, hedef slot ile arasındaki mesafe `Vector3.Distance` ile hesaplanır. Eğer mesafe eşik değerin altındaysa, parça `Vector3.Lerp` ile yumuşak bir animasyonla yuvaya oturur.
* **Feedback:** Doğru ve yanlış hamlelerde Particle System efektleri ve ses geri bildirimleri tetiklenir.

### 2. 2.5D Denizaltı Oyunu
Fizik tabanlı hareket ve UI etkileşimli soru sistemi içeren bir simülasyon.

![Submarine Game Screenshot](Gorseller/SubmarineGame.png)

* **Hareket Fiziği:** Denizaltı kontrolü `Rigidbody` fiziği kullanılarak sağlanmıştır. `FixedUpdate` içerisinde `velocity` manipülasyonu ile hareket verilirken, dönüşler `Quaternion.Slerp` ile yumuşatılmıştır.
* **Yüzey Sınırlandırması:** Denizaltının su yüzeyine (Y = 50.5f) çıkmasını engellemek için pozisyon ve hız vektörleri kod tarafında `Mathf.Clamp` mantığı ile sınırlandırılmıştır.
* **Dinamik Post-Processing:** Su altı atmosferini sağlamak için **Global Volume** kullanılmıştır. Kamera su seviyesinin altına indiğinde kod tarafında Volume bileşeninin `weight` (ağırlık) değeri `Mathf.MoveTowards` ile artırılarak bulanıklık ve renk efektleri dinamik olarak devreye girer.
* **Sandık ve Soru Sistemi:** Trigger tabanlı toplama sistemi kullanılmıştır. 5 sandık toplandığında oyun durur (Pause State) ve UI üzerinden soru-cevap mekanizması (State Machine) devreye girer.

---

## 📂 Proje Yapısı ve Kullanılan Desenler

* **Singleton Pattern:** `AudioManager`, `PuzzleGameManager` ve `SubmarineGameManager` gibi yönetici sınıflar sahneler arası veri kaybını önlemek ve tek merkezden erişim sağlamak için Singleton olarak tasarlanmıştır.
* **New Input System:** Tüm kontrol şeması (WASD, Mouse Position, Click, ESC) tek bir `InputActions` asset dosyası üzerinden yönetilmekte ve C# eventleri ile scriptlere bağlanmaktadır.
