# MiniGameDuo

Bu proje, portfolyoyu büyütmek amacıyla geliştirilmiş; içerisinde 2 adet oyun barındıran bütünleşik bir Unity oyun projesidir.

![Main Menu Screenshot](Images/MainMenuu.png)

## 🛠 Teknik Bilgiler

* **Unity Versiyonu:** 2022.3.62f2
* **Render Pipeline:** URP (Projeye göre güncelleyebilirsin)
* **Versiyon Kontrol:** Git LFS (Large File Storage)

---

## 🚀 Kurulum ve Çalıştırma Adımları

Projeyi sorunsuz çalıştırmak için aşağıdaki adımları izleyin:

1.  **Repoyu Klonlama:**
    Proje büyük dosyalar (Assetler) içerdiği için **Git LFS** gereklidir.
    ```bash
    git lfs install
    git clone https://github.com/Eymen179/MiniGameDuo/
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

![Puzzle Game Screenshot](Images/PuzzleGame.png)

* **Input Yönetimi:** Unity'nin **New Input System**'i kullanılmıştır. Bununla birlikte Event System arayüzleri (`IPointerDownHandler`, `IDragHandler`, `IPointerUpHandler`) implemente edilmiştir.
* **Etkileşim:** Kameraya eklenen `Physics 2D Raycaster` sayesinde, UI olmayan Sprite objeleriyle Event System üzerinden etkileşime geçilmesi sağlanmıştır.
* **Snap (Yerleştirme) Mantığı:** Sürüklenen parça bırakıldığında, hedef slot ile arasındaki mesafe `Vector3.Distance` ile hesaplanır. Eğer mesafe eşik değerin altındaysa, parça `Vector3.Lerp` ile yumuşak bir animasyonla yuvaya oturur.
* **Feedback:** Doğru ve yanlış hamlelerde Particle System efektleri ve ses geri bildirimleri tetiklenir.

### 2. 2.5D Denizaltı Oyunu
Fizik tabanlı hareket ve UI etkileşimli soru sistemi içeren bir simülasyon.

![Submarine Game Screenshot](Images/SubmarineGame.png)

* **Hareket Fiziği:** Denizaltı kontrolü `Rigidbody` fiziği kullanılarak sağlanmıştır. `FixedUpdate` içerisinde `velocity` manipülasyonu ile hareket verilirken, dönüşler `Quaternion.Slerp` ile yumuşatılmıştır.
* **Yüzey Sınırlandırması:** Denizaltının su yüzeyine (Y = 50.5f) çıkmasını engellemek için pozisyon ve hız vektörleri kod tarafında `Mathf.Clamp` mantığı ile sınırlandırılmıştır.
* **Dinamik Post-Processing:** Su altı atmosferini sağlamak için **Global Volume** kullanılmıştır. Kamera su seviyesinin altına indiğinde kod tarafında Volume bileşeninin `weight` (ağırlık) değeri `Mathf.MoveTowards` ile artırılarak bulanıklık ve renk efektleri dinamik olarak devreye girer.
* **Sandık ve Soru Sistemi:** Trigger tabanlı toplama sistemi kullanılmıştır. 5 sandık toplandığında oyun durur (Pause State) ve UI üzerinden soru-cevap mekanizması (State Machine) devreye girer.

---

## 📂 Proje Yapısı ve Kullanılan Desenler

* **Singleton Pattern:** `MiniGameManager`, `AudioManager`, `PuzzleGameManager` ve `SubmarineGameManager` sınıfları sahneler arası veri kaybını önlemek ve tek merkezden erişim sağlamak için Singleton olarak tasarlanmıştır.
* **New Input System:** Tüm kontrol şeması (WASD, Mouse Position, Click, ESC) tek bir `InputActions` asset dosyası üzerinden yönetilmekte ve C# eventleri ile scriptlere bağlanmaktadır.

## 📂 Kullanılan Assetler
* https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153
* https://assetstore.unity.com/packages/3d/vehicles/sea/steampunk-submarine-diver-115731
* https://assetstore.unity.com/packages/3d/environments/low-poly-tropical-island-lite-242437
* https://assetstore.unity.com/packages/2d/gui/violet-themed-ui-235559
* https://pngimg.com/image/22169
* https://github.com/Eymen179/Line-And-Ball/blob/main/LineAndBallOfficial/Assets/Sfx/button2.wav
* https://www.myinstants.com/en/instant/among-us-card-swipe-deny-27349/
* https://www.myinstants.com/en/instant/ding-sound-effect/
* https://www.youtube.com/watch?v=Nwk5NF9eSPk
* https://freesound.org/people/theredshore/sounds/83979/
* https://freesound.org/people/mellow_minx/sounds/711024/
* https://www.myinstants.com/en/instant/yaaaaaaaay/
* https://freesound.org/people/phiIRaco/sounds/572626/

