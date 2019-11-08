using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Rail.Controls
{
    public abstract class NumericSlider<T> : Control
    {
        private RepeatButton incRepeatButton;
        private RepeatButton decRepeatButton;

        public NumericSlider()
        {
            this.Text = ValueToString();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //if (Spinner != null)
            //    Spinner.Spin -= OnSpinnerSpin;

            this.incRepeatButton = GetTemplateChild("incRepeatButton") as RepeatButton;
            this.decRepeatButton = GetTemplateChild("decRepeatButton") as RepeatButton;

            if (this.incRepeatButton != null)
            {
                this.incRepeatButton.Click += OnIncRepeatButtonClick;
            }
            if (this.decRepeatButton != null)
            {
                this.decRepeatButton.Click += OnDecRepeatButtonClick;
            }
            //if (Spinner != null)
            //    Spinner.Spin += OnSpinnerSpin;
            //Slider
        }

        protected void OnIncRepeatButtonClick(object sender, RoutedEventArgs e)
        {
            IncrementValue();
        }

        protected void OnDecRepeatButtonClick(object sender, RoutedEventArgs e)
        {
            DecrementValue();
        }

        protected abstract void IncrementValue();
        protected abstract void DecrementValue();

        #region CultureInfo

        public static readonly DependencyProperty CultureInfoProperty =
            DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(NumericSlider<T>), new UIPropertyMetadata(CultureInfo.CurrentCulture, OnCultureInfoChanged));

        public CultureInfo CultureInfo
        {
            get
            {
                return (CultureInfo)GetValue(CultureInfoProperty);
            }
            set
            {
                SetValue(CultureInfoProperty, value);
            }
        }

        private static void OnCultureInfoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> numericSlider = o as NumericSlider<T>;
            if (numericSlider != null)
            {
                numericSlider.OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue);
            }
        }

        protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {

        }

        #endregion //CultureInfo
        #region FormatString

        public static readonly DependencyProperty FormatStringProperty =
            DependencyProperty.Register("FormatString", typeof(string), typeof(NumericSlider<T>), new UIPropertyMetadata(String.Empty, OnFormatStringChanged));

        public string FormatString
        {
            get
            {
                return (string)GetValue(FormatStringProperty);
            }
            set
            {
                SetValue(FormatStringProperty, value);
            }
        }

        private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> numericSlider = o as NumericSlider<T>;
            if (numericSlider != null)
            {
                numericSlider.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
            }
        }

        private void OnFormatStringChanged(string oldValue, string newValue)
        {
            this.Text = ValueToString();
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NumericSlider<T>), new FrameworkPropertyMetadata(default(String), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged, null, false, UpdateSourceTrigger.LostFocus));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> numericSlider = o as NumericSlider<T>;
            if (numericSlider != null)
            {

            }
        }

        protected virtual void OnTextChanged(string oldValue, string newValue)
        {

        }

        #endregion //Text

        #region Increment

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(T), typeof(NumericSlider<T>), new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));

        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        private static void OnIncrementChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> numericUpDown = o as NumericSlider<T>;
            if (numericUpDown != null)
                numericUpDown.OnIncrementChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnIncrementChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                //SetValidSpinDirection();
            }
        }

        private static object OnCoerceIncrement(DependencyObject d, object baseValue)
        {
            NumericSlider<T> numericUpDown = d as NumericSlider<T>;
            if (numericUpDown != null)
                return numericUpDown.OnCoerceIncrement((T)baseValue);

            return baseValue;
        }

        protected virtual T OnCoerceIncrement(T baseValue)
        {
            return baseValue;
        }

        #endregion

        #region Maximum

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(T), typeof(NumericSlider<T>), new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));

        public T Maximum
        {
            get
            {
                return (T)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> upDown = o as NumericSlider<T>;
            if (upDown != null)
                upDown.OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnMaximumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                //SetValidSpinDirection();
            }
        }

        private static object OnCoerceMaximum(DependencyObject d, object baseValue)
        {
            NumericSlider<T> upDown = d as NumericSlider<T>;
            if (upDown != null)
                return upDown.OnCoerceMaximum((T)baseValue);

            return baseValue;
        }

        protected virtual T OnCoerceMaximum(T baseValue)
        {
            return baseValue;
        }

        #endregion

        #region Minimum

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(T), typeof(NumericSlider<T>), new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));

        public T Minimum
        {
            get
            {
                return (T)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> upDown = o as NumericSlider<T>;
            if (upDown != null)
                upDown.OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
        }

        protected virtual void OnMinimumChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                //SetValidSpinDirection();
            }
        }

        private static object OnCoerceMinimum(DependencyObject d, object baseValue)
        {
            NumericSlider<T> upDown = d as NumericSlider<T>;
            if (upDown != null)
                return upDown.OnCoerceMinimum((T)baseValue);

            return baseValue;
        }

        protected virtual T OnCoerceMinimum(T baseValue)
        {
            return baseValue;
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(T), typeof(NumericSlider<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static object OnCoerceValue(DependencyObject o, object basevalue)
        {
            return ((NumericSlider<T>)o).OnCoerceValue(basevalue);
        }

        protected virtual object OnCoerceValue(object newValue)
        {
            return newValue;
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NumericSlider<T> numericSlider = o as NumericSlider<T>;
            if (numericSlider != null)
            {
                numericSlider.OnValueChanged((T)e.OldValue, (T)e.NewValue);
            }
        }

        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            this.Text = ValueToString();
        }

        protected abstract string ValueToString();

        #endregion

        public static readonly DependencyProperty TickFrequencyProperty =
            DependencyProperty.Register("TickFrequency", typeof(T), typeof(NumericSlider<T>), new UIPropertyMetadata(default(T)));

        public T TickFrequency
        {
            get
            {
                return (T)GetValue(TickFrequencyProperty);
            }
            set
            {
                SetValue(TickFrequencyProperty, value);
            }
        }

        public static readonly DependencyProperty TickPlacementProperty =
            DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(NumericSlider<T>), new UIPropertyMetadata(TickPlacement.None));

        public TickPlacement TickPlacement
        {
            get
            {
                return (TickPlacement)GetValue(TickPlacementProperty);
            }
            set
            {
                SetValue(TickPlacementProperty, value);
            }
        }

        public static readonly DependencyProperty SliderWidthProperty =
            DependencyProperty.Register("SliderWidth", typeof(double), typeof(NumericSlider<T>), new UIPropertyMetadata(160.0));

        public double SliderWidth
        {
            get
            {
                return (double)GetValue(SliderWidthProperty);
            }
            set
            {
                SetValue(SliderWidthProperty, value);
            }
        }

        public static readonly DependencyProperty TextWidthProperty =
            DependencyProperty.Register("TextWidth", typeof(double), typeof(NumericSlider<T>), new UIPropertyMetadata(50.0));

        public double TextWidth
        {
            get
            {
                return (double)GetValue(TextWidthProperty);
            }
            set
            {
                SetValue(TextWidthProperty, value);
            }
        }
    }
}
