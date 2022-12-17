using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Models
{
    public class ThemesProvider
    {
        Dictionary<string, Theme> themes = new Dictionary<string, Theme>();

        public ThemesProvider()
        {
            themes.Add("Light", new Theme()
            {
                textColor = Color.FromArgb(0, 0, 0),
                backgroundTextColor = Color.FromArgb(255, 255, 255),
                backgroundUIColor = Color.FromArgb(245, 245, 245),
                backgroundUIHoverColor = Color.FromArgb(240, 240, 240),
                backgroundUISelectedColor = Color.FromArgb(224, 224, 224)

            });

            themes.Add("Dark", new Theme()
            {
                textColor = Color.FromArgb(255, 255, 240),
                backgroundTextColor = Color.FromArgb(48, 48, 48),
                backgroundUIColor = Color.FromArgb(24, 24, 24),
                backgroundUIHoverColor = Color.FromArgb(48, 48, 48),
                backgroundUISelectedColor = Color.FromArgb(64, 64, 64)
            });

            themes.Add("DarkKhaki", new Theme()
            {
                // textColor = nerly to red
                textColor = Color.FromArgb(255, 255, 240),
                backgroundTextColor = Color.FromArgb(189, 183, 107),
                backgroundUIColor = Color.FromArgb(176, 173, 97),
                backgroundUIHoverColor = Color.FromArgb(189, 183, 107),
                backgroundUISelectedColor = Color.FromArgb(189, 183, 107)

            });

            themes.Add("CornflowerBlue", new Theme()
            {
                // textColor = nerly to white
                textColor = Color.FromArgb(255, 255, 240),
                backgroundTextColor = Color.FromArgb(100, 149, 237),
                backgroundUIColor = Color.FromArgb(90, 139, 227),
                backgroundUIHoverColor = Color.FromArgb(100, 149, 237),
                backgroundUISelectedColor = Color.FromArgb(100, 149, 237)
            });
        }

        public Theme GetTheme(string name)
        {
            if (themes.ContainsKey(name))
            {
                return themes[name];
            }
            else
            {
                return themes["Light"];
            }
        }
    }

    public class Theme
    {
        public Color textColor { get; set; }
        public Color backgroundTextColor { get; set; }
        public Color backgroundUIColor { get; set; }
        public Color backgroundUIHoverColor { get; set; }
        public Color backgroundUISelectedColor { get; set; }
        
    }
}
