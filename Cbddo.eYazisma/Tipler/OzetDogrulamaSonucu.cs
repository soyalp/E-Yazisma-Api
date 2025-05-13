using System;

namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// PaketOzeti bileşeninin doğrulanması sırasında ortaya çıkan hatalara ilişkin bilgiler.
    /// </summary>
    public class OzetDogrulamaHatasi
    {
        /// <summary>
        /// Hata oluşan bileşenin paket içindeki URI'si.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Oluşan hatanın açıklaması.
        /// </summary>
        public string Hata { get; set; }
        /// <summary>
        /// Hata kodu.
        /// </summary>
        public OzetDogrulamaHataKodu HataKodu { get; set; }
        /// <summary>
        /// Inner exception.
        /// </summary>
        public Exception InnerException { get; set; }
    }
}
