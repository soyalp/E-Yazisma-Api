using Cbddo.eYazisma.Xsd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// Güncelleme paketi içerisindeki güncelleme bilgisini tanımlar.
    /// </summary>
    public abstract class GuncellemeBilgisi : IDisposable
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
        internal abstract CT_GuncellemeBilgisi CT_GuncellemeBilgisi { get; set; }

        /// <summary>
        /// Paket içerisine güncelleme ekler
        /// </summary>
        /// <param name="guncellemeTuru"></param>
        /// <param name="guncelleme"></param>
        public abstract void GuncellemeEkle(ST_GuncellemeTuru guncellemeTuru, object guncelleme);

        /// <summary>
        /// Paket içerisine eklenmiş olan tüm güncelleme bilgilerini getirir
        /// </summary>
        /// <returns></returns>
        public abstract List<CT_Guncelleme> GuncellemeleriAl();

        internal abstract void KontrolEt();
    }

    class GuncellemeBilgisiInternal : GuncellemeBilgisi
    {
        internal override CT_GuncellemeBilgisi CT_GuncellemeBilgisi { get; set; }

        public GuncellemeBilgisiInternal()
        {
            CT_GuncellemeBilgisi = new CT_GuncellemeBilgisi();
        }

        internal GuncellemeBilgisiInternal(CT_GuncellemeBilgisi guncellemeBilgisi)
        {
            CT_GuncellemeBilgisi = guncellemeBilgisi;
        }

        internal override void KontrolEt()
        {
            if (CT_GuncellemeBilgisi == null)
            {
                throw new ApplicationException("CT_GuncellemeBilgisi nesnesi boş");
            }
            if (CT_GuncellemeBilgisi.Guncellemeler == null || CT_GuncellemeBilgisi.Guncellemeler.Length == 0)
            {
                throw new Exception("Güncellemeler eklenmemiş");
            }
            foreach (var item in CT_GuncellemeBilgisi.Guncellemeler)
            {
                if (item.GuncellemeTuru == ST_GuncellemeTuru.GuvenlikKodu)
                {
                    if (!(item.Item is CT_GuvenlikKoduDegisiklikBilgisi))
                    {
                        throw new ArgumentException("GuncellemeTuru GuvenlikKodu seçildiğinde Güncelleme verisi \"CT_GuvenlikKoduDegisiklikBilgisi\" olabilir");
                    }
                    var guvenlikKoduDegisiklik = (CT_GuvenlikKoduDegisiklikBilgisi)item.Item;

                    if (guvenlikKoduDegisiklik.DegistirmeTarihi == DateTime.MinValue ||
                        guvenlikKoduDegisiklik.DegistirmeTarihi == DateTime.MaxValue ||
                        guvenlikKoduDegisiklik.DegistirmeTarihi == new DateTime(0001, 01, 01))
                    {
                        throw new ArgumentNullException("DegistirmeTarihi geçerli bir tarih değil");
                    }
                    if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.Aciklama))
                    {
                        throw new ArgumentNullException("Aciklama boş olamaz");
                    }
                    if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.KomisyonKarariBelgeId))
                    {
                        throw new ArgumentNullException("KomisyonKarariBelgeId boş olamaz");
                    }
                    if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.KomisyonKarariBelgeNo))
                    {
                        throw new ArgumentNullException("KomisyonKarariBelgeNo boş olamaz");
                    }
                }
                else
                {
                    throw new Exception("Eklenen değişikliğin DegisiklikTuru belirlenemedi");
                }
            }
        }

        public override List<CT_Guncelleme> GuncellemeleriAl()
        {
            return CT_GuncellemeBilgisi?.Guncellemeler?.ToList();
        }

        public override void GuncellemeEkle(ST_GuncellemeTuru guncellemeTuru, object guncelleme)
        {
            if (CT_GuncellemeBilgisi == null)
            {
                CT_GuncellemeBilgisi = new CT_GuncellemeBilgisi();
            }
            if (CT_GuncellemeBilgisi.Guncellemeler == null)
            {
                CT_GuncellemeBilgisi.Guncellemeler = new CT_Guncelleme[0];
            }

            if (guncellemeTuru == ST_GuncellemeTuru.GuvenlikKodu)
            {
                if (!(guncelleme is CT_GuvenlikKoduDegisiklikBilgisi))
                {
                    throw new ArgumentException("GuncellemeTuru GuvenlikKodu seçildiğinde Güncelleme verisi \"CT_GuvenlikKoduDegisiklikBilgisi\" olabilir");
                }
                var guvenlikKoduDegisiklik = (CT_GuvenlikKoduDegisiklikBilgisi)guncelleme;
                if (guvenlikKoduDegisiklik.DegistirmeTarihi == DateTime.MinValue ||
                    guvenlikKoduDegisiklik.DegistirmeTarihi == DateTime.MaxValue ||
                    guvenlikKoduDegisiklik.DegistirmeTarihi == new DateTime(0001, 01, 01))
                {
                    throw new ArgumentNullException("DegistirmeTarihi geçerli bir tarih değil");
                }
                if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.Aciklama))
                {
                    throw new ArgumentNullException("Aciklama boş olamaz");
                }
                if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.KomisyonKarariBelgeId))
                {
                    throw new ArgumentNullException("KomisyonKarariBelgeId boş olamaz");
                }
                if (string.IsNullOrWhiteSpace(guvenlikKoduDegisiklik.KomisyonKarariBelgeNo))
                {
                    throw new ArgumentNullException("KomisyonKarariBelgeNo boş olamaz");
                }
            }

            var list = CT_GuncellemeBilgisi.Guncellemeler.ToList();
            list.Add(new CT_Guncelleme
            {
                GuncellemeTuru = guncellemeTuru,
                Item = guncelleme
            });
            CT_GuncellemeBilgisi.Guncellemeler = list.ToArray();
        }
    }
}
