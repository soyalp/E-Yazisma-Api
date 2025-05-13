using System;
using System.Collections.Generic;
using System.Linq;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{

    /// <summary>
    /// Ustveri bileşeni bilgileri.
    /// </summary>
    public abstract class Ustveri : IDisposable
    {
        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }

        internal abstract CT_Ustveri CT_Ustveri { get; set; }

        /// <summary>
        /// Belgenin konusunu belirler.
        /// </summary>
        /// <param name="konu">Belge konusu.</param>
        /// <example><code>paket.Ustveri.KonuBelirle("e-Yazışma Test Dokümanı");</code></example>
        public abstract void KonuBelirle(TextType konu);

        /// <summary>
        /// Belge konusunu döner.
        /// </summary>
        /// <returns>Belge konusu.</returns>
        public abstract TextType KonuAl();

        /// <summary>
        /// Belge tekil anahtarını belirler.
        /// </summary>
        /// <param name="belgeId">Tekil anahtar değeri.</param>
        /// <example><code>paket.Ustveri.BelgeIdBelirle(Guid.NewGuid());</code></example>
        public abstract void BelgeIdBelirle(Guid belgeId);

        /// <summary>
        /// Belge tekil anahtar değerini döner.
        /// </summary>
        /// <returns>Belge tekil anahtar değeri.</returns>
        public abstract string BelgeIdAl();

        /// <summary>
        /// Belge mime türünü belirler.
        /// </summary>
        /// <param name="mimeTuru">Mime türü.</param>
        internal abstract void MimeTuruBelirle(string mimeTuru);

        /// <summary>
        /// Belge mime türünü döner.
        /// </summary>
        /// <returns>Mime türü.</returns>
        public abstract string MimeTuruAl();

        /// <summary>
        /// Belge güvenlik kodunu belirler.
        /// </summary>
        /// <param name="guvenlikKodu">Güvenlik kodu.</param>
        /// <example><code>paket.Ustveri.GuvenlikKoduBelirle(ST_KodGuvenlikKodu.HZO);</code></example>
        public abstract void GuvenlikKoduBelirle(ST_KodGuvenlikKodu guvenlikKodu);

        /// <summary>
        /// Belge güvenlik kodunu döner.
        /// </summary>
        /// <returns>Güvenlik kodu.</returns>
        public abstract ST_KodGuvenlikKodu GuvenlikKoduAl();

        /// <summary>
        /// Belge güvenlik kodu geçerlilik tarihini belirler.
        /// </summary>
        /// <param name="gecerlilikTarihi">Güvenlik kodu geçerlilik tarihi.</param>
        /// <example><code>var islemTarihi = DateTime.Now;
        /// paket.Ustveri.GuvenlikGecerlilikTarihiBelirle(islemTarihi.AddYears(5));</code></example>
        public abstract void GuvenlikGecerlilikTarihiBelirle(DateTime gecerlilikTarihi);

        /// <summary>
        /// Belge güvenlik kodu geçerlilik tarihini döner.
        /// </summary>
        /// <returns>Güvenlik kodu geçerlilik tarihi.</returns>
        public abstract DateTime? GuvenlikGecerlilikTarihiAl();

        /// <summary>
        /// Belge öz tekil anahtar değerini belirler.
        /// </summary>
        /// <param name="ozId">Belgenin oluşturulduğu sistemdeki tekil anahtar değeri.</param>
        /// <param name="schemeId">Belgenin oluşturulduğu sistemdeki tekil anahtar değerinin şeması veya türü.</param>
        /// <remarks>Belgenin oluşturulduğu veya saklandığı sistemde, belge için belirlenmiş ID değeridir.</remarks>
        /// <example><code>paket.Ustveri.OzIdBelirle("6A690BBB-6680-43FA-A8B9-FA820A344CB9", "GUID");
        /// paket.Ustveri.OzIdBelirle("736723", "INT");</code></example>
        public abstract void OzIdBelirle(string ozId, string schemeId);

        /// <summary>
        /// Belge öz tekil anahtar değerini döner.
        /// </summary>
        /// <returns>Öz tekil anahtar değeri</returns>
        public abstract IdentifierType OzIdAl();

        /// <summary>
        /// Belge dilini belirler.
        /// </summary>
        /// <param name="dil">Belgenin dili.</param>
        /// <example><code>Ustveri.DilBelirle("tur");</code></example>
        /// <remarks>ISO 639-3 standartına uygun dil kodu verilmelidir.</remarks>
        public abstract void DilBelirle(string dil);

        /// <summary>
        /// Belge dilini döner.
        /// </summary>
        /// <returns>Belgenin dili.</returns>
        public abstract string DilAl();

        /// <summary>
        /// Belge oluşturanını belirler.
        /// </summary>
        /// <param name="kurumKurulus">Oluşturan kurum/kuruluş.</param>
        /// <example><code>
        /// var olusturan = new CT_KurumKurulus()
        /// {
        ///     KKK = "23ED5E7A75E0D1",
        ///     Adi = new NameType()
        ///     {
        ///         Value = "Kalkınma Bakanlığı"
        ///     },
        ///     IletisimBilgisi = new CT_IletisimBilgisi()
        ///     {
        ///         Ulke = new NameType() { Value = "Türkiye" },
        ///         Il = new NameType() { Value = "Ankara" },
        ///     }
        /// };
        /// paket.Ustveri.OlusturanBelirle(olusturan);</code></example> 
        public abstract void OlusturanBelirle(CT_KurumKurulus kurumKurulus);

        /// <summary>
        /// Belge oluşturanını belirler.
        /// </summary>
        /// <param name="tuzelSahis">Oluşturan tüzel şahıs.</param>
        public abstract void OlusturanBelirle(CT_TuzelSahis tuzelSahis);

        /// <summary>
        /// Belge oluşturanını belirler.
        /// </summary>
        /// <param name="gercekSahis">Oluşturan gerçek şahıs.</param>
        public abstract void OlusturanBelirle(CT_GercekSahis gercekSahis);

        /// <summary>
        /// Belge oluşturan bilgisini döner.
        /// </summary>
        /// <returns>Oluşturan.</returns>
        public abstract CT_Olusturan OlusturanAl();

        /// <summary>
        /// Belge üstverisine ek ekler.
        /// </summary>
        /// <param name="ek">Ek nesnesi.</param>
        /// <example><code>
        /// var ek1 = new CT_Ek()
        ///    {
        ///        Id = new CT_Id() { Value = Guid.NewGuid().ToString() },
        ///        BelgeNo = "B.06.0.000.0.00.00.00-320/5128",
        ///        Ad = new TextType() { Value = "Paket Standartları Analiz Raporu" },
        ///        DosyaAdi = "Analiz Raporu şŞiİğĞüÜöÖçÇıI.pdf",
        ///        MimeTuru = "application/pdf",
        ///        SiraNo = 1,
        ///        SiraNoSpecified = true,
        ///        Tur = ST_KodEkTuru.DED
        ///    };
        ///    paket.Ustveri.EkEkle(ek1);</code></example>
        public abstract void EkEkle(CT_Ek ek);

        /// <summary>
        /// Belge eklerini döner.
        /// </summary>
        /// <returns>Ek listesi.</returns>
        public abstract CT_Ek[] EkleriAl();

        /// <summary>
        /// Bele üstverisine ilgi ekler.
        /// </summary>
        /// <param name="ilgi">İlgi nesnesi.</param>
        /// <example><code>
        ///  var ilgi1 = new CT_Ilgi()
        ///   {
        ///       Id = new CT_Id() { Value = Guid.NewGuid().ToString() },
        ///       BelgeNo = "B.06.0.000.0.00.00.00-110/132",
        ///       Etiket = "a",
        ///       Tarih = new DateTime(2009, 10, 2),
        ///       TarihSpecified = true,
        ///       Aciklama = new TextType() { Value = "" },
        ///       EkId = ek1.Id.Value
        ///   };
        ///   paket.Ustveri.IlgiEkle(ilgi1);</code>
        /// </example>
        public abstract void IlgiEkle(CT_Ilgi ilgi);

        /// <summary>
        /// Belge ilgilerini döner.
        /// </summary>
        /// <returns>İlgi listesi.</returns>
        public abstract CT_Ilgi[] IlgileriAl();

        /// <summary>
        /// Belge üstverisine dağıtım ekler.
        /// </summary>
        /// <param name="dagitim">Dağıtım nesnesi.</param>
        /// <example><code>
        /// var dagitim1 = new CT_Dagitim()
        ///   {
        ///       Item = new CT_KurumKurulus()
        ///       {
        ///           KKK = "23ED5E763584E6",
        ///           Adi = new NameType()
        ///           {
        ///               Value = "Cumhurbaşkanlığı"
        ///           },
        ///           IletisimBilgisi = new CT_IletisimBilgisi()
        ///           {
        ///               Ulke = new NameType() { Value = "Türkiye" },
        ///               Il = new NameType() { Value = "Ankara" },
        ///           }
        ///       },
        ///       DagitimTuru = ST_KodDagitimTuru.GRG,
        ///       Ivedilik = ST_KodIvedilik.ACL
        ///   };
        ///   paket.Ustveri.DagitimEkle(dagitim1);</code>
        /// </example>
        public abstract void DagitimEkle(CT_Dagitim dagitim);

        /// <summary>
        /// Belge dağıtımlarını alır.
        /// </summary>
        /// <returns>Dağıtım listesi.</returns>
        public abstract CT_Dagitim[] DagitimlariAl();

        /// <summary>
        /// Belge üstverisine ilgili ekler.
        /// </summary>
        /// <param name="kurumKurulus">İlgili kurum kuruluş.</param>
        public abstract void IlgiliEkle(CT_KurumKurulus kurumKurulus);

        /// <summary>
        /// Belge üstverisine ilgili ekler.
        /// </summary>
        /// <param name="tuzelSahis">İlgili tüzel şahıs.</param>
        public abstract void IlgiliEkle(CT_TuzelSahis tuzelSahis);

        /// <summary>
        /// Belge üstverisine ilgili ekler.
        /// </summary>
        /// <param name="gercekSahis">İlgili gerçek şahıs.</param>
        /// <example>
        /// <code>
        /// var ilgili = new CT_GercekSahis()
        ///   {
        ///       Kisi = new CT_Kisi()
        ///       {
        ///           OnEk = new TextType() { Value = "Sayın" },
        ///           IlkAdi = new NameType() { Value = "Mehmet" },
        ///           Soyadi = new NameType() { Value = "Güngör" }
        ///       },
        ///       Gorev = new TextType() { Value = "" },
        ///       TCKN = "98765432109",
        ///       IletisimBilgisi = new CT_IletisimBilgisi()
        ///       {
        ///           Telefon = "3122909292-1434",
        ///           EPosta = "mehmetg@mfa.gov.tr",
        ///           WebAdresi = "",
        ///           Faks = "3122909293"
        ///       }
        ///   };
        ///   paket.Ustveri.IlgiliEkle(ilgili);
        ///   </code>
        /// </example>
        public abstract void IlgiliEkle(CT_GercekSahis gercekSahis);

        /// <summary>
        /// Belge üstverisine ilgili ekler.
        /// </summary>
        /// <returns>İlgili listesi.</returns>
        public abstract CT_Ilgili[] IlgilileriAl();

        public abstract void SdpEkle(CT_SDP sdp);

        public abstract void DigerSdpEkle(CT_SDP sdp);

        public abstract CT_SDPBilgisi SDPBilgisiAl();

        public abstract void HeysKoduEkle(CT_HEYSK heysk);

        public abstract CT_HEYSK[] HeysKodlariniAl();

        /// <summary>
        /// Belge üstverisine belge doğrulama bilgilerini ekler
        /// </summary>
        /// <example>
        /// <code>
        /// var dogrulamaBilgisi = new CT_DogrulamaBilgisi
        /// {
        ///     DogrulamaAdresi = "https://www.turkiye.gov.tr/belge-dogrulama"
        /// }; 
        /// </code>
        /// </example>
        /// <param name="dogrulamaBilgisi">Doğrulama adresi içeren doğrulama bilgisi</param>
        public abstract void DogrulamaBilgisiEkle(CT_DogrulamaBilgisi dogrulamaBilgisi);

        /// <summary>
        /// Belge üstverisinden doğrulama bilgilerini alır
        /// </summary>
        /// <returns></returns>
        public abstract CT_DogrulamaBilgisi DogrulamaBilgisiAl();

        /// <summary>
        /// Belgenin dosya sisteminde kullanılmak üzere adını belirler.
        /// </summary>
        /// <param name="dosyaAdi">Dosya adı.</param>
        /// <example><code>Ustveri.DosyaAdiBelirle("Sunum.pdf");</code></example>
        public abstract void DosyaAdiBelirle(string dosyaAdi);
        /// <summary>
        /// Belgenin dosya sisteminde kullanılmak üzere adını döner.
        /// </summary>
        /// <returns>Dosya adı.</returns>
        public abstract string DosyaAdiAl();
        /// <summary>
        /// Üstverinin kurallara uygun olup olmadığını kontrol eder.
        /// </summary>
        internal abstract void KontrolEt();
    }

    internal class UstveriInternal : Ustveri
    {
        internal override CT_Ustveri CT_Ustveri { get; set; }

        public UstveriInternal()
        {
            CT_Ustveri = new CT_Ustveri();
        }
        internal UstveriInternal(CT_Ustveri ustveri)
        {
            CT_Ustveri = ustveri;
        }
        public override void KonuBelirle(TextType konu)
        {
            if (konu == null)
                throw new ArgumentNullException("konu");
            if (konu.Value == null || konu.Value.IsNullOrWhiteSpace())
                throw new Exception("Konu değeri boş olamaz.");
            CT_Ustveri.Konu = konu;
        }

        public override void BelgeIdBelirle(Guid belgeId)
        {
            if (belgeId == Guid.Empty)
                throw new ArgumentNullException("belgeId");
            CT_Ustveri.BelgeId = belgeId.ToString().ToUpperInvariant();
        }

        internal override void MimeTuruBelirle(string mimeTuru)
        {
            if (mimeTuru.IsNullOrWhiteSpace())
                throw new ArgumentNullException("mimeTuru");
            CT_Ustveri.MimeTuru = mimeTuru;
        }
        public override void GuvenlikKoduBelirle(ST_KodGuvenlikKodu guvenlikKodu)
        {
            if(guvenlikKodu == ST_KodGuvenlikKodu.OZL)
            {
                throw new Exception("Özel güvenlik kodu kaldırılmıştır.");
            }
            CT_Ustveri.GuvenlikKodu = guvenlikKodu;
        }
        public override void GuvenlikGecerlilikTarihiBelirle(DateTime guvenlikGecerlilikTarihi)
        {
            CT_Ustveri.GuvenlikKoduGecerlilikTarihi = guvenlikGecerlilikTarihi;
            CT_Ustveri.GuvenlikKoduGecerlilikTarihiSpecified = true;
        }
        public override void OzIdBelirle(string ozId, string schemeId)
        {
            if (ozId.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ozId");
            if (schemeId.IsNullOrWhiteSpace())
                throw new ArgumentNullException("schemeId");
            CT_Ustveri.OzId = new IdentifierType() { Value = ozId, schemeID = schemeId.ToUpperInvariant() };
        }
        public override void DilBelirle(string dil)
        {
            if (dil == null)
                throw new ArgumentNullException("dil");
            if (string.IsNullOrEmpty(dil))
                throw new ArgumentNullException("dil");
            if (dil.Trim().Length != 3)
                throw new ArgumentException("dil");
            CT_Ustveri.Dil = dil;
        }
        public override void OlusturanBelirle(CT_KurumKurulus kurumKurulus)
        {
            if (kurumKurulus == null)
                throw new ArgumentNullException("kurumKurulus");
            kurumKurulus.KontrolEt();
            CT_Ustveri.Olusturan = new CT_Olusturan() { Item = kurumKurulus };
        }
        public override void OlusturanBelirle(CT_TuzelSahis tuzelSahis)
        {
            if (tuzelSahis == null)
                throw new ArgumentNullException("tuzelSahis");
            CT_Ustveri.Olusturan = new CT_Olusturan() { Item = tuzelSahis };
        }
        public override void OlusturanBelirle(CT_GercekSahis gercekSahis)
        {
            if (gercekSahis == null)
                throw new ArgumentNullException("gercekSahis");
            CT_Ustveri.Olusturan = new CT_Olusturan() { Item = gercekSahis };
        }
        public override void EkEkle(CT_Ek ek)
        {
            if (ek == null)
                throw new ArgumentNullException("ek");

            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("Id");

            switch (ek.Tur)
            {
                case ST_KodEkTuru.DED:
                    if (ek.MimeTuru.IsNullOrWhiteSpace())
                        throw new ArgumentNullException("MimeTuru");
                    break;
                case ST_KodEkTuru.HRF:
                    if (ek.Referans.IsNullOrWhiteSpace())
                        throw new ArgumentNullException("Referans");
                    break;
                case ST_KodEkTuru.FZK:
                    break;
            }

            ek.Id.Value = ek.Id.Value.ToUpperInvariant();

            if (CT_Ustveri.Ekler == null)
                CT_Ustveri.Ekler = new CT_Ek[0];

            List<CT_Ek> ekler = CT_Ustveri.Ekler.ToList();
            ekler.Add(ek);
            CT_Ustveri.Ekler = ekler.ToArray();
        }
        public override void IlgiEkle(CT_Ilgi ilgi)
        {
            if (ilgi == null)
                throw new ArgumentNullException("ilgi");

            if (ilgi.Id == null || ilgi.Id.Value.IsNullOrWhiteSpace())
                throw new ArgumentNullException("ilgi.Id");

            ilgi.Id.Value = ilgi.Id.Value.ToUpperInvariant();

            if (!string.IsNullOrWhiteSpace(ilgi.EkId))
            {
                ilgi.EkId = ilgi.EkId.ToUpperInvariant();
            }

            if (CT_Ustveri.Ilgiler == null)
                CT_Ustveri.Ilgiler = new CT_Ilgi[0];

            List<CT_Ilgi> ilgiler = CT_Ustveri.Ilgiler.ToList();
            ilgiler.Add(ilgi);
            CT_Ustveri.Ilgiler = ilgiler.ToArray();
        }
        internal override void KontrolEt()
        {
            if (CT_Ustveri.Konu == null || CT_Ustveri.Konu.Value.IsNullOrWhiteSpace())
                throw new Exception("Üstveri bileşeni, \"Konu\" alanı için değer verilmemiş.");
            if (CT_Ustveri.MimeTuru.IsNullOrWhiteSpace())
                throw new Exception("Üstveri bileşeni, \"MimeTuru\" alanı için değer verilmemiş.");
            if (CT_Ustveri.BelgeId.IsNullOrWhiteSpace())
                throw new Exception("Üstveri bileşeni, \"BelgeId\" alanı için değer verilmemiş.");
            if (CT_Ustveri.DosyaAdi.IsNullOrWhiteSpace())
                throw new Exception("Üstveri bileşeni, \"DosyaAdi\" alanı için değer verilmemiş.");
            if (CT_Ustveri.DogrulamaBilgisi == null)
                throw new Exception("Üstveri bileşeni, \"DogrulamaBilgisi\" alanı için değer verilmemiş.");
            if (string.IsNullOrWhiteSpace(CT_Ustveri.DogrulamaBilgisi.DogrulamaAdresi))
                throw new Exception("Üstveri bileşeni, \"DogrulamaBilgisi.DogrulamaAdresi\" alanı için değer verilmemiş.");
            if (CT_Ustveri.DagitimListesi == null || CT_Ustveri.DagitimListesi.Length == 0)
                throw new Exception("Üstveri bileşeni, \"DagitimListesi\" alanı için değer verilmemiş.");

            foreach (var item in CT_Ustveri.DagitimListesi)
                item.KontrolEt();

            if (CT_Ustveri.Ekler != null && CT_Ustveri.Ekler.Length > 0)
                foreach (var ek in CT_Ustveri.Ekler)
                    ek.KontrolEt();
            if (CT_Ustveri.Ilgiler != null && CT_Ustveri.Ilgiler.Length > 0)
                foreach (var ilgi in CT_Ustveri.Ilgiler)
                    ilgi.KontrolEt();
            if (CT_Ustveri.IlgiliListesi != null && CT_Ustveri.IlgiliListesi.Length > 0)
                foreach (var ilgili in CT_Ustveri.IlgiliListesi)
                    ilgili.KontrolEt();
            if (CT_Ustveri.HeyskListesi != null && CT_Ustveri.HeyskListesi.Length > 0)
                foreach (CT_HEYSK heysk in CT_Ustveri.HeyskListesi)
                    heysk.KontrolEt();

            CT_Ustveri.Olusturan.KontrolEt();
            if (CT_Ustveri.OzId != null)
            {
                if (CT_Ustveri.OzId.Value.IsNullOrWhiteSpace())
                    throw new Exception("OzId alan değeri boş olamaz.");
                if (CT_Ustveri.OzId.schemeID.IsNullOrWhiteSpace())
                    throw new Exception("OzId alanı SchemeID değeri boş olamaz.");
            }
        }
        public override void IlgiliEkle(CT_KurumKurulus kurumKurulus)
        {
            if (CT_Ustveri.IlgiliListesi == null)
                CT_Ustveri.IlgiliListesi = new CT_Ilgili[0];
            List<CT_Ilgili> ilgililer = CT_Ustveri.IlgiliListesi.ToList();
            CT_Ilgili ilgili = new CT_Ilgili();
            ilgili.Item = kurumKurulus;
            ilgililer.Add(ilgili);
            CT_Ustveri.IlgiliListesi = ilgililer.ToArray();
        }
        public override void SdpEkle(CT_SDP sdp)
        {
            if (CT_Ustveri.SdpBilgisi == null)
                CT_Ustveri.SdpBilgisi = new CT_SDPBilgisi();
            CT_Ustveri.SdpBilgisi.AnaSdp = sdp;
        }
        public override void DigerSdpEkle(CT_SDP sdp)
        {
            if (CT_Ustveri.SdpBilgisi == null)
                CT_Ustveri.SdpBilgisi = new CT_SDPBilgisi();
            if (CT_Ustveri.SdpBilgisi.DigerSdpler == null)
                CT_Ustveri.SdpBilgisi.DigerSdpler = new CT_SDP[0];
            List<CT_SDP> digerSdpler = CT_Ustveri.SdpBilgisi.DigerSdpler.ToList();
            digerSdpler.Add(sdp);
            CT_Ustveri.SdpBilgisi.DigerSdpler = digerSdpler.ToArray();
        }
        public override void IlgiliEkle(CT_TuzelSahis tuzelSahis)
        {
            if (CT_Ustveri.IlgiliListesi == null)
                CT_Ustveri.IlgiliListesi = new CT_Ilgili[0];
            List<CT_Ilgili> ilgililer = CT_Ustveri.IlgiliListesi.ToList();
            CT_Ilgili ilgili = new CT_Ilgili();
            ilgili.Item = tuzelSahis;
            ilgililer.Add(ilgili);
            CT_Ustveri.IlgiliListesi = ilgililer.ToArray();
        }

        public override void IlgiliEkle(CT_GercekSahis gercekSahis)
        {
            if (CT_Ustveri.IlgiliListesi == null)
                CT_Ustveri.IlgiliListesi = new CT_Ilgili[0];

            List<CT_Ilgili> ilgililer = CT_Ustveri.IlgiliListesi.ToList();
            CT_Ilgili ilgili = new CT_Ilgili();
            ilgili.Item = gercekSahis;
            ilgililer.Add(ilgili);
            CT_Ustveri.IlgiliListesi = ilgililer.ToArray();
        }

        public override void DagitimEkle(CT_Dagitim dagitim)
        {
            if (dagitim == null)
                throw new ArgumentNullException("dagitim");

            if (dagitim.KonulmamisEkListesi != null && dagitim.KonulmamisEkListesi.Length > 0)
            {
                foreach (var ctKonulmamisEk in dagitim.KonulmamisEkListesi)
                {
                    ctKonulmamisEk.EkId = ctKonulmamisEk.EkId.ToUpperInvariant();
                }
            }

            if (CT_Ustveri.DagitimListesi == null)
                CT_Ustveri.DagitimListesi = new CT_Dagitim[0];
            List<CT_Dagitim> dagitimlar = CT_Ustveri.DagitimListesi.ToList();
            dagitimlar.Add(dagitim);
            CT_Ustveri.DagitimListesi = dagitimlar.ToArray();
        }

        public override TextType KonuAl()
        {
            return CT_Ustveri.Konu;
        }


        public override string BelgeIdAl()
        {
            return CT_Ustveri.BelgeId;
        }

        public override string MimeTuruAl()
        {
            return CT_Ustveri.MimeTuru;
        }

        public override ST_KodGuvenlikKodu GuvenlikKoduAl()
        {
            return CT_Ustveri.GuvenlikKodu;
        }

        public override DateTime? GuvenlikGecerlilikTarihiAl()
        {
            return CT_Ustveri.GuvenlikKoduGecerlilikTarihi;
        }

        public override IdentifierType OzIdAl()
        {
            return CT_Ustveri.OzId;
        }

        public override string DilAl()
        {
            return CT_Ustveri.Dil;
        }

        public override CT_Olusturan OlusturanAl()
        {
            return CT_Ustveri.Olusturan;
        }

        public override CT_Ek[] EkleriAl()
        {
            return CT_Ustveri.Ekler;
        }

        public override CT_Ilgi[] IlgileriAl()
        {
            return CT_Ustveri.Ilgiler;
        }

        public override CT_Dagitim[] DagitimlariAl()
        {
            return CT_Ustveri.DagitimListesi;
        }

        public override CT_Ilgili[] IlgilileriAl()
        {
            return CT_Ustveri.IlgiliListesi;
        }
        public override CT_SDPBilgisi SDPBilgisiAl()
        {
            return CT_Ustveri.SdpBilgisi;
        }

        public override void HeysKoduEkle(CT_HEYSK heysk)
        {
            if (CT_Ustveri.HeyskListesi == null)
                CT_Ustveri.HeyskListesi = new CT_HEYSK[0];
            List<CT_HEYSK> heyskListesi = CT_Ustveri.HeyskListesi.ToList();
            heyskListesi.Add(heysk);
            CT_Ustveri.HeyskListesi = heyskListesi.ToArray();
        }

        public override CT_HEYSK[] HeysKodlariniAl()
        {
            return CT_Ustveri.HeyskListesi;
        }

        public override void DogrulamaBilgisiEkle(CT_DogrulamaBilgisi dogrulamaBilgisi)
        {
            if (dogrulamaBilgisi == null)
                throw new ArgumentNullException("dogrulamaBilgisi");
            if (string.IsNullOrWhiteSpace(dogrulamaBilgisi.DogrulamaAdresi))
                throw new ArgumentNullException("dogrulamaAdresi");
            CT_Ustveri.DogrulamaBilgisi = dogrulamaBilgisi;
        }

        public override CT_DogrulamaBilgisi DogrulamaBilgisiAl()
        {
            return CT_Ustveri.DogrulamaBilgisi;
        }

        public override string DosyaAdiAl()
        {
            return CT_Ustveri.DosyaAdi;
        }

        public override void DosyaAdiBelirle(string dosyaAdi)
        {
            CT_Ustveri.DosyaAdi = dosyaAdi;
        }
    }
}
