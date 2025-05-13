namespace Cbddo.eYazisma.Tipler
{
    /// <summary>
    /// Paket açma işleminin modunu belirtir.
    /// </summary>
    public enum PaketModu
    {
        /// <summary>
        /// Oluştur. Yeni bir paket oluşturma operasyonunu belirtir.
        /// </summary>
        Olustur = 1,
        /// <summary>
        /// Aç. Paketin okuma için açıldığını belirtir.
        /// </summary>
        Ac = 2,
        /// <summary>
        /// Güncelle. Paketin güncelleme için açıldığını belirtir.
        /// </summary>
        Guncelle = 3,
    }
}
