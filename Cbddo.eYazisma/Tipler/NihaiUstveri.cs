using System;
using System.Collections.Generic;
using System.Linq;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{

    /// <summary>
    /// NihaiUstveri bileşeni bilgileri.
    /// </summary>
    public abstract class NihaiUstveri : IDisposable
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
        internal abstract CT_NihaiUstveri CT_NihaiUstveri { get; set; }
        /// <summary>
        /// Belgenin tarihini belirler.
        /// </summary>
        /// <param name="tarih">Belge tarihi.</param>
        /// <example><code>var islemTarihi = DateTime.Now;
        /// paket.NihaiUstveri.TarihBelirle(islemTarihi);</code></example>
        public abstract void TarihBelirle(DateTime tarih);
        /// <summary>
        /// Belge tarihini döner.
        /// </summary>
        /// <returns>Belge tarihi.</returns>
        public abstract DateTime TarihAl();
        /// <summary>
        /// Belge numarasını belirler.
        /// </summary>
        /// <param name="belgeNo">Belge numarası.</param>
        /// <example><code>paket.NihaiUstveri.BelgeNoBelirle("B.22.0.000.0.00.00.00-658/128");</code></example>
        public abstract void BelgeNoBelirle(String belgeNo);
        /// <summary>
        /// Belge numarasını döner.
        /// </summary>
        /// <returns>Belge numarası.</returns>
        public abstract String BelgeNoAl();

        /// <summary>
        /// BelgeImza bileşenindeki imzaları döner.
        /// </summary>
        /// <returns>Sıralı <see cref="CT_Imza"/> nesnesi.</returns>
        public abstract CT_Imza[] ImzalariAl();

        /// <summary>
        /// BelgeImza bileşenine hedef ekler.
        /// </summary>
        /// <param name="imza">Eklenecek <see cref="CT_Imza"/> nesnesi.</param>
        public abstract void ImzaEkle(CT_Imza imza);

        /// <summary>
        /// Üstverinin kurallara uygun olup olmadığını kontrol eder.
        /// </summary>
        internal abstract void KontrolEt();
    }

    internal class NihaiUstveriInternal : NihaiUstveri
    {
        internal override CT_NihaiUstveri CT_NihaiUstveri { get; set; }


        public NihaiUstveriInternal()
        {
            CT_NihaiUstveri = new CT_NihaiUstveri();
        }
        internal NihaiUstveriInternal(CT_NihaiUstveri nihaiUstveri)
        {
            CT_NihaiUstveri = nihaiUstveri;
        }

        public override void TarihBelirle(DateTime tarih)
        {
            CT_NihaiUstveri.Tarih = tarih;
        }

        public override void BelgeNoBelirle(String belgeNo)
        {
            if (belgeNo.IsNullOrWhiteSpace())
                throw new ArgumentNullException("belgeNo");
            CT_NihaiUstveri.BelgeNo = belgeNo;
        }


        internal override void KontrolEt()
        {
            if (CT_NihaiUstveri.BelgeNo.IsNullOrWhiteSpace())
                throw new Exception("Üstveri bileşeni, \"BelgeNo\" alanı için değer verilmemiş.");
            if (CT_NihaiUstveri.Tarih <= DateTime.MinValue)
                throw new Exception("Üstveri bileşeni, \"Tarih\" alanı için değer verilmemiş.");
            if (CT_NihaiUstveri.BelgeImzalar != null && CT_NihaiUstveri.BelgeImzalar.Length > 0)
                foreach (var imza in CT_NihaiUstveri.BelgeImzalar)
                    imza.KontrolEt();
        }

        public override DateTime TarihAl()
        {
            return this.CT_NihaiUstveri.Tarih;
        }

        public override string BelgeNoAl()
        {
            return this.CT_NihaiUstveri.BelgeNo;
        }

        public override CT_Imza[] ImzalariAl()
        {
            return this.CT_NihaiUstveri.BelgeImzalar;
        }

        public override void ImzaEkle(CT_Imza imza)
        {
            if (CT_NihaiUstveri.BelgeImzalar == null)
                CT_NihaiUstveri.BelgeImzalar = new CT_Imza[0];
            List<CT_Imza> L = CT_NihaiUstveri.BelgeImzalar.ToList();
            L.Add(imza);
            CT_NihaiUstveri.BelgeImzalar = L.ToArray();
        }

    }
}
