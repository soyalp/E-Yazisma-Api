using System;
using System.Collections.Generic;
using System.Linq;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// BelgeHedef bileşeni bilgileri.
    /// </summary>
    public abstract class BelgeHedef : IDisposable
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
        internal abstract CT_BelgeHedef CT_BelgeHedef { get; set; }
        /// <summary>
        /// BelgeHedef bileşenindeki hedefleri döner.
        /// </summary>
        /// <returns>Sıralı <see cref="CT_Hedef"/> nesnesi.</returns>
        public abstract CT_Hedef[] HedefleriAl();
        /// <summary>
        /// BelgeHedef bileşenine hedef ekler.
        /// </summary>
        /// <param name="hedef">Eklenecek <see cref="CT_Hedef"/> nesnesi.</param>
        public abstract void HedefEkle(CT_Hedef hedef);
        internal abstract void KontrolEt();
    }

    internal class BelgeHedefInternal : BelgeHedef
    {

        internal override CT_BelgeHedef CT_BelgeHedef { get; set; }

        public BelgeHedefInternal()
        {
            CT_BelgeHedef = new CT_BelgeHedef();
        }

        internal BelgeHedefInternal(CT_BelgeHedef belgeHedef)
        {
            CT_BelgeHedef = belgeHedef;
        }

        public override CT_Hedef[] HedefleriAl()
        {
            return this.CT_BelgeHedef.HedefListesi;
        }
        public override void HedefEkle(CT_Hedef hedef)
        {
            if (CT_BelgeHedef.HedefListesi==null)
                CT_BelgeHedef.HedefListesi = new CT_Hedef[0];
            List<CT_Hedef> L = CT_BelgeHedef.HedefListesi.ToList();
            L.Add(hedef);
            CT_BelgeHedef.HedefListesi = L.ToArray();
        }

        internal override void KontrolEt()
        {
            this.CT_BelgeHedef.KontrolEt();
        }
    }
}
