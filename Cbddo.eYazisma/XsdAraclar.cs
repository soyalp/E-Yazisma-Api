using System;
using System.Text.RegularExpressions;
using System.Xml;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// Xml nesneleri için gerekli yardımcı fonksiyonları içerir.
    /// </summary>
    public static class XsdAraclar
    {
        /// <summary>
        /// Kisi nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="kisi">Kisi</param>
        public static void KontrolEt(this CT_Kisi kisi)
        {
            if (kisi == null)
            {
                throw new ArgumentNullException("kisi");
            }
            if (kisi.IlkAdi == null)
            {
                throw new Exception("Kişi nesnesi, ilk adı alanı boş olamaz.");
            }
            if (kisi.IlkAdi.Value.IsNullOrWhiteSpace())
            {
                throw new Exception("Kişi nesnesi, ilk adı alan değeri boş olamaz.");
            }
            if (kisi.Soyadi == null)
            {
                throw new Exception("Kişi nesnesi, soyadı alanı boş olamaz.");
            }
            if (kisi.Soyadi.Value.IsNullOrWhiteSpace())
            {
                throw new Exception("Kişi nesnesi, soyadı alan değeri boş olamaz.");
            }
        }
        /// <summary>
        /// CT_KurumKurulus nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="kurumKurulus">CT_KurumKurulus</param>
        public static void KontrolEt(this CT_KurumKurulus kurumKurulus)
        {
            if (kurumKurulus == null)
            {
                throw new ArgumentNullException("kurumKurulus");
            }
            if (kurumKurulus.KKK.IsNullOrWhiteSpace())
            {
                throw new Exception("Kurum Kimlik Kodu boş olamaz.");
            }
            if (!(new Regex("^[1-9]{1}[0-9]{7}$")).Match(kurumKurulus.KKK).Success)
            {
                throw new Exception("Kurum Kimlik Kodu formatı uygun değil.");
            }
        }
        /// <summary>
        /// CT_TuzelSahis nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="tuzelSahis">CT_TuzelSahis</param>
        public static void KontrolEt(this CT_TuzelSahis tuzelSahis)
        {
            if (tuzelSahis == null)
            {
                throw new ArgumentNullException("tuzelSahis");
            }
            if (tuzelSahis.Id == null)
            {
                throw new Exception("TuzelSahis nesnesi, ID alanı boş olamaz.");
            }
            if (tuzelSahis.Id.Value.IsNullOrWhiteSpace())
            {
                throw new Exception("TuzelSahis nesnesi, ID alanı değeri boş olamaz.");
            }
            if (tuzelSahis.Id.schemeID.IsNullOrWhiteSpace())
            {
                throw new Exception("TuzelSahis nesnesi, ID alanı SchemeID değeri boş olamaz.");
            }
        }
        /// <summary>
        /// CT_GercekSahis nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="gercekSahis">CT_GercekSahis</param>
        public static void KontrolEt(this CT_GercekSahis gercekSahis)
        {
            if (gercekSahis == null)
            {
                throw new ArgumentNullException("gercekSahis");
            }
            if (gercekSahis.Kisi == null)
            {
                throw new Exception("GercekSahis nesnesi, Kişi alanı boş olamaz.");
            }
            gercekSahis.Kisi.KontrolEt();
        }
        /// <summary>
        /// CT_Ilgili nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="ilgili">CT_Ilgili</param>
        public static void KontrolEt(this CT_Ilgili ilgili)
        {
            if (ilgili == null)
            {
                throw new ArgumentNullException("ilgili");
            }
            if (ilgili.Item == null)
            {
                throw new Exception("İlgili değeri için bir tane KurumKurulus, TuzelSahis veya GercekSahis değeri verilmelidir.");
            }
            if (ilgili.Item.GetType() == typeof(CT_KurumKurulus))
            {
                ((CT_KurumKurulus)ilgili.Item).KontrolEt();
            }
            else if (ilgili.Item.GetType() == typeof(CT_TuzelSahis))
            {
                ((CT_TuzelSahis)ilgili.Item).KontrolEt();
            }
            else if (ilgili.Item.GetType() == typeof(CT_GercekSahis))
            {
                ((CT_GercekSahis)ilgili.Item).KontrolEt();
            }
        }
        /// <summary>
        /// CT_IlgiliListesi nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="ilgiliListesi">CT_IlgiliListesi</param>
        public static void KontrolEt(this CT_IlgiliListesi ilgiliListesi)
        {
            if (ilgiliListesi == null)
            {
                throw new ArgumentNullException("ilgiliListesi");
            }
            if (ilgiliListesi.Ilgili == null || ilgiliListesi.Ilgili.Length == 0)
            {
                throw new Exception("İlgiliListesi değeri için en az bir tane Ilgili değeri verilmelidir.");
            }
        }
        /// <summary>
        /// CT_KonulmamisEk nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="konulmamisEk">CT_KonulmamisEk</param>
        public static void KontrolEt(this CT_KonulmamisEk konulmamisEk)
        {
            if (konulmamisEk == null)
            {
                throw new ArgumentNullException("konulmamisEk");
            }
            if (konulmamisEk.EkId.IsNullOrWhiteSpace())
            {
                throw new Exception("KonulmamisEk nesnesi, EkId boş olamaz.");
            }
        }
        /// <summary>
        /// CT_KonulmamisEkListesi nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="konulmamisEkListesi">CT_KonulmamisEkListesi</param>
        public static void KontrolEt(this CT_KonulmamisEkListesi konulmamisEkListesi)
        {
            if (konulmamisEkListesi == null)
            {
                throw new ArgumentNullException("konulmamisEkListesi");
            }
            if (konulmamisEkListesi.KonulmamisEk == null || konulmamisEkListesi.KonulmamisEk.Length == 0)
            {
                throw new Exception("KonulmamisEkListesi değeri için en az bir tane KonulmamisEk değeri verilmelidir.");
            }
        }
        /// <summary>
        /// CT_Dagitim nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="dagitim">CT_Dagitim</param>
        public static void KontrolEt(this CT_Dagitim dagitim)
        {
            if (dagitim == null)
            {
                throw new ArgumentNullException("dagitim");
            }
            if (dagitim.Item == null)
            {
                throw new Exception("Dagitim değeri için bir tane KurumKurulus, TuzelSahis veya GercekSahis değeri verilmelidir.");
            }
            if (dagitim.Item.GetType() == typeof(CT_KurumKurulus))
                ((CT_KurumKurulus)dagitim.Item).KontrolEt();
            else if (dagitim.Item.GetType() == typeof(CT_TuzelSahis))
            {
                ((CT_TuzelSahis)dagitim.Item).KontrolEt();
            }
            else if (dagitim.Item.GetType() == typeof(CT_GercekSahis))
            {
                ((CT_GercekSahis)dagitim.Item).KontrolEt();
            }
            if (!(dagitim.Miat.IsNullOrWhiteSpace()))
            {
                try
                {
                    TimeSpan miatDegeri = XmlConvert.ToTimeSpan(dagitim.Miat);
                }
                catch (Exception)
                {
                    throw new Exception("Dagitim nesnesi, Miat alanı xsd:duration tipinde olmalıdır.");
                }
            }
            if (dagitim.KonulmamisEkListesi != null && dagitim.KonulmamisEkListesi.Length == 0)
            {
                throw new Exception("Dagitim nesnesi, Konulmamış ek listesi alanında en az bir tane ek olmalıdır.");
            }
            if (dagitim.KonulmamisEkListesi != null && dagitim.KonulmamisEkListesi.Length > 0)
            {
                foreach (CT_KonulmamisEk konulmamisEk in dagitim.KonulmamisEkListesi)
                {
                    konulmamisEk.KontrolEt();
                }
            }
        }
        /// <summary>
        /// CT_Olusturan nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="olusturan">CT_Olusturan</param>
        public static void KontrolEt(this CT_Olusturan olusturan)
        {
            if (olusturan == null)
            {
                throw new ArgumentNullException("olusturan");
            }
            if (olusturan.Item == null)
                throw new Exception("Olusturan değeri için bir tane KurumKurulus, TuzelSahis veya GercekSahis değeri verilmelidir.");
            if (olusturan.Item.GetType() == typeof(CT_KurumKurulus))
            {
                ((CT_KurumKurulus)olusturan.Item).KontrolEt();
            }
            else if (olusturan.Item.GetType() == typeof(CT_TuzelSahis))
            {
                ((CT_TuzelSahis)olusturan.Item).KontrolEt();
            }
            else if (olusturan.Item.GetType() == typeof(CT_GercekSahis))
            {
                ((CT_GercekSahis)olusturan.Item).KontrolEt();
            }
        }
        /// <summary>
        /// CT_Imza nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="imza">CT_Imza</param>
        public static void KontrolEt(this CT_Imza imza)
        {
            if (imza == null)
            {
                throw new ArgumentNullException("imza");
            }
            if (imza.Imzalayan == null)
            {
                throw new Exception("İmza alanı için imzalayan alanı boş olamaz.");
            }
            imza.Imzalayan.KontrolEt();
            if (imza.YetkiDevreden != null)
            {
                imza.YetkiDevreden.KontrolEt();
            }
            if (imza.VekaletVeren != null)
            {
                imza.VekaletVeren.KontrolEt();
            }
        }
        /// <summary>
        /// CT_Hedef nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="hedef">CT_Hedef</param>
        public static void KontrolEt(this CT_Hedef hedef)
        {
            if (hedef == null)
            {
                throw new ArgumentNullException("hedef");
            }
            if (hedef.Item == null)
            {
                throw new Exception("Hedef değeri için bir tane KurumKurulus, TuzelSahis veya GercekSahis değeri verilmelidir.");
            }
            if (hedef.Item.GetType() == typeof(CT_KurumKurulus))
            {
                ((CT_KurumKurulus)hedef.Item).KontrolEt();
            }
            else if (hedef.Item.GetType() == typeof(CT_TuzelSahis))
            {
                ((CT_TuzelSahis)hedef.Item).KontrolEt();
            }
            else if (hedef.Item.GetType() == typeof(CT_GercekSahis))
            {
                ((CT_GercekSahis)hedef.Item).KontrolEt();
            }
        }
        /// <summary>
        /// CT_Ek nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="ek">CT_Ek</param>
        public static void KontrolEt(this CT_Ek ek)
        {
            if (ek == null)
            {
                throw new ArgumentNullException("ek");
            }
            if (ek.Id == null || ek.Id.Value.IsNullOrWhiteSpace())
            {
                throw new Exception("Ek nesnesi, Id değeri boş olamaz.");
            }
            if (ek.Tur == ST_KodEkTuru.DED)
            {
                if (ek.DosyaAdi.IsNullOrWhiteSpace())
                {
                    throw new Exception("Eke ait DosyaAdi değeri boş olamaz.");
                }
            }
            if (ek.OzId != null && (ek.OzId.schemeID == null || ek.OzId.schemeID.Length == 0))
            {
                throw new Exception("OzId değeri verilen ek için, SchemeId değeri boş olamaz.");
            }
            if ((ek.Tur == ST_KodEkTuru.HRF || ek.Tur == ST_KodEkTuru.FZK) && ek.ImzaliMi == true)
            {
                throw new Exception("Harici Referans ve Fiziksel nesne türündeki ekler, imzalı olarak pakete eklenemezler.");
            }
            if (ek.Tur == ST_KodEkTuru.HRF && ek.Ozet != null)
            {
                ek.Ozet.KontrolEt();
            }
        }
        /// <summary>
        /// CT_Ozet nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="ozet">CT_Ozet</param>
        public static void KontrolEt(this CT_Ozet ozet)
        {
            if (ozet == null)
            {
                throw new ArgumentNullException("ozet");
            }
            if (ozet.OzetAlgoritmasi == null)
            {
                throw new ArgumentNullException("Ozet değeri için OzetAlgoritması değeri verilmelidir.");
            }
            if (ozet.OzetAlgoritmasi.Algorithm == null || ozet.OzetAlgoritmasi.Algorithm.Length == 0)
            {
                throw new ArgumentNullException("OzetAlgoritması için Algorithm değeri verilmelidir.");
            }
            if (ozet.OzetDegeri == null || ozet.OzetDegeri.Length == 0)
            {
                throw new ArgumentNullException("Ozet değeri için OzetDegeri değeri verilmelidir.");
            }
        }
        /// <summary>
        /// CT_Ilgi nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="ilgi">CT_Ilgi</param>
        public static void KontrolEt(this CT_Ilgi ilgi)
        {
            if (ilgi == null)
            {
                throw new ArgumentNullException("ilgi");
            }
            if (ilgi.Id == null || ilgi.Id.Value.IsNullOrWhiteSpace())
            {
                throw new Exception("Ilgi nesnesi, Id değeri boş olamaz.");
            }
            if (ilgi.Etiket.IsNullOrWhiteSpace())
            {
                throw new Exception("Ilgi nesnesi, Etiket değeri boş olamaz.");
            }
            if (ilgi.OzId != null && (ilgi.OzId.schemeID == null || ilgi.OzId.schemeID.Length == 0))
                throw new Exception("Ilgi nesnesi, OzId SchemeId değeri boş olamaz.");
        }

        /// <summary>
        /// CT_HEYSK nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="heysk">CT_Ilgi</param>
        public static void KontrolEt(this CT_HEYSK heysk)
        {
            if (heysk == null)
            {
                throw new ArgumentNullException("heysk");
            }
            if (heysk.Kod == 0)
            {
                throw new Exception("HEYSK nesnesi, Kod değeri 0 olamaz.");
            }
            if (string.IsNullOrWhiteSpace(heysk.Ad))
            {
                throw new Exception("HEYSK nesnesi, Ad değeri boş olamaz.");
            }
        }

        /// <summary>
        /// CT_BelgeHedef nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="belgeHedef">CT_BelgeHedef</param>
        public static void KontrolEt(this CT_BelgeHedef belgeHedef)
        {
            if (belgeHedef == null)
                throw new ArgumentNullException("belgeHedef");
            if (belgeHedef.HedefListesi == null || belgeHedef.HedefListesi.Length == 0)
            {
                throw new Exception("BelgeHedef değeri için en az bir tane Hedef değeri verilmelidir.");
            }
            foreach (var hedef in belgeHedef.HedefListesi)
            {
                hedef.KontrolEt();
            }
        }
        /// <summary>
        /// CT_Reference nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="reference">CT_Reference</param>
        public static void KontrolEt(this CT_Reference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (reference.DigestItem == null)
            {
                throw new Exception("Reference nesnesi, DigestItem değeri boş olamaz.");
            }
            if (reference.DigestItem1 == null)
            {
                throw new Exception("Reference nesnesi, DigestItem1 değeri boş olamaz.");
            }
            reference.DigestItem.KontrolEt();
            reference.DigestItem1.KontrolEt();
        }
        /// <summary>
        /// CT_DigestItem nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="digestItem">CT_Reference</param>
        public static void KontrolEt(this CT_DigestItem digestItem)
        {
            if (digestItem == null)
            {
                throw new ArgumentNullException("DigestItem");
            }
            if (digestItem.DigestMethod == null)
            {
                throw new Exception("DigestItem nesnesi, DigestMethod değeri boş olamaz.");
            }
            digestItem.DigestMethod.KontrolEt();
            if (digestItem.DigestValue == null || digestItem.DigestValue.Length == 0)
            {
                throw new Exception("DigestItem nesnesi, DigestValue değeri boş olamaz.");
            }
        }
        /// <summary>
        /// CT_DigestMethod nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="digetMethod">CT_DigestMethod</param>
        public static void KontrolEt(this CT_DigestMethod digetMethod)
        {
            if (digetMethod == null)
            {
                throw new ArgumentNullException("digetMethod");
            }
            if (digetMethod.Algorithm.IsNullOrWhiteSpace())
            {
                throw new Exception("DigestMethod nesnesi, Algorithm adı alan değeri boş olamaz.");
            }
        }
        /// <summary>
        /// CT_PaketOzeti nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="paketOzeti">CT_PaketOzeti</param>
        public static void KontrolEt(this CT_PaketOzeti paketOzeti)
        {
            if (paketOzeti == null)
            {
                throw new ArgumentNullException("paketOzeti");
            }
            if (paketOzeti.Reference == null || paketOzeti.Reference.Length == 0)
            {
                throw new Exception("PaketOzeti değeri için en az bir tane Reference değeri verilmelidir.");
            }
            if (paketOzeti.Id.IsNullOrWhiteSpace())
            {
                throw new Exception("PaketOzeti ID değeri boş olamaz.");
            }
            foreach (var reference in paketOzeti.Reference)
            {
                reference.KontrolEt();
            }
        }
        /// <summary>
        /// CT_ParafOzeti nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="parafOzeti">CT_ParafOzeti</param>
        public static void KontrolEt(this CT_ParafOzeti parafOzeti)
        {
            if (parafOzeti == null)
            {
                throw new ArgumentNullException("parafOzeti");
            }
            if (parafOzeti.Reference == null || parafOzeti.Reference.Length == 0)
            {
                throw new Exception("ParafOzeti değeri için en az bir tane Reference değeri verilmelidir.");
            }
            if (parafOzeti.Id.IsNullOrWhiteSpace())
            {
                throw new Exception("ParafOzeti ID değeri boş olamaz.");
            }
            foreach (var reference in parafOzeti.Reference)
            {
                reference.KontrolEt();
            }
        }
        /// <summary>
        /// KCT_NihaiOzetisi nesnesininin kurallara uygun olmadığını kontrol eder.
        /// </summary>
        /// <param name="nihaiOzet">CT_NihaiOzet</param>
        public static void KontrolEt(this CT_NihaiOzet nihaiOzet)
        {
            if (nihaiOzet == null)
            {
                throw new ArgumentNullException("nihaiOzet");
            }
            if (nihaiOzet.Reference == null || nihaiOzet.Reference.Length == 0)
            {
                throw new Exception("NihaiOzet değeri için en az bir tane Reference değeri verilmelidir.");
            }
            if (nihaiOzet.Id.IsNullOrWhiteSpace())
            {
                throw new Exception("NihaiOzet ID değeri boş olamaz.");
            }
            foreach (var reference in nihaiOzet.Reference)
            {
                reference.KontrolEt();
            }
        }
    }
}
