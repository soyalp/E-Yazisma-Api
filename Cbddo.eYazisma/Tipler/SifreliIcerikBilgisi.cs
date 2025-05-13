using System;
using Cbddo.eYazisma.Xsd;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// Şifreli paket içerisindeki şifreli veriyi tanımlar.
    /// </summary>
    public abstract class SifreliIcerikBilgisi : IDisposable
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
        internal abstract CT_SifreliIcerikBilgisi CT_SifreliIcerikBilgisi { get; set; }
        internal abstract void KontrolEt();
    }

    class SifreliIcerikBilgisiInternal : SifreliIcerikBilgisi
    {
        internal override CT_SifreliIcerikBilgisi CT_SifreliIcerikBilgisi { get; set; }
        
        public SifreliIcerikBilgisiInternal()
        {
            CT_SifreliIcerikBilgisi = new CT_SifreliIcerikBilgisi();
        }
        internal SifreliIcerikBilgisiInternal(CT_SifreliIcerikBilgisi sifreliIcerikBilgisi)
        {
            CT_SifreliIcerikBilgisi = sifreliIcerikBilgisi;
        }
        internal override void KontrolEt()
        {
            //this.CT_SifreliIcerikBilgisi.KontrolEt();
        }
    }
}
