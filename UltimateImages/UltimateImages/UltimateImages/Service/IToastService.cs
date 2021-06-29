using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateImages.Service
{
    public interface IToastService
    {
        void ShowLongAlert(string message);
        void ShowShortAlert(string message);

    }
}
