namespace ProbablyNuclear
{
    public static class ModCorpos
    {
        public static void DisplayCorpo(RichTextBuilder builder, string manufacturer)
        {
            if (manufacturer.Equals("interdyne"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_interdyne"), ignoreEmpty: true, RenderHandler.ColorPalette.Blue, bold: false, italic: false, underline: true);
            else if (manufacturer.Equals("gorlex"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_gorlex"), ignoreEmpty: true, RenderHandler.ColorPalette.DarkRed, bold: false, italic: false, underline: true);
            else if (manufacturer.Equals("waffle"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_waffle"), ignoreEmpty: true, RenderHandler.ColorPalette.DarkOrange, bold: false, italic: false, underline: true);
            else if (manufacturer.Equals("self"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_self"), ignoreEmpty: true, RenderHandler.ColorPalette.LightBlue, bold: false, italic: false, underline: true);
            else if (manufacturer.Equals("donk"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_donk"), ignoreEmpty: true, RenderHandler.ColorPalette.LightPurple, bold: false, italic: false, underline: true);
            else if (manufacturer.Equals("cybersun"))
                builder.AddLine(ModHelper.GetLocalized("probably_nuclear", "corpo_name_cybersun"), ignoreEmpty: true, RenderHandler.ColorPalette.Red, bold: false, italic: false, underline: true);
        }
    }
}
