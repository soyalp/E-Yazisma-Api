using Cbddo.eYazisma.Xsd;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// e-Yazışma paketini tanımlar. Paket oluşturma ve açma işlemleri bu sınıf ile yapılır.
    /// </summary>
    public class GuncellemePaketi : IDisposable
    {
        internal Package _package;
        internal PaketModu _paketModu;
        internal bool _orijinalPaketEklenmisMi;
        private OzetModu _varsayilanOzetModu = OzetModu.SHA384;
        private Stream _streamInternal;

        /// <summary>
        /// Paketin içerisindeki güncelleme bilgisine ilişkin bilgileri içeren nesneye ulaşılır.
        /// </summary>
        /// <value> 
        /// GuncellemeBilgisi nesnesi.
        /// </value>
        public GuncellemeBilgisi GuncellemeBilgisi { get; private set; }

        /// <summary>
        /// Paket içerisinde mühürlenen bileşenlere ait özet bilgilerinin bulunduğu objeye ulaşılır.
        /// </summary>
        /// <value> 
        /// NihaiOzet nesnesi. 
        /// </value>
        public NihaiOzet NihaiOzet { get; private set; }

        protected GuncellemePaketi()
        {

        }

        #region Aç Kapat
        /// <summary>
        /// Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.
        /// </summary>
        /// <exception cref="Exception">Güncelleme veya açma modunda e-Yazışma standartlarına uygun bir paket olmaması durumunda EXCEPTION oluşur.</exception>
        /// <param name="stream">Pakete ilişkin STREAM objesidir. </param>
        /// <param name="paketModu">Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir.</param>
        /// <returns>  İşlem yapılan paket objesi. </returns>
        /// <example>  
        /// Güncelleme paketi oluşturma örneği.
        /// <code>
        /// using (GuncellemePaketi paket = GuncellemePaketi.Ac(GuncellemePaketiDosyasiYolu, PaketModu.Olustur))
        /// {
        ///     CT_Olusturan olusturan;
        ///     using (Paket orijinalPaket = Paket.Ac(PaketDosyasiYolu, PaketModu.Ac))
        ///     {
        ///         olusturan = orijinalPaket.Ustveri.OlusturanAl();
        ///         orijinalPaket.Kapat();
        ///     }
        ///     paket.GuncellemeBilgisi.GuncellemeEkle(ST_GuncellemeTuru.GuvenlikKodu, new CT_GuvenlikKoduDegisiklikBilgisi
        ///     {
        ///         YeniGizlilikDerecesi = ST_KodGuvenlikKodu.YOK,
        ///         DegistirmeTarihi = new DateTime(2022, 05, 05),
        ///         Aciklama = "Belgenin gizlilik süresinin dolması nedeni ile Hizmete Özel olan gizlilik derecesi kaldırılmıştır",
        ///         KomisyonKarariBelgeNo = "E-68244839-170.01-44181",
        ///         KomisyonKarariBelgeId = "85E65A34-F460-4590-8BFA-A2DDB07E1B1D"
        ///     });
        ///     paket.GuncellemeBilgisiOlustur();
        ///     paket.OrijinalPaketEkle(new MemoryStream(File.ReadAllBytes(PaketDosyasiYolu)));
        ///     paket.CoreOlustur(Guid.NewGuid(), olusturan, "Gizlilik Derecesi Değiştirme");
        ///     if (!paket.NihaiOzetVarMi())
        ///     {
        ///         paket.NihaiOzetOlustur();
        ///     }
        ///     Stream nihaiOzet = paket.NihaiOzetAl();
        ///     byte[] imzaliNihaiOzet = Imzala(StreamToByteArray(nihaiOzet));
        ///     paket.MuhurEkle(imzaliNihaiOzet);
        ///     paket.Kapat();
        /// }        
        /// </code>
        /// </example>
        public static GuncellemePaketi Ac(Stream stream, PaketModu paketModu)
        {
            switch (paketModu)
            {
                case PaketModu.Guncelle:
                    var paket = new GuncellemePaketi
                    {
                        _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite)
                    };
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).Count() == 1)
                    {
                        try
                        {
                            CT_GuncellemeBilgisi readedGuncellemeBilgisi = (CT_GuncellemeBilgisi)new XmlSerializer(typeof(CT_GuncellemeBilgisi)).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).First().TargetUri)).GetStream(FileMode.Open));
                            paket.GuncellemeBilgisi = new GuncellemeBilgisiInternal(readedGuncellemeBilgisi);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"GuncellemeBilgisi\" bileşeni hatalı.", ex);
                        }
                    }
                    else
                    {
                        paket.GuncellemeBilgisi = new GuncellemeBilgisiInternal();
                    }
                    if (paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() == 1)
                    {
                        try
                        {
                            CT_NihaiOzet readedNihaiOzet = (CT_NihaiOzet)new XmlSerializer(typeof(CT_NihaiOzet)).Deserialize(paket._package.GetPart(PackUriHelper.CreatePartUri(paket._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri)).GetStream(FileMode.Open));
                            paket.NihaiOzet = new NihaiOzetInternal(readedNihaiOzet);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiOzet\" bileşeni hatalı.", ex);
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
                    var paketAcilan = new GuncellemePaketi { _package = Package.Open(stream, FileMode.Open, FileAccess.Read) };

                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"Core\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"NihaiOzet\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"OrijinalPaket\" bileşeni yok veya birden fazla.");
                    }
                    if (paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).Count() != 1)
                    {
                        throw new Exception("Geçersiz e-Yazışma paketi. \"GuncellemeBilgisi\" bileşeni yok veya birden fazla.");
                    }

                    Uri readedNihaiOzetUriAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_NIHAIOZET).First().TargetUri);
                    CT_NihaiOzet readedNihaiOzetAcilan = (CT_NihaiOzet)(new XmlSerializer(typeof(CT_NihaiOzet))).Deserialize(paketAcilan._package.GetPart(readedNihaiOzetUriAcilan).GetStream(FileMode.Open));

                    Uri uriGuncellemeBilgisiAcilan = PackUriHelper.CreatePartUri(paketAcilan._package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).First().TargetUri);
                    var readedGuncellemeBilgisiAcilan = (CT_GuncellemeBilgisi)(new XmlSerializer(typeof(CT_GuncellemeBilgisi))).Deserialize(paketAcilan._package.GetPart(uriGuncellemeBilgisiAcilan).GetStream(FileMode.Open));


                    paketAcilan.NihaiOzet = new NihaiOzetInternal(readedNihaiOzetAcilan);
                    paketAcilan.GuncellemeBilgisi = new GuncellemeBilgisiInternal(readedGuncellemeBilgisiAcilan);
                    paketAcilan.GuncellemeBilgisi.KontrolEt();
                    paketAcilan._paketModu = paketModu;
                    paketAcilan._streamInternal = stream;
                    return paketAcilan;

                case PaketModu.Olustur:
                    var paketOlusturulan = new GuncellemePaketi
                    {
                        NihaiOzet = new NihaiOzetInternal(),
                        GuncellemeBilgisi = new GuncellemeBilgisiInternal(),
                        _package = Package.Open(stream, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    };
                    paketOlusturulan._paketModu = paketModu;
                    paketOlusturulan._streamInternal = stream;
                    return paketOlusturulan;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Yeni bir paket oluşturmak, var olan bir paketi açmak veya güncellemek için kullanılır.
        /// </summary>
        /// <param name="dosyaYolu">Pakete ilişkin dosya yoludur. </param>
        /// <param name="paketModu">Paketin açılma, oluşturma veya güncelleme amacıyla açıldığını belirtir. </param>
        /// <returns>   İşlem yapılan paket objesi. </returns>
        public static GuncellemePaketi Ac(string dosyaYolu, PaketModu paketModu)
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

        /// <summary>
        /// İşlem yapılan paket için sonladırma işlemi yapılarak açılan kaynaklar kapatılır.
        /// </summary>
        public void Kapat()
        {
            _package.Close();
        }
        #endregion

        #region Ozellikler

        #region Olusturan Belirler

        /// <summary>
        /// Paket açıklama değeri verilir.
        /// </summary>
        /// <param name="aciklama">Verilen açıklama değeri.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: An explanation of the content of the resource. [Example: 
        /// Values might include an abstract, table of contents, 
        /// reference to a graphical representation of content, and a 
        /// free-text account of the content. end example] </remarks>
        public void PaketAciklamasiBelirle(string aciklama)
        {
            //DC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAciklamasiBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Description = aciklama;
        }

        /// <summary>
        /// Paket son yazdılırılma tarihi verilir.
        /// </summary>
        /// <param name="sonYazdirilmaTarihi">Son yazdırılma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The date and time of the last printing. </remarks>
        public void PaketSonYazdirilmaTarihiBelirle(DateTime? sonYazdirilmaTarihi)
        {
            // OPC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonYazdirilmaTarihiBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.LastPrinted = sonYazdirilmaTarihi;
        }

        /// <summary>
        /// Paket güncelleme tarihi verilir.
        /// </summary>
        /// <param name="guncellemeTarihi">Güncelleme tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date on which the resource was changed. </remarks>
        public void PaketGuncellemeTarihiBelirle(DateTime? guncellemeTarihi)
        {
            // DC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketGuncellemeTarihiBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Modified = guncellemeTarihi;
        }

        /// <summary>
        /// Paket son güncelleyen bilgisi verilir.
        /// </summary>
        /// <param name="sonGuncelleyen">Son güncelleyen bilgisi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The user who performed the last modification. The 
        ///identification is environment-specific. [Example: A name, 
        ///email address, or employee ID. end example] It is 
        ///recommended that this value be as concise as possible. </remarks>
        public void PaketSonGuncelleyenBelirle(string sonGuncelleyen)
        {
            // OPC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketSonGuncelleyenBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.LastModifiedBy = sonGuncelleyen;
        }

        /// <summary>
        /// Paket anahtar kelimeleri verilir.
        /// </summary>
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
        /// <value xml:lang="tr-TR">colour</value> 
        /// <value xml:lang="en-CA">colour</value> 
        /// <value xml:lang="fr-FR">couleur</value>
        /// </keywords>         
        ///end example]  </remarks>
        public void PaketAnahtarKelimeleriBelirle(string anahtarKelimeler)
        {
            // OPC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketAnahtarKelimeleriBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Keywords = anahtarKelimeler;
        }

        /// <summary>
        /// Paket durumu verilir.
        /// </summary>
        /// <param name="durum">Durum.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The status of the content. [Example: Values might include 
        ///“Draft”, “Reviewed”, and “Final”.  end example]  </remarks>
        public void PaketDurumuBelirle(string durum)
        {
            // OPC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDurumuBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.ContentStatus = durum;
        }

        /// <summary>
        /// Paket oluşturulma tarihi verilir.
        /// </summary>
        /// <param name="olusturulmaTarihi">Oluşturulma tarihi.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: Date of creation of the resource. </remarks>
        public void PaketOlusturulmaTarihiBelirle(DateTime? olusturulmaTarihi)
        {
            // DC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketOlusturulmaTarihiBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Created = olusturulmaTarihi;
        }

        /// <summary>
        /// Paket başlığı verilir.
        /// </summary>
        /// <param name="baslik">Başlık.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The name given to the resource. </remarks>
        public void PaketBasligiBelirle(string baslik)
        {
            // DC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketBasligiBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Title = baslik;
        }

        /// <summary>
        /// Paket dili verilir.
        /// </summary>
        /// <param name="dil">Dil.</param>
        /// <remarks>ISO/IEC 29500 Açıklaması: The language of the intellectual content of the resource. 
        /// [Note: IETF RFC 3066 provides guidance on encoding to 
        /// represent languages.  end note]  </remarks>
        public void PaketDiliBelirle(string dil)
        {
            //DC, olusturan belirler
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için PaketDiliBelirle fonksiyonu kullanılamaz.");
            }
            _package.PackageProperties.Language = dil;
        }
        #endregion

        #region Ustveri Ile Ayni

        private void PaketBelirteciBelirle(string belirtec)
        {
            //DC, Ustveri alanı ile aynı
            _package.PackageProperties.Identifier = belirtec;
        }

        private void PaketOlusturanBelirle(string olusturan)
        {
            // DC, Ustveri alanı ile aynı
            _package.PackageProperties.Creator = olusturan;
        }

        private void PaketKonusuBelirle(string konu)
        {
            // DC, Ustveri alanı ile aynı
            _package.PackageProperties.Subject = konu;
        }

        #endregion

        #region Static

        private void PaketKategorisiBelirle(string kategori)
        {
            // OPC, static
            _package.PackageProperties.Category = kategori;
        }

        private void PaketIcerikTuruBelirle(string icerikTuru)
        {
            // OPC, static
            _package.PackageProperties.ContentType = icerikTuru;
        }

        private void PaketVersiyonuBelirle(string versiyon)
        {
            // DC, static
            _package.PackageProperties.Version = versiyon;
        }

        private void PaketRevizyonuBelirle(string revisyon)
        {
            // DC, static
            _package.PackageProperties.Revision = revisyon;
        }

        #endregion

        #endregion

        #region Core
        /// <summary>
        /// Core bileşenini oluşturur
        /// </summary>
        /// <param name="belgeId"></param>
        /// <param name="olusturan"></param>
        /// <param name="konu"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void CoreOlustur(Guid belgeId, CT_Olusturan olusturan, string konu)
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için Core fonksiyonu kullanılamaz.");
            }
            if (_paketModu == PaketModu.Olustur)
            {
                if (belgeId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(belgeId));
                }

                if (olusturan == null)
                {
                    throw new ArgumentNullException(nameof(olusturan));
                }

                if (konu != null)
                {
                    PaketKonusuBelirle(konu);
                }

                PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(olusturan));
                PaketBelirteciBelirle(belgeId.ToString().ToUpperInvariant());
                PaketOlusturulmaTarihiBelirle(DateTime.Now);
                PaketKategorisiBelirle(Araclar.RESMIYAZISMAGUNCELLEME);
                PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
                PaketVersiyonuBelirle(Araclar.PAKET_VERSIYON_2_1);
                PaketRevizyonuBelirle(string.Format(Araclar.PAKET_REVIZYON, Assembly.GetAssembly(typeof(Paket)).GetName().Version));
                _package.Flush();

            }
            else if (_paketModu == PaketModu.Guncelle)
            {
                if (belgeId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(belgeId));
                }

                if (olusturan == null)
                {
                    throw new ArgumentNullException(nameof(olusturan));
                }

                if (konu != null)
                {
                    PaketKonusuBelirle(konu);
                }

                PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(olusturan));
                PaketBelirteciBelirle(belgeId.ToString().ToUpperInvariant());
                PaketGuncellemeTarihiBelirle(DateTime.Now);
                PaketOlusturanBelirle(Araclar.OlusturanAdiOlustur(olusturan));
                PaketKategorisiBelirle(Araclar.RESMIYAZISMAGUNCELLEME);
                PaketIcerikTuruBelirle(Araclar.EYAZISMAMIMETURU);
                PaketVersiyonuBelirle(Araclar.PAKET_VERSIYON_2_1);
                PaketRevizyonuBelirle(string.Format(Araclar.PAKET_REVIZYON, Assembly.GetAssembly(typeof(Paket)).GetName().Version));

                _package.Flush();

            }
        }

        #endregion

        #region Güncelleme Bilgisi

        /// <summary>
        /// Paket içerisine kelenmiş güncelleme bilgisini alır
        /// </summary>
        /// <returns></returns>
        public Stream GuncellemeBilgisiAl()
        {

            if (_package.PartExists(new Uri(Araclar.URI_GUNCELLEME_BILGISI, UriKind.Relative)))
            {
                // Create a MemoryStream to hold the content
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_GUNCELLEME_BILGISI, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }
            return null;
        }

        /// <summary>
        /// Ustveri bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.
        /// </summary>
        /// <remarks>
        /// GuncellemeBilgisi bileşeninin varsayılan OzetModu ile özet değeri hesaplanarak NihaiOzet objesine eklenir. 
        /// Oluşturma sırasında Ustveri bileşeni olası hatalara karşı kontrol edilir.
        /// </remarks>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void GuncellemeBilgisiOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new ApplicationException("PaketModu 'Ac' olarak işlem yapılan paketler için GuncellemeBilgisiOlustur fonksiyonu kullanılamaz.");
            }
            GuncellemeBilgisi.KontrolEt();
            var partUriGuncellemeBilgisi = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_GUNCELLEME_BILGISI, UriKind.Relative));
            if (_paketModu == PaketModu.Guncelle)
            {
                if (_package.PartExists(partUriGuncellemeBilgisi))
                {
                    _package.DeletePart(partUriGuncellemeBilgisi);
                    _package.DeleteRelationship(Araclar.ID_GUNCELLEME_BILGISI);
                }
            }

            var partGuncellemeBilgisi = _package.CreatePart(partUriGuncellemeBilgisi, Araclar.MIME_XML, CompressionOption.Normal);
            _package.CreateRelationship(partGuncellemeBilgisi.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_GUNCELLEME_BILGISI, Araclar.ID_GUNCELLEME_BILGISI);

            XmlSerializerNamespaces ss = new XmlSerializerNamespaces();
            ss.Add("tipler", "urn:dpt:eyazisma:schema:xsd:Tipler-2");

            XmlSerializer x = new XmlSerializer(typeof(CT_GuncellemeBilgisi));
            using (Stream stream = partGuncellemeBilgisi.GetStream()) // Open the stream for writing
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                // Set the formatting for pretty-printed XML
                xmlTextWriter.Formatting = Formatting.Indented;

                x.Serialize(xmlTextWriter, GuncellemeBilgisi.CT_GuncellemeBilgisi, ss);
            }
        }

        /// <summary>
        /// Paket içerisine GuncellemeBilgisi bileşeni eklenmiş mi kontrol eder.
        /// </summary>
        /// <returns></returns>
        public bool GuncellemeBilgisiVarMi()
        {
            return _package.PartExists(PackUriHelper.CreatePartUri(new Uri(Araclar.URI_GUNCELLEME_BILGISI, UriKind.Relative)));
        }

        #endregion

        #region Orijinal Paket

        /// <summary>
        /// Mevcut bir e-yazışma paketi veya güncelleme paketini oluşturulan paketin içerisine ekler
        /// </summary>
        /// <param name="paket">Paketi barındıran STREAM nesnesi.</param>
        public void OrijinalPaketEkle(Stream paket, PaketTipi paketTipi)
        {
            if (paketTipi == PaketTipi.SifreliPaket)
            {
                throw new Exception("Paket tipi SifreliPaket kullanılamaz");
            }
            if (_orijinalPaketEklenmisMi)
            {
                throw new ApplicationException("Daha önce paket eklenmiş.");
            }

            if (paket == null)
            {
                throw new ArgumentNullException(nameof(paket));
            }

            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception($"PaketModu 'Ac' olarak işlem yapılan paketler için {nameof(OrijinalPaketEkle)} fonksiyonu kullanılamaz.");
            }

            string dosyaUzantisi = string.Empty;
            if (paketTipi == PaketTipi.NormalPaket)
            {
                dosyaUzantisi = "eyp";
            }
            else if (paketTipi == PaketTipi.GuncellemePaketi)
            {
                dosyaUzantisi = "eypg";
            }

            var assemblyPartUri = PackUriHelper.CreatePartUri(new Uri(string.Format(Araclar.URI_FORMAT_ORIJINAL_PAKET, dosyaUzantisi), UriKind.Relative));
            var part = _package.CreatePart(assemblyPartUri, "application/x-zip-compressed", CompressionOption.Maximum);
            using (Stream stream = part.GetStream())
                paket.CopyTo(stream);

            _package.CreateRelationship(part.Uri, TargetMode.Internal, Araclar.RELATION_TYPE_ORIJINAL_PAKET, Araclar.ID_ORIJINAL_PAKET);
            paket.Position = 0;
            _orijinalPaketEklenmisMi = true;
        }


        /// <summary>  
        /// Paket içerisindeki OrijinalPaketi STREAM olarak döner. </summary>
        /// <returns>   
        /// Paket içerisindeki OrijinalPaketi STREAM nesnesi. OrijinalPaketi bileşeni olmaması durumunda null döner. </returns>
        public Stream OrijinalPaketAl()
        {
            // Get all relationships of the specified type
            var relationships = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).ToList();

            // Check if there is exactly one relationship
            if (relationships.Count == 1)
            {

                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(PackUriHelper.CreatePartUri(relationships.First().TargetUri)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }

            // Return null if there are no relationships or more than one
            return null;
        }

        /// <summary>
        /// Paket içerisine OrijinalPaket eklenmiş mi kontrol eder
        /// </summary>
        /// <returns></returns>
        public bool OrijinalPaketVarMi()
        {
            return _package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).Count() == 1;
        }

        #endregion

        #region Muhur

        /// <summary>
        /// Paket içerisine NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-A) değeri eklenir.
        /// </summary>
        /// <param name="muhur">NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-A) değeri.</param>
        /// <exception cref="System.ArgumentNullException">Muhur değeri null olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void MuhurEkle(byte[] muhur)
        {
            if (muhur == null)
            {
                throw new ArgumentNullException(nameof(muhur));
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
            part02.GetStream().Write(muhur, 0, muhur.Length);
            _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).CreateRelationship(PackUriHelper.GetRelativeUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative), assemblyPartUri), TargetMode.Internal, Araclar.RELATION_TYPE_MUHUR, Araclar.ID_MUHUR);
        }

        /// <summary> 
        /// Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-A) değeri olup olmadığını döner. 
        /// </summary>
        /// <returns>   
        /// Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-X-L) değeri varsa true aksi durumda false döner. 
        /// </returns>
        public bool MuhurVarMi()
        {
            return _package.PartExists(PackUriHelper.CreatePartUri(new Uri(Araclar.URI_MUHUR, UriKind.Relative)));
        }

        /// <summary>   
        /// Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-A) değeri STREAM olarak döner. 
        /// </summary>
        /// <returns>   
        /// Paket içerisindeki NihaiOzet bileşeninin imzalı (Ayrık olmayan CAdES-A) STREAM nesnesi. Mühürlü bileşen olmaması durumunda null döner. 
        /// </returns>
        public Stream MuhurAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_MUHUR, UriKind.Relative)))
            {
                // Create a MemoryStream to hold the content
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_MUHUR, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }
            return null;
        }

        #endregion

        #region Nihai Ozeti

        /// <summary>   
        /// Paket içerisindeki NihaiOzet bileşeni olup olmadığını döner. 
        /// </summary>
        /// <returns>   
        /// Paket içerisindeki NihaiOzet bileşen varsa true aksi durumda false döner. 
        /// </returns>
        public bool NihaiOzetVarMi()
        {
            return _package.PartExists(PackUriHelper.CreatePartUri(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)));
        }

        /// <summary>   
        /// Paket içerisindeki NihaiOzet bileşenini STREAM olarak döner. 
        /// </summary>
        /// <returns>   
        /// Paket içerisindeki NihaiOzet bileşeninin STREAM nesnesi. NihaiOzet bileşeni olmaması durumunda null döner. 
        /// </returns>
        public Stream NihaiOzetAl()
        {
            if (_package.PartExists(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)))
            {
                // Create a MemoryStream to hold the content
                MemoryStream memoryStream = new MemoryStream();
                using (Stream partStream = _package.GetPart(new Uri(Araclar.URI_NIHAIOZET, UriKind.Relative)).GetStream())
                {
                    partStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0; // Reset position for reading
                return memoryStream;
            }
            return null;
        }

        /// <summary>
        /// NihaiOzet bileşeninin 'serialize' edilerek paket içerisine eklenmesini sağlar.
        /// </summary>
        /// <exception cref="System.ApplicationException">PaketModu "Ac" olarak kullanıldığında oluşur.</exception>
        public void NihaiOzetOlustur()
        {
            if (_paketModu == PaketModu.Ac)
            {
                throw new Exception("PaketModu 'Ac' olarak işlem yapılan paketler için NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }

            if (!GuncellemeBilgisiVarMi())
            {
                throw new ApplicationException("Güncelleme bilgisi bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }

            if (!OrijinalPaketVarMi())
            {
                throw new ApplicationException("Orijinal paket bulunmadığı durumlarda NihaiOzetOlustur fonksiyonu kullanılamaz.");
            }


            var partUriGuncellemeBilgisi = PackUriHelper.CreatePartUri(new Uri(Araclar.URI_GUNCELLEME_BILGISI, UriKind.Relative));
            using (Stream guncellemeBilgiStream = GuncellemeBilgisiAl())
            {
                byte[] ozetGuncellemeBilgi = Araclar.OzetHesapla(guncellemeBilgiStream, _varsayilanOzetModu);
                guncellemeBilgiStream.Position = 0;
                byte[] ozetUstVeriSha512 = Araclar.OzetHesapla(guncellemeBilgiStream, OzetModu.SHA512);
                NihaiOzet.Ekle(_varsayilanOzetModu, ozetGuncellemeBilgi, ozetUstVeriSha512, partUriGuncellemeBilgisi);
            }

            var partUriOrijinalPaket = PackUriHelper.CreatePartUri(_package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).First().TargetUri);
            using (Stream orjinalPaketStream = OrijinalPaketAl())
            {
                byte[] ozetOrjinalPaket = Araclar.OzetHesapla(orjinalPaketStream, _varsayilanOzetModu);
                orjinalPaketStream.Position = 0;
                byte[] ozetOrjinalPaketSha512 = Araclar.OzetHesapla(orjinalPaketStream, OzetModu.SHA512);
                NihaiOzet.Ekle(_varsayilanOzetModu, ozetOrjinalPaket, ozetOrjinalPaketSha512, partUriOrijinalPaket);
            }


            var coreRelations = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_CORE);
            if (coreRelations == null || !coreRelations.Any())
            {
                throw new ApplicationException("Core bileşeni ilişkisi alınamadı.");
            }
            var corePart = _package.GetPart(coreRelations.First().TargetUri);
            var stream = corePart.GetStream();
            stream.Position = 0;
            byte[] ozet = Araclar.OzetHesapla(stream, _varsayilanOzetModu);
            stream.Position = 0;
            byte[] ozetSha512 = Araclar.OzetHesapla(stream, OzetModu.SHA512);
            NihaiOzet.Ekle(_varsayilanOzetModu, ozet, ozetSha512, coreRelations.First().TargetUri);

            NihaiOzet.CT_NihaiOzet.Id = _package.PackageProperties.Identifier.ToUpperInvariant();
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
            using (Stream nihaiOzetStream = partNihaiOzet.GetStream()) // Open the stream for writing
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(nihaiOzetStream, Encoding.UTF8))
            {
                // Set the formatting for pretty-printed XML
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlSerializer.Serialize(xmlTextWriter, NihaiOzet.CT_NihaiOzet);
            }
        }


        /// <summary>
        /// Verilen NihaiOzet nesnesindeki özet değerleri ile paket içerisindeki bileşenlerin özet değerlerini doğrular.
        /// </summary>
        /// <param name="ozet">Doğrulanacak NihaiOzet nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.        
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.        
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.                
        /// 28  DigestItem değeri boş
        /// 29  DigestItem1 değeri boş
        /// 30  DigestItem ve DigestItem1 için kullanılan algoritmalar aynı.
        /// 31  DigestItem ve DigestItem1'den en az birinde SHA512 kullanılmalı.
        /// 34  NihaiOzet'te Güncelleme Bilgisi bileşeni özet değeri yok.
        /// 36  NihaiOzet'te Güncelleme Paket bileşeni özet değeri yok.
        /// </returns>
        /// <remarks> Java API için açıklama. .Net API kullanıyorsanız bu açıklamyı dikkate almayınız. 
        /// Kullanılan Apache POI API'sı, 'core' bileşenine ulaşılmasına engel olduğundan dolayı, NihaiOzetDogrula metodu Core bileşeni özetini doğrulamaz. Bu nedenle NihaiÖzetDogrula metodundan sonra NihaiOzetCoreDogrula yardımcı metodu kullanılarak doğrulama işlemi tamamlanır. </remarks>
        public bool NihaiOzetDogrula(CT_NihaiOzet ozet, ref List<OzetDogrulamaHatasi> sonuclar)
        {
            if (sonuclar == null)
            {
                return false;
            }
            if (ozet == null)
            {
                sonuclar.Add(new OzetDogrulamaHatasi()
                {
                    Hata = "Ozet değeri verilmemiş.",
                    HataKodu = OzetDogrulamaHataKodu.OZET_DEGERI_VERILMEMIS
                });
                return false;
            }

            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI) == null
                || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Güncelleme bilgisi bileşeni yok.",
                    HataKodu = OzetDogrulamaHataKodu.GUNCELLEME_BILGISI_BILESENI_YOK
                });
                return false;
            }
            else
            {
                Uri readedUstveriUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_GUNCELLEME_BILGISI).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstveriUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = "NihaiOzet'te güncelleme bilgisi ozet değeri yok.",
                        HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_GUNCELLEME_BILGISI_OZET_DEGERI_YOK
                    });
                    return false;
                }
            }
            if (_package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET) == null || _package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).Count() == 0)
            {
                sonuclar.Add(new OzetDogrulamaHatasi
                {
                    Hata = "Orijinal paket bileşeni yok.",
                    HataKodu = OzetDogrulamaHataKodu.ORIJINAL_PANEL_BILESENI_YOK
                });
                return false;
            }
            else
            {
                Uri readedUstYaziUri = _package.GetRelationshipsByType(Araclar.RELATION_TYPE_ORIJINAL_PAKET).First().TargetUri;
                if (ozet.Reference.Where(x => (x.URI == PackUriHelper.CreatePartUri(readedUstYaziUri).ToString())).Count() == 0)
                {
                    sonuclar.Add(new OzetDogrulamaHatasi
                    {
                        Hata = "NihaiOzet'te Orijinal paket değeri yok.",
                        HataKodu = OzetDogrulamaHataKodu.NIHAIOZET_TE_ORIJINAL_PAKET_OZET_DEGERI_YOK
                    });
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
                ReferansDogrula(item, ref sonuclar);
            }
            if (sonuclar.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tek bir özet değeri ile paket içerisindeki ilgili bileşenin özet değerlerini doğrular.
        /// </summary>
        /// <param name="item">Doğrulanacak ÖzetDeğerini barındıran CT_Reference  nesnesi.</param>
        /// <param name="sonuclar">Doğrulanamayan bileşenler için açıklamaları barındırır.</param>
        /// <returns>Bileşen doğrulandığında true, aksi halde false döner.
        /// Hata Kodu Açıklamaları:
        /// 1   Ozet değeri verilmemiş.        
        /// 8   Reference değeri verilmemiş.
        /// 9   URI değeri boş
        /// 10  DigestMethod değeri boş.
        /// 11  Algorithm değeri boş.
        /// 12  Desteklenmeyen OzetModu.
        /// 13  Paket bileşeni alınamadı.
        /// 14  Paket bileşenine ait hash hesaplanamadı.
        /// 15  Hashler eşit değil.        
        /// 17  Imza bileşeni yok.
        /// 18  Core bileşeni yok.                
        /// 28  DigestItem değeri boş
        /// 29  DigestItem1 değeri boş
        /// 30  DigestItem ve DigestItem1 için kullanılan algoritmalar aynı.
        /// 31  DigestItem ve DigestItem1'den en az birinde SHA512 kullanılmalı.
        /// 34  NihaiOzet'te Güncelleme Bilgisi bileşeni özet değeri yok.
        /// 36  NihaiOzet'te Orijinal Paket bileşeni özet değeri yok.
        /// </returns>
        public bool ReferansDogrula(CT_Reference item, ref List<OzetDogrulamaHatasi> sonuclar)
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

            Stream stream = _package.GetPart(PackUriHelper.CreatePartUri(new Uri(Uri.UnescapeDataString(item.URI), UriKind.Relative))).GetStream();


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

        /// <summary>
        /// Kullanılan kaynakları kapatır.
        /// </summary>
        public void Dispose()
        {
            _streamInternal?.Close();
            _streamInternal?.Dispose();
        }
    }
}
