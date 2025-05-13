using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// <example>
    /// Yeni bir paket oluşturma örneği
    /// <code>
    /// static void PaketOlustur(string dosyaYolu)
    /// {
    ///     var paketId = Guid.NewGuid();
    ///     var islemTarihi = DateTime.Now;
    ///     var paketKonu = new TextType() { Value = "e-Yazışma Test Paketi" };
    ///     var ek1 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },    // Zorunlu alan
    ///         BelgeNo = "72131250-010.03-936",
    ///         Tur = ST_KodEkTuru.DED,        // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi1),
    ///         MimeTuru = "application/pdf",
    ///         Ad = new TextType() { Value = "Birinci ek" },
    ///         SiraNo = 1,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "Birinci ek açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "A14A4DCC-AE6A-4FD5-AAB3-8A33DC6125DD",
    ///             schemeID = "GUID"     // Zorunlu alan
    ///         },
    ///         ImzaliMi = true,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ek2 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "35826416-020-987",
    ///         Tur = ST_KodEkTuru.HRF,       // Zorunlu alan
    ///         Ad = new TextType() { Value = "Harici referans" },
    ///         SiraNo = 2,        // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "Hairici referans açıklaması" },
    ///         Referans = "http://www.bilgitoplumu.gov.tr/Documents/1/Raporlar/Calisma_Raporu_2.pdf",
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "0C9C4D34-D370-46C2-A996-0FADC0FA591F",
    ///             schemeID = "GUID"       // Zorunlu alan
    ///         },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true,
    ///         Ozet = new CT_Ozet()
    ///         {
    ///             OzetAlgoritmasi = new CT_OzetAlgoritmasi() { Algorithm = Araclar.ALGORITHM_SHA256 },    // Zorunlu alan
    ///             OzetDegeri = Convert.FromBase64String("X4TAKrqfQNl9IR+h2yU/bAkNe0w=")   // Zorunlu alan
    ///         }
    ///     };
    /// 
    ///     var ek3 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "48154937-612.01.03-942",
    ///         Tur = ST_KodEkTuru.FZK,        // Zorunlu alan
    ///         Ad = new TextType() { Value = "Fiziksel ek" },
    ///         SiraNo = 3,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İki adet CD" },
    ///         OzId = new IdentifierType() { Value = "45A778BA-B943-4B68-8504-71854DF0DDFF", schemeID = "GUID" },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ek4 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
    ///         BelgeNo = "65410891-020-408",
    ///         Tur = ST_KodEkTuru.DED,     // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi2),
    ///         MimeTuru = "application/MSWord",
    ///         Ad = new TextType() { Value = "İmzasız ek" },
    ///         SiraNo = 4,     // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İmzasız ek açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "76E16DDA-AB44-46DB-B615-42FBE7E9BEE3",
    ///             schemeID = "GUID"
    ///         },
    ///         ImzaliMi = false,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ek5 = new CT_Ek()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = true },     // Zorunlu alan
    ///         BelgeNo = "65410891-020-444",
    ///         Tur = ST_KodEkTuru.DED,            // Zorunlu alan
    ///         DosyaAdi = System.IO.Path.GetFileName(ekDosyasi3),
    ///         MimeTuru = "application/pdf",
    ///         Ad = new TextType() { Value = "İkinci ek" },
    ///         SiraNo = 5,         // Zorunlu alan
    ///         Aciklama = new TextType() { Value = "İkinci ek açıklaması" },
    ///         OzId = new IdentifierType() { Value = "D7D16B9C-7419-451B-82B0-34D334572464", schemeID = "GUID" },
    ///         ImzaliMi = true,
    ///         ImzaliMiSpecified = true
    ///     };
    /// 
    ///     var ilgiA = new CT_Ilgi()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },       // Zorunlu alan
    ///         BelgeNo = "65410891-020-480",
    ///         Tarih = new DateTime(2009, 11, 5),
    ///         TarihSpecified = true,
    ///         Etiket = "a",       // Zorunlu alan
    ///         EkId = ek1.Id.Value,
    ///         Ad = new TextType() { Value = "İlgi (a) yazı" },
    ///         Aciklama = new TextType() { Value = "İlgi (a) yazının açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
    ///             schemeID = "GUID"       // Zorunlu alan
    ///         }
    ///     };
    /// 
    ///     var ilgiB = new CT_Ilgi()
    ///     {
    ///         Id = new CT_Id() { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },    // Zorunlu alan   
    ///         BelgeNo = "48154937-612.01.03-945",
    ///         Tarih = new DateTime(2009, 11, 5),
    ///         TarihSpecified = true,
    ///         Etiket = "b",      // Zorunlu alan                
    ///         Ad = new TextType() { Value = "İlgi (b) yazı" },
    ///         Aciklama = new TextType() { Value = "İlgi (b) yazının açıklaması" },
    ///         OzId = new IdentifierType()
    ///         {
    ///             Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
    ///             schemeID = "GUID"
    ///         }
    ///     };
    /// 
    ///     var ilgili = new CT_GercekSahis()   // Seçimli zorunlu (Kurum, gerçek, tüzel)
    ///     {
    ///         Kisi = new CT_Kisi()        // Zorunlu alan
    ///         {
    ///             OnEk = new TextType() { Value = "Uzman" },
    ///             IlkAdi = new NameType() { Value = "Hüseyin" },      // Zorunlu alan
    ///             Soyadi = new NameType() { Value = "Yetmez" }        // Zorunlu alan
    ///         },
    ///         Gorev = new TextType() { Value = "Birim Sorumlusu" },
    ///         TCKN = "12345678903",
    ///         IletisimBilgisi = new CT_IletisimBilgisi()
    ///         {
    ///             Telefon = "0-505-4821111",
    ///             TelefonDiger = "0-312-2444444"
    ///         }
    ///     };
    /// 
    ///     var olusturan = new CT_KurumKurulus()
    ///     {
    ///         KKK = "24301013",     // Zorunlu alan
    ///         Adi = new NameType() { Value = "Başbakanlık" },
    ///         IletisimBilgisi = new CT_IletisimBilgisi()
    ///         {
    ///             Telefon = "0-312-2947586",
    ///             EPosta = "info@basbakanlik.gov.tr",
    ///             Faks = "0-312-2663333",
    ///             Adres = new TextType() { Value = "Kızılay" },
    ///             Ilce = new NameType() { Value = "Çankaya" },
    ///             Il = new NameType() { Value = "Ankara" },
    ///             Ulke = new NameType() { Value = "Türkiye" },
    ///             WebAdresi = "www.basbakanlik.gov.tr"
    ///         }
    ///     };
    /// 
    ///     var dagitim1 = new CT_Dagitim()
    ///     {
    ///         DagitimTuru = ST_KodDagitimTuru.GRG,        // Zorunlu alan
    ///         Ivedilik = ST_KodIvedilik.ACL,      // Zorunlu alan
    ///         Item = new CT_KurumKurulus()        // Seçimli zorunlu
    ///         {
    ///             KKK = "24301050",      // Zorunlu alan
    ///             Adi = new NameType() { Value = "Adalet Bakanlığı" },
    ///             IletisimBilgisi = new CT_IletisimBilgisi()
    ///             {
    ///                 Telefon = "0-312-2665555",
    ///                 EPosta = "info@adalet.gov.tr",
    ///                 Faks = "0-312-2665556",
    ///                 Adres = new TextType() { Value = "Kızılay" },
    ///                 Ilce = new NameType() { Value = "Çankaya" },
    ///                 Il = new NameType() { Value = "Ankara" },
    ///                 Ulke = new NameType() { Value = "Türkiye" },
    ///                 WebAdresi = "www.adalet.gov.tr"
    ///             },
    ///             BirimKKK = "24301051"
    ///         },
    ///         KonulmamisEkListesi = new CT_KonulmamisEk[]
    ///         {
    ///             new CT_KonulmamisEk()       // Zorunlu alan
    ///             {
    ///                 EkId = konulmamisEk.Id.Value     // Zorunlu alan
    ///             }
    ///         }
    ///     };
    /// 
    ///     var dagitim2 = new CT_Dagitim()
    ///     {
    ///         DagitimTuru = ST_KodDagitimTuru.BLG, // Zorunlu alan
    ///         Ivedilik = ST_KodIvedilik.GNL,      // Zorunlu alan
    ///         Item = new CT_KurumKurulus()
    ///         {
    ///             KKK = "24322010",     // Zorunlu alan
    ///             Adi = new NameType() { Value = "Sağlık Bakanlığı" }
    ///         }
    ///     };
    ///     var dogrulamaBilgisi = new CT_DogrulamaBilgisi
    ///     {
    ///         DogrulamaAdresi = "https://www.turkiye.gov.tr/belge-dogrulama"
    ///     }; 
    ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Olustur))
    ///     {
    ///         paket.Ustveri.BelgeIdBelirle(paketId);      // Zorunlu alan
    ///         paket.Ustveri.DogrulamaBilgisiEkle(dogrulamaBilgisi); //Zorunlu alan
    ///         paket.Ustveri.KonuBelirle(paketKonu);       // Zorunlu alan
    ///         paket.Ustveri.GuvenlikKoduBelirle(ST_KodGuvenlikKodu.HZO);      // Zorunlu alan
    ///         paket.Ustveri.GuvenlikGecerlilikTarihiBelirle(islemTarihi.AddYears(10));
    ///         paket.Ustveri.OzIdBelirle("8CEA7FF7-75F2-4CCF-B3C1-1B9B2054E8E9", "GUID");
    ///         paket.Ustveri.DagitimEkle(dagitim1);        // Zorunlu alan
    ///         paket.Ustveri.DagitimEkle(dagitim2);        // Zorunlu alan
    ///         paket.Ustveri.EkEkle(ek1);
    ///         paket.Ustveri.EkEkle(ek2);
    ///         paket.Ustveri.EkEkle(ek3);
    ///         paket.Ustveri.EkEkle(ek4);
    ///         paket.Ustveri.EkEkle(ek5);
    ///         paket.Ustveri.IlgiEkle(ilgiA);
    ///         paket.Ustveri.IlgiEkle(ilgiB);
    ///         paket.Ustveri.DilBelirle("tur");
    ///         paket.Ustveri.OlusturanBelirle(olusturan);      // Zorunlu alan
    ///         paket.Ustveri.IlgiliEkle(ilgili);
    ///         paket.Ustveri.DosyaAdiBelirle(System.IO.Path.GetFileName(ustYaziDosyasi));      // Zorunlu alan
    ///         paket.Ustveri.SdpEkle(new CT_SDP() { Ad = "İç Genelgeler", Aciklama = "", Kod = "010.06.01" });
    ///         paket.Ustveri.DigerSdpEkle(new CT_SDP() { Ad = "Duyurular", Aciklama = "", Kod = "010.07.01" });
    ///         paket.Ustveri.DigerSdpEkle(new CT_SDP() { Ad = "Rehber Kılavuz", Aciklama = "", Kod = "010.08" });
    ///
    ///         paket.EkEkle(ek1, ekDosyasi1);
    ///         paket.EkEkle(ek4, ekDosyasi2, OzetModu.Yok);
    ///         paket.EkEkle(ek5, ekDosyasi3, OzetModu.SHA256);
    ///         paket.UstYaziEkle(ustYaziDosyasi, "application/pdf", OzetModu.SHA256);        // Zorunlu alan
    ///
    ///         paket.EkleriKontrolEt();
    ///         paket.IlgileriKontrolEt();
    ///
    ///         paket.UstveriOlustur();     // Zorunlu alan
    /// 
    ///         paket.ParafOzetiOlustur();
    /// 
    ///         Stream parafOzeti = paket.ParafOzetiAl();
    ///         Byte[] imzaliParafOzeti = Imzala(StreamToByteArray(parafOzeti));
    ///         paket.ParafImzaEkle(imzaliParafOzeti);         // Zorunlu alan
    /// 
    ///         paket.PaketOzetiOlustur();
    ///         Stream paketOzeti = paket.PaketOzetiAl();
    ///         Byte[] imzaliPaketOzeti = Imzala(StreamToByteArray(paketOzeti));
    ///         paket.ImzaEkle(imzaliPaketOzeti);         // Zorunlu alan
    ///     
    ///        var imzaci1 =  new CT_Imza()
    ///         {
    ///             Imzalayan = new CT_GercekSahis()        // Zorunlu alan
    ///             {
    ///                 Kisi = new CT_Kisi()        // Zorunlu alan
    ///                 {
    ///                     IlkAdi = new NameType() { Value = "Osman" },        // Zorunlu alan
    ///                     Soyadi = new NameType() { Value = "Yeşil" },        // Zorunlu alan
    ///                     Unvan = new NameType() { Value = "Büyükelçi" },
    ///                     IkinciAdi = new NameType() { Value = "Murat" },
    ///                     OnEk = new TextType() { Value = "Dr." }
    ///                 },
    ///                 TCKN = "12345678902",
    ///                 Gorev = new TextType() { Value = "Daire Başkanı" },
    ///                 IletisimBilgisi = new CT_IletisimBilgisi()
    ///                 {
    ///                     Telefon = "0-312-2666666",
    ///                     TelefonDiger = "0-532-4828282",
    ///                     EPosta = "om_yesil@xyz.com.tr",
    ///                     Faks = "0-312-2888888",
    ///                     Adres = new TextType() { Value = "T.C Cumhurbaşkanlığı Külliyesi 06560 Beştepe - Ankara" },
    ///                     Ilce = new NameType() { Value = "Çankaya" },
    ///                     Il = new NameType() { Value = "Ankara" },
    ///                     Ulke = new NameType() { Value = "Türkiye" },
    ///                     WebAdresi = "https://www.cbddo.gov.tr/"
    ///                 }
    ///             },
    ///             YetkiDevreden = new CT_GercekSahis()
    ///             {
    ///                 Kisi = new CT_Kisi()   // Zorunlu alan
    ///                 {
    ///                     IlkAdi = new NameType() { Value = "Hasan" },        // Zorunlu alan
    ///                     Soyadi = new NameType() { Value = "Aydın" },        // Zorunlu alan
    ///                     Unvan = new NameType() { Value = "Genel Müdür" },
    ///                     IkinciAdi = new NameType() { Value = "Şaban" },
    ///                     OnEk = new TextType() { Value = "Uzman" }
    ///                },
    ///                 TCKN = "12345588901",
    ///                Gorev = new TextType() { Value = "Genel Müdür" },
    ///                 IletisimBilgisi = new CT_IletisimBilgisi()
    ///                 {
    ///                     Telefon = "0-312-2999999",
    ///                     TelefonDiger = "0-532-4827777",
    ///                     EPosta = "iletisim@abc.com",
    ///                     Faks = "0-312-2946345",
    ///                     Adres = new TextType() { Value = "TUİK Necatibey Cad. No:112" },
    ///                     Ilce = new NameType() { Value = "Çankaya" },
    ///                     Il = new NameType() { Value = "Ankara" },
    ///                     Ulke = new NameType() { Value = "Türkiye" },
    ///                     WebAdresi = "www.tuik.gov.tr"
    ///                 }
    ///             },
    ///             VekaletVeren = new CT_GercekSahis()
    ///             {
    ///                 Kisi = new CT_Kisi()  // Zorunlu alan
    ///                 {
    ///                     IlkAdi = new NameType() { Value = "Semih" },  // Zorunlu alan
    ///                     Soyadi = new NameType() { Value = "Yılmaz" },  // Zorunlu alan
    ///                     Unvan = new NameType() { Value = "Dr." }
    ///                 },
    ///                 Gorev = new TextType() { Value = "Müsteşar" }
    ///             },
    ///             Makam = new NameType() { Value = "Müsteşar" },
    ///             Amac = new TextType() { Value = "Onay" },
    ///             Aciklama = new TextType() { Value = "İmzanın açıklaması" },
    ///             Tarih = DateTime.Now,
    ///             TarihSpecified = true
    ///         };
    /// 
    /// 
    ///        var imzaci2 = new CT_Imza()
    ///         {
    ///             Imzalayan = new CT_GercekSahis()        // Zorunlu alan
    ///             {
    ///                 Kisi = new CT_Kisi()        // Zorunlu alan
    ///                 {
    ///                     IlkAdi = new NameType() { Value = "Metin" },        // Zorunlu alan
    ///                     Soyadi = new NameType() { Value = "Demir" },        // Zorunlu alan
    ///                     OnEk = new TextType() { Value = "Dr." }
    ///                 },
    ///                 TCKN = "98787678902",
    ///                 Gorev = new TextType() { Value = "Şube Müdürü" },
    ///             },
    ///             Makam = new NameType() { Value = "Daire" },
    ///             Amac = new TextType() { Value = "Onay" },
    ///             Aciklama = new TextType() { Value = "İmzanın açıklaması" },
    ///             Tarih = DateTime.Now,
    ///             TarihSpecified = true
    ///         };
    /// 
    ///         paket.NihaiUstveri.BelgeNoBelirle("1234");
    ///         paket.NihaiUstveri.TarihBelirle(DateTime.Now);
    /// 
    ///         paket.NihaiUstveri.ImzaEkle(imzaci1);
    ///         paket.NihaiUstveri.ImzaEkle(imzaci2);
    ///         paket.NihaiUstveriOlustur();
    ///         paket.CoreOlustur();
    ///         paket.NihaiOzetOlustur();
    /// 
    ///         Stream nihaiOzet = paket.NihaiOzetAl();
    ///         Byte[] imzaliNihaiOzet = Imzala(StreamToByteArray(nihaiOzet));
    ///         paket.MuhurEkle(imzaliNihaiOzet);         // Zorunlu alan
    /// 
    ///         paket.Kapat();
    ///     }
    ///     Console.WriteLine("Paket oluşturuldu.");
    /// }
    /// </code>
    /// Yardımcı metodlar.
    /// <code>
    /// static byte[] Imzala(byte[] imzalanacakVeri)
    /// {
    ///     return new byte[] { 1, 2, 3 }; // burada imzalanacakVeri herhangi bir imzala API'si ile imzalanarak imzali veri donulur
    /// }
    /// 
    /// static byte[] Muhurle(byte[] muhurlenecekVeri)
    /// {
    ///     return new byte[] { 1, 2, 3 }; // burada muhurlenecekVeri herhangi bir muhur API'si ile muhurlenerek donulur
    /// }
    /// 
    /// static byte[] StreamToByteArray(Stream input)
    /// {
    ///     byte[] buffer = new byte[16 * 1024];
    ///     using (MemoryStream ms = new MemoryStream())
    ///     {
    ///         int read;
    ///         while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
    ///         {
    ///             ms.Write(buffer, 0, read);
    ///         }
    ///         return ms.ToArray();
    ///     }
    /// }
    /// 
    /// static byte[] PaketSifrele(String dosyaYolu)
    /// {
    ///     return System.IO.File.ReadAllBytes(dosyaYolu);  // Burada, şifrelenen paketin geri döndürülmesi gerekiyor.
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public class Paket : IDisposable
    {
        internal Package _package;
        internal PaketModu _paketModu;

        private Stream _streamInternal;

        /// <summary>   Belgeye ait üstveri alanlarını barıdıran objeye ulaşılır. </summary>
        /// <value> Ustveri nesnesi. </value>
        public Ustveri Ustveri { get; private set; }

        /// <summary>   Belgeye ait nihai üstveri alanlarını barıdıran objeye ulaşılır. </summary>
        /// <value> NihaiUstveri nesnesi. </value>
        public NihaiUstveri NihaiUstveri { get; private set; }

        /// <summary>Paket içerisinde imzalanan bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> PaketOzeti nesnesi. </value>
        public PaketOzeti PaketOzeti { get; private set; }

        /// <summary>Paket içerisinde paraflanan bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> ParafOzeti nesnesi. </value>
        public ParafOzeti ParafOzeti { get; private set; }

        /// <summary>Paket içerisinde mühürlenen bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.</summary>
        /// <value> NihaiOzet nesnesi. </value>
        public NihaiOzet NihaiOzet { get; private set; }

        /// <summary>
        /// PaketOzeti'ine eklenecek özetlerin oluşturulmasında kullanılacak algoritmayı belirtir.
        /// </summary>
        public OzetModu VarsayilanOzetModu { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Paket()
        {
            VarsayilanOzetModu = OzetModu.SHA384;
        }

        #region Aç Kapat

        /// <summary>
        /// Api versiyon numarası kontrol eder. Eğer oluşturulan paketin versiyon numarası 2'den küçük ise hata verir.
        /// </summary>
        /// <param name="package"></param>
        private static void VersiyonNumarasiKontrolEt(Package package)
        {
            if (!string.IsNullOrEmpty(package.PackageProperties.Version))
            {
                var versiyonNumarasiParcalari = package.PackageProperties.Version.Split('.');
                if (versiyonNumarasiParcalari.Length != 2)
                {
                    throw new Exception("e-Yazışma paketi versiyon numarası alınamadı.");
                }

                if (int.TryParse(versiyonNumarasiParcalari[0], out var ilkBolum))
                {
                    if (ilkBolum < 2)
                    {
                        throw new Exception($"e-Yazışma paketi version numarası {package.PackageProperties.Version}. Versiyon 2 API önceki versiyon API'ler ile oluşturulmuş e-Yazışma paketlerini desteklememektedir. Lütfen önceki sürümleri açabilmek için version 1 API'leri kullanınız.");
                    }
                }
                else
                {
                    throw new Exception("e-Yazışma paketi versiyon numarası alınamadı.");
                }
            }
            else
            {
                throw new Exception("e-Yazışma paketi versiyon numarası alınamadı.");
            }
        }

        /// <summary>Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.</summary>
        /// <param name="stream">       Pakete ilişkin STREAM objesidir. </param>
        /// <param name="paketModu">    Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>  İşlem yapılan paket objesi. </returns>
        /// <example>
        /// Mevcut bir paketi açma örneği.
        /// <code>
        /// static void PaketAc(String dosyaYolu)
        /// {
        ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Ac))
        ///     {
        ///         var paketOzetiDogrulamaSonucu = new List&lt;OzetDogrulamaHatasi&gt;();
        ///         var nihaiOzetDogrulamaSonucu = new List&lt;OzetDogrulamaHatasi&gt;();
        /// 
        ///         if ((paket.Ustveri.EkleriAl() != null) &amp;&amp; (paket.Ustveri.EkleriAl().Count() &gt; 0))
        ///         {
        ///             var dedEkler = paket.Ustveri.EkleriAl().Where(x =&gt; x.Tur == ST_KodEkTuru.DED);
        ///             if ((dedEkler != null) &amp;&amp; (dedEkler.Count() &gt; 0))
        ///             {
        ///                 if (!System.IO.Directory.Exists(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu)))
        ///                     System.IO.Directory.CreateDirectory(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu));
        ///                 foreach (var dedEk in dedEkler)
        ///                 {
        ///                     var ek = paket.EkAl(new Guid(dedEk.Id.Value));
        ///                     if (ek != null)
        ///                         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + dedEk.DosyaAdi))
        ///                         {
        ///                             Cbddo.eYazisma.Tipler.Araclar.CopyStream(ek, s);
        ///                         }
        ///                 }
        ///             }
        ///         }
        /// 
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\ImzaCades.imz"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(paket.ImzaAl(), s);
        ///         }
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\ParafImzaCades.imz"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(paket.ParafImzaAl(), s);
        ///         }
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\MuhurCades.imz"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(paket.MuhurAl(), s);
        ///         }
        /// 
        ///         var belgeImza = paket.BelgeImzaAl();
        ///         var muhur = paket.MuhurAl();
        ///         var nihaiOzetler = paket.NihaiOzet.OzetleriAl();
        ///         var nihaiOzet = paket.NihaiOzetAl();
        ///         var paketOzetleri = paket.PaketOzeti.OzetleriAl();
        ///         var paketOzetiDogrulamasi = paket.PaketOzetiDogrula(Araclar.PaketOzetiAl(paket.PaketOzetiAl()), ref paketOzetiDogrulamaSonucu);
        ///         var nihaiOzetDogrulamasi = paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(paket.NihaiOzetAl()), ref nihaiOzetDogrulamaSonucu);
        ///         var balgeId = paket.Ustveri.BelgeIdAl();
        ///         var dagitimlar = paket.Ustveri.DagitimlariAl();
        ///         var dil = paket.Ustveri.DilAl();
        ///         var ekler = paket.Ustveri.EkleriAl();
        ///         var guvenlikGecerlilikTarihi = paket.Ustveri.GuvenlikGecerlilikTarihiAl();
        ///         var guvenlikKodu = paket.Ustveri.GuvenlikKoduAl();
        ///         var ilgiler = paket.Ustveri.IlgileriAl();
        ///         var ilgililer = paket.Ustveri.IlgilileriAl();
        ///         var konu = paket.Ustveri.KonuAl();
        ///         var mimeTuru = paket.Ustveri.MimeTuruAl();
        ///         var olusturan = paket.Ustveri.OlusturanAl();
        ///         var ozId = paket.Ustveri.OzIdAl();
        ///         var ustVeri = paket.UstveriAl();
        ///         var ustYazi = paket.UstYaziAl();
        ///         var parafOzeti = paket.ParafOzetiAl();
        ///         var paketOzeti = paket.PaketOzetiAl();
        ///         var nihaiUstveri = paket.NihaiUstveriAl();
        ///         var nihaiOzet = paket.NihaiOzetAl();
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "Ustveri.xml"))
        ///         {
        ///             KaCbddolkinma.eYazisma.Tipler.Araclar.CopyStream(ustVeri, s);
        ///         }
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "ustyazi.pdf"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(ustYazi, s);
        ///         }
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "ParafOzeti.xml"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(parafOzeti, s);
        ///         }
        ///         
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "PaketOzeti.xml"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(paketOzeti, s);
        ///         }
        /// 
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "NihaiUstveri.xml"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(nihaiUstveri, s);
        ///         }
        ///     
        ///         using (Stream s = File.OpenWrite(System.IO.Path.GetFileNameWithoutExtension(dosyaYolu) + "\\" + "NihaiOzet.xml"))
        ///         {
        ///             Cbddo.eYazisma.Tipler.Araclar.CopyStream(nihaiOzet, s);
        ///         }
        ///     }
        /// } 
        /// </code>
        /// </example>
        public static Paket Ac(Stream stream, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    var paket = new Paket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite)
                    };

                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni birden fazla.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ÜstYazı\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÜstveri\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ParafÖzeti\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni birden fazla olamaz.");
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 1)
                    {
                        try
                        {
                            CT_Ustveri readedUstveri = (CT_Ustveri)(new XmlSerializer(typeof(CT_Ustveri))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.Ustveri = new UstveriInternal(readedUstveri);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni hatalı.", ex);
                        }
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).Count() == 1)
                    {
                        try
                        {
                            CT_NihaiUstveri readedNihaiUstveri = (CT_NihaiUstveri)(new XmlSerializer(typeof(CT_NihaiUstveri))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.NihaiUstveri = new NihaiUstveriInternal(readedNihaiUstveri);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÜstveri\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paket.NihaiUstveri = new NihaiUstveriInternal();
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() == 1)
                    {
                        try
                        {
                            CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.PaketOzeti = new PaketOzetiInternal(readedPaketOzeti);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paket.PaketOzeti = new PaketOzetiInternal();
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).Count() == 1)
                    {
                        try
                        {
                            CT_ParafOzeti readedParafOzeti = (CT_ParafOzeti)(new XmlSerializer(typeof(CT_ParafOzeti))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.ParafOzeti = new ParafOzetiInternal(readedParafOzeti);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"ParafÖzeti\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paket.ParafOzeti = new ParafOzetiInternal();
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() == 1)
                    {
                        try
                        {
                            CT_NihaiOzet readedNihaiOzet = (CT_NihaiOzet)(new XmlSerializer(typeof(CT_NihaiOzet))).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri)).GetStream(FileMode.Open));
                            paket.NihaiOzet = new NihaiOzetInternal(readedNihaiOzet);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paket.NihaiOzet = new NihaiOzetInternal();
                    }
                    paket._paketModu = paketModu;
                    paket._streamInternal = stream;
                    return paket;
                case PaketModu.Ac:
                    var paketAcilan = new Paket
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.Read)
                    };
                    if (!paketAcilan._package.GetRelationships().Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"İlişki\" bileşeni bulunamadı");
                    }
                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni birden fazla.");
                    }

                    VersiyonNumarasiKontrolEt(paketAcilan._package);

                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Üstveri\" bileşeni birden fazla.");
                    }
                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ÜstYazı\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ÜstYazı\" bileşeni birden fazla.");
                    }
                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiUstveri\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiUstveri\" bileşeni birden fazla olamaz.");
                    }
                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketÖzeti\" bileşeni birden fazla.");
                    }
                    if (!paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Any())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni yok.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiÖzet\" bileşeni birden fazla olamaz.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).Count() > 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"ParafÖzeti\" bileşeni birden fazla olamaz.");
                    }


                    try
                    {
                        Uri readedUstveriUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri);
                        CT_Ustveri readedUstveriAcilan = (CT_Ustveri)new XmlSerializer(typeof(CT_Ustveri)).Deserialize(paketAcilan._package.GetPart(readedUstveriUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.Ustveri = new UstveriInternal(readedUstveriAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Ustveri\" bileşeni hatalı.", ex);
                    }


                    try
                    {
                        Uri readedNihaiUstveriUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIUSTVERI).First().TargetUri);
                        CT_NihaiUstveri readedNihaiUstveriAcilan = (CT_NihaiUstveri)new XmlSerializer(typeof(CT_NihaiUstveri)).Deserialize(paketAcilan._package.GetPart(readedNihaiUstveriUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.NihaiUstveri = new NihaiUstveriInternal(readedNihaiUstveriAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiUstveri\" bileşeni hatalı.", ex);
                    }

                    try
                    {
                        Uri readedPaketOzetiUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).First().TargetUri);
                        CT_PaketOzeti readedPaketOzetiAcilan = (CT_PaketOzeti)new XmlSerializer(typeof(CT_PaketOzeti)).Deserialize(paketAcilan._package.GetPart(readedPaketOzetiUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.PaketOzeti = new PaketOzetiInternal(readedPaketOzetiAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"PaketOzeti\" bileşeni hatalı.", ex);
                    }

                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).Count() != 0)
                    {
                        try
                        {
                            Uri readedParafOzetiUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_PARAFOZETI).First().TargetUri);
                            CT_ParafOzeti readedParafOzetiAcilan = (CT_ParafOzeti)new XmlSerializer(typeof(CT_ParafOzeti)).Deserialize(paketAcilan._package.GetPart(readedParafOzetiUriAcilan).GetStream(FileMode.Open));
                            paketAcilan.ParafOzeti = new ParafOzetiInternal(readedParafOzetiAcilan);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"ParafOzeti\" bileşeni hatalı.", ex);
                        }
                    }

                    try
                    {
                        Uri readedNihaiOzetUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri);
                        CT_NihaiOzet readedNihaiOzetAcilan = (CT_NihaiOzet)new XmlSerializer(typeof(CT_NihaiOzet)).Deserialize(paketAcilan._package.GetPart(readedNihaiOzetUriAcilan).GetStream(FileMode.Open));
                        paketAcilan.NihaiOzet = new NihaiOzetInternal(readedNihaiOzetAcilan);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiOzet\" bileşeni hatalı.", ex);
                    }

                    if (!paketAcilan.ImzaVarMi())
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Imza\" bileşeni bulunamadı.");
                    }

                    paketAcilan._paketModu = paketModu;
                    paketAcilan._streamInternal = stream;

                    return paketAcilan;

                case PaketModu.Olustur:
                    var paketOlusturulan = new Paket
                    {
                        Ustveri = new UstveriInternal(),
                        NihaiUstveri = new NihaiUstveriInternal(),
                        PaketOzeti = new PaketOzetiInternal(),
                        ParafOzeti = new ParafOzetiInternal(),
                        NihaiOzet = new NihaiOzetInternal(),
                        _package = Package.Open(stream, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    };
                    paketOlusturulan._paketModu = paketModu;
                    paketOlusturulan._streamInternal = stream;
                    return paketOlusturulan;
                default:
                    return null;
            }
        }

        /// <summary>Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.</summary>
        /// <param name="dosyaYolu">Pakete ilişkin dosya yoludur. </param>
        /// <param name="paketModu">Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>   İşlem yapılan paket objesi. </returns>
        public static Paket Ac(string dosyaYolu, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    return Ac(File.Open(dosyaYolu, FileMode.Open, FileAccess.ReadWrite), paketModu);
                case PaketModu.Ac:
                    return Ac(File.Open(dosyaYolu, FileMode.Open), paketModu);
                case PaketModu.Olustur:
                    return Ac(File.Open(dosyaYolu, FileMode.OpenOrCreate, FileAccess.ReadWrite), paketModu);
                default:
                    return null;
            }

        }

        /// <summary>Üstveri'de ek olarak belirtilmiş ilgiye ilişkin ekin paket içerisinde bulunup bulunmadığını kontrol eder.</summary>
        /// <remarks>Paket oluşturma sırasında, <see cref="Paket.Kapat"/> metodundan önce kullanılmalıdır.</remarks>
        /// <exception cref="System.ApplicationException">Ek olarak belirtilmiş ilgiye ait ek paket içinde yoksa oluşur.</exception>
        public void IlgileriKontrolEt()
        {
            if (Ustveri.CT_Ustveri.Ilgiler != null)
            {
                var ekOlarakBelirtilmisIlgiler = Ustveri.CT_Ustveri.Ilgiler.Where(ilgi => !(ilgi.EkId.IsNullOrWhiteSpace())).ToList();
                if (ekOlarakBelirtilmisIlgiler.Count > 0)
                {
                    if (Ustveri.CT_Ustveri.Ekler == null || !Ustveri.CT_Ustveri.Ekler.Any())
                    {
                        throw new ApplicationException("Üstveri bileşeninde verilen ilgilerden, ek olarak belirtilmiş ilgi için, paket içerisine eklenmiş Ek bileşeni bulunamadı.");
                    }
                    foreach (var ekOlarakBelirtilmisIlgi in ekOlarakBelirtilmisIlgiler)
                    {
                        if (Ustveri.CT_Ustveri.Ekler.Where(ek => string.Compare(ek.Id.Value.ToString(), ekOlarakBelirtilmisIlgi.EkId, true) == 0).Count() == 0)
                        {
                            throw new ApplicationException("Üstveri bileşeninde verilen ilgilerden, ek olarak belirtilmiş ilgi için, paket içerisine eklenmiş Ek bileşeni bulunamadı.");
                        }
                    }
                }

                if (Ustveri.CT_Ustveri.Ilgiler != null && Ustveri.CT_Ustveri.Ilgiler.Select(x => x.Etiket).Distinct().Count() < Ustveri.CT_Ustveri.Ilgiler.Count())
                {
                    throw new ApplicationException("Paket içerisine aynı Etiket değerine sahip birden fazla ilgi eklenemez.");
                }
            }
        }

        /// <summary>Üstveri'de Dahili Elektronik Dosya türünden ek olarak belirtilmiş ekin paket içerisinde bulunup bulunmadığını kontrol eder. Pakete eklenmiş ek dosyalarının üstveride belirtilip belirtilmediğini kontrol eder.</summary>
        /// <remarks>Paket oluşturma sırasında, <see cref="Paket.Kapat"/> metodundan önce kullanılmalıdır.</remarks>
        /// <param name="dagitimIdentifier">Ekleri kontrol ederken verilen (KKK, TCKN veya Id) değerine göre dağıtım için ek konulmamış ise hata vermez</param>
        /// <exception cref="System.ApplicationException">Üstveri ile paket içerisindeki eklerin uyumsuz olması durumunda oluşur.</exception>
        public void EkleriKontrolEt(IdentifierType dagitimIdentifier = null)
        {
            if (Ustveri.CT_Ustveri.Ekler != null)
            {
                foreach (var ustveriEki in Ustveri.CT_Ustveri.Ekler)
                {
                    if (ustveriEki.Tur == ST_KodEkTuru.DED)
                    {
                        if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK).Where(xRelationship => string.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0).Count() == 0)
                        {
                            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_IMZASIZEK).Where(xRelationship => string.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_IMZASIZEK + ustveriEki.Id.Value, true) == 0).Count() == 0)
                            {
                                if (dagitimIdentifier == null)
                                {
                                    throw new ApplicationException(string.Format("Üstveri bileşeni için eklenen ek, paket içerisine eklenmemiş. EkId:{0}", ustveriEki.Id.Value));
                                }

                                var dagitimlar = Ustveri.DagitimlariAl();
                                if (dagitimlar == null || dagitimlar.Length == 0)
                                {
                                    throw new ApplicationException("Paket içerisine eklenmiş dağıtımlar bulunamadı.");
                                }

                                CT_Dagitim ctDagitim = null;
                                foreach (var dagitim in dagitimlar)
                                {
                                    if (dagitim.Item is CT_KurumKurulus)
                                    {
                                        var kurumKurulus = (CT_KurumKurulus)dagitim.Item;

                                        if (!string.IsNullOrWhiteSpace(kurumKurulus.KKK) && kurumKurulus.KKK == dagitimIdentifier.Value)
                                        {
                                            ctDagitim = dagitim;
                                            break;
                                        }
                                    }
                                    else if (dagitim.Item is CT_TuzelSahis)
                                    {
                                        var tuzelSahis = (CT_TuzelSahis)dagitim.Item;
                                        if (tuzelSahis.Id != null && string.IsNullOrWhiteSpace(tuzelSahis.Id.Value) && tuzelSahis.Id.Value == dagitimIdentifier.Value)
                                        {
                                            ctDagitim = dagitim;
                                            break;
                                        }
                                    }
                                    else if (dagitim.Item is CT_GercekSahis)
                                    {
                                        var gercekSahis = (CT_GercekSahis)dagitim.Item;
                                        if (!string.IsNullOrWhiteSpace(gercekSahis.TCKN) && gercekSahis.TCKN == dagitimIdentifier.Value)
                                        {
                                            ctDagitim = dagitim;
                                            break;
                                        }
                                    }

                                }
                                if (ctDagitim == null || ctDagitim.KonulmamisEkListesi == null || ctDagitim.KonulmamisEkListesi.Length == 0 || !ctDagitim.KonulmamisEkListesi.Any(p => p.EkId == ustveriEki.Id.Value))
                                {
                                    throw new ApplicationException(string.Format("Üstveri bileşeni için eklenen ek, paket içerisine eklenmemiş. EkId:{0}", ustveriEki.Id.Value));
                                }
                            }
                        }
                    }
                }
            }
            foreach (var relationship in _package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK))
            {
                if (Ustveri.CT_Ustveri.Ekler == null)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş eklerin hiç biri, üstveri bileşeninde belirtilmemiş.");
                }
                var UstveriEkleri = Ustveri.CT_Ustveri.Ekler.Where(ustveriEki => String.Compare(relationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0);
                if (UstveriEkleri.Count() == 0)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde belirtilmemiş.");
                }
                else
                {
                    if (UstveriEkleri.First().Tur != ST_KodEkTuru.DED)
                    {
                        throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde DED (Dahili Elektronik Dosya) olarak belirtilmelidir.");
                    }
                }
            }
            foreach (var relationship in _package.GetRelationshipsByType(Araclar.RELATION_TYPE_IMZASIZEK))
            {
                if (Ustveri.CT_Ustveri.Ekler == null)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş eklerin hiç biri, üstveri bileşeninde belirtilmemiş.");
                }
                var UstveriEkleri = Ustveri.CT_Ustveri.Ekler.Where(ustveriEki => String.Compare(relationship.Id.ToString(), Araclar.ID_ROOT_IMZASIZEK + ustveriEki.Id.Value, true) == 0);
                if (UstveriEkleri.Count() == 0)
                {
                    throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde belirtilmemiş.");
                }
                else
                {
                    if (UstveriEkleri.First().Tur != ST_KodEkTuru.DED)
                    {
                        throw new ApplicationException("Paket içerisine eklenmiş ek, üstveri bileşeninde DED (Dahili Elektronik Dosya) olarak belirtilmelidir.");
                    }
                }
            }
            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Where(x => x.SiraNo <= 0).Count() > 0)
            {
                throw new ApplicationException("Paket içerisine SiraNo değeri '1'den küçük olan ek eklenemez.");
            }
            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Select(x => x.SiraNo).Distinct().Count() < Ustveri.CT_Ustveri.Ekler.Count())
            {
                throw new ApplicationException("Paket içerisine aynı SiraNo değerine sahip birden fazla ek eklenemez.");
            }
            foreach (var item in Ustveri.CT_Ustveri.DagitimListesi.Where(x => x.KonulmamisEkListesi != null && x.KonulmamisEkListesi.Length > 0).Select(x => x.KonulmamisEkListesi).SelectMany(x => x))
            {
                if (Ustveri.CT_Ustveri.Ekler.Where(x => x.Id.Value.ToString().ToUpperInvariant() == item.EkId.ToUpperInvariant()).Count() == 0)
                {
                    throw new ApplicationException("UstVeri'de belirtilmemiş bir ek, DagitimListesinde KonulmamisEk olarak belirtilemez.");
                }
            }
        }

        /// <summary>Core bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <remarks> Java API için açıklama. .Net API kullanıyorsanız bu açıklamyı dikkate almayınız. 
        /// Kullanılan Apache POI API'sı, 'core' bileşeninin oluşturulması aşamasına müdahaleye engel olduğundan dolayı, nihaiOzeteCoreEkle yardımcı metod kullanılarak Core bileşeni özeti NihaiOzet'e eklenir. </remarks>
        public void CoreOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için Core fonksiyonu kullanılamaz.");
            }

            PaketBelirteciBelirle(Ustveri.CT_Ustveri.BelgeId);
            PaketOlusturulmaTarihiBelirle(DateTime.Now);
            PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(Ustveri.CT_Ustveri.Olusturan));
            PaketKonusuBelirle(Ustveri.CT_Ustveri.Konu.Value);
            PaketKategorisiBelirle(Araclar.RESMIYAZISMA);
            PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
            PaketVersiyonuBelirle(Araclar.PAKET_VERSIYON);

            PaketRevizyonuBelirle(string.Format(Araclar.PAKET_REVIZYON, System.Reflection.Assembly.GetAssembly(typeof(Paket)).GetName().Version));

            _package.Flush();

        }

        /// <summary>PaketOzeti bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void PaketOzetiOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOzetiOlustur fonksiyonu kullanılamaz.");
            }

            if (UstveriVarMi() == false)
            {
                throw new ApplicationException("Üstveri bileşeni bulunmadığı durumlarda PaketOzetiOlustur fonksiyonu kullanılamaz.");
            }

            if (UstYaziVarMi() == false)
            {
                throw new ApplicationException("ÜstYazı bileşeni bulunmadığı durumlarda PaketOzetiOlustur fonksiyonu kullanılamaz.");
            }

            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
            using (Stream ustVeriStream = UstveriAl())
            {
                byte[] ozetUstVeri = Araclar.OzetHesapla(ustVeriStream, VarsayilanOzetModu);
                ustVeriStream.Position = 0;
                byte[] ozetUstVeriSha512 = Araclar.OzetHesapla(ustVeriStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstVeri, ozetUstVeriSha512, partUriUstveri);
            }

            var partUriUstyazi = PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri);
            using (Stream ustYaziStream = UstYaziAl())
            {
                byte[] ozetUstYazi = Araclar.OzetHesapla(ustYaziStream, VarsayilanOzetModu);
                ustYaziStream.Position = 0;
                byte[] ozetUstYaziSha512 = Araclar.OzetHesapla(ustYaziStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstYazi, ozetUstYaziSha512, partUriUstyazi);
            }

            if (ParafOzetiVarMi())
            {
                var partUriParafOzeti = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative));
                using (Stream parafOzetiStream = ParafOzetiAl())
                {
                    byte[] ozetParafOzeti = Araclar.OzetHesapla(parafOzetiStream, VarsayilanOzetModu);
                    parafOzetiStream.Position = 0;
                    byte[] ozetParafOzetiSha512 = Araclar.OzetHesapla(parafOzetiStream, OzetModu.SHA512);
                    PaketOzeti.Ekle(VarsayilanOzetModu, ozetParafOzeti, ozetParafOzetiSha512, partUriParafOzeti);
                }
            }

            if (ParafImzaVarMi())
            {
                var partUriParafImza = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative));
                using (Stream parafImzaStream = ParafImzaAl())
                {
                    byte[] ozetParafImza = Araclar.OzetHesapla(parafImzaStream, VarsayilanOzetModu);
                    parafImzaStream.Position = 0;
                    byte[] ozetParafImzaSha512 = Araclar.OzetHesapla(parafImzaStream, OzetModu.SHA512);
                    PaketOzeti.Ekle(VarsayilanOzetModu, ozetParafImza, ozetParafImzaSha512, partUriParafImza);
                }
            }

            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Length > 0)
            {
                foreach (var ustveriEki in this.Ustveri.CT_Ustveri.Ekler)
                {
                    if (ustveriEki.Tur == ST_KodEkTuru.DED && ustveriEki.ImzaliMi == true)
                    {
                        var relationShip = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK).SingleOrDefault(xRelationship => String.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0);
                        using (var ekStream = _package.GetPart(relationShip.TargetUri).GetStream())
                        {
                            byte[] ekOzeti = Araclar.OzetHesapla(ekStream, VarsayilanOzetModu);
                            ekStream.Position = 0;
                            byte[] ekOzetiSha512 = Araclar.OzetHesapla(ekStream, OzetModu.SHA512);
                            PaketOzeti.Ekle(VarsayilanOzetModu, ekOzeti, ekOzetiSha512, relationShip.TargetUri);
                        }
                    }
                }
            }

            PaketOzeti.CT_PaketOzeti.Id = Ustveri.BelgeIdAl().ToUpperInvariant();
            PaketOzeti.KontrolEt();
            var partUriPaketOzeti = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriPaketOzeti))
                {
                    _package.DeletePart(partUriPaketOzeti);
                    _package.DeleteRelationship(Araclar.ID_PAKETOZETI);
                }
            }

            var partPaketOzeti = _package.CreatePart(partUriPaketOzeti, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partPaketOzeti.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_PAKETOZETI, Araclar.ID_PAKETOZETI);
            // Create the XmlSerializer for the CT_PaketOzeti type
            var xmlSerializer = new XmlSerializer(typeof(CT_PaketOzeti));

            // Open the stream for writing
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(partPaketOzeti.GetStream(), Encoding.UTF8)
            {
                // Indent the XML for better readability
                Formatting = Formatting.Indented
            })
            {
                // Serialize the CT_PaketOzeti object and write it to the stream
                xmlSerializer.Serialize(xmlTextWriter, PaketOzeti.CT_PaketOzeti);
            }

        }

        /// <summary>ParafOzeti bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void ParafOzetiOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için ParafOzetiOlustur fonksiyonu kullanılamaz.");
            }
            if (UstveriVarMi() == false)
            {
                throw new ApplicationException("Üstveri bileşeni bulunmadığı durumlarda ParafOzetiOlustur fonksiyonu kullanılamaz.");
            }
            if (UstYaziVarMi() == false)
            {
                throw new ApplicationException("ÜstYazı bileşeni bulunmadığı durumlarda ParafOzetiOlustur fonksiyonu kullanılamaz.");
            }

           
            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
            using (Stream ustVeriStream = UstveriAl())
            {
                byte[] ozetUstVeri = Araclar.OzetHesapla(ustVeriStream, VarsayilanOzetModu);
                ustVeriStream.Position = 0;
                byte[] ozetUstVeriSha512 = Araclar.OzetHesapla(ustVeriStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstVeri, ozetUstVeriSha512, partUriUstveri);
                ParafOzeti.Ekle(VarsayilanOzetModu, ozetUstVeri, ozetUstVeriSha512, partUriUstveri);
            }

            var partUriUstyazi = PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri);
            using (Stream ustYaziStream = UstYaziAl())
            {
                byte[] ozetUstYazi = Araclar.OzetHesapla(ustYaziStream, VarsayilanOzetModu);
                ustYaziStream.Position = 0;
                byte[] ozetUstYaziSha512 = Araclar.OzetHesapla(ustYaziStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstYazi, ozetUstYaziSha512, partUriUstyazi);
                ParafOzeti.Ekle(VarsayilanOzetModu, ozetUstYazi, ozetUstYaziSha512, partUriUstyazi);
            }

            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Length > 0)
            {
                foreach (var ustveriEki in this.Ustveri.CT_Ustveri.Ekler)
                {
                    if (ustveriEki.Tur == ST_KodEkTuru.DED && ustveriEki.ImzaliMi)
                    {
                        PackageRelationship relationShip = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK).SingleOrDefault(xRelationship => String.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0);

                        using (Stream ekStream = EkOzetAl(relationShip))
                        {
                            // Calculate the hash with the default mode
                            byte[] ekOzeti = Araclar.OzetHesapla(ekStream, VarsayilanOzetModu);

                            // Reset the stream position before calculating SHA512
                            ekStream.Position = 0;

                            // Calculate SHA512 hash
                            byte[] ekOzetiSha512 = Araclar.OzetHesapla(ekStream, OzetModu.SHA512);

                            // Add the calculated hashes to the ParafOzeti object
                            ParafOzeti.Ekle(VarsayilanOzetModu, ekOzeti, ekOzetiSha512, relationShip.TargetUri);
                        }
                        
                    }
                }
            }

            ParafOzeti.CT_ParafOzeti.Id = Ustveri.BelgeIdAl().ToUpperInvariant();
            ParafOzeti.KontrolEt();
            var partUriParafOzeti = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriParafOzeti))
                {
                    _package.DeletePart(partUriParafOzeti);
                    _package.DeleteRelationship(Araclar.ID_PARAFOZETI);
                }
            }

            var partParafOzeti = _package.CreatePart(partUriParafOzeti, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partParafOzeti.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_PARAFOZETI, Araclar.ID_PARAFOZETI);
            //var xmlSerializer = new XmlSerializer(typeof(CT_ParafOzeti));
            //XmlTextWriter xmlTextWriter = new XmlTextWriter(partParafOzeti.GetStream(), Encoding.UTF8)
            //{
            //    Formatting = Formatting.Indented
            //};
            //xmlSerializer.Serialize(xmlTextWriter, ParafOzeti.CT_ParafOzeti);
            // Create the XmlSerializer for the CT_ParafOzeti type
            var xmlSerializer = new XmlSerializer(typeof(CT_ParafOzeti));

            // Open the stream for writing with proper disposal
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(partParafOzeti.GetStream(), Encoding.UTF8)
            {
                Formatting = Formatting.Indented // Indent for better readability
            })
            {
                // Serialize the CT_ParafOzeti object and write it to the stream
                xmlSerializer.Serialize(xmlTextWriter, ParafOzeti.CT_ParafOzeti);
            }

        }

        /// <summary>NihaiOzet bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void NihaiOzetOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }

            if (UstveriVarMi() == false)
            {
                throw new ApplicationException("Üstveri bileşeni bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }
            if (UstYaziVarMi() == false)
            {
                throw new ApplicationException("ÜstYazı bileşeni bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }
            if (NihaiUstveriVarMi() == false)
            {
                throw new ApplicationException("NihaiÜstveri bileşeni bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }
            if (ImzaVarMi() == false)
            {
                throw new ApplicationException("Elektronik İmza bileşeni bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }

            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
            using (Stream ustVeriStream = UstveriAl())
            {
                byte[] ozetUstVeri = Araclar.OzetHesapla(ustVeriStream, VarsayilanOzetModu);
                ustVeriStream.Position = 0;
                byte[] ozetUstVeriSha512 = Araclar.OzetHesapla(ustVeriStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstVeri, ozetUstVeriSha512, partUriUstveri);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozetUstVeri, ozetUstVeriSha512, partUriUstveri);
            }

            var partUriUstyazi = PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri);
            using (Stream ustYaziStream = UstYaziAl())
            {
                byte[] ozetUstYazi = Araclar.OzetHesapla(ustYaziStream, VarsayilanOzetModu);
                ustYaziStream.Position = 0;
                byte[] ozetUstYaziSha512 = Araclar.OzetHesapla(ustYaziStream, OzetModu.SHA512);
                PaketOzeti.Ekle(VarsayilanOzetModu, ozetUstYazi, ozetUstYaziSha512, partUriUstyazi);
                NihaiOzet.Ekle(VarsayilanOzetModu, ozetUstYazi, ozetUstYaziSha512, partUriUstyazi);
            }

            if (ParafOzetiVarMi())
            {
                

                // Create the part URI for the second stream (ParafOzeti)
                var partUriParafOzeti = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative));

                using (Stream parafOzetiStream = ParafOzetiAl()) // Using 'using' for the second stream
                {
                    byte[] ozetParafOzeti = Araclar.OzetHesapla(parafOzetiStream, VarsayilanOzetModu);
                    parafOzetiStream.Position = 0;
                    byte[] ozetParafOzetiSha512 = Araclar.OzetHesapla(parafOzetiStream, OzetModu.SHA512);

                    // Add the calculated hashes to the NihaiOzet object
                    PaketOzeti.Ekle(VarsayilanOzetModu, ozetParafOzeti, ozetParafOzetiSha512, partUriParafOzeti);
                    NihaiOzet.Ekle(VarsayilanOzetModu, ozetParafOzeti, ozetParafOzetiSha512, partUriParafOzeti);
                }
            }

            if (ParafImzaVarMi())
            {
                // Get the part URI for ParafImza
                var partUriParafImza = _package.GetPart(new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative)).Uri;

                using (Stream parafImzaStream = ParafImzaAl()) // Using 'using' for the ParafImza stream
                {
                    // Calculate the hash with the default mode
                    byte[] ozetParafImza = Araclar.OzetHesapla(parafImzaStream, VarsayilanOzetModu);

                    parafImzaStream.Position = 0; // Reset position before recalculating SHA512 hash

                    // Calculate the SHA512 hash
                    byte[] ozetParafImzaSha512 = Araclar.OzetHesapla(parafImzaStream, OzetModu.SHA512);

                    // Add the calculated hashes to the NihaiOzet object
                    NihaiOzet.Ekle(VarsayilanOzetModu, ozetParafImza, ozetParafImzaSha512, partUriParafImza);
                }

            }

            // Get the part URI for PaketOzeti
            var partUriPaketOzeti = _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).Uri;

            using (Stream paketOzetiStream = PaketOzetiAl()) // Using 'using' for PaketOzeti stream
            {
                byte[] ozetPaketOzeti = Araclar.OzetHesapla(paketOzetiStream, VarsayilanOzetModu);
                paketOzetiStream.Position = 0; // Reset position before recalculating SHA512 hash
                byte[] ozetPaketOzetiSha512 = Araclar.OzetHesapla(paketOzetiStream, OzetModu.SHA512);

                // Add the calculated hashes to the NihaiOzet object
                NihaiOzet.Ekle(VarsayilanOzetModu, ozetPaketOzeti, ozetPaketOzetiSha512, partUriPaketOzeti);
            }

            // Get the part URI for Imza
            var partUriImza = _package.GetPart(new Uri(Araclar.URI_IMZA, UriKind.Relative)).Uri;

            using (Stream imzaStream = ImzaAl()) // Using 'using' for Imza stream
            {
                byte[] ozetImza = Araclar.OzetHesapla(imzaStream, VarsayilanOzetModu);
                imzaStream.Position = 0; // Reset position before recalculating SHA512 hash
                byte[] ozetImzaSha512 = Araclar.OzetHesapla(imzaStream, OzetModu.SHA512);

                // Add the calculated hashes to the NihaiOzet object
                NihaiOzet.Ekle(VarsayilanOzetModu, ozetImza, ozetImzaSha512, partUriImza);
            }

            // Get the part URI for NihaiUstveri
            var partUriNihaiUstveri = _package.GetPart(new Uri(Araclar.URI_NIHAIUSTVERI, UriKind.Relative)).Uri;

            using (Stream nihaiUstveriStream = NihaiUstveriAl()) // Using 'using' for NihaiUstveri stream
            {
                byte[] ozetNihaiUstveri = Araclar.OzetHesapla(nihaiUstveriStream, VarsayilanOzetModu);
                nihaiUstveriStream.Position = 0; // Reset position before recalculating SHA512 hash
                byte[] ozetNihaiUstveriSha512 = Araclar.OzetHesapla(nihaiUstveriStream, OzetModu.SHA512);

                // Add the calculated hashes to the NihaiOzet object
                NihaiOzet.Ekle(VarsayilanOzetModu, ozetNihaiUstveri, ozetNihaiUstveriSha512, partUriNihaiUstveri);
            }


            if (Ustveri.CT_Ustveri.Ekler != null && Ustveri.CT_Ustveri.Ekler.Length > 0)
            {
                foreach (var ustveriEki in Ustveri.CT_Ustveri.Ekler)
                {
                    if (ustveriEki.Tur == ST_KodEkTuru.DED && ustveriEki.ImzaliMi == true)
                    {
                        var relationShip = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_EK)
                                                   .SingleOrDefault(xRelationship =>
                                                       String.Compare(xRelationship.Id.ToString(), Araclar.ID_ROOT_EK + ustveriEki.Id.Value, true) == 0);

                        // Get the stream from the related part
                        using (Stream ekStream = _package.GetPart(relationShip.TargetUri).GetStream())
                        {
                            if (ekStream == null || ekStream.Length == 0)
                            {
                                // Handle the case where the stream is null or empty.
                                // Consider logging or throwing an exception if necessary.
                                return; // or handle the error as required
                            }

                            // Calculate the hash with the default mode
                            byte[] ekOzeti = Araclar.OzetHesapla(ekStream, VarsayilanOzetModu);

                            // Reset the stream position before calculating SHA512 hash
                            ekStream.Position = 0;

                            // Calculate the SHA512 hash
                            byte[] ekOzetiSha512 = Araclar.OzetHesapla(ekStream, OzetModu.SHA512);

                            // Add the calculated hashes to the NihaiOzet object for further processing
                            NihaiOzet.Ekle(VarsayilanOzetModu, ekOzeti, ekOzetiSha512, relationShip.TargetUri);
                        }

                    }

                }
            }

            var coreRelations = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE);
            if (coreRelations == null || !coreRelations.Any())
            {
                throw new ApplicationException("Core bileşeni ilişkisi alınamadı.");
            }
            var corePart = _package.GetPart(coreRelations.First().TargetUri);
         

            // Use a 'using' block to ensure the stream is disposed of properly
            using (var stream = corePart.GetStream())
            {
                // Reset position to the beginning of the stream
                stream.Position = 0;

                // Calculate the hash with the default mode
                byte[] ozet = Araclar.OzetHesapla(stream, VarsayilanOzetModu);

                // Reset stream position before calculating the SHA512 hash
                stream.Position = 0;

                // Calculate the SHA512 hash
                byte[] ozetSha512 = Araclar.OzetHesapla(stream, OzetModu.SHA512);

                // Add both hashes to NihaiOzet
                NihaiOzet.Ekle(VarsayilanOzetModu, ozet, ozetSha512, coreRelations.First().TargetUri);
            }

            NihaiOzet.CT_NihaiOzet.Id = Ustveri.BelgeIdAl().ToUpperInvariant();
            NihaiOzet.KontrolEt();
            var partUriNihaiOzet = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriNihaiOzet))
                {
                    _package.DeletePart(partUriNihaiOzet);
                    _package.DeleteRelationship(Araclar.ID_NIHAIOZET);
                }
            }
            var partNihaiOzet = _package.CreatePart(partUriNihaiOzet, Araclar.MIME_XML, CompressionOption.Maximum);
            _package.CreateRelationship(partNihaiOzet.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_NIHAIOZET, Araclar.ID_NIHAIOZET);
            var xmlSerializer = new XmlSerializer(typeof(CT_NihaiOzet));
            //XmlTextWriter xmlTextWriter = new XmlTextWriter(partNihaiOzet.GetStream(), Encoding.UTF8)
            //{
            //    Formatting = Formatting.Indented
            //};
            using (Stream stream = partNihaiOzet.GetStream()) // Open the stream for writing
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    // Set formatting for readable XML
                    xmlTextWriter.Formatting = Formatting.Indented;

                    // Write the XML declaration (optional but common)
                    //xmlTextWriter.WriteStartDocument();

                    // Start the root element
                    //xmlTextWriter.WriteStartElement("NihaiOzet");

                    //// Add sample elements or write actual content
                    //xmlTextWriter.WriteElementString("Element1", "Value1");
                    //xmlTextWriter.WriteElementString("Element2", "Value2");

                    // End the root element
                    //xmlTextWriter.WriteEndElement();

                    // End the document
                    //xmlTextWriter.WriteEndDocument();
                    xmlSerializer.Serialize(xmlTextWriter, NihaiOzet.CT_NihaiOzet);
                }
            }

            
        }

        /// <summary>İşlem yapılan paket için sonladırma işlemi yapılarak açılan kaynaklar kapatılır.</summary>
        public void Kapat()
        {
            if (_paketModu == PaketModu.Olustur)
            {
                var ustYaziRelation = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI);
                if (!ustYaziRelation.Any())
                {
                    throw new Exception("Üst yazı eklenmemiş.");
                }
                _package.Close();
            }
            else if (_paketModu == PaketModu.Guncelle)
            {
                _package.Close();
            }
            else if (_paketModu == PaketModu.Ac)
            {
                _package.Close();
            }

        }

        #endregion

        #region Üstveri

        /// <summary>   Paket içerisindeki Üstveri elemanının olup olmadığını döner. </summary>
        /// <returns>   Üstveri elemanı varsa <code>true</code> aksi durumda <code>false</cade> döner. </returns>
        public bool UstveriVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
        }

        /// <summary>   Paket içerisindeki Üstveri elemanını STREAM olarak verir. </summary>
        /// <returns>   Üstveri bileşenine ait STREAM nesnesini döner. Bileşenin bulunmaması durumunda null döner. </returns>
        public Stream UstveriAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_USTVERI, UriKind.Relative)))
            {
                // Create a MemoryStream to hold the content
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_USTVERI, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }
            return null;
        }

        /// <summary>Ustveri bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <remarks>Ustveri bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak PaketOzeti ve NihaiOzet objesine eklenir. Oluşturma sırasında Ustveri bileşeni olası hatalara karşı kontrol edilir.</remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void UstveriOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için UstveriOlustur fonksiyonu kullanılamaz.");
            }
            Ustveri.KontrolEt();
            var partUriUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_USTVERI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriUstveri))
                {
                    _package.DeletePart(partUriUstveri);
                    _package.DeleteRelationship(Araclar.ID_USTVERI);
                }
            }

            var partUstveri = _package.CreatePart(partUriUstveri, Araclar.MIME_XML, CompressionOption.Normal);
            _package.CreateRelationship(partUstveri.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_USTVERI, Araclar.ID_USTVERI);

            XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
            ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-2");
            // Create the XmlSerializer for the CT_Ustveri type
            var x = new XmlSerializer(typeof(CT_Ustveri));

            // Open the stream for writing with proper disposal
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(partUstveri.GetStream(), Encoding.UTF8)
            {
                Formatting = Formatting.Indented // Indent for better readability
            })
            {
                // Serialize the CT_Ustveri object and write it to the stream
                x.Serialize(xmlTextWriter, Ustveri.CT_Ustveri);
            }

        }

        #endregion

        #region NihaiÜstveri

        /// <summary>   Paket içerisindeki nihai üstveri bileşeni olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki nihai üstveri bileşeni varsa true aksi durumda false döner. </returns>
        public bool NihaiUstveriVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_NIHAIUSTVERI, UriKind.Relative));
        }

        /// <summary>   Paket içerisindeki NihaiÜstveri elemanını STREAM olarak verir. </summary>
        /// <returns>   NihaiÜstveri bileşenine ait STREAM nesnesini döner. Bileşenin bulunmaması durumunda null döner. </returns>
        public Stream NihaiUstveriAl()
        {
            Uri nihaiUstveriUri = new Uri(Araclar.URI_NIHAIUSTVERI, UriKind.Relative);

            // Guard clause to return null if the part doesn't exist
            if (!_package.PartExists(nihaiUstveriUri))
            {
                return null;
            }

            // Create a MemoryStream to hold the content
            MemoryStream memoryStream = new MemoryStream();
            using (Stream partStream = _package.GetPart(nihaiUstveriUri).GetStream())
            {
                // Copy the content of the partStream into memoryStream
                partStream.CopyTo(memoryStream);
            }

            // Reset the position for reading
            memoryStream.Position = 0;

            return memoryStream;
        }


        /// <summary>NihaiUstveri bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.</summary>
        /// <remarks>NihaiUstveri bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak NihaiOzet objesine eklenir. Oluşturma sırasında NihaiUstveri bileşeni olası hatalara karşı kontrol edilir.</remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void NihaiUstveriOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için NihaiUstveriOlustur fonksiyonu kullanılamaz.");
            }
            NihaiUstveri.KontrolEt();
            var partUriNihaiUstveri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIUSTVERI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriNihaiUstveri))
                {
                    _package.DeletePart(partUriNihaiUstveri);
                    _package.DeleteRelationship(Araclar.ID_NIHAIUSTVERI);
                }
            }

            var partNihaiUstveri = _package.CreatePart(partUriNihaiUstveri, Araclar.MIME_XML, CompressionOption.Normal);
            _package.CreateRelationship(partNihaiUstveri.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_NIHAIUSTVERI, Araclar.ID_NIHAIUSTVERI);

            XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
            ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-2");

            // Create a MemoryStream to hold the content
            XmlSerializer x = new XmlSerializer(typeof(CT_NihaiUstveri));
            using (Stream stream = partNihaiUstveri.GetStream()) // Open the stream for writing
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                // Set the formatting for pretty-printed XML
                xmlTextWriter.Formatting = Formatting.Indented;

                x.Serialize(xmlTextWriter, NihaiUstveri.CT_NihaiUstveri, ss);
            }
        }
        #endregion

        #region Ust Yazi
        /// <summary>   Paket içerisine üst yazı bileşeni ekler. </summary>
        /// <param name="dosyaYolu">    Üst yazı bileşenine ilişkin dosya yoludur. </param>
        /// <param name="mimeTuru">     Üst yazı bileşeni dosyasının mime türüdür. </param>
        /// <param name="ozetModu">     Eklenen üst yazıya ait özet değerinin paket özeti bileşenine hangi algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="SystemException">          Daha önceden üst yazı eklenmiş bir pakete tekrar üst yazı eklenemez. </exception>
        /// ### <exception cref="ArgumentNullException">    Dosya yolu boş veya Mime türü null olamaz. </exception>
        /// ### <exception cref="Exception">                Açma modunda kullanıldığında EXCEPTIONoluşur. </exception>
        public void UstYaziEkle(string dosyaYolu, String mimeTuru, OzetModu ozetModu = OzetModu.SHA256)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open, FileAccess.Read))
            {
                UstYaziEkle(fileStream, System.IO.Path.GetFileName(dosyaYolu), mimeTuru, ozetModu);
            }
        }

        /// <summary>   Paket içerisine üst yazı bileşeni ekler. </summary>
        /// <exception cref="SystemException">          Daha önceden üst yazı eklenmiş bir pakete tekrar üst yazı eklenemez. </exception>
        /// <exception cref="ArgumentNullException">    Dosya yolu ve mime türü  boş veya null olamaz. </exception>
        /// <exception cref="Exception">                Açma modunda kullanıldığında EXCEPTION oluşur. </exception>
        /// <param name="dosya">    Üst yazı bileşenine ilişkin STREAM'dir. </param>
        /// <param name="dosyaAdi"> Eklenen üst yazı dosyasının adıdır. Dosya adında bulunan boşluklar kaldırılır ve Türkçe karakterler İngilizce olanlarla değiştirilir.</param>
        /// <param name="mimeTuru"> Üst yazı bileşeni dosyasının mime türüdür. </param>
        /// <param name="ozetModu"> Eklenen üst yazıya ait özet değerinin paket özeti bileşenine hangi algoritma ile ekleneceğini belirtir. </param>
        public void UstYaziEkle(Stream dosya, String dosyaAdi, String mimeTuru, OzetModu ozetModu = OzetModu.SHA256)
        {
            var ustYaziRelation = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI);
            if (ustYaziRelation != null && ustYaziRelation.Count() > 0)
            {
                throw new SystemException("Daha önce üstyazı eklenmiş.");
            }
            if (dosya == null)
                throw new ArgumentNullException("dosya");
            if (dosyaAdi == null)
                throw new ArgumentNullException("dosyaAdi");
            if (mimeTuru == null)
                throw new ArgumentNullException("mimeTuru");
            if (ozetModu == OzetModu.Yok)
                throw new ArgumentException("OzetModu YOK olamaz.");
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için UstYaziEkle fonksiyonu kullanılamaz.");
            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format(Araclar.URI_FORMAT_USTYAZI, Araclar.EncodePath(dosyaAdi)), UriKind.Relative));
            // Create the part for the package
            var part = _package.CreatePart(assemblyPartUri, mimeTuru, CompressionOption.Maximum);

            // Open the target stream from the package part within a 'using' block
            using (Stream partStream = part.GetStream())
            {
                // Copy the data from 'dosya' stream to 'partStream'
                dosya.CopyTo(partStream);
            }

            // Create a relationship for the part
            _package.CreateRelationship(part.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_USTYAZI, Araclar.ID_USTYAZI);

            // Set the mime type for the Ustveri
            Ustveri.MimeTuruBelirle(mimeTuru);

        }

        /// <summary>   Paket içerisindeki üst yazı bileşeni olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki üst yazı bileşeni varsa true aksi durumda false döner. </returns>
        public bool UstYaziVarMi()
        {
            return _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 1;
        }

        /// <summary>   Paket içerisindeki üst yazı bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki üst yazı bileşeni STREAM'i. UstYazi bileşeni yoksa null döner. </returns>
        public Stream UstYaziAl()
        {
            // Get all relationships of the specified type
            var ustYaziRelationships = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).ToList();

            // Check if there is exactly one relationship
            if (ustYaziRelationships.Count == 1)
            {
                // Retrieve the stream of the related part
                //return _package.GetPart(PackUriHelper.CreatePartUri(ustYaziRelationships.First().TargetUri)).GetStream();

                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(PackUriHelper.CreatePartUri(ustYaziRelationships.First().TargetUri)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }

            // Return null if there are no relationships or more than one
            return null;
        }

        public Stream EkOzetAl(PackageRelationship relationShip)
        {
            // Get all relationships of the specified type
            MemoryStream memoryStream = new MemoryStream();
            using (Stream partStream = _package.GetPart(relationShip.TargetUri).GetStream())
            {
                partStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0; // Reset position for reading
            return memoryStream;
          
        }
        public Stream GetStreamFromPackagePart(CT_Reference item)
        {
            // Get all relationships of the specified type
            MemoryStream memoryStream = new MemoryStream();
            using (Stream partStream = _package.GetPart(PackUriHelper.CreatePartUri(new Uri(Uri.UnescapeDataString(item.URI), UriKind.Relative))).GetStream())
            {
                partStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0; // Reset position for reading
            return memoryStream;

        }


        #endregion

        #region Ek
        /// <summary>   Paket içerisine ek bileşeni ekler. </summary>
        /// <exception cref="ArgumentNullException">    Dosya yolu boş veya null olamaz. </exception>
        /// <exception cref="Exception">                Verilen ek dosyası bulunamazsa EXCEPTION oluşur. </exception>
        /// <param name="ek">           Eklenen ek bileşenine ait üstveriyi barındıran objedir. </param>
        /// <param name="dosyaYolu">    Ek bileşenine ilişkin dosya yoludur. </param>
        /// <param name="ozetModu">     Eklenen ek bileşenine ait özet değerinin paket özeti bileşenine
        ///                             hangi algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="ArgumentNullException">    Ek objesi null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi mime türü null ise EXCEPTION
        ///                                                 oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi ID null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="Exception">                PaketModu 'Ac' olarak işlem yapılan paketler
        ///                                                 için EkEkle fonksiyonu kullanılamaz. </exception>
        public void EkEkle(CT_Ek ek, string dosyaYolu, OzetModu ozetModu = OzetModu.SHA256)
        {
            if (dosyaYolu == null)
                throw new ArgumentNullException("dosyaYolu");
            if (!(File.Exists(dosyaYolu)))
                throw new Exception("Dosya bulunamadı.");
            if (ek == null)
                throw new ArgumentNullException("ek");
            if (ek.MimeTuru.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.MimeTuru");
            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.Item");
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için EkEkle fonksiyonu kullanılamaz.");
            using (var t = System.IO.File.OpenRead(dosyaYolu))
            {
                EkEkle(ek, t, System.IO.Path.GetFileName(dosyaYolu), ozetModu);
            }
        }

        /// <summary>   Paket içerisine ek bileşeni ekler. </summary>
        /// <exception cref="ArgumentNullException">    Dosya null olamaz. </exception>
        /// <param name="ek">       Eklenen ek bileşenine ait üstveriyi barındıran objedir. </param>
        /// <param name="dosya">    Ek bileşenine ilişkin STREAM'dir. </param>
        /// <param name="dosyaAdi"> Eklenen ek bileşeni dosyasının adıdır. Dosya adında bulunan boşluklar kaldırılır ve Türkçe karakterler İngilizce olanlarla değiştirilir.</param>
        /// <param name="ozetModu"> Eklenen ek bileşenine ait özet değerinin paket özeti bileşenine hangi
        ///                         algoritma ile ekleneceğini belirtir. </param>
        /// ### <exception cref="ArgumentNullException">    Dosya adı null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi mime türü null ise EXCEPTION
        ///                                                 oluşur. </exception>
        /// ### <exception cref="ArgumentNullException">    Ek objesi ID null ise EXCEPTION oluşur. </exception>
        /// ### <exception cref="Exception">                PaketModu 'Ac' olarak işlem yapılan paketler
        ///                                                 için EkEkle fonksiyonu kullanılamaz. </exception>
        public void EkEkle(CT_Ek ek, Stream dosya, String dosyaAdi, OzetModu ozetModu = OzetModu.SHA256)
        {
            if (dosya == null)
                throw new ArgumentNullException("dosya");
            if (dosyaAdi.IsNullOrWhiteSpace())
                throw new ArgumentNullException("dosyaAdi");
            try
            {
                string fileName = Path.GetFileName(dosyaAdi);
                if (fileName == null)
                {
                    throw new ArgumentException("dosyaAdi");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("dosyaAdi");
            }
            if (ek == null)
                throw new ArgumentNullException("ek");
            if (ek.MimeTuru.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.MimeTuru");
            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ek.Item");
            if (ek.ImzaliMiSpecified == true && ek.ImzaliMi == true && ozetModu == OzetModu.Yok)
            {
                throw new ApplicationException("İmzalı bir ek, ÖzetModu değeri verilmeksizin pakete eklenemez.");
            }
            if (ek.ImzaliMiSpecified == false && ozetModu == OzetModu.Yok)
            {
                throw new ApplicationException("İmzalı bir ek, ÖzetModu değeri verilmeksizin pakete eklenemez.");
            }
            if (ek.ImzaliMiSpecified == true && ek.ImzaliMi == false && ozetModu != OzetModu.Yok)
            {
                throw new ApplicationException("İmzasız bir ek, ÖzetModu değeri verilerek pakete eklenemez.");
            }

            String klasorAdi;
            String iliskiAdi;
            String id;
            if (ozetModu == OzetModu.Yok)
            {
                klasorAdi = Araclar.URI_ROOT_IMZASIZEK;
                iliskiAdi = Araclar.RELATION_TYPE_IMZASIZEK;
                id = Araclar.ID_ROOT_IMZASIZEK;
            }
            else
            {
                klasorAdi = Araclar.URI_ROOT_EK;
                iliskiAdi = Araclar.RELATION_TYPE_EK;
                id = Araclar.ID_ROOT_EK;
            }
            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format("/{0}/{1}", klasorAdi, Araclar.EncodePath(dosyaAdi)), UriKind.Relative));
            var part = _package.CreatePart(assemblyPartUri, ek.MimeTuru, CompressionOption.Maximum);
            // Open the part's stream and copy the data from the 'dosya' stream
            using (var partStream = part.GetStream())
            {
                dosya.CopyTo(partStream);
            }
            _package.CreateRelationship(part.Uri, TargetMode.Internal, iliskiAdi, id + ek.Id.Value.ToUpperInvariant());

        }

        /// <summary>Id'si verilen eke ait STREAM'i döner.</summary>
        /// <param name="id">Alınmak istenen eke ait Id değeri.</param>
        /// <returns>Alınan eke ait STREAM nesnesi. Ek bulunamaması durumunda null döner.</returns>
        public Stream EkAl(Guid id)
        {
            // Try to get the stream based on the provided ID
            Stream GetStreamForRelationship(string relationshipId)
            {
                if (_package.RelationshipExists(relationshipId))
                {

                    MemoryStream memoryStream = new MemoryStream();

                    var relationship = _package.GetRelationship(relationshipId);
                    using (Stream partStream = _package.GetPart(PackUriHelper.CreatePartUri(relationship.TargetUri)).GetStream())
                    {
                        partStream.CopyTo(memoryStream);
                    }
                    memoryStream.Position = 0; // Reset position for reading
                    return memoryStream;
                }
                
                
                return null;
            }

            // Attempt to get the stream using the root EK ID
            var ekStream = GetStreamForRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant());

            // If not found, attempt to get the stream using the root IMZASIZEK ID
            if (ekStream == null)
            {
                ekStream = GetStreamForRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant());
            }

            return ekStream;  // Return the stream or null if not found
        }


        /// <summary>Id'si verilen ek paketten çıkartılır.</summary>
        /// <param name="id">Çıkartılmak istenen eke ait Id.</param>
        /// <returns>Çıkarma işlemi başarılı ise true, aksi takdirde false döner.</returns>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> Mevcut bir paketten ek çıkartma örneği.
        /// <code>
        /// static void PakettenEkCikar(String dosyaYolu)
        /// {
        ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///     {
        ///         paket.EkCikar(new Guid(paket.Ustveri.EkleriAl().First().Id.Value));
        ///         paket.Kapat();
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool EkCikar(Guid id)
        {
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için EkCikar fonksiyonu kullanılamaz.");
            if (_package.RelationshipExists(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()))
            {
                _package.DeletePart(_package.GetRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant()).TargetUri);
                _package.DeleteRelationship(Araclar.ID_ROOT_EK + id.ToString().ToUpperInvariant());
                return true;
            }
            if (_package.RelationshipExists(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()))
            {
                _package.DeletePart(_package.GetRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant()).TargetUri);
                _package.DeleteRelationship(Araclar.ID_ROOT_IMZASIZEK + id.ToString().ToUpperInvariant());
                return true;
            }
            return false;
        }

        #endregion

        #region Ozellikler

        #region Olusturan Belirler

        /// <summary>Paket açıklama değeri verilir.</summary>
        /// <param name="aciklama">Verilen açıklama değeri.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: An explanation of the content of the resource. [Example: 
        /// Values might include an abstract, table of contents, 
        /// reference to a graphical representation of content, and a 
        /// free-text account of the content. end example] </remarks>
        public void PaketAciklamasiBelirle(String aciklama) //DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAciklamasiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Description = aciklama;
        }

        /// <summary>Paket son yazdılırılma tarihi verilir.</summary>
        /// <param name="sonYazdirilmaTarihi">Son yazdırılma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The date and time of the last printing. </remarks>
        public void PaketSonYazdirilmaTarihiBelirle(DateTime? sonYazdirilmaTarihi) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonYazdirilmaTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.LastPrinted = sonYazdirilmaTarihi;
        }

        /// <summary>Paket güncelleme tarihi verilir.</summary>
        /// <param name="guncellemeTarihi">Güncelleme tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date on which the resource was changed. </remarks>
        public void PaketGuncellemeTarihiBelirle(DateTime? guncellemeTarihi) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketGuncellemeTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Modified = guncellemeTarihi;
        }

        /// <summary>Paket son güncelleyen bilgisi verilir.</summary>
        /// <param name="sonGuncelleyen">Son güncelleyen bilgisi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The user who performed the last modification. The 
        ///identification is environment-specific. [Example: A name, 
        ///email address, or employee ID. end example] It is 
        ///recommended that this value be as concise as possible. </remarks>
        public void PaketSonGuncelleyenBelirle(String sonGuncelleyen) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonGuncelleyenBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.LastModifiedBy = sonGuncelleyen;
        }

        /// <summary>Paket anahtar kelimeleri verilir.</summary>
        /// <param name="anahtarKelimeler">Anahtar kelimeler.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: A delimited set of keywords to support searching and 
        ///indexing. This is typically a list of terms that are not 
        ///available elsewhere in the properties.  
        /// 
        ///The definition of this element uniquely allows for: 
        ///  Use of the xml:lang attribute to identify languages 
        ///  A mixed content model, such that keywords can be 
        ///flagged individually 
        ///
        ///[Example: The following instance of the keywords element 
        ///has keywords in English (Canada), English (U.S.), and French 
        ///(France): 
        /// 
        /// <keywords xml:lang="en-US"> 
        /// color  
        /// <value xml:lang="en-CA">colour</value> 
        /// <value xml:lang="fr-FR">couleur</value> 
        /// </keywords> 
        ///
        ///end example]  </remarks>
        public void PaketAnahtarKelimeleriBelirle(String anahtarKelimeler) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAnahtarKelimeleriBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Keywords = anahtarKelimeler;
        }

        /// <summary>Paket durumu verilir.</summary>
        /// <param name="durum">Durum.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The status of the content. [Example: Values might include 
        ///“Draft”, “Reviewed”, and “Final”.  end example]  </remarks>
        public void PaketDurumuBelirle(String durum) // OPC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDurumuBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.ContentStatus = durum;
        }

        /// <summary>Paket oluşturulma tarihi verilir.</summary>
        /// <param name="olusturulmaTarihi">Oluşturulma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date of creation of the resource. </remarks>
        public void PaketOlusturulmaTarihiBelirle(DateTime? olusturulmaTarihi) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOlusturulmaTarihiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Created = olusturulmaTarihi;
        }

        /// <summary>Paket başlığı verilir.</summary>
        /// <param name="baslik">Başlık.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The name given to the resource. </remarks>
        public void PaketBasligiBelirle(String baslik) // DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketBasligiBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Title = baslik;
        }

        /// <summary>Paket dili verilir.</summary>
        /// <param name="dil">Dil.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The language of the intellectual content of the resource. 
        /// [Note: IETF RFC 3066 provides guidance on encoding to 
        /// represent languages.  end note]  </remarks>
        public void PaketDiliBelirle(String dil) //DC, olusturan belirler
        {
            if (_paketModu == PaketModu.Ac) throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDiliBelirle fonksiyonu kullanılamaz.");
            _package.PackageProperties.Language = dil;
        }

        #endregion

        #region Ustveri Ile Ayni
        private void PaketBelirteciBelirle(String belirtec) //DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Identifier = belirtec;
        }

        private void PaketOlusturanBelirle(String olusturan) // DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Creator = olusturan;
        }

        private void PaketKonusuBelirle(String konu) // DC, Ustveri alanı ile aynı
        {
            _package.PackageProperties.Subject = konu;
        }

        #endregion

        #region Static

        private void PaketKategorisiBelirle(String kategori) // OPC, static
        {
            _package.PackageProperties.Category = kategori;
        }

        private void PaketIcerikTuruBelirle(String icerikTuru) // OPC, static
        {
            _package.PackageProperties.ContentType = icerikTuru;
        }

        private void PaketVersiyonuBelirle(String versiyon) // DC, static
        {
            _package.PackageProperties.Version = versiyon;
        }

        private void PaketRevizyonuBelirle(String revisyon) // DC, static
        {
            _package.PackageProperties.Revision = revisyon;
        }

        #endregion

        #endregion

        #region Imza
        /// <summary>Paket içerisine PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri eklenir.</summary>
        /// <param name="imza">PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri.</param>
        /// <exception cref="System.ArgumentNullException">İmza değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> İmzasız bir paketi güncelleme modunda açarak imza ekleme örneği.
        /// <code>
        ///private static void ImzasizPaketeImzaEkle(string dosyaYolu, byte[] imza)
        ///{
        ///    using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///    {
        ///         if (paket.PaketOzetiVarMi() == false)
        ///         {
        ///              paket.PaketOzetiOlustur();
        ///         }
        ///
        ///         if (paket.ImzaVarMi() == false)
        ///         {
        ///              Stream paketOzeti = paket.PaketOzetiAl();
        ///              Byte[] imzaliPaketOzeti = Imzala(StreamToByteArray(paketOzeti));
        ///              paket.ImzaEkle(imzaliPaketOzeti);         // Zorunlu alan
        ///         }
        ///         else
        ///         {
        ///              Stream imza = paket.ImzaAl();
        ///              Byte[] imzaliImza = Imzala(StreamToByteArray(imza));
        ///              paket.ImzaEkle(imzaliImza);         // Zorunlu alan
        ///         }
        ///        paket.Kapat();
        ///    }
        ///}
        /// </code>
        /// </example>
        public void ImzaEkle(Byte[] imza)
        {
            if (imza == null)
            {
                throw new ArgumentNullException("imza");
            }
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için ImzaEkle fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_IMZA, UriKind.Relative));

            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).DeleteRelationship(Araclar.ID_IMZA);
            }
            PackagePart part02 = _package.CreatePart(assemblyPartUri, Araclar.MIME_OCTETSTREAM, CompressionOption.Maximum);
            using (var partStream = part02.GetStream())
            {
                // Write the byte array 'imza' to the stream starting from index 0, for its full length
                partStream.Write(imza, 0, imza.Length);
            }
            _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).CreateRelationship(PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative), assemblyPartUri), TargetMode.Internal, Araclar.RELATION_TYPE_IMZA, Araclar.ID_IMZA);
        }

        /// <summary>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri varsa true aksi durumda false döner. </returns>
        public bool ImzaVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_IMZA, UriKind.Relative));
        }

        /// <summary>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) STREAM nesnesi. Imzali bileşen olmaması durumunda null döner. </returns>
        public Stream ImzaAl()
        {
            // Check if the part exists
            MemoryStream memoryStream = new MemoryStream();

            if (_package.PartExists(new Uri(Araclar.URI_IMZA, UriKind.Relative)))
            {

                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_IMZA, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
               
            }
            else
            {
                // Return null if part doesn't exist
                return null;
            }

        }

        #endregion

        #region ParafImza
        /// <summary>Paket içerisine PafarOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri eklenir.</summary>
        /// <param name="imza">ParafOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri.</param>
        /// <exception cref="System.ArgumentNullException">İmza değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> İmzasız bir paketi güncelleme modunda açarak imza ekleme örneği.
        /// <code>
        /// private static void ImzasizPaketeParafImzaEkle(string dosyaYolu, byte[] imza)
        ///  {
        ///      using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///      {
        ///         if (paket.ParafOzetiVarMi() == false)
        ///         {
        ///              paket.ParafOzetiOlustur();
        ///         }
        ///         
        ///         if (paket.ParafImzaVarMi() == false)
        ///         {
        ///             Stream parafOzeti = paket.ParafOzetiAl();
        ///             Byte[] imzaliParafOzeti = Imzala(StreamToByteArray(parafOzeti));
        ///             paket.ParafImzaEkle(imzaliParafOzeti);         // Zorunlu alan
        ///         }
        ///         else
        ///         {
        ///             Stream parafImza = paket.ParafImzaAl();
        ///             Byte[] imzaliParafImza = Imzala(StreamToByteArray(parafImza));
        ///             paket.ParafImzaEkle(imzaliParafImza);         // Zorunlu alan
        ///         }
        ///         paket.Kapat();
        ///      }
        ///  }
        /// </code>
        /// </example>
        public void ParafImzaEkle(Byte[] imza)
        {
            if (imza == null)
            {
                throw new ArgumentNullException("imza");
            }
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için ParafImzaEkle fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative));

            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative)).DeleteRelationship(Araclar.ID_PARAFIMZA);
            }
            PackagePart part02 = _package.CreatePart(assemblyPartUri, Araclar.MIME_OCTETSTREAM, CompressionOption.Maximum);
            //using (MemoryStream fileStream = new MemoryStream(imza, 0, imza.Length, false))
            //{
            //    fileStream.CopyTo(part02.GetStream());
            //}
            using (Stream part02Stream = part02.GetStream()) // Wrap part02.GetStream() in a using block
            {
                // Write the 'imza' byte array to the part02 stream
                part02Stream.Write(imza, 0, imza.Length);
            }


            _package.GetPart(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative))
                .CreateRelationship(
                    PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative), assemblyPartUri),
                    TargetMode.Internal, Araclar.RELATION_TYPE_PARAFIMZA, Araclar.ID_PARAFIMZA);
        }

        /// <summary>   Paket içerisindeki ParafOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki ParafOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) STREAM nesnesi. Imzali bileşen olmaması durumunda null döner. </returns>
        public Stream ParafImzaAl()
        {
            Uri parafImzaUri = new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative);

            if (_package.PartExists(parafImzaUri))
            {
         
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(parafImzaUri).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;

            }
            else
            {
                // Return null if the part does not exist
                return null;
            }
        }


        /// <summary>   Paket içerisindeki ParafOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki ParafOzeti bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri olması durumunda true aksi durumda false döner. </returns>
        public bool ParafImzaVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative));
        }

        /// <summary>ParafImza bileşenini paketten çıkartır.</summary>
        /// <returns>Çıkarma işlemi başarılı ise true, aksi takdirde false döner.</returns>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        /// <example> Mevcut bir paketten ParafImza çıkartma örneği.
        /// <code>
        /// static void PakettenParafImzaCikar(String dosyaYolu)
        /// {
        ///     using (var paket = Paket.Ac(dosyaYolu, PaketModu.Guncelle))
        ///     {
        ///         paket.ParafImzaCikar();
        ///         paket.Kapat();
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool ParafImzaCikar()
        {
            if (_paketModu == PaketModu.Ac)
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için EkCikar fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_PARAFIMZA, UriKind.Relative));
            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative)).DeleteRelationship(Araclar.ID_PARAFIMZA);
                return true;
            }
            return false;
        }

        #endregion

        #region Muhur
        /// <summary>Paket içerisine NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri eklenir.</summary>
        /// <param name="muhur">NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri.</param>
        /// <exception cref="System.ArgumentNullException">Muhur değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void MuhurEkle(Byte[] muhur)
        {
            if (muhur == null)
            {
                throw new ArgumentNullException("muhur");
            }
            if (_paketModu == PaketModu.Ac)
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için MuhurEkle fonksiyonu kullanılamaz.");
            Uri assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative));

            if (_package.PartExists(assemblyPartUri))
            {
                _package.DeletePart(assemblyPartUri);
                _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).DeleteRelationship(Araclar.ID_MUHUR);
            }
            PackagePart part02 = _package.CreatePart(assemblyPartUri, Araclar.MIME_OCTETSTREAM, CompressionOption.Maximum);
            //using (MemoryStream fileStream = new MemoryStream(muhur, 0, muhur.Length, false))
            //{
            //    Araclar.CopyStream(fileStream, part02.GetStream());
            //}
            using (Stream part02Stream = part02.GetStream()) // Ensure the stream is disposed of after use
            {
                // Write the 'muhur' byte array to the stream
                part02Stream.Write(muhur, 0, muhur.Length);
            }

            _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).CreateRelationship(PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative), assemblyPartUri), TargetMode.Internal, Araclar.RELATION_TYPE_MUHUR, Araclar.ID_MUHUR);
        }

        /// <summary>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri varsa true aksi durumda false döner. </returns>
        public bool MuhurVarMi()
        {
            return _package.PartExists(PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative)));
        }

        /// <summary>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) STREAM nesnesi. Mühürlü bileşen olmaması durumunda null döner. </returns>
        public Stream MuhurAl()
        {
            Uri muhurUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative));

            if (_package.PartExists(muhurUri))
            {
                // Retrieve the part and open the stream
                using (Stream partStream = _package.GetPart(muhurUri).GetStream())
                {
                    // Copy the part stream to a memory stream for further reuse if needed
                    MemoryStream memoryStream = new MemoryStream();
                    partStream.CopyTo(memoryStream);
                    memoryStream.Position = 0; // Reset the position for reading
                    return memoryStream; // Return the memory stream instead of the original one
                }
            }
            else
            {
                // Return null if the part does not exist
                return null;
            }
        }



        #endregion

        #region Paket Ozeti
        /// <summary>   Paket içerisindeki PaketOzeti bileşeni bulunup bulunmadığını döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeni varsa true aksi durumda false döner. </returns>
        public bool PaketOzetiVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative));
        }

        /// <summary>   Paket içerisindeki PaketOzeti bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki PaketOzeti bileşeninin STREAM nesnesi. PaketOzeti bileşeni olmaması durumunda null döner. </returns>
        public Stream PaketOzetiAl()
{
        if (_package.PartExists(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)))
        {
            MemoryStream memoryStream = new MemoryStream();
            using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).GetStream())
            {
                partStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
        return null;
}

        /// <summary>
        /// Verilen PaketOzeti nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak PaketÖzeti nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 3   PaketOzet'inde Ustveri ozet değeri yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 5   PaketOzet'inde BelgeHedef ozet değeri yok.
        /// 6   UstYazi bileşeni yok.
        /// 7   PaketOzet'inde UstYazi ozet değeri yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.
        /// 16  BelgeImza bileşeni yok.
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.
        /// 19  NihaiOzet'te Ustveri özet değeri yok.
        /// 20  NihaiOzet'te BelgeHedef özet değeri yok.
        /// 21  NihaiOzet'te UstYazi özet değeri yok.
        /// 22  NihaiOzet'te UstYazi özet değeri yok.
        /// 23  NihaiOzet'te UstYazi özet değeri yok.
        /// 24  NihaiOzet'te UstYazi özet değeri yok.
        /// 25  PaketOzeti bileşeni yok.
        /// 26  ParafOzet'inde UstYazi özet değeri yok.
        /// 27  ParafOzet'inde Ustveri özet değeri yok.
        /// 28  DigestItem değeri boş
        /// 29  DigestItem1 değeri boş
        /// 30  DigestItem ve DigestItem1 için kullanılan algoritmalar aynı.
        /// 31  DigestItem ve DigestItem1'den en az birinde SHA512 kullanılmalı.
        /// </returns>
        public bool PaketOzetiDogrula(CT_PaketOzeti ozet, ref List<OzetDogrulamaHatasi> sonuclar, IdentifierType dagitimIdentifier = null)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Ozet değeri verilmemiş.",
                    HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS
                });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = " ",
                    HataKodu = OzetDogrulamaHataKodu.USTVERI_BILESENI_YOK
                });
                return false;
            }

            Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri;
            if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "PaketOzet'inde Ustveri ozet değeri yok.",
                    HataKodu = OzetDogrulamaHataKodu.PAKETOZET_INDE_USTVERI_OZET_DEGERI_YOK
                });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "UstYazi bileşeni yok.",
                    HataKodu = OzetDogrulamaHataKodu.USTYAZI_BILESENI_YOK
                });
                return false;
            }


            Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri;
            if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "PaketOzet'inde UstYazi ozet değeri yok.",
                    HataKodu = OzetDogrulamaHataKodu.PAKETOZET_INDE_USTYAZI_OZET_DEGERI_YOK
                });
                return false;
            }
            foreach (CT_Reference item in ozet.Reference)
            {
                ReferansDogrula(item, ref sonuclar, dagitimIdentifier);
            }
            return sonuclar.Count == 0;
        }

        /// <summary>
        /// Tek bir özet değeri ile paket içerisindeki ilgili bileşenin özet değerlerini doğrular.
        /// </summary>
        /// <param name="item">Doğrulanacak ÖzetDeğerini barındıran CT_Reference  nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 3   PaketOzet'inde Ustveri ozet değeri yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 5   PaketOzet'inde BelgeHedef ozet değeri yok.
        /// 6   UstYazi bileşeni yok.
        /// 7   PaketOzet'inde UstYazi ozet değeri yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.
        /// 16  BelgeImza bileşeni yok.
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.
        /// 19  NihaiOzet'te Ustveri özet değeri yok.
        /// 20  NihaiOzet'te BelgeHedef özet değeri yok.
        /// 21  NihaiOzet'te UstYazi özet değeri yok.
        /// 22  NihaiOzet'te UstYazi özet değeri yok.
        /// 23  NihaiOzet'te UstYazi özet değeri yok.
        /// 24  NihaiOzet'te UstYazi özet değeri yok.
        /// 25  PaketOzeti bileşeni yok.
        /// 26  ParafOzet'inde UstYazi özet değeri yok.
        /// 27  ParafOzet'inde Ustveri özet değeri yok.
        /// 28  DigestItem değeri boş
        /// 29  DigestItem1 değeri boş
        /// 30  DigestItem ve DigestItem1 için kullanılan algoritmalar aynı.
        /// 31  DigestItem ve DigestItem1'den en az birinde SHA512 kullanılmalı.
        /// </returns>
        public bool ReferansDogrula(CT_Reference item, ref List<OzetDogrulamaHatasi> sonuclar, IdentifierType dagitimIdentifier = null)
        {
            if (sonuclar == null)
                return false;
            if (item == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Reference değeri verilmemiş.",
                    HataKodu = OzetDogrulamaHataKodu.REFERENCE_DEGERI_VERILMEMIS
                });
                return false;
            }
            if (item.URI == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "\"URI\" değeri boş.",
                    HataKodu = OzetDogrulamaHataKodu.URI_DEGERI_BOS
                });
                return false;
            }
            if (item.DigestItem == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"DigestItem\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.DIGESTITEM_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }
            if (item.DigestItem1 == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"DigestItem1\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.DIGESTITEM1_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }
            if (item.DigestItem.DigestMethod == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"DigestMethod\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.DIGESTMETHOD_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }
            if (item.DigestItem.DigestMethod.Algorithm == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"Algorithm\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.ALGORITHM_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }
            if (item.DigestItem1.DigestMethod == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"DigestMethod\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.DIGESTMETHOD_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }
            if (item.DigestItem1.DigestMethod.Algorithm == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("\"Algorithm\" değeri boş. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.ALGORITHM_DEGERI_BOS,
                    Uri = item.URI
                });
                return false;
            }

            if (string.Compare(item.DigestItem.DigestMethod.Algorithm, item.DigestItem1.DigestMethod.Algorithm, true) == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("İki DigestItem için \"Algorithm\" değerleri aynı. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.ALGORITHM_DEGERLERI_AYNI,
                    Uri = item.URI
                });
                return false;
            }
            if (string.Compare(item.DigestItem.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA512), true) != 0
                &&
                string.Compare(item.DigestItem1.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA512), true) != 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("En az bir DigestItem için \"Algorithm\" değeri SHA512 olmalıdır. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.ALGORITHM_SHA512_KULLANILMAMIS,
                    Uri = item.URI
                });
                return false;
            }

            OzetModu mod;
            if (string.Compare(item.DigestItem.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA256), true) == 0)
                mod = OzetModu.SHA256;
            else if (string.Compare(item.DigestItem.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA512), true) == 0)
                mod = OzetModu.SHA512;
            else if (string.Compare(item.DigestItem.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA384), true) == 0)
                mod = OzetModu.SHA384;
            else
            {
                sonuclar.Add(new OzetDogrulamaHatasi { Hata = string.Format("Desteklenmeyen OzetModu. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.DESTEKLENMEYEN_OZETMODU, Uri = item.URI });
                return false;
            }
            OzetModu mod1;
            if (string.Compare(item.DigestItem1.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA256), true) == 0)
                mod1 = OzetModu.SHA256;
            else if (string.Compare(item.DigestItem1.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA512), true) == 0)
                mod1 = OzetModu.SHA512;
            else if (string.Compare(item.DigestItem1.DigestMethod.Algorithm, Araclar.OzetModuToString(OzetModu.SHA384), true) == 0)
                mod1 = OzetModu.SHA384;
            else
            {
                sonuclar.Add(new OzetDogrulamaHatasi { Hata = string.Format("Desteklenmeyen OzetModu. URI:{0}.", item.URI), HataKodu = OzetDogrulamaHataKodu.DESTEKLENMEYEN_OZETMODU, Uri = item.URI });
                return false;
            }
            if (item.Type != null && item.Type == Araclar.HARICI_PAKET_BILESENI_REFERANS_TIPI)
            {
                return true;
            }


            Stream stream;
            try
            {
                stream= GetStreamFromPackagePart(item);
                //stream = _package.GetPart(PackUriHelper.CreatePartUri(new Uri(Uri.UnescapeDataString(item.URI), UriKind.Relative))).GetStream();
            }
            catch (Exception e)
            {
                /*
                 * /Paraflar/ParafImzaCades.imz zorunlu olmadığından ve daha sonra paket içerisinden çıkartılabildiğinden dolayı
                 * eğer paket içerisinde yok ise paketin içerisinden çıkartıldığı çıkartıldığı düşünülmektedir ve doğrulama sonucunda özel hata kodu döndürülmektedir.
                 * Kendisine gönderilen paketi açan kurum ParafImza bileşeninin çıkartıldığını varsaymalıdır ve paketin doğruluğunu kabul etmelidir.
                 */
                if (item.URI == Araclar.URI_PARAFIMZA)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PARAF_IMZA_BILESENI_YOK,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }
                if (!item.URI.StartsWith("/Ekler/"))
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }

                if (dagitimIdentifier == null)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }

                var dagitimlar = Ustveri.DagitimlariAl();
                if (dagitimlar == null || dagitimlar.Length == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }

                var ekler = Ustveri.EkleriAl();
                if (ekler == null || ekler.Length == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }

                var ekAdi = item.URI.Replace("/Ekler/", "");
                if (string.IsNullOrWhiteSpace(ekAdi))
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }

                var ctEk = ekler.FirstOrDefault(p => p.DosyaAdi == ekAdi);
                if (ctEk == null)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }


                CT_Dagitim ctDagitim = null;
                foreach (var dagitim in dagitimlar)
                {
                    if (dagitim.Item is CT_KurumKurulus)
                    {
                        var kurumKurulus = (CT_KurumKurulus)dagitim.Item;
                        if (!string.IsNullOrWhiteSpace(kurumKurulus.KKK) && kurumKurulus.KKK == dagitimIdentifier.Value)
                        {
                            ctDagitim = dagitim;
                            break;
                        }
                    }
                    else if (dagitim.Item is CT_TuzelSahis)
                    {
                        var tuzelSahis = (CT_TuzelSahis)dagitim.Item;
                        if (tuzelSahis.Id != null && string.IsNullOrWhiteSpace(tuzelSahis.Id.Value) && tuzelSahis.Id.Value == dagitimIdentifier.Value)
                        {
                            ctDagitim = dagitim;
                            break;
                        }
                    }
                    else if (dagitim.Item is CT_GercekSahis)
                    {
                        var gercekSahis = (CT_GercekSahis)dagitim.Item;
                        if (!string.IsNullOrWhiteSpace(gercekSahis.TCKN) && gercekSahis.TCKN == dagitimIdentifier.Value)
                        {
                            ctDagitim = dagitim;
                            break;
                        }
                    }
                }

                if (ctDagitim == null || ctDagitim.KonulmamisEkListesi == null || ctDagitim.KonulmamisEkListesi.Length == 0 || !ctDagitim.KonulmamisEkListesi.Any(p => p.EkId == ctEk.Id.Value))
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = string.Format("Paket bileşeni alınamadı. URI:{0}.", item.URI),
                        HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENI_ALINAMADI,
                        Uri = item.URI,
                        InnerException = e
                    });
                    return false;
                }


                return true;
            }

            byte[] computedDigestValue;
            byte[] computedDigestValue1;
            try
            {
                computedDigestValue = Araclar.OzetHesapla(stream, mod);
                stream.Position = 0;
                computedDigestValue1 = Araclar.OzetHesapla(stream, mod1);
            }
            catch (Exception e)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("Paket bileşenine ait hash hesaplanamadı. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.PAKET_BILESENINE_AIT_HASH_HESAPLANAMADI,
                    Uri = item.URI,
                    InnerException = e
                });
                return false;
            }

            if (!computedDigestValue.SequenceEqual(item.DigestItem.DigestValue))
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("Hashler eşit değil. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.HASHLER_ESIT_DEGIL,
                    Uri = item.URI
                });
                return false;
            }

            if (!computedDigestValue1.SequenceEqual(item.DigestItem1.DigestValue))
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = string.Format("Hashler eşit değil. URI:{0}.", item.URI),
                    HataKodu = OzetDogrulamaHataKodu.HASHLER_ESIT_DEGIL,
                    Uri = item.URI
                });
                return false;
            }

            return sonuclar.Count == 0;
        }

        #endregion

        #region Paraf Ozeti
        /// <summary>   Paket içerisindeki ParafOzeti bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki ParafOzeti bileşeninin STREAM nesnesi. ParafOzeti bileşeni olmaması durumunda null döner. </returns>
        public Stream ParafOzetiAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative)))
            {
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;
                return memoryStream;
            }
            return null;
        }


        /// <summary>   Paket içerisinde ParafOzeti olup olmadığını döner. </summary>
        /// <returns>   Paket içerisinde ParafOzeti varsa true, yoksa false döner. </returns>
        public bool ParafOzetiVarMi()
        {
            return _package.PartExists(new Uri(Araclar.URI_PARAFOZETI, UriKind.Relative));
        }
        /// <summary>
        /// Verilen ParafOzeti nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak ParafÖzeti nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 3   ParafOzet'inde Ustveri ozet değeri yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 5   ParafOzet'inde BelgeHedef ozet değeri yok.
        /// 6   UstYazi bileşeni yok.
        /// 7   ParafOzet'inde UstYazi ozet değeri yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.</returns>
        public bool ParafOzetiDogrula(CT_ParafOzeti ozet, ref List<OzetDogrulamaHatasi> sonuclar, IdentifierType dagitimIdentifier = null)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Ozet değeri verilmemiş.",
                    HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS
                });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Ustveri bileşeni yok.",
                    HataKodu = OzetDogrulamaHataKodu.USTVERI_BILESENI_YOK
                });
                return false;
            }

            Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri;
            if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "ParafOzet'inde Ustveri ozet değeri yok.",
                    HataKodu = OzetDogrulamaHataKodu.PARAFOZET_INDE_USTVERI_OZET_DEGERI_YOK
                });
                return false;
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "UstYazi bileşeni yok.",
                    HataKodu = OzetDogrulamaHataKodu.USTYAZI_BILESENI_YOK
                });
                return false;
            }

            Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri;
            if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "ParafOzet'inde UstYazi ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.PARAFOZET_INDE_USTYAZI_OZET_DEGERI_YOK });
                return false;
            }
            foreach (CT_Reference item in ozet.Reference)
            {
                ReferansDogrula(item, ref sonuclar, dagitimIdentifier);
            }
            return sonuclar.Count == 0;
        }

        #endregion

        #region Nihai Ozeti
        /// <summary>   Paket içerisindeki NihaiOzet bileşeni olup olmadığını döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşen varsa true aksi durumda false döner. </returns>
        public bool NihaiOzetVarMi()
        {
            return _package.PartExists(PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)));
        }

        /// <summary>   Paket içerisindeki NihaiOzet bileşenini STREAM olarak döner. </summary>
        /// <returns>   Paket içerisindeki NihaiOzet bileşeninin STREAM nesnesi. NihaiOzet bileşeni olmaması durumunda null döner. </returns>
        public Stream NihaiOzetAl()
        {
            Uri nihaiOzetUri = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative));

            // Guard clause to return null if the part doesn't exist
            if (!_package.PartExists(nihaiOzetUri))
            {
                return null;
            }

            MemoryStream memoryStream = new MemoryStream();
            using (Stream partStream = _package.GetPart(nihaiOzetUri).GetStream())
            {
                // Copy the content of the partStream into memoryStream
                partStream.CopyTo(memoryStream);
            }

            // Ensure the MemoryStream's position is set to the beginning for reading
            memoryStream.Position = 0;

            return memoryStream;
        }


        /// <summary>
        /// Verilen NihaiOzet nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak NihaiOzet nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.
        /// 2   Ustveri bileşeni yok.
        /// 4   BelgeHedef bileşeni yok.
        /// 6   UstYazi bileşeni yok.
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.
        /// 16  BelgeImza bileşeni yok.
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.
        /// 19  NihaiOzet'te Ustveri ozet değeri yok.
        /// 20  NihaiOzet'te BelgeHedef ozet değeri yok.
        /// 21  NihaiOzet'te UstYazi ozet değeri yok.
        /// 22  NihaiOzet'te UstYazi ozet değeri yok.
        /// 23  NihaiOzet'te UstYazi ozet değeri yok.
        /// 24  NihaiOzet'te UstYazi ozet değeri yok.
        /// </returns>
        /// <remarks> Java API için açıklama. .Net API kullanıyorsanız bu açıklamyı dikkate almayınız. 
        /// Kullanılan Apache POI API'sı, 'core' bileşenine ulaşılmasına engel olduğundan dolayı, NihaiOzetDogrula metodu Core bileşeni özetini doğrulamaz. Bu nedenle NihaiÖzetDogrula metodundan sonra NihaiOzetCoreDogrula yardımcı metodu kullanılarak doğrulama işlemi tamamlanır. </remarks>
        public bool NihaiOzetDogrula(CT_NihaiOzet ozet, ref List<OzetDogrulamaHatasi> sonuclar, IdentifierType dagitimIdentifier = null)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ozet değeri verilmemiş.", HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Ustveri bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTVERI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTVERI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Ustveri ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_USTVERI_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "UstYazi bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.USTYAZI_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_USTYAZI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te UstYazi ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_USTYAZI_OZET_DEGERI_YOK });
                    return false;
                }
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_PAKETOZETI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "PaketOzeti bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.PAKETOZETI_BILESENI_YOK });
                return false;
            }
            else
            {
                var relImza = _package.GetPart(new Uri(Araclar.URI_PAKETOZETI, UriKind.Relative)).GetRelationshipsByType(Araclar.RELATION_TYPE_IMZA);
                if (relImza == null || relImza.Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Imza bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.IMZA_BILESENI_YOK });
                    return false;
                }
                Uri readedImzaUri = relImza.First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedImzaUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Imza ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_IMZA_OZET_DEGERI_YOK });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "Core bileşeni yok.", HataKodu = OzetDogrulamaHataKodu.CORE_BILESENI_YOK });
                return false;
            }
            else
            {
                Uri readedCoreUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedCoreUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi() { Hata = "NihaiOzet'te Core ozet değeri yok.", HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_CORE_OZET_DEGERI_YOK });
                    return false;
                }
            }
            foreach (CT_Reference item in ozet.Reference)
            {
                ReferansDogrula(item, ref sonuclar, dagitimIdentifier);
            }
            if (sonuclar.Count == 0)
            {
                return true;
            }
            return false;
        }

        #endregion


        /// <summary> Kullanılan kaynakları kapatır.</summary>
        /// <remarks>Paket objesinin using bloğu ile kullanılması önerilir.</remarks>
        /// <example><code>
        ///  using (var paket = Paket.Ac("63771493-DDC0-476D-98A4-E025C5D3B42B.eyp"), PaketModu.Guncelle))
        ///  {
        ///     paket.EkCikar(new Guid(paket.Ustveri.EkleriAl().First().Id.Value));
        ///     paket.Kapat();
        ///  }
        ///  </code>
        /// </example>
        public void Dispose()
        {
            if (_streamInternal != null)
                _streamInternal.Close();
            if (_streamInternal != null)
                _streamInternal.Dispose();
        }
    }
}
