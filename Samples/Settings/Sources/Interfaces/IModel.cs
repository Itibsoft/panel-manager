using System;

namespace Settings
{
    public interface IModel : IDisposable
    {
        public int CountClicked { get; set; }
    }
}