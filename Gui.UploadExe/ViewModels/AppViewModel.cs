using Caliburn.Micro;
using Gui.UploadExe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Gui.UploadExe.ViewModels
{
    [Export(typeof(AppViewModel))]
    public class AppViewModel : PropertyChangedBase,IHandle<ColorEvent>
    {
        private int _count = 50;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyOfPropertyChange(nameof(Count));
                NotifyOfPropertyChange(nameof(CanIncrementCount));
            }
        }

        private SolidColorBrush _Color;
        public SolidColorBrush Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                NotifyOfPropertyChange(nameof(Color));
            }
        }

        public ColorViewModel ColorModel { get; private set; }
        [ImportingConstructor]
        public AppViewModel(ColorViewModel colorModel, IEventAggregator events)
        {
            ColorModel = colorModel;
            events.Subscribe(this);
        }

        public void IncrementCount(int d)
        {
            Count+=d;
        }

        public Task HandleAsync(ColorEvent message, CancellationToken cancellationToken)
        {
            Color = message.Color;
            return Task.CompletedTask;
        }

        public bool CanIncrementCount
        {
            get=> Count < int.MaxValue;
        }
    }
}
