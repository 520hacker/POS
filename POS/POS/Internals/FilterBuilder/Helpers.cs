namespace POS.Internals.FilterBuilder
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    static internal class Helpers
    {

        #region " Public methods "

        /// <summary>
        /// Returns the specified filter as a filter string.
        /// </summary>
        /// <param name="filter">
        /// The filter to return.
        /// </param>
        /// <returns>
        /// The string representation of the filter.
        /// </returns>
        public static string ReturnFilterAsString(POS.Internals.FilterBuilder.FilterBuilder.Filters filter)
        {
            //Return the correct filter string for the filter items
            switch (filter)
            {

                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WordDocuments:

                    return "Microsoft Word Documents (*.doc)|*.doc";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WordOpenXMLDocuments:

                    return "Microsoft Word Open XML Documents (*.docx)|*.docx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.LogFiles:

                    return "Log Files (*.log)|*.log";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MailMessages:

                    return "Mail Messages (*.msg)|*.msg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PagesDocuments:

                    return "Pages Documents (*.pages)|*.pages";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.RichTextFiles:

                    return "Rich Text Files (*.rtf)|*.rtf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.TextFiles:

                    return "Plain Text Files (*.txt)|*.txt";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WordPerfectDocuments:

                    return "WordPerfect Documents (*.wpd)|*.wpd";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WorksWordProcessorDocuments:

                    return "Microsoft Works Word Processor Documents (*.wps)|*.wps";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.Lotus123Spreadsheets:

                    return "Lotus 1-2-3 Spreadsheets (*.123)|*.123";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.Access2007DatabaseFiles:

                    return "Access 2007 Database Files (*.accdb)|*.accdb";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.CSV_Files:

                    return "Comma Separated Values Files (*.csv)|*.csv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DataFiles:

                    return "Data Files (*.dat)|*.dat";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DatabaseFiles:

                    return "Database Files (*.db)|*.db";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DLL_Files:

                    return "Dynamic Link Library Files (*.dll)|*.dll";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AccessDatabaseFiles:

                    return "Microsoft Access Database Files (*.mdb)|*.mdb";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PowerPointSlideShows:

                    return "PowerPoint Slide Shows (*.pps)|*.pps";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PowerPointPresentations:

                    return "PowerPoint Presentations (*.ppt)|*.ppt";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PowerPointOpenXMLDocuments:

                    return "Microsoft PowerPoint Open XML Documents (*.pptx)|*.pptx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.OpenOfficeBaseDatabaseFiles:

                    return "OpenOffice.org Base Database Files (*.sdb)|*.sdb";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.SQLDataFiles:

                    return "SQL Data Files (*.sql)|*.sql";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.vCardFiles:

                    return "vCard Files (*.vcf)|*.vcf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.UCConversionFiles:

                    return "Universal Converter Conversion Files (*.ucv)|*.ucv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WorksSpreadsheets:

                    return "Microsoft Works Spreadsheets (*.wks)|*.wks";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ExcelSpreadsheets:

                    return "Microsoft Excel Spreadsheets (*.xls)|*.xls";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ExcelOpenXMLDocuments:

                    return "Microsoft Excel Open XML Documents (*.xlsx)|*.xlsx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.XML_Files:

                    return "XML Files (*.xml)|*.xml";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.BMP_ImageFiles:

                    return "Bitmap Image Files (*.bmp)|*.bmp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.GIF_ImageFiles:

                    return "Graphical Interchange Format Files (*.gif)|*.gif";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.JPEG_ImageFiles:

                    return "JPEG Image Files (*.jpg,*.jpeg)|*.jpg;*.jpeg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PNG_ImageFiles:

                    return "Portable Network Graphic Files (*.png)|*.png";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AllImageFiles:

                    return "All Supported Image Files|*.bmp;*.gif;*.jpg" + ";*.jpeg;*.png;*.tif;*.tiff";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PhotoshopDocuments:

                    return "Photoshop Documents (*.psd)|*.psd";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PaintShopProImageFiles:

                    return "Paint Shop Pro Image Files (*.psp)|*.psp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ThumbnailImageFiles:

                    return "Thumbnail Image Files (*.thm)|*.thm";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.TIFF_ImageFiles:

                    return "Tagged Image Files (*.tif,*.tiff)|*.tif;*.tiff";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AdobeIllustratorFiles:

                    return "Adobe Illustrator Files (*.ai)|*.ai";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DrawingFiles:

                    return "Drawing Files (*.drw)|*.drw";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DrawingExchangeFormatFiles:

                    return "Drawing Exchange Format Files (*.dxf)|*.dxf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.EncapsulatedPostScriptFiles:

                    return "Encapsulated PostScript Files (*.eps)|*.eps";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PostScriptFiles:

                    return "PostScript Files (*.ps)|*.ps";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.SVG_Files:

                    return "Scalable Vector Graphics Files (*.svg)|*.svg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.Rhino3DModels:

                    return "Rhino 3D Models (*.3dm)|*.3dm";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AutoCADDrawingDatabaseFiles:

                    return "AutoCAD Drawing Database Files (*.dwg)|*.dwg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ArchiCADProjectFiles:

                    return "ArchiCAD Project Files (*.pln)|*.pln";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AdobeInDesignFiles:

                    return "Adobe InDesign Files (*.indd)|*.indd";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PDF_Files:

                    return "Portable Document Format Files (*.pdf)|*.pdf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AAC_Files:

                    return "Advanced Audio Coding Files (*.aac)|*.aac";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AIF_Files:

                    return "Audio Interchange File Format Files (*.aif)|*.aif";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.IIF_Files:

                    return "Interchange File Format Files (*.iif)|*.iif";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MediaPlaylistFiles:

                    return "Media Playlist Files (*.m3u)|*.m3u";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MIDI_Files:

                    return "MIDI Files (*.mid,*.midi)|*.mid;*.midi";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MP3_AudioFiles:

                    return "MP3 Audio Files (*.mp3)|*.mp3";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MPEG2_AudioFiles:

                    return "MPEG-2 Audio Files (*.mpa)|*.mpa";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.RealAudioFiles:

                    return "Real Audio Files (*.ra)|*.ra";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WAVE_AudioFiles:

                    return "WAVE Audio Files (*.wav)|*.wav";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsMediaAudioFiles:

                    return "Windows Media Audio Files (*.wma)|*.wma";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters._3GPP2MultimediaFiles:

                    return "3GPP2 Multimedia Files (*.3g2)|*.3g2";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters._3GPPMultimediaFiles:

                    return "3GPP Multimedia Files (*.3gp)|*.3gp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AVI_Files:

                    return "Audio Video Interleave Files (*.avi)|*.avi";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.FlashVideoFiles:

                    return "Flash Video Files (*.flv)|*.flv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MatroskaVideoFiles:

                    return "Matroska Video Files (*.mkv)|*.mkv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AppleQuickTimeMoviesMov:

                    return "Apple QuickTime Movie Files (*.mov)|*.mov";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MPEG4_VideoFiles:

                    return "MPEG-4 Video Files (*.mp4)|*.mp4";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MPEG_VideoFiles:

                    return "MPEG Video Files (*.mpg)|*.mpg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AppleQuickTimeMoviesQT:

                    return "Apple QuickTime Movie Files (*.qt)|*.qt";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.RealMediaFiles:

                    return "Real Media Files (*.rm)|*.rm";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.FlashMovies:

                    return "Flash Movies (*.swf)|*.swf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DVDVideoObjectFiles:

                    return "DVD Video Object Files (*.vob)|*.vob";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsMediaVideoFiles:

                    return "Windows Media Video Files (*.wmv)|*.wmv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ActiveServerPages:

                    return "Active Server Pages (*.asp)|*.asp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.CascadingStyleSheets:

                    return "Cascading Style Sheets (*.css)|*.css";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.HTML_Files:

                    return "HTML Files (*.htm,*.html)|*.htm;*.html";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.JavaScriptFiles:

                    return "JavaScript Files (*.js)|*.js";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.JavaServerPages:

                    return "Java Server Pages (*.jsp)|*.jsp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PHP_Files:

                    return "Hypertext Preprocessor Files (*.php)|*.php";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.RichSiteSummaryFiles:

                    return "Rich Site Summary Files (*.rss)|*.rss";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.XHTML_Files:

                    return "XHTML Files (*.xhtml)|*.xhtml";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsFontFiles:

                    return "Windows Font Files (*.fnt)|*.fnt";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.GenericFontFiles:

                    return "Generic Font Files (*.fon)|*.fon";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.OpenTypeFonts:

                    return "OpenType Fonts (*.otf)|*.otf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.TrueTypeFonts:

                    return "TrueType Fonts (*.ttf)|*.ttf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ExcelAddInFiles:

                    return "Excel Add-In Files (*.xll)|*.xll";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsCabinetFiles:

                    return "Windows Cabinet Files (*.cab)|*.cab";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsControlPanel:

                    return "Windows Control Panel (*.cpl)|*.cpl";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsCursors:

                    return "Windows Cursors (*.cur)|*.cur";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsMemoryDumps:

                    return "Windows Memory Dumps (*.dmp)|*.dmp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DeviceDrivers:

                    return "Device Drivers (*.drv)|*.drv";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.SecurityKeys:

                    return "Security Keys (*.key)|*.key";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.FileShortcuts:

                    return "File Shortcuts (*.lnk)|*.lnk";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsSystemFiles:

                    return "Windows System Files (*.sys)|*.sys";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ConfigurationFiles:

                    return "Configuration Files (*.cfg)|*.cfg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.INI_Files:

                    return "Windows Initialization Files (*.ini)|*.ini";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.OutlookProfileFiles:

                    return "Outlook Profile Files (*.prf)|*.prf";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MacOSXApplications:

                    return "Mac OS X Applications (*.app)|*.app";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DOSBatchFiles:

                    return "DOS Batch Files (*.bat)|*.bat";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.CGI_Files:

                    return "Common Gateway Interface Scripts (*.cgi)|*.cgi";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DOSCommandFiles:

                    return "DOS Command Files (*.com)|*.com";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsExecutableFiles:

                    return "Windows Executable File (*.exe)|*.exe";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsScripts:

                    return "Windows Scripts (*.ws)|*.ws";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters._7ZipCompressedFiles:

                    return "7-Zip Compressed Files (*.7z)|*.7z";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DebianSoftwarePackages:

                    return "Debian Software Packages (*.deb)|*.deb";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.GnuZippedFile:

                    return "Gnu Zipped Files (*.gz)|*.gz";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MacOSXInstallerPackages:

                    return "Mac OS X Installer Packages (*.pkg)|*.pkg";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WinRARCompressedArchives:

                    return "WinRAR Compressed Archives (*.rar)|*.rar";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.SelfExtractingArchives:

                    return "Self-Extractingd Archives (*.sea)|*.sea";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.StuffitArchives:

                    return "Stuffit Archives (*.sit)|*.sit";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.StuffitXArchives:

                    return "Stuffit X Archives (*.sitx)|*.sitx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ZippedFiles:

                    return "Zipped Files (*.zip)|*.zip";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ExtendedZipFiles:

                    return "Extended Zip Files (*.zipx)|*.zipx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.BinHex4EncodedFiles:

                    return "BinHex 4.0 Encoded Files (*.hqx)|*.hqx";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.MultiPurposeInternetMailMessages:

                    return "Multi-Purpose Internet Mail Messages (*.mim)|*.mim";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.UuencodedFiles:

                    return "Uuencoded Files (*.uue)|*.uue";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.C_CPlusPlus_SourceCodeFiles:

                    return "C/C++ Source Code Files (*.c)|*.c";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.CPlusPlus_SourceCodeFiles:

                    return "C++ Source Code Files (*.cpp)|*.cpp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.Java_SourceCodeFiles:

                    return "Java Source Code Files (*.java)|*.java";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PerlScripts:

                    return "Perl Scripts (*.pl)|*.pl";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.VB_SourceCodeFiles:

                    return "VB Source Code Files (*.vb)|*.vb";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.VisualStudioSolutionFiles:

                    return "Visual Studio Solution Files (*.sln)|*.sln";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.CSharp_SourceCodeFiles:

                    return "C# Source Code Files (*.cs)|*.cs";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.BackupFiles_BAK:

                    return "Backup Files (*.bak)|*.bak";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.BackupFiles_BUP:

                    return "Backup Files (*.bup)|*.bup";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.NortonGhostBackupFiles:

                    return "Norton Ghost Backup Files (*.gho)|*.gho";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.OriginalFiles:

                    return "Original Files (*.ori)|*.ori";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.TemporaryFiles:

                    return "Temporary Files (*.tmp)|*.tmp";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.DiscImageFiles:

                    return "Disc Image Files (*.iso)|*.iso";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.ToastDiscImages:

                    return "Toast Disc Images (*.toast)|*.toast";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.Virtual_CDs:

                    return "Virtual CDs (*.vcd)|*.vcd";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsInstallerPackages:

                    return "Windows Installer Packages (*.msi)|*.msi";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.PartiallyDownloadedFiles:

                    return "Partially Downloaded Files (*.part)|*.part";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.BitTorrentFiles:

                    return "BitTorrent Files (*.torrent)|*.torrent";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.YahooMessengerDataFiles:

                    return "Yahoo! Messenger Data Files (*.yps)|*.yps";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.AllFiles:

                    return "All Files (*.*)|*.*";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.WindowsIcons:

                    return "Windows Icons (*.ico)|*.ico";
                case POS.Internals.FilterBuilder.FilterBuilder.Filters.EXIF_ImageFiles:

                    return "Exchangeable Image Format Files (*.exif)|*.exif";
                default:

                    return null;
            }
        }

        #endregion

    }
}