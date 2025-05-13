using System;
using System.IO;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// Sabit tanımlar ve yardımcı metodları içerir.
    /// </summary>
    public static class Araclar
    {
        /// <summary>
        /// Ustveri bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_USTVERI = "http://eyazisma.dpt/iliskiler/ustveri";
        /// <summary>
        /// Ustveri bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_NIHAIUSTVERI = "http://eyazisma.dpt/iliskiler/nihaiustveri";
        /// <summary>
        /// BelgeHedef bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_BELGEHEDEF = "http://eyazisma.dpt/iliskiler/belgehedef";
        /// <summary>
        /// PaketOzeti bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_PAKETOZETI = "http://eyazisma.dpt/iliskiler/paketozeti";
        /// <summary>
        /// ParafOzeti bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_PARAFOZETI = "http://eyazisma.dpt/iliskiler/parafozeti";
        /// <summary>
        /// NihaiOzet bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_NIHAIOZET = "http://eyazisma.dpt/iliskiler/nihaiozet";
        /// <summary>
        /// UstYazi bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_USTYAZI = "http://eyazisma.dpt/iliskiler/ustyazi";
        /// <summary>
        /// Imzali PaketOzeti bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_IMZA = "http://eyazisma.dpt/iliskiler/imzacades";
        /// <summary>
        /// Imzali ParafOzeti bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_PARAFIMZA = "http://eyazisma.dpt/iliskiler/parafimzacades";
        /// <summary>
        /// Mühürlü NihaiOzet bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_MUHUR = "http://eyazisma.dpt/iliskiler/muhurcades";
        /// <summary>
        /// Güncelleme bilgisi bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_GUNCELLEME_BILGISI = "http://eyazisma.dpt/iliskiler/guncellemebilgisi";
        /// <summary>
        /// Ek bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_EK = "http://eyazisma.dpt/iliskiler/ek";
        /// <summary>
        /// İmzasız ek bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_IMZASIZEK = "http://eyazisma.dpt/iliskiler/imzasizEk";
        /// <summary>
        /// Core bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_CORE = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties";
        /// <summary>
        /// ArşivOzellikleri bileşeni ilişki türü. Bu bileşenin tanımı ileride yapılacaktır.
        /// </summary>
        public const string RELATION_TYPE_ARSIVOZELLIKLERI = "http://eyazisma.dpt/iliskiler/arsivozellikleri";
        /// <summary>
        /// SifreliIcerik bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_SIFRELIICERIK = "http://eyazisma.dpt/iliskiler/sifreliicerik";
        /// <summary>
        /// OrijinalPaket bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_ORIJINAL_PAKET = "http://eyazisma.dpt/iliskiler/orijinalpaket";
        /// <summary>
        /// SifreliIcerikBilgisi bileşeni ilişki türü.
        /// </summary>
        public const string RELATION_TYPE_SIFRELIICERIKBILGISI = "http://eyazisma.dpt/iliskiler/sifreliicerikbilgisi";
        /// <summary>
        /// W3C XML Signature Syntax and Processing, SHA256 namespace
        /// </summary>
        public const string ALGORITHM_SHA256 = "http://www.w3.org/2001/04/xmlenc#sha256";
        /// <summary>
        /// W3C XML Signature Syntax and Processing, SHA512 namespace
        /// </summary>
        public const string ALGORITHM_SHA512 = "http://www.w3.org/2001/04/xmlenc#sha512";
        /// <summary>
        /// W3C XML Signature Syntax and Processing, SHA384 namespace
        /// </summary>
        public const string ALGORITHM_SHA384 = "http://www.w3.org/2001/04/xmldsig-more#sha384";
        /// <summary>
        /// Paket Özeti ve Nihai Özete eklenen paket dışı bileşenlere ilişkin tip değeri
        /// </summary>
        public const string HARICI_PAKET_BILESENI_REFERANS_TIPI = "http://eyazisma.dpt/bilesen#harici";
        /// <summary>
        /// Paket Özeti ve Nihai Özete eklenen paket içi bileşenlere ilişkin tip değeri
        /// </summary>
        public const string DAHILI_PAKET_BILESENI_REFERANS_TIPI = "http://eyazisma.dpt/bilesen#dahili";
        /// <summary>
        /// Şifreleme Yöntemi
        /// </summary>
        public const string SIFRELEME_YONTEMI = "Elektronik Belgeleri Açık Anahtar Altyapısı Kullanarak Güvenli İşleme Rehberi";
        /// <summary>
        /// Şifreleme Yöntemi Rehberi URI
        /// </summary>
        public const string SIFRELEME_YONTEMI_URI_1 = "http://www.kamusm.gov.tr/dokumanlar/belgeler";
        /// <summary>
        /// Şifreleme Yöntemi Rehberi URI
        /// </summary>
        public const string SIFRELEME_YONTEMI_URI_2 = "http://www.e-yazisma.gov.tr/SitePages/dokumanlar.aspx";
        /// <summary>
        /// Şifreleme Yöntemi Versiyonu
        /// </summary> 
        public const string SIFRELEME_YONTEMI_VERSIYONU = "1.4";
        /// <summary>
        /// e-Yazışma paketi kategorisi
        /// </summary>
        public const string RESMIYAZISMA = "RESMIYAZISMA";
        /// <summary>
        /// Şifreli e-Yazışma paketi kategorisi
        /// </summary>
        public const string RESMIYAZISMASIFRELI = "RESMIYAZISMA/SIFRELI";
        /// <summary>
        /// Güncelleme e-Yazışma paketi kategorisi
        /// </summary>
        public const string RESMIYAZISMAGUNCELLEME = "RESMIYAZISMA/GUNCELLEME";
        /// <summary>
        /// e-Yazışma paketi MIME türü
        /// </summary>
        public const string EYAZISMAMIMETURU = "application/eyazisma";
        /// <summary>
        /// Paket Versiyonu
        /// </summary>
        public const string PAKET_VERSIYON = "2.0";
        public const string PAKET_VERSIYON_2_1 = "2.1";
        /// <summary>
        /// Paket revizyonu
        /// </summary>
        public const string PAKET_REVIZYON = "DotNet/{0}";
        /// <summary>
        /// PaketOzeti bileşenine ait URI.
        /// </summary>
        public const string URI_PAKETOZETI = "/PaketOzeti/PaketOzeti.xml";
        /// <summary> 
        /// ParafOzeti bileşenine ait URI.
        /// </summary>
        public const string URI_PARAFOZETI = "/ParafOzeti/ParafOzeti.xml";
        /// <summary>
        /// NihaiOzet bileşenine ait URI.
        /// </summary>
        public const string URI_NIHAIOZET = "/NihaiOzet/NihaiOzet.xml";
        /// <summary> 
        /// Ustveri bileşenine ait URI.
        /// </summary>
        public const string URI_USTVERI = "/Ustveri/Ustveri.xml";
        /// <summary> 
        /// NihaiUstveri bileşenine ait URI.
        /// </summary>
        public const string URI_NIHAIUSTVERI = "/NihaiUstveri/NihaiUstveri.xml";
        /// <summary> 
        /// BelgeHedef bileşenine ait URI.
        /// </summary>
        public const string URI_BELGEHEDEF = "/BelgeHedef/BelgeHedef.xml";
        /// <summary> 
        /// UstYazi bileşenine ait URI formatı.
        /// </summary>
        public const string URI_FORMAT_USTYAZI = "/UstYazi/{0}";
        /// <summary> 
        /// Ek bileşenine ait URI.
        /// </summary>
        public const string URI_ROOT_EK = "Ekler";
        /// <summary> 
        /// İmzasızEk bileşenine ait URI.
        /// </summary>
        public const string URI_ROOT_IMZASIZEK = "ImzasizEkler";
        /// <summary> 
        /// Imzali PaketOzeti bileşenine ait URI.
        /// </summary>
        public const string URI_IMZA = "/Imzalar/ImzaCades.imz";
        /// <summary> 
        /// Imzali ParafOzeti bileşenine ait URI.
        /// </summary>
        public const string URI_PARAFIMZA = "/Paraflar/ParafImzaCades.imz";
        /// <summary> 
        /// Mühürlü NihaiOzet bileşenine ait URI.
        /// </summary>
        public const string URI_MUHUR = "/Muhur/MuhurCades.imz";
        /// <summary> 
        /// ArşivOzellikleri bileşenine ait URI. Bu bileşenin tanımı ileride yapılacaktır.
        /// </summary>
        public const string URI_ARSIVOZELLIKLERI = "/Arsiv/ArsivOzellikleri.xml";
        /// <summary>
        /// SifreliIcerik bileşenine ait URI.
        /// </summary>
        public const string URI_FORMAT_SIFRELIICERIK = "/SifreliIcerik/{0}";
        /// <summary> 
        /// SifreliIcerikBilgisi bileşenine ait URI.
        /// </summary>
        public const string URI_SIFRELIICERIKBILGISI = "/SifreliIcerikBilgisi/SifreliIcerikBilgisi.xml";
        /// <summary> 
        /// GuncellemeBilgisi bileşenine ait URI.
        /// </summary>
        public const string URI_GUNCELLEME_BILGISI = "/GuncellemeBilgisi/GuncellemeBilgisi.xml";
        /// <summary> 
        /// OrijinalPaket bileşenine ait URI.
        /// </summary>
        public const string URI_FORMAT_ORIJINAL_PAKET = "/OrijinalPaket/OrijinalPaket.{0}";
        /// <summary>
        /// Ek bileşeni ilişkisi Id formatı.
        /// </summary>
        public const string ID_ROOT_EK = "IdEk_";
        /// <summary> 
        /// İmzasız Ek bileşeni ilişkisi Id formatı.
        /// </summary>
        public const string ID_ROOT_IMZASIZEK = "IdImzasizEk_";
        /// <summary> 
        /// İmzalı PaketOzeti bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_IMZA = "IdImzaCades";
        /// <summary> 
        /// İmzalı ParafOzetu bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_PARAFIMZA = "IdParafImzaCades";
        /// <summary> 
        /// İmzalı NihaiOzet bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_MUHUR = "IdMuhurCades";
        /// <summary>
        /// UstYazi bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_USTYAZI = "IdUstYazi";
        /// <summary> 
        /// Ustveri bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_USTVERI = "IdUstveri";
        /// <summary> 
        /// NihaiUstveri bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_NIHAIUSTVERI = "IdNihaiUstveri";
        /// <summary>
        /// BelgeHedef bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_BELGEHEDEF = "IdBelgeHedef";
        /// <summary> 
        /// PaketOzeti bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_PAKETOZETI = "IdPaketOzeti";
        /// <summary> 
        /// ParafOzeti bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_PARAFOZETI = "IdParafOzeti";
        /// <summary> 
        /// NihaiOzet bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_NIHAIOZET = "IdNihaiOzet";
        /// <summary> 
        /// SifreliIcerik bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_SIFRELIICERIK = "IdSifreliIcerik";
        /// <summary> 
        /// Ek bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_SIFRELIICERIKBILGISI = "IdSifreliIcerikBilgisi";
        /// <summary> 
        /// Güncelleme bilgisi bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_GUNCELLEME_BILGISI = "IdGuncellemeBilgisi";
        /// <summary> 
        /// Orijinal paket bileşeni ilişkisi Id'si.
        /// </summary>
        public const string ID_ORIJINAL_PAKET = "IdOrijinalPaket";
        /// <summary> 
        /// XML Mime türü.
        /// </summary>
        public const string MIME_XML = "application/xml";
        /// <summary> 
        /// XML Octet-stream türü.
        /// </summary>
        public const string MIME_OCTETSTREAM = "application/octet-stream";


        #region Sha256
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA256 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha256OzetHesapla(string dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha256OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA256 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha256OzetHesapla(Stream dosya)
        {
            using (var sha256Managed = new SHA256Managed())
            {
                return sha256Managed.ComputeHash(dosya);
            }
        }
        #endregion

        #region Sha512
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA512 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha512OzetHesapla(string dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha512OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA512 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha512OzetHesapla(Stream dosya)
        {
            using (var sha512Managed = new SHA512Managed())
            {
                return sha512Managed.ComputeHash(dosya);
            }
        }
        #endregion


        #region Sha384
        /// <summary>
        /// Verilen lokasyondaki dosyanın SHA384 özetini hesaplar.
        /// </summary>
        /// <param name="dosyaYolu">Özeti hesaplanacak dosyanın yolu.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha384OzetHesapla(string dosyaYolu)
        {
            using (var fileStream = new FileStream(dosyaYolu, FileMode.Open))
            {
                return Sha384OzetHesapla(fileStream);
            }
        }
        /// <summary>
        /// Verilen STREAM'in SHA384 özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <returns>Özet değeri.</returns>
        public static byte[] Sha384OzetHesapla(Stream dosya)
        {
            using (var sha384Managed = new SHA384Managed())
            {
                return sha384Managed.ComputeHash(dosya);
            }
        }
        #endregion

        #region Ozet
        /// <summary>
        /// Verilen STREAM'in belirtilen algoritma ile özetini hesaplar.
        /// </summary>
        /// <param name="dosya">Özeti hesaplanacak STREAM.</param>
        /// <param name="mod">Özetleme algoritması.</param>
        /// <returns></returns>
        public static byte[] OzetHesapla(Stream dosya, OzetModu mod)
        {
            switch (mod)
            {
                case OzetModu.Yok:
                    return null;
                case OzetModu.SHA256:
                    return Sha256OzetHesapla(dosya);
                case OzetModu.SHA512:
                    return Sha512OzetHesapla(dosya);
                case OzetModu.SHA384:
                    return Sha384OzetHesapla(dosya);
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// Verilen CT_Dagitim nesnesini <see cref="CT_Hedef"/> nesnesine dönüştürür.
        /// </summary>
        /// <param name="dagitim">Dönüştürülecek <see cref="CT_Dagitim"/> nesnesi.</param>
        /// <returns><see cref="CT_Hedef"/> nesnesi.</returns>
        public static CT_Hedef Dagitim2Hedef(CT_Dagitim dagitim)
        {
            return new CT_Hedef
            {
                Item = dagitim.Item
            };            
        }

        /// <summary>
        /// .Net 4.0'da bulunan <see cref="String.IsNullOrWhiteSpace"/> fonksiyonu.
        /// </summary>
        /// <param name="s">Kontrol edilecek <see cref="String"/></param>
        /// <returns>true if the value parameter is null or <see cref="String.Empty"/>, or if value consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Verilen <see cref="CT_GercekSahis"/> nesnesindeki bilgileri kullanarak gerçek şahıs adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_GercekSahis"/> nesnesi.</param>
        /// <returns>Gerçek şahsın adı.</returns>
        private static string GercekSahisAdiOlustur(CT_GercekSahis olusturan)
        {
            string sonuc = string.Empty;
            string tcKimlikNo = olusturan.TCKN;
            string gorev = string.Empty;
            string ad = string.Empty;
            if (olusturan.Gorev != null && !olusturan.Gorev.Value.IsNullOrWhiteSpace())
            {
                gorev = olusturan.Gorev.Value;
            }
            if (olusturan.Kisi != null)
            {
                if (olusturan.Kisi.OnEk != null && !olusturan.Kisi.OnEk.Value.IsNullOrWhiteSpace())
                {
                    ad += olusturan.Kisi.OnEk.Value;
                }
                if (olusturan.Kisi.Unvan != null && !olusturan.Kisi.Unvan.Value.IsNullOrWhiteSpace())
                {
                    if (ad != string.Empty)
                    {
                        ad += " ";
                    }
                    ad += olusturan.Kisi.Unvan.Value;
                }
                if (olusturan.Kisi.IlkAdi != null && !olusturan.Kisi.IlkAdi.Value.IsNullOrWhiteSpace())
                {
                    if (ad != string.Empty)
                    {
                        ad += " ";
                    }
                    ad += olusturan.Kisi.IlkAdi.Value;
                }
                if (olusturan.Kisi.IkinciAdi != null && !olusturan.Kisi.IkinciAdi.Value.IsNullOrWhiteSpace())
                {
                    if (ad != string.Empty)
                    {
                        ad += " ";
                    }
                    ad += olusturan.Kisi.IkinciAdi.Value;
                }
                if (olusturan.Kisi.Soyadi != null && !olusturan.Kisi.Soyadi.Value.IsNullOrWhiteSpace())
                {
                    if (ad != string.Empty)
                    {
                        ad += " ";
                    }
                    ad += olusturan.Kisi.Soyadi.Value;
                }
            }
            if (!tcKimlikNo.IsNullOrWhiteSpace())
            {
                sonuc += tcKimlikNo + ",";
            }
            if (!gorev.IsNullOrWhiteSpace())
            {
                sonuc += gorev + ",";
            }
            if (!ad.IsNullOrWhiteSpace())
            {
                sonuc += ad + ",";
            }
            if (!sonuc.IsNullOrWhiteSpace())
            {
                sonuc = sonuc.Substring(0, sonuc.Length - 1);
            }
            return sonuc;
        }

        /// <summary>
        /// Verilen <see cref="CT_TuzelSahis"/> nesnesindeki bilgileri kullanarak tüzel şahıs adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_TuzelSahis"/> nesnesi.</param>
        /// <returns>Tüzel şahsın adı.</returns>
        private static string TuzelSahisAdiOlustur(CT_TuzelSahis olusturan)
        {
            if (olusturan.Adi != null && !(olusturan.Adi.Value.IsNullOrWhiteSpace()))
                return (olusturan.Adi.Value);

            if (olusturan.Id != null && !(olusturan.Id.Value.IsNullOrWhiteSpace()))
            {
                string id = string.Empty;
                if (olusturan.Id != null && !(olusturan.Id.schemeID.IsNullOrWhiteSpace()))
                    id += olusturan.Id.schemeID;
                if (olusturan.Id != null && !(olusturan.Id.Value.IsNullOrWhiteSpace()))
                {
                    if (!(id.IsNullOrWhiteSpace()))
                        id += ":";
                    id += olusturan.Id.Value;
                }
                return id;
            }
            return string.Empty;
        }

        /// <summary>
        /// Verilen <see cref="CT_KurumKurulus"/> nesnesindeki bilgileri kullanarak kurum/kuruluş adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_KurumKurulus"/> nesnesi.</param>
        /// <returns>Kurum kuruluşun adı.</returns>
        private static string KurumKurulusAdiOlustur(CT_KurumKurulus olusturan)
        {
            if (olusturan.Adi != null && !olusturan.Adi.Value.IsNullOrWhiteSpace())
            {
                if (!string.IsNullOrEmpty(olusturan.KKK))
                {
                    return string.Format("{0}/{1}", olusturan.Adi.Value, olusturan.KKK);
                }
                return olusturan.Adi.Value;
            }
            return olusturan.KKK;
        }

        /// <summary>
        /// Verilen <see cref="CT_Olusturan"/> nesnesindeki bilgileri kullanarak oluşturan adı oluşturur.
        /// </summary>
        /// <param name="olusturan">İsmi oluşturulacak <see cref="CT_Olusturan"/> nesnesi.</param>
        /// <returns>Oluşturan adı.</returns>
        public static string OlusturanAdiOlustur(CT_Olusturan olusturan)
        {
            if (olusturan.Item.GetType() == typeof(CT_KurumKurulus))
                return KurumKurulusAdiOlustur((CT_KurumKurulus)olusturan.Item);
            if (olusturan.Item.GetType() == typeof(CT_TuzelSahis))
                return TuzelSahisAdiOlustur((CT_TuzelSahis)olusturan.Item);
            if (olusturan.Item.GetType() == typeof(CT_GercekSahis))
                return GercekSahisAdiOlustur((CT_GercekSahis)olusturan.Item);
            return string.Empty;
        }

        /// <summary>
        /// Verilen sıralı byte nesnesini deserialize ederek <see cref="CT_PaketOzeti"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="paketOzetiVerisi">Deserialize edilecek sıralı byte.</param>
        /// <returns>Oluşturulan <see cref="CT_PaketOzeti"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Sıralı byte'ın STREAM'e dönüştürülmesinde hata olduğunda oluşur.</exception>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_PaketOzeti PaketOzetiAl(byte[] paketOzetiVerisi)
        {
            MemoryStream mStream;
            try
            {
                mStream = new MemoryStream(paketOzetiVerisi);
            }
            catch (Exception e)
            {
                throw new ApplicationException("MemoryStream oluşturulamadı.", e);
            }
            try
            {
                CT_PaketOzeti readedPaketOzeti = (CT_PaketOzeti)(new XmlSerializer(typeof(CT_PaketOzeti))).Deserialize(mStream);
                return readedPaketOzeti;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
            finally
            {
                mStream.Dispose();
                mStream = null;
            }
        }

        /// <summary>
        /// Verilen sıralı STREAM'i deserialize ederek <see cref="CT_PaketOzeti"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="paketOzetiStream">Deserialize edilecek STREAM.</param>
        /// <returns>Oluşturulan <see cref="CT_PaketOzeti"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_PaketOzeti PaketOzetiAl(Stream paketOzetiStream)
        {
            try
            {
                return (CT_PaketOzeti)new XmlSerializer(typeof(CT_PaketOzeti)).Deserialize(paketOzetiStream);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
        }

        /// <summary>
        /// Verilen sıralı STREAM'i deserialize ederek <see cref="CT_NihaiOzet"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="nihaiOzetStream">Deserialize edilecek STREAM.</param>
        /// <returns>Oluşturulan <see cref="CT_NihaiOzet"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_NihaiOzet NihaiOzetAl(Stream nihaiOzetStream)
        {
            try
            {
                return (CT_NihaiOzet)new XmlSerializer(typeof(CT_NihaiOzet)).Deserialize(nihaiOzetStream);

            }
            catch (Exception e)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", e);
            }
        }

        /// <summary>
        /// Verilen sıralı STREAM'i deserialize ederek <see cref="CT_ParafOzeti"/> nesnesi oluşturur.
        /// </summary>
        /// <param name="parafOzetiStream">Deserialize edilecek STREAM.</param>
        /// <returns>Oluşturulan <see cref="CT_ParafOzeti"/> nesnesi.</returns>
        /// <exception cref="System.ApplicationException">Deserialize hatası.</exception>
        public static CT_ParafOzeti ParafOzetiAl(Stream parafOzetiStream)
        {
            try
            {
                return (CT_ParafOzeti)new XmlSerializer(typeof(CT_ParafOzeti)).Deserialize(parafOzetiStream);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Deserialize işlemi sırasında hata oluştu.", ex);
            }
        }

        /// <summary>
        /// Bir STREAM'deki veriyi diğerine kopyalar.
        /// </summary>
        /// <param name="source">Kaynak STREAM.</param>
        /// <param name="target">Hedef STREAM.</param>
        /// <exception cref="System.ArgumentNullException">Kaynak veya hedef STREAM'in null olması durumunda oluşur.</exception>
        public static void CopyStream(Stream source, Stream target)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("source");

            const int bufSize = 0x1000;
            var buf = new byte[bufSize];
            int bytesRead;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
        }

        /// <summary>
        /// Verilen STREAM'de bulunan paketin şifreli bir paket olup olmadığı bilgisini döner.
        /// </summary>
        /// <param name="stream">Paketin bulunduğu STREAM.</param>
        /// <returns>Paket şifreli ise true, aksi halde false döner.</returns>
        /// <exception cref="System.Exception">Paketin geçerli olmaması durumunda oluşur.</exception>
        public static bool PaketSifreliMi(Stream stream)
        {
            using (Package _package = Package.Open(stream, FileMode.Open, FileAccess.ReadWrite))
            {
                if (_package.PackageProperties.Category == null)
                    throw new Exception("Geçersiz e-yazışma paketi.");
                if (_package.PackageProperties.Category == RESMIYAZISMASIFRELI)
                    return true;
                if (_package.PackageProperties.Category == RESMIYAZISMA)
                    return false;
                throw new Exception("Geçersiz e-yazışma paketi.");
            }
        }

        internal static string EncodePath(string s)
        {
            s = s.Replace('ş', 's')
                .Replace('Ş', 'S')
                .Replace('ö', 'o')
                .Replace('Ö', 'O')
                .Replace('ç', 'c')
                .Replace('Ç', 'C')
                .Replace('i', 'i')
                .Replace('İ', 'I')
                .Replace('ı', 'i')
                .Replace('I', 'I')
                .Replace('ğ', 'g')
                .Replace('Ğ', 'G')
                .Replace('ü', 'u')
                .Replace('Ü', 'U');            
            s = new Regex("[^\\x20-\\x7e]").Replace(s, "");
            s = new Regex("\\s+").Replace(s, "");
            return s;
        }

        internal static string OzetModuToString(OzetModu ozetModu)
        {
            switch (ozetModu)
            {
                case OzetModu.Yok:
                    return "";
                case OzetModu.SHA256:
                    return ALGORITHM_SHA256;
                case OzetModu.SHA512:
                    return ALGORITHM_SHA512;
                case OzetModu.SHA384:
                    return ALGORITHM_SHA384;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Paketi açar ve paket özelliklerinde bulunan versiyon numarasının ilk bölümünü getirir
        /// </summary>
        /// <example>
        /// 1.3 olan paket numarasını 1 olarak getirir.
        /// 2.0 olan paket numarasını 2 olarak getirir.
        /// </example>
        /// <param name="stream"></param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        public static int? PaketVersiyonuGetir(Stream stream)
        {
            var paket = Package.Open(stream, FileMode.Open, FileAccess.Read);
            if (string.IsNullOrWhiteSpace(paket.PackageProperties.Version))
            {
                return null;
            }

            var versiyonBilgisi = paket.PackageProperties.Version.Split('.');
            if (versiyonBilgisi.Length != 2)
            {
                //Versiyon bilgisi alanı 1.3, 2.0 şeklinde olmaktadır.
                return null;
            }

            int versiyon;
            if (int.TryParse(versiyonBilgisi[0], out versiyon))
            {
                return versiyon;
            }

            return null;
        }

        /// <summary>
        /// Paketi açar ve paket özelliklerinde bulunan category alanından paketin tipini getirir.
        /// </summary>
        /// <example>
        /// Category alanı eğer
        ///     RESMIYAZISMA ise PaketTipi.NormalPaket
        ///     RESMIYAZISMASIFRELI ise PaketTipi.SifreliPaket
        ///     RESMIYAZISMAGUNCELLEME ise PaketTipi.GuncellemePaketi
        /// </example>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static PaketTipi? PaketTipiGetir(Stream stream)
        {
            var paket = Package.Open(stream, FileMode.Open, FileAccess.Read);
            if (string.IsNullOrWhiteSpace(paket.PackageProperties.Category))
            {
                return null;
            }

            var category = paket.PackageProperties.Category;
            if (string.IsNullOrWhiteSpace(category))
            {
                return null;
            }

            switch (category)
            {
                case RESMIYAZISMA:
                    return PaketTipi.NormalPaket;
                case RESMIYAZISMASIFRELI:
                    return PaketTipi.SifreliPaket;
                case RESMIYAZISMAGUNCELLEME:
                    return PaketTipi.GuncellemePaketi;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Paketi açar ve paket özelliklerinde bulunan category alanından paketin tipini getirir.
        /// </summary>
        /// <example>
        /// Category alanı eğer
        ///     RESMIYAZISMA ise PaketTipi.NormalPaket
        ///     RESMIYAZISMASIFRELI ise PaketTipi.SifreliPaket
        ///     RESMIYAZISMAGUNCELLEME ise PaketTipi.GuncellemePaketi
        /// </example>
        /// <param name="dosyaYolu"></param>
        /// <returns></returns>
        public static PaketTipi? PaketTipiGetir(string dosyaYolu)
        {
            if (!File.Exists(dosyaYolu))
                return null;
            using (var stream = new MemoryStream(File.ReadAllBytes(dosyaYolu)))
            {
                return PaketTipiGetir(stream);
            }
        }
    }
}
