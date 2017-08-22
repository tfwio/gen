# GeneratorTool

This project was written out of the inspired need to generate code IMMEDIATELY—which
means that the inception point of this program was simply for a data-admin familiar/proficient
in CSharp.NET data semantics and WPF binding.
In other words, it was a work in progress and became stable within the condition
that you know how the application works, or proficient enough a programmer to get this working.

[issue #4](https://github.com/tfwio/cor3-gen/issues/4)
The application is not yet quite prepared for public domain release primarily due to my withholding the configuration files which generally put the application into action.

Sample configuration files **have been added** within the `../source/gen/schematics` ('gen' command-line project) and now we have other excuses for not releasing yet---almost there!

## REFERENCE ASSEMBLY NOTES

* [ModernUI for WPF](http://www.nuget.org/packages/ModernUI.WPF/) by [kozw](http://www.nuget.org/profiles/kozw/) via Nuget; See: [mui.codeplex.com](http://mui.codeplex.com)
* [ICSharpCode.AvalonEdit](https://github.com/icsharpcode/SharpDevelop/tree/master/src/Libraries/AvalonEdit/ICSharpCode.AvalonEdit), `ICSharpCode.SharpDevelop.Widgets`, and other SD binaries.  What I did was implement the zoom-widget as it is implemented by SharpDevelop.
* [An unofficial packaging of AvalonDock](http://www.nuget.org/packages/AvalonDock.Unofficial/) By: vitaminSP
* [cor3.stock](https://github.com/tfwio/cor3.stock) images, contains Oxygen Icons, Variations of the Oxygen Icon set, and other images I might not have properly attributed as of yet...

## Some old configuration settings that had been used

* [ExpressionBlend 4 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=10801).  EpxressionBlend4 was at a time, merely used to include a custom-font with the distribution. (ExpressionBlend v4.0.20525.0) I don't quite remember where I obtained this but the link should suffice.
    - All references to this assembly may be removed, but its unknown to me at this time weather it will cause a runtime error.
    - ...because it causes a error when loading in SharpDevelop on my VPC/XP setup.

In the CSProj PropertyGroup settings and some other removed references.  These are just some notes on the prior configuration...

    <ExpressionBlendVersion>4.0.20525.0</ExpressionBlendVersion>
    <RobotoBlack>Assets\Typo\Roboto 900.ttf</RobotoBlack>
    <UbuntuB>Fonts\Ubuntu-B-webfont.ttf</UbuntuB>
    <TargetFrameworkProfile></TargetFrameworkProfile>

Project-level Import

	<Import Project="$(MSBuildExtensionsPath)\Microsoft\Expression\Blend\.NETFramework\v4.0\Microsoft.Expression.Blend.WPF.targets" />
