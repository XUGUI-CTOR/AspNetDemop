using Caliburn.Micro;
using Gui.UploadExe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Gui.UploadExe.ViewModels
{
    [Export(typeof(ColorViewModel))]
    public class ColorViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator @event;
        [ImportingConstructor]
        public ColorViewModel(IEventAggregator @event)
        {
            this.@event = @event;
        }
        public void Red()
        {
            @event.PublishAsync(new ColorEvent(new SolidColorBrush(Colors.Red)),(f)=> f());
        }

        public void Green()
        {
            @event.PublishAsync(new ColorEvent(new SolidColorBrush(Colors.Green)), (f) => f());
        }

        public void Blue()
        {
            @event.PublishAsync(new ColorEvent(new SolidColorBrush(Colors.Blue)), (f) => f());
        }
    }
}
