using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Rail.Mvvm
{
    public class ColViewSource : DependencyObject
    {
        private static readonly DependencyPropertyKey ViewPropertyKey = DependencyProperty.RegisterReadOnly("View", typeof(ICollectionView), typeof(ColViewSource), new FrameworkPropertyMetadata((object)null));

        public static readonly DependencyProperty ViewProperty = ViewPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ColViewSource), new FrameworkPropertyMetadata((object)null, (PropertyChangedCallback)OnSourceChanged));

		//[ReadOnly(true)]
		//public ICollectionView View => GetOriginalView(CollectionView);

		public object Source
		{
			get
			{
				return GetValue(SourceProperty);
			}
			set
			{
				SetValue(SourceProperty, value);
			}
		}

		private static ICollectionView GetOriginalView(ICollectionView view)
		{
			//for (CollectionViewProxy collectionViewProxy = view as CollectionViewProxy; collectionViewProxy != null; collectionViewProxy = (view as CollectionViewProxy))
			//{
			//	view = collectionViewProxy.ProxiedView;
			//}
			return view;
		}

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColViewSource collectionViewSource = (ColViewSource)d;
			collectionViewSource.OnSourceChanged(e.OldValue, e.NewValue);
			//collectionViewSource.EnsureView();
		}

		protected virtual void OnSourceChanged(object oldSource, object newSource)
		{
			if (oldSource is INotifyCollectionChanged oldNotify)
			{
				oldNotify.CollectionChanged -= OnCollectionChanged;
			}
			if (newSource is INotifyCollectionChanged newNotify)
			{
				newNotify.CollectionChanged += OnCollectionChanged;
			}
			
		}

		private void OnCollectionChanged(object sender, EventArgs e)
		{
			//CollectionView
			//ICollectionView
		}
	}
}
