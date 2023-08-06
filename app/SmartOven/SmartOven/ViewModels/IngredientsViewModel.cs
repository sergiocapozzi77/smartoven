// <copyright file="IngredientsViewModel.cs" company="McLaren Applied Technologies Ltd.">
// Copyright (c) McLaren Applied Technologies Ltd.</copyright>

using SmartOvenV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartOvenV2.ViewModels
{
    public class IngredientsViewModel : INotifyPropertyChanged
    {
        private double flour;
        private double fat;
        private bool oilNeeded;
        private int doughBalls;
        private string pizzaTypeImage;
        private double salt;
        private PizzaType selectedPizzaType;
        private Yeast selectedYeast;
        private double water;
        private double yeast;
        private string flourTip;
        private string waterTip;
        private int roomTemperature;
        private int ballWeight;
        private int waterPerc;
        private float saltPerc;
        private int totalLeavening;
        private int fridgeLeavening;
        private float fatPerc;
        private double waterBiga1;
        private double waterBiga2;
        private double yeastBiga;
        private double saltBiga;
        private double flourBiga;
        private double fatBiga;
        private int bigaPercentage;
        private double flourBigaDough;
        private int bigaWaterPercentage;

        public IngredientsViewModel()
        {
            this.Yeasts = new List<string>
                {
                    "Dried", "Fresh", "Sourdough"
                };

            this.SelectedPizzaTypeCommand = new Command<string>(this.UserSelectedPizzaType);
            this.SelectedYeastCommand = new Command<string>(this.UserSelectedYeastType);

            this.DoughBalls = Preferences.Get(nameof(DoughBalls), 1);
            this.BallWeight = Preferences.Get(nameof(BallWeight), 270); ;
            this.WaterPerc = Preferences.Get(nameof(WaterPerc), 65); ;
            this.RoomTemperature = Preferences.Get(nameof(RoomTemperature), 21); ;
            this.SaltPerc = Preferences.Get(nameof(SaltPerc), 2.8F);
            this.FatPerc = Preferences.Get(nameof(FatPerc), 2.7F);
            this.TotalLeavening = Preferences.Get(nameof(TotalLeavening), 24);
            this.FridgeLeavening = Preferences.Get(nameof(FridgeLeavening), 20);
            this.BigaPercentage = Preferences.Get(nameof(BigaPercentage), 70);
            this.BigaWaterPercentage = Preferences.Get(nameof(BigaWaterPercentage), 50);
            this.SelectedPizzaType = (PizzaType)Preferences.Get(nameof(SelectedPizzaType), (int)PizzaType.Tray);
            this.SelectedYeast = (Models.Yeast)Preferences.Get(nameof(SelectedYeast), (int)Models.Yeast.Dried1to2);

            FillTips();

            this.Recalculate();
        }

        private void UserSelectedYeastType(string yeastType)
        {
            var yeast = (Yeast)Enum.Parse(typeof(Yeast), yeastType);
            this.SelectedYeast = yeast;
        }

        private void UserSelectedPizzaType(string pizzaType)
        {
            var ptype = (PizzaType)Enum.Parse(typeof(PizzaType), pizzaType);
            this.SelectedPizzaType = ptype;
        }

        public bool IsDoughballs => SelectedPizzaType == PizzaType.DoughBalls;

        public bool IsTray => SelectedPizzaType == PizzaType.Tray;

        public bool IsDriedYeast23 => SelectedYeast == Models.Yeast.Dried2to3;
        public bool IsDriedYeast => SelectedYeast == Models.Yeast.Dried1to2;
        public bool IsFreshYeast => SelectedYeast == Models.Yeast.Fresh;
        public bool IsSourdoughYeast => SelectedYeast == Models.Yeast.Sourdough;

        public Command<string> SelectedPizzaTypeCommand { get; private set; }
        public Command<string> SelectedYeastCommand { get; private set; }

        public double Water
        {
            get => this.water;
            set
            {
                this.SetProperty(ref this.water, value);
            }
        }

        public double Yeast
        {
            get => this.yeast;
            set
            {
                this.SetProperty(ref this.yeast, value);
            }
        }
        public double Salt
        {
            get => this.salt;
            set
            {
                this.SetProperty(ref this.salt, value);
            }
        }

        public double Flour
        {
            get => this.flour;
            set
            {
                this.SetProperty(ref this.flour, value);
            }
        }

        public double Fat
        {
            get => this.fat;
            set
            {
                this.SetProperty(ref this.fat, value);
            }
        }

        // BIGA:

        public double WaterBiga1
        {
            get => this.waterBiga1;
            set
            {
                this.SetProperty(ref this.waterBiga1, value);
            }
        }

        public double WaterBiga2
        {
            get => this.waterBiga2;
            set
            {
                this.SetProperty(ref this.waterBiga2, value);
            }
        }

        public double YeastBiga
        {
            get => this.yeastBiga;
            set
            {
                this.SetProperty(ref this.yeastBiga, value);
            }
        }
        public double SaltBiga
        {
            get => this.saltBiga;
            set
            {
                this.SetProperty(ref this.saltBiga, value);
            }
        }

        public double FlourBiga
        {
            get => this.flourBiga;
            set
            {
                this.SetProperty(ref this.flourBiga, value);
            }
        }

        public double FlourBigaDough
        {
            get => this.flourBigaDough;
            set
            {
                this.SetProperty(ref this.flourBigaDough, value);
                OnPropertyChanged(nameof(NeedFlourBigaDough));
            }
        }

        public double FatBiga
        {
            get => this.fatBiga;
            set
            {
                this.SetProperty(ref this.fatBiga, value);
            }
        }

        public bool OilNeeded
        {
            get => this.oilNeeded;
            set => this.SetProperty(ref this.oilNeeded, value);
        }

        public string FlourTip
        {
            get => this.flourTip;
            set => this.SetProperty(ref this.flourTip, value);
        }

        public string WaterTip
        {
            get => this.waterTip;
            set => this.SetProperty(ref this.waterTip, value);
        }

        public int DoughBalls
        {
            get => this.doughBalls;
            set
            {
                this.SetProperty(ref this.doughBalls, value);
                Preferences.Set(nameof(DoughBalls), value);
                this.Recalculate();
            }
        }

        public int BallWeight
        {
            get => this.ballWeight;
            set
            {
                this.SetProperty(ref this.ballWeight, value);
                Preferences.Set(nameof(BallWeight), value);
                this.Recalculate();
            }
        }

        public float SaltPerc
        {
            get => this.saltPerc;
            set
            {
                this.SetProperty(ref this.saltPerc, value);
                Preferences.Set(nameof(SaltPerc), value);
                this.Recalculate();
            }
        }

        public float FatPerc
        {
            get => this.fatPerc;
            set
            {
                this.SetProperty(ref this.fatPerc, value);
                Preferences.Set(nameof(FatPerc), value);
                this.Recalculate();
            }
        }

        public int BigaPercentage
        {
            get => this.bigaPercentage;
            set
            {
                this.SetProperty(ref this.bigaPercentage, value);
                Preferences.Set(nameof(BigaPercentage), value);
                this.Recalculate();
            }
        }

        public int BigaWaterPercentage

        {
            get => this.bigaWaterPercentage;
            set
            {
                this.SetProperty(ref this.bigaWaterPercentage, value);
                Preferences.Set(nameof(BigaWaterPercentage), value);
                this.Recalculate();
            }
        }

        public int TotalLeavening
        {
            get => this.totalLeavening;
            set
            {
                this.SetProperty(ref this.totalLeavening, value);
                Preferences.Set(nameof(TotalLeavening), value);
                this.Recalculate();
            }
        }

        public int FridgeLeavening
        {
            get => this.fridgeLeavening;
            set
            {
                this.SetProperty(ref this.fridgeLeavening, value);
                Preferences.Set(nameof(FridgeLeavening), value);
                this.Recalculate();
            }
        }

        private void Recalculate()
        {
            var i = this.TotalLeavening;
            var s = this.FridgeLeavening;
            var l = this.SaltPerc * 10;
            var d = this.FatPerc * 10;
            var t = this.WaterPerc;
            var r = this.DoughBalls;
            var e = this.BallWeight;
            var g = 0.005;
            var v = 0;
            var p = this.RoomTemperature;
            var o = this.IsTray ? 1 : 0;
            var n = p * (1 - .25 * o );

            var c = i - 9 * s / 10.0;
            var N = 81.4206918743428 + 78.3939060802556 * Math.Log10(i);
            var m = 10 * Math.Round(N / 10.0);
            var h = 2250 * (1 + l / 200) * (1 + d / 300) / ((4.2 * t - 80 - .0305 * t * t) * Math.Pow(n, 2.5) * Math.Pow(c, 1.2));
            var u = r * e;
            var C = t * (l + d) + 1e3 * (t + 100);

            var f = u * v / 100;
            var k = 1e5 * (u - f) / C;
            var S = (1e3 * t * (u - f) / C);
            var x = (l * t * (u - f) / C);
            var w = (d * t * (u - f) / C);
            var b = (k * h - g * f);

            this.Flour = k;
            this.Water = S;
            this.Yeast = AdjustYeast(b);
            this.Salt = x;
            this.Fat = w;

            this.FlourBiga = k * (this.BigaPercentage / 100.0);
            this.WaterBiga1 = this.FlourBiga * this.BigaWaterPercentage / 100.0;
            this.WaterBiga2 = S - this.WaterBiga1;
            this.YeastBiga = AdjustYeast(this.FlourBiga * 0.01);
            this.FlourBigaDough = k - this.FlourBiga;
            this.SaltBiga = x;
            this.FatBiga = w;
        }

        double AdjustYeast(double yeast)
        {
            if (this.IsDriedYeast)
            {
                return yeast / 2;
            }
            else if (this.IsDriedYeast23)
            {
                return yeast * 2 / 3;
            }
            else if (this.IsSourdoughYeast)
            {
                return yeast * 120;
            }

            return yeast;
        }

        public string PizzaTypeImage
        {
            get => this.pizzaTypeImage;
            set => this.SetProperty(ref this.pizzaTypeImage, value);
        }

        public PizzaType SelectedPizzaType
        {
            get => this.selectedPizzaType;
            set
            {
                this.SetProperty(ref this.selectedPizzaType, value);

                switch (this.selectedPizzaType)
                {
                    case PizzaType.Tray:
                        this.PizzaTypeImage = "PizzaTeglia.jpg";
                        this.OilNeeded = true;
                        break;
                    case PizzaType.DoughBalls:
                        this.PizzaTypeImage = "PizzaRound.jpg";
                        this.OilNeeded = false;
                        break;
                }

                this.Recalculate();

                OnPropertyChanged(nameof(IsDoughballs));
                OnPropertyChanged(nameof(IsTray));
                Preferences.Set(nameof(SelectedPizzaType), (int) value);
            }
        }

        public int RoomTemperature
        {
            get => this.roomTemperature;
            set
            {
                this.SetProperty(ref this.roomTemperature, value);
                Preferences.Set(nameof(RoomTemperature), value);
                this.Recalculate();
            }
        }

        public bool NeedFlourBigaDough => this.FlourBigaDough > 0;       

        public int WaterPerc
        {
            get => this.waterPerc;
            set
            {
                this.SetProperty(ref this.waterPerc, value);
                Preferences.Set(nameof(WaterPerc), value);
                this.Recalculate();
            }
        }

        public Yeast SelectedYeast
        {
            get => this.selectedYeast;
            set
            {
                this.SetProperty(ref this.selectedYeast, value);
                Preferences.Set(nameof(SelectedYeast), (int) value);
                this.Recalculate();

                OnPropertyChanged(nameof(IsDriedYeast));
                OnPropertyChanged(nameof(IsDriedYeast23));
                OnPropertyChanged(nameof(IsFreshYeast));
                OnPropertyChanged(nameof(IsSourdoughYeast));
            }
        }

        public List<string> Yeasts { get; }

        private void FillTips()
        {
            var sb = new StringBuilder();

            sb.AppendLine("It is important that the flour type is STRONG or EXTRA STRONG");
            sb.AppendLine();
            sb.AppendLine("If there is no description in the package, an easy way to find out if your flour is STRONG is to look at the content of proteins in 100gr. It should be a percentage between 12% / 14%");
            sb.AppendLine();
            sb.AppendLine("In some countries you can also find specified the W of the flower. It must be a value between 260 / 300");

            this.FlourTip = sb.ToString();


            sb = new StringBuilder();

            sb.AppendLine("You can use normal cold tap water.");

            this.WaterTip = sb.ToString();
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
 [CallerMemberName] string propertyName = "",
 Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

}
