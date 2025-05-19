using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheSpaceSystem
{
    public class Line : INotifyPropertyChanged
    {
        Player _currentturn = Player.None;
        private Color _backColor = Color.Transparent;
        private bool _enabled = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Player CurrentTurn
        {
            get => _currentturn;
            set
            {
                if (_currentturn != value)
                {
                    _currentturn = value;
                    InvokePropertyChanged();
                }
            }
        }
        public System.Drawing.Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    this.InvokePropertyChanged();
                    this.InvokePropertyChanged("BackColorMaui");
                }
            }
        } 

        public Microsoft.Maui.Graphics.Color BackColorMaui
        {
            get => this.ConvertToMauiColor(this.BackColor);
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    InvokePropertyChanged();
                }
            }
        }

        public void Clear() {
            BackColor = Color.Transparent;
            Enabled = true;
            CurrentTurn = Player.None;
        }
        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        private Microsoft.Maui.Graphics.Color ConvertToMauiColor(System.Drawing.Color systemColor)
        {
            float red = systemColor.R / 255f;
            float green = systemColor.G / 255f;
            float blue = systemColor.B / 255f;
            float alpha = systemColor.A / 255f;

            return new Microsoft.Maui.Graphics.Color(red, green, blue, alpha);
        }

    }
}
