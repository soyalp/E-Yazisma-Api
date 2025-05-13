using System;
using System.Collections.Generic;
using System.Linq;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// PaketOzeti bileşeni bilgileri.
    /// </summary>
    public abstract class PaketOzeti : IDisposable
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
        internal abstract CT_PaketOzeti CT_PaketOzeti { get; set; }
        /// <summary>
        /// PaketOzeti bileşenine özet ekler.
        /// </summary>
        /// <param name="ozetModu">Eklenen özetin hangi algoritma ile alındığı bilgisi.</param>
        /// <param name="ozetDegeri">Eklenecek özet değeri.</param>
        /// <param name="uri">Eklenen özet değerine ilişkin bileşenin paket içindeki URI'si.</param>
        /// <param name="hariciBilesenMi">Eklenen özet değerine ilişkin bileşenin paket dışında bir bileşen olması durumunda kullanılır.</param>
        public abstract void Ekle(OzetModu ozetModu, Byte[] ozetDegeri, Byte[] ozetDegeriSha512, Uri uri, bool hariciBilesenMi = false);
        /// <summary>
        /// PaketOzeti bileşenindeki özetleri döner.
        /// </summary>
        /// <returns>Sıralı <see cref="CT_Reference"/> nesnesi.</returns>
        public abstract CT_Reference[] OzetleriAl();
        internal abstract void KontrolEt();
    }

    class PaketOzetiInternal : PaketOzeti
    {
        internal override CT_PaketOzeti CT_PaketOzeti { get; set; }

        public PaketOzetiInternal()
        {
            CT_PaketOzeti = new CT_PaketOzeti();
        }
        internal PaketOzetiInternal(CT_PaketOzeti paketOzeti)
        {
            CT_PaketOzeti = paketOzeti;
        }

        public override void Ekle(OzetModu ozetModu, Byte[] ozetDegeri, Byte[] ozetDegeriSha512, Uri uri, bool hariciBilesenMi = false)
        {
            if (ozetModu == OzetModu.Yok)
                throw new ArgumentException("\"ozetModu\" değeri \"YOK\" olamaz.");
            if (CT_PaketOzeti.Reference == null)
                CT_PaketOzeti.Reference = new CT_Reference[0];
            List<CT_Reference> referanslar = CT_PaketOzeti.Reference.ToList();
            var dahaOncekiOzetler = referanslar.Where(x => string.Compare(x.URI, uri.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
            
            if (dahaOncekiOzetler.Any())
                referanslar.Remove(dahaOncekiOzetler.First());

            var yeniReferans = new CT_Reference
            {
                DigestItem = new CT_DigestItem
                {
                    DigestMethod = new CT_DigestMethod() { Algorithm = Araclar.OzetModuToString(ozetModu) },
                    DigestValue = ozetDegeri,
                },
                DigestItem1 = new CT_DigestItem
                {
                    DigestMethod = new CT_DigestMethod() { Algorithm = Araclar.OzetModuToString(OzetModu.SHA512) },
                    DigestValue = ozetDegeriSha512,
                },
                URI = uri.ToString()
            };
            if (hariciBilesenMi)
                yeniReferans.Type = Araclar.HARICI_PAKET_BILESENI_REFERANS_TIPI;
            else
                yeniReferans.Type = Araclar.DAHILI_PAKET_BILESENI_REFERANS_TIPI;
            referanslar.Add(yeniReferans);
            CT_PaketOzeti.Reference = referanslar.ToArray();
        }
        public override CT_Reference[] OzetleriAl()
        {
            return this.CT_PaketOzeti.Reference;
        }
        internal override void KontrolEt()
        {
            this.CT_PaketOzeti.KontrolEt();
        }
    }
}
