using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Timotheus.Views.Dialogs;

namespace Timotheus
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            /*
            Schedule.Event[] array =
            {
                new Schedule.Event(new DateTime(2022, 8, 8), new DateTime(2022, 8, 8), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 8, 15), new DateTime(2022, 8, 15), "Møde v. missionærerne Ruth og Philip Bach Svendsen, Tanzania. Mission med litteratur, ægteskabskurser og pigerettigheder.", string.Empty),
                new Schedule.Event(new DateTime(2022, 8, 22), new DateTime(2022, 8, 22), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 8, 29), new DateTime(2022, 8, 29), "Fællesspisning kl. 17.45", string.Empty),
                new Schedule.Event(new DateTime(2022, 8, 29), new DateTime(2022, 8, 29), "Kl. 18.30 møde. Louise Høgild Pedersen, Underviser i latin og sjælesorg, DBI. Tekster fra Ruths bog.", string.Empty),

                new Schedule.Event(new DateTime(2022, 9, 5), new DateTime(2022, 9, 5), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 9, 9), new DateTime(2022, 9, 11), "Weekendlejr. Hakon Christensen, Landsleder i Bibellæser-Ringen, holder bibeltimer: Den sejrende Kristus i Johannes’ Åbenbaring. Program følger på lmfyn.dk", string.Empty),
                new Schedule.Event(new DateTime(2022, 9, 19), new DateTime(2022, 9, 19), "Møde v. Jens Peter Rejkær, Hillerød, Missionsmedarbejder i LM", string.Empty),
                new Schedule.Event(new DateTime(2022, 9, 26), new DateTime(2022, 9, 26), "Bibelkreds", string.Empty),

                new Schedule.Event(new DateTime(2022, 10, 5), new DateTime(2022, 10, 5), "Fællesmøde med spisning kl 17.45", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 5), new DateTime(2022, 10, 5), "Møde kl 18.30 v. Erik Haahr Andersen, prædikant i LM. Emne: Er mine penge mine? (onsdag). LMU er værter.", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 12), new DateTime(2022, 10, 12), "Lovsangsaften (onsdag), LM Fyn arrangement, LMU Odense er værter", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 17), new DateTime(2022, 10, 17), "Møde. Bibeltime årgang 700 – Lectio Divina – Individuel bibellæsning som bøn – Gud ønsker at være dig nær! (uge 42)", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 24), new DateTime(2022, 10, 24), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 31), new DateTime(2022, 10, 31), "Fællesspisning kl. 17.45", string.Empty),
                new Schedule.Event(new DateTime(2022, 10, 31), new DateTime(2022, 10, 31), "Kl. 18.30 møde: se lmfyn.dk", string.Empty),

                new Schedule.Event(new DateTime(2022, 11, 1), new DateTime(2022, 11, 1), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 11, 7), new DateTime(2022, 11, 11), "LMU inviterer til møderække v. Morten Friis, konsulent i Discipel 24-7. Program følger på odenselmu.dk", string.Empty),
                new Schedule.Event(new DateTime(2022, 11, 14), new DateTime(2022, 11, 14), "Møde v. Ingvard Christensen, prædikant i LM. Emne: Inspiration fra Nazaret og Magdala - i Bibelens fodspor i billeder og ord.", string.Empty),
                new Schedule.Event(new DateTime(2022, 11, 21), new DateTime(2022, 11, 21), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 11, 30), new DateTime(2022, 11, 30), "Fællesspisning kl. 17.45", string.Empty),
                new Schedule.Event(new DateTime(2022, 11, 30), new DateTime(2022, 11, 30), "Kl. 18.30 møde v. Christian Medom, præst i Sankt Hans Kirke. LMU er værter", string.Empty),

                new Schedule.Event(new DateTime(2022, 12, 5), new DateTime(2022, 12, 5), "Bibelkreds", string.Empty),
                new Schedule.Event(new DateTime(2022, 12, 13), new DateTime(2022, 12, 13), "Julefest på Kratholmskolen i Bellinge (tirsdag). Indbydelse følger på lmfyn.dk", string.Empty),
                new Schedule.Event(new DateTime(2022, 12, 19), new DateTime(2022, 12, 19), "Julemøde i missionshuset v. Enok Mogensen, lærer på LME. Emne: Vores utålmodighed - Guds langmodighed. 2. Pet 3, 8-18.", string.Empty),

                new Schedule.Event(new DateTime(2023, 1, 2), new DateTime(2023, 1, 2), "Nytårsmøde", string.Empty),
                new Schedule.Event(new DateTime(2023, 1, 9), new DateTime(2023, 1, 13), "Evangelisk Alliance bedeuge, mandag – fredag.", string.Empty),
                new Schedule.Event(new DateTime(2023, 1, 20), new DateTime(2023, 1, 21), "Bibelweekend LM-Fyn, Taler: Peter Nissen, afdelingsmissionær LM Sønderjylland-Fyn, fredag – lørdag.", string.Empty)
            };
            System.Collections.Generic.List<Schedule.Event> events = new(array);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.pdf";
            string title = "Luthersk Mission Odense";
            string subtitle = "Program efterår 2022";
            string comment = "Se mere på lmfyn.dk";
            string backpage = "#Om Luthersk Mission Odense\nI Luthersk Mission samles vi for at høre, hvad Gud ønsker at fortælle os. Vi er et åbent fællesskab, som mødes til forskellige arrangementer i mødelokalet, på lejre og til bibellæsning, samtale og bøn i smågrupper.\n\nVi er en del af foreningen Luthersk Mission: www.dlm.dk\n\nFor yderligere information eller ønske om at være med i en smågruppe: Kontakt Hans Ejnar Winther, e-mail: bobjerg@msn.com Tlf: 25253742\n\n#Praktiske oplysninger\nVi mødes primært mandage i Godthåbskirken, Rosenlunden 15, 5000 Odense C. Møderne begynder kl. 19.00 med mindre andet er nævnt.\n\n#Fællesspisning\nGerne tilmelding til Bent på ab.roager@it.dk eller sms på tlf.: 20 66 18 13. Prisen er 30 kr. pr. person - dog maks. 100 kr. pr. familie og unge 15 kr. Børn under skolealder er gratis. Ved fællesspisning slutter mødet kl. 20.00\n\n#Økonomi\nLM er drevet ved frivillige gaver, som kan gives ved bankoverførsel: Reg.nr.: 1551 konto: 0006623735 eller MobilePay 60945\n\n#Ønsker du en samtale om tro og liv\nkan du kontakte: Anette Sørensen, anette.h.soerensen@gmail.com / 29257505 eller Jeff Sørensen, jeff@dadlnet.dk / 40754910.\n\nAnette og Jeff tilbyder også forlovelsessamtaler, se: http://dlm.dk/lm-danmark/forlovelsessamtaler";
            string logopath = @"C:\Users\marti\Desktop\LM.png";
            Utility.PDF.CreateBook(events, path, title, subtitle, comment, backpage, logopath);
            */
            
#if DEBUG
            Timotheus.Initalize();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
#else
            try
            {
                Timotheus.Initalize();
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                Timotheus.Log(e);
            }
#endif
            
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect();

        public async static void Error(string title, Exception exception, Window window)
        {
            Log(exception);

            MessageBox msDialog = new()
            {
                DialogTitle = title,
                DialogText = exception.Message
            };
            await msDialog.ShowDialog(window);
        }

        /// <summary>
        /// Adds text to the current log.
        /// </summary>
        public static void Log(string text)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Timotheus/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                File.AppendAllText(path + DateTime.Now.ToString("d") + ".txt", "[" + DateTime.Now + "]: " + text + "\n");
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Adds exception to the current log.
        /// </summary>
        public static void Log(Exception e)
        {
            Log(e.ToString());
            if (e.InnerException != null)
                Log(e.InnerException);
        }
    }
}