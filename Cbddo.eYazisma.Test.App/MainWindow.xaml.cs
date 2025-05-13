using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using Cbddo.eYazisma.Test.App.Tipler;
using Cbddo.eYazisma.Tipler;
using Cbddo.eYazisma.Xsd;
using Microsoft.Win32;

namespace Cbddo.eYazisma.Test.App
{
    public partial class MainWindow : Window
    {
        private Model _model = null;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _model = new Model();
            DataContext = _model;
        }
    }

    public class Model : INotifyPropertyChanged
    {
        private string ustYaziDosyasi = @"..\..\dosyalar\ustyazi.pdf";
        private string ekDosyasi1 = @"..\..\dosyalar\dosya1.pdf";
        private string ekDosyasi2 = @"..\..\dosyalar\dosya_imzasiz.docm";
        private string ekDosyasi3 = @"..\..\dosyalar\dosya3.docx";

        private List<PaketDurumu> _paketDurumlari { get; set; }
        private Paket _paket;

        ObservableCollection<Cikti> konsolCiktilari;
        public ObservableCollection<Cikti> KonsolCiktilari
        {
            get
            {
                return konsolCiktilari;
            }
            set
            {
                if (Equals(value, konsolCiktilari)) return;
                konsolCiktilari = value;
                NotifyPropertyChanged(() => KonsolCiktilari);
            }
        }

        string paketDosyasiYolu;
        public string PaketDosyasiYolu
        {
            get
            {
                return paketDosyasiYolu;
            }
            set
            {
                if (Equals(value, paketDosyasiYolu)) return;
                paketDosyasiYolu = value;
                NotifyPropertyChanged(() => PaketDosyasiYolu);
            }
        }

        string sifreliPaketDosyasiYolu;
        public string SifreliPaketDosyasiYolu
        {
            get
            {
                return sifreliPaketDosyasiYolu;
            }
            set
            {
                if (Equals(value, sifreliPaketDosyasiYolu)) return;
                sifreliPaketDosyasiYolu = value;
                NotifyPropertyChanged(() => SifreliPaketDosyasiYolu);
            }
        }

        string guncellemePaketiDosyasiYolu;
        public string GuncellemePaketiDosyasiYolu
        {
            get
            {
                return guncellemePaketiDosyasiYolu;
            }
            set
            {
                if (Equals(value, guncellemePaketiDosyasiYolu)) return;
                guncellemePaketiDosyasiYolu = value;
                NotifyPropertyChanged(() => GuncellemePaketiDosyasiYolu);
            }
        }

        PaketDurumu paketDurumu;
        public PaketDurumu PaketDurumu
        {
            get
            {
                return paketDurumu;
            }
            set
            {
                if (Equals(value, paketDurumu)) return;
                paketDurumu = value;
                NotifyPropertyChanged(() => PaketDurumu);
            }
        }

        #region Commands

        ICommand _paketOlusturCommand;
        public ICommand PaketOlusturCommand
        {
            get
            {
                if (_paketOlusturCommand == null)
                    _paketOlusturCommand = new RelayCommand(param => PaketOlustur(), param => IsEnabledPaketOlustur());
                return _paketOlusturCommand;
            }
        }

        ICommand _imzaEkleCommand;
        public ICommand ImzaEkleCommand
        {
            get
            {
                if (_imzaEkleCommand == null)
                    _imzaEkleCommand = new RelayCommand(param => ImzaEkle(), param => IsEnabledImzaEkle());
                return _imzaEkleCommand;
            }
        }

        ICommand _parafEkleCommand;
        public ICommand ParafEkleCommand
        {
            get
            {
                if (_parafEkleCommand == null)
                    _parafEkleCommand = new RelayCommand(param => ParafEkle(), param => IsEnabledParafEkle());
                return _parafEkleCommand;
            }
        }

        ICommand _muhurEkleCommand;
        public ICommand MuhurEkleCommand
        {
            get
            {
                if (_muhurEkleCommand == null)
                    _muhurEkleCommand = new RelayCommand(param => MuhurEkle(), param => IsEnabledMuhurEkle());
                return _muhurEkleCommand;
            }
        }

        ICommand _kapatCommand;
        public ICommand KapatCommand
        {
            get
            {
                if (_kapatCommand == null)
                    _kapatCommand = new RelayCommand(param => Kapat(), param => IsEnabledKapat());
                return _kapatCommand;
            }
        }

        ICommand _acGuncellemeCommand;
        public ICommand AcGuncellemeCommand
        {
            get
            {
                if (_acGuncellemeCommand == null)
                    _acGuncellemeCommand = new RelayCommand(param => AcGuncellemeModu(), param => IsEnabledAcGuncellemeModu());
                return _acGuncellemeCommand;
            }
        }

        ICommand _acOkumaCommand;
        public ICommand AcOkumaCommand
        {
            get
            {
                if (_acOkumaCommand == null)
                    _acOkumaCommand = new RelayCommand(param => AcOkumaModu(), param => IsEnabledAcOkumaModu());
                return _acOkumaCommand;
            }
        }

        ICommand _paraflariCikarCommand;
        public ICommand ParaflariCikarCommand
        {
            get
            {
                if (_paraflariCikarCommand == null)
                    _paraflariCikarCommand = new RelayCommand(param => ParaflariCikar(), param => IsEnabledParaflariCikar());
                return _paraflariCikarCommand;
            }
        }

        ICommand _paketiGoruntuleCommand;
        public ICommand PaketiGoruntuleCommand
        {
            get
            {
                if (_paketiGoruntuleCommand == null)
                    _paketiGoruntuleCommand = new RelayCommand(param => PaketiGoruntule(), param => IsEnabledAcGuncellemeModu());
                return _paketiGoruntuleCommand;
            }
        }

        ICommand _paketIceriginiCikartCommand;
        public ICommand PaketIceriginiCikartCommand
        {
            get
            {
                if (_paketIceriginiCikartCommand == null)
                    _paketIceriginiCikartCommand = new RelayCommand(param => PaketIceriginiCikart(), param => IsEnabledIcerikCikartAc());
                return _paketIceriginiCikartCommand;
            }
        }

        ICommand _paketiSifreleCommand;
        public ICommand PaketiSifreleCommand
        {
            get
            {
                if (_paketiSifreleCommand == null)
                    _paketiSifreleCommand = new RelayCommand(param => PaketiSifrele(), param => IsEnabledSifrele());
                return _paketiSifreleCommand;
            }
        }

        ICommand _ustveriGuncellemePaketiOlusturCommand;
        public ICommand UstveriGuncellemePaketiOlusturCommand
        {
            get
            {
                if (_ustveriGuncellemePaketiOlusturCommand == null)
                    _ustveriGuncellemePaketiOlusturCommand = new RelayCommand(param => UstveriGuncellemePaketiOlustur(), param => IsEnabledUstVeriGuncelleme());
                return _ustveriGuncellemePaketiOlusturCommand;
            }
        }

        private ICommand _paketAcCommand;

        public ICommand PaketAcCommand
        {
            get
            {
                if (_paketAcCommand == null)
                    _paketAcCommand = new RelayCommand(param => PaketAc(), param => IsEnabledPaketAc());
                return _paketAcCommand;
            }
        }


        #endregion

        #region Private Properties

        private bool IsEnabledPaketOlustur()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.PaketOlustur);
        }

        private bool IsEnabledImzaEkle()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.ImzaEkle);
        }

        private bool IsEnabledParafEkle()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.ParafEkle);
        }

        private bool IsEnabledMuhurEkle()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.MuhurEkle);
        }

        private bool IsEnabledKapat()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.Kapat);
        }

        private bool IsEnabledAcGuncellemeModu()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.AcGuncellemeModunda);
        }

        private bool IsEnabledAcOkumaModu()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.AcOkumaModunda);
        }

        private bool IsEnabledParaflariCikar()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.ParafImzaCikar);
        }

        private bool IsEnabledSifrele()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.Sifrele);
        }

        private bool IsEnabledUstVeriGuncelleme()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.UstveriGuncelleme);
        }

        private bool IsEnabledPaketAc()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.Ac);
        }

        private bool IsEnabledIcerikCikartAc()
        {
            return PaketDurumu != null && PaketDurumu.OlasiAksiyonlar != null && PaketDurumu.OlasiAksiyonlar.Contains(AksiyonEnum.IcerikCikart);
        }


        #endregion

        public Model()
        {
            _paketDurumlari = new List<PaketDurumu>();
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.Yok, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.PaketOlustur, AksiyonEnum.Ac } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.YeniOlusturuldu, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.Kapat, AksiyonEnum.ParafEkle, AksiyonEnum.ImzaEkle } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.Kapali, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.AcGuncellemeModunda, AksiyonEnum.AcOkumaModunda, AksiyonEnum.Sifrele, AksiyonEnum.IcerikCikart, AksiyonEnum.UstveriGuncelleme } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.ParafEklendi, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.ParafEkle, AksiyonEnum.ImzaEkle, AksiyonEnum.Kapat } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.ImzaEklendi, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.ImzaEkle, AksiyonEnum.MuhurEkle, AksiyonEnum.Kapat } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.MuhurEklendi, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.Kapat, AksiyonEnum.ParafImzaCikar } });
            _paketDurumlari.Add(new PaketDurumu { PaketDurumuEnum = PaketDurumuEnum.OkumaModundaAcildi, OlasiAksiyonlar = new List<AksiyonEnum> { AksiyonEnum.Kapat } });

            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.Yok);
            KonsolCiktilari = new ObservableCollection<Cikti>();
        }

        #region Paket Operasyonlari

        public void PaketOlustur()
        {
            CiktilariDuzenle();

            PaketDosyasiYolu = $@"..\..\dosyalar\{DateTime.Now.ToString("yyyyMMddHHmmss")}.eyp";
            SifreliPaketDosyasiYolu = $@"..\..\dosyalar\{DateTime.Now.ToString("yyyyMMddHHmmss")}.eyps";
            GuncellemePaketiDosyasiYolu = $@"..\..\dosyalar\{DateTime.Now.ToString("yyyyMMddHHmmss")}.eypg";
            _paket = Paket.Ac(PaketDosyasiYolu, PaketModu.Olustur);

            var paketId = Guid.NewGuid();
            var islemTarihi = DateTime.Now;
            var paketKonu = new TextType { Value = "e-Yazışma Test Paketi" };

            CT_Ek ek1 = EkOlusturDahiliDijitalDosya(ekDosyasi1);
            CT_Ek ek2 = EkOlusturHariciReferans();
            CT_Ek ek3 = EkOlusturFiziksel();
            CT_Ek ek4 = EkOlusturDahiliDijitalDosyaImzasiz(ekDosyasi2);
            CT_Ek ek5 = EkOlusturDahiliDijitalDosyaImzali(ekDosyasi3);
            CT_Ilgi ilgiA = IlgiOlustur1(ek1);
            CT_Ilgi ilgiB = IlgiOlustur2();


            CT_GercekSahis ilgili = IlgiliOlustur();
            CT_KurumKurulus olusturan = OlusturanOlustur();
            CT_Dagitim dagitim1 = DagitimOlustur1(ek5);
            CT_Dagitim dagitim2 = DagitimOlustur2();

            var dogrulamaBilgisi = new CT_DogrulamaBilgisi
            {
                DogrulamaAdresi = "https://www.turkiye.gov.tr/belge-dogrulama"
            };

            _paket.Ustveri.BelgeIdBelirle(paketId);      // Zorunlu alan
            _paket.Ustveri.DogrulamaBilgisiEkle(dogrulamaBilgisi); //Zorunlu alan
            _paket.Ustveri.KonuBelirle(paketKonu);       // Zorunlu alan
            _paket.Ustveri.GuvenlikKoduBelirle(ST_KodGuvenlikKodu.HZO);      // Zorunlu alan
            _paket.Ustveri.GuvenlikGecerlilikTarihiBelirle(islemTarihi.AddYears(10));
            _paket.Ustveri.OzIdBelirle("8CEA7FF7-75F2-4CCF-B3C1-1B9B2054E8E9", "GUID");
            _paket.Ustveri.DagitimEkle(dagitim1);        // Zorunlu alan
            _paket.Ustveri.DagitimEkle(dagitim2);        // Zorunlu alan
            _paket.Ustveri.EkEkle(ek1);
            _paket.Ustveri.EkEkle(ek2);
            _paket.Ustveri.EkEkle(ek3);
            _paket.Ustveri.EkEkle(ek4);
            _paket.Ustveri.EkEkle(ek5);
            _paket.Ustveri.IlgiEkle(ilgiA);
            _paket.Ustveri.IlgiEkle(ilgiB);

            _paket.Ustveri.DilBelirle("tur");
            _paket.Ustveri.OlusturanBelirle(olusturan);      // Zorunlu alan
            _paket.Ustveri.IlgiliEkle(ilgili);
            _paket.Ustveri.DosyaAdiBelirle(Path.GetFileName(ustYaziDosyasi));      // Zorunlu alan
            _paket.Ustveri.SdpEkle(new CT_SDP
            {
                Ad = "İç Genelgeler",
                Aciklama = "",
                Kod = "010.06.01"
            });
            _paket.Ustveri.DigerSdpEkle(new CT_SDP
            {
                Ad = "Duyurular",
                Aciklama = "",
                Kod = "010.07.01"
            });
            _paket.Ustveri.DigerSdpEkle(new CT_SDP
            {
                Ad = "Rehber Kılavuz",
                Aciklama = "",
                Kod = "010.08"
            });

            _paket.Ustveri.HeysKoduEkle(new CT_HEYSK
            {
                Kod = 999999,
                Ad = "Ayrıntılı Harcama Programının Hazırlanması",
                Tanim = "Ayrıntılı Harcama Programının Hazırlanması ve Maliye Bakanlığına Gönderilmesi"
            });
            _paket.Ustveri.HeysKoduEkle(new CT_HEYSK
            {
                Kod = 111111,
                Ad = "Yatırım Programlarının Hazırlanması",
                Tanim = "Yatırım Programlarının Hazırlanıp Başbakanlığa Gönderilmesi"
            });

            _paket.EkEkle(ek1, ekDosyasi1);
            _paket.EkEkle(ek4, ekDosyasi2, OzetModu.Yok);
            _paket.EkEkle(ek5, ekDosyasi3);
            CiktiEkle("Ekler eklendi.");

            _paket.UstYaziEkle(ustYaziDosyasi, "application/pdf");        // Zorunlu alan
            CiktiEkle("Üstyazı eklendi.");

            _paket.EkleriKontrolEt();
            _paket.IlgileriKontrolEt();

            _paket.UstveriOlustur();     // Zorunlu alan
            CiktiEkle("Üstveri oluşturuldu.");

            CiktiEkle($"Paket oluşturuldu. Dosya yolu {Path.GetFileName(PaketDosyasiYolu)}");
            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.YeniOlusturuldu);
        }

        public void Kapat()
        {
            CiktilariDuzenle();

            _paket.Kapat();
            _paket.Dispose();
            _paket = null;
            CiktiEkle("Paket kapatıldı.");
            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.Kapali);
        }

        public void ParafEkle()
        {
            CiktilariDuzenle();

            if (_paket.ParafOzetiVarMi() == false)
            {
                _paket.ParafOzetiOlustur();
                CiktiEkle("ParafOzeti oluşturuldu.");
            }

            if (_paket.ParafImzaVarMi() == false)
            {
                Stream parafOzeti = _paket.ParafOzetiAl();
                byte[] imzaliParafOzeti = Imzala(StreamToByteArray(parafOzeti));
                _paket.ParafImzaEkle(imzaliParafOzeti);         // Zorunlu alan
                CiktiEkle("Paraf eklendi.");
            }
            else
            {
                Stream parafImza = _paket.ParafImzaAl();
                byte[] imzaliParafImza = Imzala(StreamToByteArray(parafImza));
                _paket.ParafImzaEkle(imzaliParafImza);         // Zorunlu alan
                CiktiEkle("Seri paraf eklendi.");
            }

            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.ParafEklendi);
        }

        public void ImzaEkle()
        {
            CiktilariDuzenle();

            if (_paket.PaketOzetiVarMi() == false)
            {
                _paket.PaketOzetiOlustur();
                CiktiEkle("PaketOzeti oluşturuldu.");
            }

            if (_paket.ImzaVarMi() == false)
            {
                Stream paketOzeti = _paket.PaketOzetiAl();
                byte[] imzaliPaketOzeti = Imzala(StreamToByteArray(paketOzeti));
                _paket.ImzaEkle(imzaliPaketOzeti);         // Zorunlu alan
                CiktiEkle("İmza eklendi.");
            }
            else
            {
                Stream imza = _paket.ImzaAl();
                byte[] imzaliImza = Imzala(StreamToByteArray(imza));
                _paket.ImzaEkle(imzaliImza);         // Zorunlu alan
                CiktiEkle("Seri imza eklendi.");
            }

            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.ImzaEklendi);
        }

        public void MuhurEkle()
        {
            CiktilariDuzenle();

            _paket.NihaiUstveri.BelgeNoBelirle("1234");
            CiktiEkle("Belge numarası atandı.");
            _paket.NihaiUstveri.TarihBelirle(DateTime.Now);
            CiktiEkle("Belge tarihi atandı.");
            _paket.NihaiUstveri.ImzaEkle(ImzaciOlustur());
            _paket.NihaiUstveri.ImzaEkle(ImzaciOlustur2());
            CiktiEkle("Belge imzacıları atandı.");
            _paket.NihaiUstveriOlustur();
            CiktiEkle("NihaiUstveri oluşturuldu.");
            _paket.CoreOlustur();
            CiktiEkle("Core oluşturuldu.");

            if (_paket.NihaiOzetVarMi() == false)
            {
                _paket.NihaiOzetOlustur();
                CiktiEkle("NihaiOzet oluşturuldu.");
            }

            Stream nihaiOzet = _paket.NihaiOzetAl();
            byte[] imzaliNihaiOzet = Imzala(StreamToByteArray(nihaiOzet));
            _paket.MuhurEkle(imzaliNihaiOzet);         // Zorunlu alan
            CiktiEkle("Mühür eklendi.");

            PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.MuhurEklendi);
        }

        private void AcOkumaModu()
        {
            CiktilariDuzenle();

            try
            {
                var paketOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                var nihaiOzetDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                var parafOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                _paket = Paket.Ac(PaketDosyasiYolu, PaketModu.Ac);
                _paket.PaketOzetiDogrula(Araclar.PaketOzetiAl(_paket.PaketOzetiAl()), ref paketOzetiDogrulamaSonucu);
                _paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(_paket.NihaiOzetAl()), ref nihaiOzetDogrulamaSonucu);
                _paket.ParafOzetiDogrula(Araclar.ParafOzetiAl(_paket.ParafOzetiAl()), ref parafOzetiDogrulamaSonucu);
                CiktiEkle("Paket AÇ modunda açıldı.");
                if (paketOzetiDogrulamaSonucu.Any())
                {
                    paketOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }
                if (nihaiOzetDogrulamaSonucu.Any())
                {
                    nihaiOzetDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }

                if (parafOzetiDogrulamaSonucu.Any())
                {
                    parafOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }
                PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.OkumaModundaAcildi);
            }
            catch (Exception ex)
            {
                CiktiEkle($"Paket AÇ işleminde hata oluştu. {ex.Message}");
            }
        }

        public void AcGuncellemeModu()
        {
            CiktilariDuzenle();

            try
            {
                _paket = Paket.Ac(PaketDosyasiYolu, PaketModu.Guncelle);
                CiktiEkle("Paket Güncelleme amaçlı açıldı.");
                if (_paket.MuhurVarMi())
                {
                    PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.MuhurEklendi);
                }
                else if (_paket.ImzaVarMi())
                {
                    PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.ImzaEklendi);
                }
                else if (_paket.ParafImzaVarMi())
                {
                    PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.ParafEklendi);
                }
                else
                {
                    PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.YeniOlusturuldu);
                }
            }
            catch (Exception ex)
            {
                CiktiEkle($"Paket Güncelleme amaçlı açma işleminde hata oluştu. {ex.Message}");
            }

        }

        private void ParaflariCikar()
        {
            CiktilariDuzenle();

            if (_paket.ParafImzaCikar())
                CiktiEkle("ParafImza çıkarıldı.");
            else
                CiktiEkle("ParafImza çıkarılamadı.");
        }

        private void PaketiGoruntule()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = PaketDosyasiYolu,
                    UseShellExecute = true
                });
                //return;
                //var hedefDosyaYolu = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(PaketDosyasiYolu), System.IO.Path.GetFileNameWithoutExtension(paketDosyasiYolu) + ".zip");
                //int sayac = 0;
                //while (true)
                //{
                //    sayac++;
                //    if (File.Exists(hedefDosyaYolu))
                //    {
                //        hedefDosyaYolu = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(PaketDosyasiYolu), System.IO.Path.GetFileNameWithoutExtension(paketDosyasiYolu) + $"{sayac}.zip");
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                //File.Copy(PaketDosyasiYolu, hedefDosyaYolu);
                //Process.Start(hedefDosyaYolu);
            }
            catch (Exception ex)
            {
                CiktilariDuzenle();
                CiktiEkle($"Paket görüntüle işleminde hata oluştu. {ex.Message}");
            }
        }

        private void UstveriGuncellemePaketiOlustur()
        {
            using (GuncellemePaketi paket = GuncellemePaketi.Ac(GuncellemePaketiDosyasiYolu, PaketModu.Olustur))
            {
                CiktiEkle("Güncelleme paketi oluşturuldu");
                CT_Olusturan olusturan;
                using (Paket orijinalPaket = Paket.Ac(PaketDosyasiYolu, PaketModu.Ac))
                {
                    olusturan = orijinalPaket.Ustveri.OlusturanAl();
                    orijinalPaket.Kapat();
                }
                paket.GuncellemeBilgisi.GuncellemeEkle(ST_GuncellemeTuru.GuvenlikKodu, new CT_GuvenlikKoduDegisiklikBilgisi
                {
                    YeniGizlilikDerecesi = ST_KodGuvenlikKodu.YOK,
                    DegistirmeTarihi = new DateTime(2022, 05, 05),
                    Aciklama = "Belgenin gizlilik süresinin dolması nedeni ile Hizmete Özel olan gizlilik derecesi kaldırılmıştır",
                    KomisyonKarariBelgeNo = "E-68244839-170.01-44181",
                    KomisyonKarariBelgeId = "85E65A34-F460-4590-8BFA-A2DDB07E1B1D"
                });
                paket.GuncellemeBilgisiOlustur();
                paket.OrijinalPaketEkle(new MemoryStream(File.ReadAllBytes(PaketDosyasiYolu)), PaketTipi.NormalPaket);
                paket.CoreOlustur(Guid.NewGuid(), olusturan, "Gizlilik Derecesi Değiştirme");
                if (!paket.NihaiOzetVarMi())
                {
                    paket.NihaiOzetOlustur();
                    CiktiEkle("Güncelleme paketi nihai özet oluşturuldu");
                }

                Stream nihaiOzet = paket.NihaiOzetAl();
                byte[] imzaliNihaiOzet = Imzala(StreamToByteArray(nihaiOzet));
                paket.MuhurEkle(imzaliNihaiOzet);         // Zorunlu alan
                CiktiEkle("Mühür eklendi.");
                paket.Kapat();
                CiktiEkle("Güncelleme paketi kapatıldı");
            }
        }

        private void PaketiSifrele()
        {
            CiktilariDuzenle();
            try
            {
                using (SifreliPaket sifreliPaket = SifreliPaket.Ac(SifreliPaketDosyasiYolu, PaketModu.Olustur))
                {
                    CT_Olusturan olusturan;
                    using (Paket paket = Paket.Ac(PaketDosyasiYolu, PaketModu.Ac))
                    {
                        sifreliPaket.NihaiOzetEkle(paket.NihaiOzetAl());
                        sifreliPaket.BelgeHedef.HedefEkle(Araclar.Dagitim2Hedef(paket.Ustveri.DagitimlariAl()[0]));
                        sifreliPaket.BelgeHedefOlustur();
                        olusturan = paket.Ustveri.OlusturanAl();
                        paket.Kapat();
                    }
                    string tempfile = Path.GetTempFileName();
                    File.WriteAllBytes(tempfile, PaketSifrele(PaketDosyasiYolu));
                    using (Stream s = File.OpenRead(tempfile))
                    {
                        sifreliPaket.SifreliIcerikEkle(s, Guid.NewGuid());
                    }


                    sifreliPaket.SifreliIcerikBilgisiOlustur();
                    sifreliPaket.Kapat(Guid.NewGuid(), olusturan, null);
                }


                CiktiEkle($"Şifreli paket oluşturuldu. Dosya yolu {Path.GetFileName(SifreliPaketDosyasiYolu)}");
            }
            catch (Exception ex)
            {
                CiktiEkle($"Şifreli paket oluşturulması sırasında hata oluştu.  {ex.Message}");
            }
        }

        #endregion



        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> exp)
        {
            var memberExpression = (MemberExpression)exp.Body;
            string propertyName = memberExpression.Member.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void CiktilariDuzenle()
        {
            konsolCiktilari.Where(x => x.SonIslemMi).ToList().ForEach(x => x.SonIslemMi = false);
        }

        private void CiktiEkle(string cikti)
        {
            konsolCiktilari.Add(new Cikti { Value = cikti, SonIslemMi = true });
        }

        private byte[] Imzala(byte[] imzalanacakVeri)
        {
            return new byte[] { 1, 2, 3 }; // burada imzalanacakVeri herhangi bir imzala API'si ile imzalanarak imzali veri donulur
        }

        private byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private CT_Dagitim DagitimOlustur2()
        {
            return new CT_Dagitim
            {
                DagitimTuru = ST_KodDagitimTuru.BLG, // Zorunlu alan
                Ivedilik = ST_KodIvedilik.GNL,      // Zorunlu alan
                Miat = "P1D",
                Item = new CT_KurumKurulus
                {
                    KKK = "24322010",     // Zorunlu alan
                    Adi = new NameType { Value = "Sağlık Bakanlığı" }
                }
            };
        }

        private CT_Dagitim DagitimOlustur1(CT_Ek konulmamisEk)
        {
            return new CT_Dagitim
            {
                DagitimTuru = ST_KodDagitimTuru.GRG,        // Zorunlu alan
                Ivedilik = ST_KodIvedilik.ACL,      // Zorunlu alan
                Item = new CT_KurumKurulus        // Seçimli zorunlu
                {
                    KKK = "24301050",      // Zorunlu alan
                    Adi = new NameType { Value = "Adalet Bakanlığı" },
                    IletisimBilgisi = new CT_IletisimBilgisi
                    {
                        Telefon = "0-312-2665555",
                        EPosta = "info@adalet.gov.tr",
                        KepAdresi = "adaletbakanligi@hs01.kep.tr",
                        Faks = "0-312-2665556",
                        Adres = new TextType { Value = "Kızılay" },
                        Ilce = new NameType { Value = "Çankaya" },
                        Il = new NameType { Value = "Ankara" },
                        Ulke = new NameType { Value = "Türkiye" },
                        WebAdresi = "www.adalet.gov.tr"
                    },
                    BirimKKK = "24301051"
                },
                KonulmamisEkListesi = new CT_KonulmamisEk[]
                {
                    new CT_KonulmamisEk       // Zorunlu alan
                    {
                        EkId = konulmamisEk.Id.Value     // Zorunlu alan
                    }
                }
            };
        }

        private CT_KurumKurulus OlusturanOlustur()
        {
            return new CT_KurumKurulus
            {
                KKK = "82223362",     // Zorunlu alan
                Adi = new NameType { Value = "Türkiye Cumhuriyeti Cumhurbaşkanlığı Dijital Dönüşüm Ofisi Başkanlığı" },
                IletisimBilgisi = new CT_IletisimBilgisi
                {
                    Telefon = "0-312-4036400",
                    EPosta = "dijitaliletisim@cbddo.gov.tr",
                    Faks = "0-312-4036500",
                    KepAdresi = "cbddo@hs01.kep.tr",
                    Adres = new TextType { Value = "T.C. Cumhurbaşkanlığı Çankaya Yerleşkesi Ziaur Rahman Cad. 06550" },
                    Ilce = new NameType { Value = "Çankaya" },
                    Il = new NameType { Value = "Ankara" },
                    Ulke = new NameType { Value = "Türkiye" },
                    WebAdresi = "https://www.cbddo.gov.tr"
                },
                BirimKKK = "12345678"
            };
        }

        private CT_GercekSahis IlgiliOlustur()
        {
            return new CT_GercekSahis   // Seçimli zorunlu (Kurum, gerçek, tüzel)
            {
                Kisi = new CT_Kisi        // Zorunlu alan
                {
                    OnEk = new TextType { Value = "Uzman" },
                    IlkAdi = new NameType { Value = "Hüseyin" },      // Zorunlu alan
                    Soyadi = new NameType { Value = "Yetmez" }        // Zorunlu alan
                },
                Gorev = new TextType { Value = "Birim Sorumlusu" },
                TCKN = "12345678903",
                IletisimBilgisi = new CT_IletisimBilgisi
                {
                    Telefon = "0-505-4821111",
                    TelefonDiger = "0-312-2444444"
                }
            };
        }

        private CT_Imza ImzaciOlustur()
        {
            return new CT_Imza
            {
                Imzalayan = new CT_GercekSahis        // Zorunlu alan
                {
                    Kisi = new CT_Kisi        // Zorunlu alan
                    {
                        IlkAdi = new NameType { Value = "Osman" },        // Zorunlu alan
                        Soyadi = new NameType { Value = "Yeşil" },        // Zorunlu alan
                        Unvan = new NameType { Value = "Büyükelçi" },
                        IkinciAdi = new NameType { Value = "Murat" },
                        OnEk = new TextType { Value = "Dr." }
                    },
                    TCKN = "12345678902",
                    Gorev = new TextType { Value = "Daire Başkanı" },
                    IletisimBilgisi = new CT_IletisimBilgisi
                    {
                        Telefon = "0-312-2666666",
                        TelefonDiger = "0-532-4828282",
                        EPosta = "om_yesil@xyz.com.tr",
                        Faks = "0-312-2888888",
                        Adres = new TextType { Value = "T.C Cumhurbaşkanlığı Külliyesi 06560 Beştepe - Ankara" },
                        Ilce = new NameType { Value = "Çankaya" },
                        Il = new NameType { Value = "Ankara" },
                        Ulke = new NameType { Value = "Türkiye" },
                        WebAdresi = "https://www.cbddo.gov.tr/"
                    }
                },
                YetkiDevreden = new CT_GercekSahis
                {
                    Kisi = new CT_Kisi   // Zorunlu alan
                    {
                        IlkAdi = new NameType { Value = "Hasan" },        // Zorunlu alan
                        Soyadi = new NameType { Value = "Aydın" },        // Zorunlu alan
                        Unvan = new NameType { Value = "Genel Müdür" },
                        IkinciAdi = new NameType { Value = "Şaban" },
                        OnEk = new TextType { Value = "Uzman" }
                    },
                    TCKN = "12345588901",
                    Gorev = new TextType { Value = "Genel Müdür" },
                    IletisimBilgisi = new CT_IletisimBilgisi
                    {
                        Telefon = "0-312-2999999",
                        TelefonDiger = "0-532-4827777",
                        EPosta = "iletisim@abc.com",
                        Faks = "0-312-2946345",
                        Adres = new TextType { Value = "TUİK Necatibey Cad. No:112" },
                        Ilce = new NameType { Value = "Çankaya" },
                        Il = new NameType { Value = "Ankara" },
                        Ulke = new NameType { Value = "Türkiye" },
                        WebAdresi = "www.tuik.gov.tr"
                    }
                },
                VekaletVeren = new CT_GercekSahis
                {
                    Kisi = new CT_Kisi  // Zorunlu alan
                    {
                        IlkAdi = new NameType { Value = "Semih" },  // Zorunlu alan
                        Soyadi = new NameType { Value = "Yılmaz" },  // Zorunlu alan
                        Unvan = new NameType { Value = "Dr." }
                    },
                    Gorev = new TextType { Value = "Müsteşar" }
                },
                Makam = new NameType { Value = "Müsteşar" },
                Amac = new TextType { Value = "Onay" },
                Aciklama = new TextType { Value = "İmzanın açıklaması" },
                Tarih = DateTime.Now,
                TarihSpecified = true
            };
        }

        private static CT_Imza ImzaciOlustur2()
        {
            return new CT_Imza
            {
                Imzalayan = new CT_GercekSahis        // Zorunlu alan
                {
                    Kisi = new CT_Kisi        // Zorunlu alan
                    {
                        IlkAdi = new NameType { Value = "Metin" },        // Zorunlu alan
                        Soyadi = new NameType { Value = "Demir" },        // Zorunlu alan
                        OnEk = new TextType { Value = "Dr." }
                    },
                    TCKN = "98787678902",
                    Gorev = new TextType { Value = "Şube Müdürü" },
                },
                Makam = new NameType { Value = "Daire" },
                Amac = new TextType { Value = "Onay" },
                Aciklama = new TextType { Value = "İmzanın açıklaması" },
                Tarih = DateTime.Now,
                TarihSpecified = true
            };
        }

        private CT_Ilgi IlgiOlustur2()
        {
            return new CT_Ilgi
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },    // Zorunlu alan   
                BelgeNo = "48154937-612.01.03-945",
                Tarih = new DateTime(2009, 11, 5),
                TarihSpecified = true,
                Etiket = "b",      // Zorunlu alan                
                Ad = new TextType { Value = "İlgi (b) yazı" },
                Aciklama = new TextType { Value = "İlgi (b) yazının açıklaması" },
                OzId = new IdentifierType
                {
                    Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
                    schemeID = "GUID"
                }
            };
        }

        private CT_Ilgi IlgiOlustur1(CT_Ek ek1)
        {
            return new CT_Ilgi
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString(), EYazismaIdMi = false },       // Zorunlu alan
                BelgeNo = "65410891-020-480",
                Tarih = new DateTime(2009, 11, 5),
                TarihSpecified = true,
                Etiket = "a",       // Zorunlu alan
                EkId = ek1.Id.Value,
                Ad = new TextType { Value = "İlgi (a) yazı" },
                Aciklama = new TextType { Value = "İlgi (a) yazının açıklaması" },
                OzId = new IdentifierType
                {
                    Value = "1C26C956-AE45-4DC1-BE86-1B8278A34904",
                    schemeID = "GUID"       // Zorunlu alan
                }
            };
        }

        private CT_Ek EkOlusturDahiliDijitalDosyaImzali(string dosyaYolu)
        {
            return new CT_Ek
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString(), EYazismaIdMi = true },     // Zorunlu alan
                BelgeNo = "65410891-020-444",
                Tur = ST_KodEkTuru.DED,            // Zorunlu alan
                DosyaAdi = Path.GetFileName(dosyaYolu),
                MimeTuru = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                Ad = new TextType { Value = "İkinci ek" },
                SiraNo = 5,         // Zorunlu alan
                Aciklama = new TextType { Value = "İkinci ek açıklaması" },
                OzId = new IdentifierType { Value = "D7D16B9C-7419-451B-82B0-34D334572464", schemeID = "GUID" },
                ImzaliMi = true,
                ImzaliMiSpecified = true
                //NULL olmayan bir propertynin serialize edilip edilmeyeceğini belirtir. 
                //Bool, int, datetime, enum gibi tipler için otomatik olarak eklenir.
                //Daha detaylı bilgi aşağıdaki bağlantıdan edinilebilir.
                //https://msdn.microsoft.com/en-us/library/ms753816(v=vs.85).aspx
            };
        }

        private CT_Ek EkOlusturDahiliDijitalDosyaImzasiz(string dosyaYolu)
        {
            return new CT_Ek
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
                BelgeNo = "65410891-020-408",
                Tur = ST_KodEkTuru.DED,     // Zorunlu alan
                DosyaAdi = Path.GetFileName(dosyaYolu),
                MimeTuru = "application/MSWord",
                Ad = new TextType { Value = "İmzasız ek" },
                SiraNo = 4,     // Zorunlu alan
                Aciklama = new TextType { Value = "İmzasız ek açıklaması" },
                OzId = new IdentifierType
                {
                    Value = "76E16DDA-AB44-46DB-B615-42FBE7E9BEE3",
                    schemeID = "GUID"
                },
                ImzaliMi = false,
                ImzaliMiSpecified = true
            };
        }

        private CT_Ek EkOlusturFiziksel()
        {
            return new CT_Ek
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
                BelgeNo = "48154937-612.01.03-942",
                Tur = ST_KodEkTuru.FZK,        // Zorunlu alan
                Ad = new TextType { Value = "Fiziksel ek" },
                SiraNo = 3,     // Zorunlu alan
                Aciklama = new TextType { Value = "İki adet CD" },
                OzId = new IdentifierType { Value = "45A778BA-B943-4B68-8504-71854DF0DDFF", schemeID = "GUID" },
                ImzaliMi = false,
                ImzaliMiSpecified = true
            };
        }

        private CT_Ek EkOlusturHariciReferans()
        {
            return new CT_Ek
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString() },     // Zorunlu alan
                BelgeNo = "35826416-020-987",
                Tur = ST_KodEkTuru.HRF,       // Zorunlu alan
                Ad = new TextType { Value = "Harici referans" },
                SiraNo = 2,        // Zorunlu alan
                Aciklama = new TextType { Value = "Hairici referans açıklaması" },
                Referans = "http://www.bilgitoplumu.gov.tr/Documents/1/Raporlar/Calisma_Raporu_2.pdf",
                OzId = new IdentifierType
                {
                    Value = "0C9C4D34-D370-46C2-A996-0FADC0FA591F",
                    schemeID = "GUID"       // Zorunlu alan
                },
                ImzaliMi = false,
                ImzaliMiSpecified = true,
                Ozet = new CT_Ozet
                {
                    OzetAlgoritmasi = new CT_OzetAlgoritmasi { Algorithm = Araclar.ALGORITHM_SHA256 },    // Zorunlu alan
                    OzetDegeri = Convert.FromBase64String("/InKUd7ATLR4PzUCBiWjyD38d9oxbWF72RR1QVSdFMo=")   // Zorunlu alan
                }
            };
        }

        private CT_Ek EkOlusturDahiliDijitalDosya(string dosyaYolu)
        {
            return new CT_Ek
            {
                Id = new CT_Id { Value = Guid.NewGuid().ToString() },    // Zorunlu alan
                BelgeNo = "72131250-010.03-936",
                Tur = ST_KodEkTuru.DED,        // Zorunlu alan
                DosyaAdi = Path.GetFileName(dosyaYolu),
                MimeTuru = "application/pdf",
                Ad = new TextType { Value = "Birinci ek" },
                SiraNo = 1,     // Zorunlu alan
                Aciklama = new TextType { Value = "Birinci ek açıklaması" },
                OzId = new IdentifierType
                {
                    Value = "A14A4DCC-AE6A-4FD5-AAB3-8A33DC6125DD",
                    schemeID = "GUID"     // Zorunlu alan
                },
                ImzaliMi = true,
                ImzaliMiSpecified = true
            };
        }

        private byte[] PaketSifrele(string dosyaYolu)
        {
            return File.ReadAllBytes(dosyaYolu);  // Burada, şifrelenen paketin geri döndürülmesi gerekiyor.
        }

        private void PaketAc()
        {
            CiktilariDuzenle();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "e-Yazışma Paketi (*.eyp, *.epys,*.eypg) | *.eyp; *.eyps;*.eypg;";
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                var paketTipi = Araclar.PaketTipiGetir(openFileDialog.FileName);
                if (paketTipi == PaketTipi.SifreliPaket)
                {
                    CiktiEkle("Şifreli paket açılamamaktadır");
                }
                else if (paketTipi == PaketTipi.GuncellemePaketi)
                {
                    try
                    {
                        var nihaiOzetDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                        var paket = GuncellemePaketi.Ac(openFileDialog.FileName, PaketModu.Ac);

                        var nihaiOzet = paket.NihaiOzetAl();
                        if (nihaiOzet != null)
                        {
                            paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(nihaiOzet), ref nihaiOzetDogrulamaSonucu);
                        }

                        if (nihaiOzetDogrulamaSonucu.Any())
                        {
                            nihaiOzetDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                        }
                        CiktiEkle("Paket AÇ modunda açıldı ve kapatıldı.");
                        paket.Kapat();
                    }
                    catch (Exception ex)
                    {
                        CiktiEkle($"Paket AÇ işleminde hata oluştu. {ex.Message}");
                        _paket.Kapat();
                    }
                }
                else if (paketTipi == PaketTipi.NormalPaket)
                {
                    try
                    {
                        var paketOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                        var nihaiOzetDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                        var parafOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                        _paket = Paket.Ac(openFileDialog.FileName, PaketModu.Ac);

                        var dagitimIdentifier = new IdentifierType();
                        dagitimIdentifier.Value = "";
                        //_paket.EkleriKontrolEt(dagitimIdentifier);

                        var parafOzeti = _paket.ParafOzetiAl();
                        if (parafOzeti != null)
                        {
                            _paket.ParafOzetiDogrula(Araclar.ParafOzetiAl(parafOzeti), ref parafOzetiDogrulamaSonucu, dagitimIdentifier);
                        }

                        var paketOzeti = _paket.PaketOzetiAl();
                        if (paketOzeti != null)
                        {
                            _paket.PaketOzetiDogrula(Araclar.PaketOzetiAl(paketOzeti), ref paketOzetiDogrulamaSonucu, dagitimIdentifier);
                        }

                        var nihaiOzet = _paket.NihaiOzetAl();
                        if (nihaiOzet != null)
                        {
                            _paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(nihaiOzet), ref nihaiOzetDogrulamaSonucu, dagitimIdentifier);
                        }

                        if (paketOzetiDogrulamaSonucu.Any())
                        {
                            paketOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                        }
                        if (nihaiOzetDogrulamaSonucu.Any())
                        {
                            nihaiOzetDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                        }

                        if (parafOzetiDogrulamaSonucu.Any())
                        {
                            parafOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                        }

                        CiktiEkle("Paket AÇ modunda açıldı.");
                        PaketDurumu = _paketDurumlari.SingleOrDefault(x => x.PaketDurumuEnum == PaketDurumuEnum.OkumaModundaAcildi);
                        paketDosyasiYolu = openFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        CiktiEkle($"Paket AÇ işleminde hata oluştu. {ex.Message}");
                        _paket.Kapat();
                    }
                }
                else
                {
                    CiktiEkle("Paket tipi belirlenemedi.");
                }
            }
        }

        private void PaketIceriginiCikart()
        {
            using (var paket = Paket.Ac(paketDosyasiYolu, PaketModu.Ac))
            {
                string klasor = string.Format("..\\..\\dosyalar\\{0}", Path.GetFileNameWithoutExtension(paketDosyasiYolu));
                if (!Directory.Exists(klasor))
                    Directory.CreateDirectory(klasor);

                var paketOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                var nihaiOzetDogrulamaSonucu = new List<OzetDogrulamaHatasi>();
                var parafOzetiDogrulamaSonucu = new List<OzetDogrulamaHatasi>();

                var paketOzetiDogrulamasi = paket.PaketOzetiDogrula(Araclar.PaketOzetiAl(paket.PaketOzetiAl()), ref paketOzetiDogrulamaSonucu);
                var nihaiOzetDogrulamasi = paket.NihaiOzetDogrula(Araclar.NihaiOzetAl(paket.NihaiOzetAl()), ref nihaiOzetDogrulamaSonucu);
                var parafOzetiDogrulamasi = paket.ParafOzetiDogrula(Araclar.ParafOzetiAl(paket.ParafOzetiAl()), ref parafOzetiDogrulamaSonucu);

                CiktiEkle("Paket okuma modunda açıldı.");

                if (paketOzetiDogrulamaSonucu.Any())
                {
                    paketOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }

                if (nihaiOzetDogrulamaSonucu.Any())
                {
                    nihaiOzetDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }

                if (parafOzetiDogrulamaSonucu.Any())
                {
                    parafOzetiDogrulamaSonucu.ForEach(p => CiktiEkle(string.Format("{0} - {1}", p.HataKodu, p.Hata)));
                }


                if (paket.Ustveri.EkleriAl() != null && paket.Ustveri.EkleriAl().Any())
                {
                    var dedEkler = paket.Ustveri.EkleriAl().Where(x => x.Tur == ST_KodEkTuru.DED).ToList();
                    if (dedEkler.Any())
                    {
                        foreach (var dedEk in dedEkler)
                        {
                            var ek = paket.EkAl(new Guid(dedEk.Id.Value));
                            if (ek != null)
                            {
                                using (Stream s = File.OpenWrite(klasor + "\\" + dedEk.DosyaAdi))
                                {
                                    Araclar.CopyStream(ek, s);
                                }
                            }

                        }
                    }
                }

                //Nihai Özet
                NihaiOzet nihaiOzet = paket.NihaiOzet;
                CT_Reference[] nihaiOzetOzetleri = paket.NihaiOzet.OzetleriAl();
                Stream streamNihaiOzet = paket.NihaiOzetAl();

                //Paket Özeti
                PaketOzeti paketOzeti = paket.PaketOzeti;
                CT_Reference[] paketOzetiOzetleri = paket.PaketOzeti.OzetleriAl();
                Stream streamPaketOzeti = paket.PaketOzetiAl();

                //Paraf Özeti
                ParafOzeti parafOzeti = paket.ParafOzeti;
                CT_Reference[] parafOzetiOzetleri = paket.ParafOzeti.OzetleriAl();
                Stream streamParafOzeti = paket.ParafOzetiAl();

                //Üstveri
                Ustveri ustveri = paket.Ustveri;
                CT_Dagitim[] dagitimlar = paket.Ustveri.DagitimlariAl();
                CT_Olusturan olusturan = paket.Ustveri.OlusturanAl();
                string belgeId = paket.Ustveri.BelgeIdAl();
                string dil = paket.Ustveri.DilAl();
                string dosyaAdi = paket.Ustveri.DosyaAdiAl();
                DateTime? guvenlikGecerlilikTarihi = paket.Ustveri.GuvenlikGecerlilikTarihiAl();
                ST_KodGuvenlikKodu guvenlikKodu = paket.Ustveri.GuvenlikKoduAl();
                CT_Ilgi[] ilgiler = paket.Ustveri.IlgileriAl();
                CT_Ilgili[] ilgililer = paket.Ustveri.IlgilileriAl();
                TextType konu = paket.Ustveri.KonuAl();
                string mimeTuru = paket.Ustveri.MimeTuruAl();
                IdentifierType ozId = paket.Ustveri.OzIdAl();
                CT_Ek[] ekler = paket.Ustveri.EkleriAl();
                CT_HEYSK[] heysKodlari = paket.Ustveri.HeysKodlariniAl();

                //Nihai Üstveri
                NihaiUstveri nihaiUstveri = paket.NihaiUstveri;
                string belgeNo = paket.NihaiUstveri.BelgeNoAl();
                CT_Imza[] imzalar = paket.NihaiUstveri.ImzalariAl();
                DateTime tarih = paket.NihaiUstveri.TarihAl();

                OzetModu varsayilanOzetModu = paket.VarsayilanOzetModu;

                CiktiEkle(paket.PaketOzetiVarMi() ? "Paket Ozeti Var" : "Paket Ozeti Yok");
                CiktiEkle(paket.ImzaVarMi() ? "İmza Var" : "İmza Yok");
                CiktiEkle(paket.ParafImzaVarMi() ? "Paraf İmza Var" : "Paraf İmza Yok");
                CiktiEkle(paket.MuhurVarMi() ? "Muhur Var" : "Muhur Yok");
                CiktiEkle(paket.NihaiOzetVarMi() ? "Nihai Ozet Var" : "Nihai Ozet Yok");
                CiktiEkle(paket.NihaiUstveriVarMi() ? "Nihai Ustveri Var" : "Nihai Ustveri Yok");
                CiktiEkle(paket.UstveriVarMi() ? "Üstveri Var" : "Üstveri Yok");
                CiktiEkle(paket.UstYaziVarMi() ? "Üstyazı Var" : "Üstyazı Yok");

                paket.EkleriKontrolEt();
                paket.IlgileriKontrolEt();

                using (Stream s = File.OpenWrite(klasor + "\\" + "ustyazi.pdf"))
                {
                    Araclar.CopyStream(paket.UstYaziAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\" + "Ustveri.xml"))
                {
                    Araclar.CopyStream(paket.UstveriAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\ParafImzaCades.imz"))
                {
                    Araclar.CopyStream(paket.ParafImzaAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\ImzaCades.imz"))
                {
                    Araclar.CopyStream(paket.ImzaAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\MuhurCades.imz"))
                {
                    Araclar.CopyStream(paket.MuhurAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\" + "ParafOzeti.xml"))
                {
                    Araclar.CopyStream(paket.ParafOzetiAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\" + "PaketOzeti.xml"))
                {
                    Araclar.CopyStream(paket.PaketOzetiAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\" + "NihaiOzet.xml"))
                {
                    Araclar.CopyStream(paket.NihaiOzetAl(), s);
                }
                using (Stream s = File.OpenWrite(klasor + "\\" + "NihaiUstveri.xml"))
                {
                    Araclar.CopyStream(paket.NihaiUstveriAl(), s);
                }
            }
        }
    }
}
