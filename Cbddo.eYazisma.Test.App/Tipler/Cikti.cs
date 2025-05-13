using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cbddo.eYazisma.Test.App.Tipler
{
    public class Cikti: INotifyPropertyChanged
    {
        bool sonIslemMi;
        public bool SonIslemMi
        {
        	get
        	{
        		return sonIslemMi;
        	}
        	set
        	{
        		if (Equals(value, sonIslemMi)) return;
        		sonIslemMi = value;
        		NotifyPropertyChanged(() => SonIslemMi);
        	}
        }
        public string Value { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> exp)
        {
            var memberExpression = (MemberExpression)exp.Body;
            string propertyName = memberExpression.Member.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
